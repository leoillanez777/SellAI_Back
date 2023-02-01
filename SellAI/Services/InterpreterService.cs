using System;
using System.Security.Claims;
using AutoMapper;
using iText.Commons.Actions.Contexts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.AI;
using SellAI.Models.AI.Objects;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Services
{
  public class InterpreterService : IInterpreter {

    private readonly IMongoClient _client;
    private readonly IMongoCollection<Sys_Menu> _db;
    private readonly IRestApi _restApi;
    private readonly IMapper _mapper;
    private readonly ISysContext _context;

    public InterpreterService(IMongoClient client,
      IOptions<ContextMongoDB> options,
      IRestApi restApi,
      IMapper map,
      ISysContext context)
    {
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Sys_Menu>(options.Value.SysMenuCollectionName);
      _restApi = restApi;
      _mapper = map;
      _context = context;
    }

    /// <summary>
    /// Method for response without context
    /// </summary>
    /// <param name="message">user text</param>
    /// <returns>response to the user with the context created.</returns>
    public async Task<string> SendMessageAsync(string message, RoleAppDTO roleApp)
    {
      // Send message and return intents.
      Message response = await _restApi.MessageAsync(message);

      MessageDTO messagesDTO = new();
      messagesDTO.msgDTOs = new();
      messagesDTO.contexts = new();

      // UNDONE: Allow greeting and action too.

      if (IsGreeting(response)) {
        var sysMessage = await _db.FindAsync(f => f.Nombre == "saludos" || f.IntentID == "1275683856349570").Result.FirstOrDefaultAsync();
        MsgDTO msgDTO = new();
        msgDTO.MessageID = sysMessage.IntentID;
        msgDTO.Content = sysMessage.Mensaje!;
        messagesDTO.msgDTOs.Add(msgDTO);
      }
      else {
        messagesDTO = await LoopIntentAndEntityAsync(response, roleApp.App);
      }

      return JsonConvert.SerializeObject(messagesDTO);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> SendResponseAsync(string message, string token, string appName)
    {
      // TODO: How to detect if the context is the same as the last?.



      // Get context from DB. Delete it?
      Sys_Context context = await _context.GetContextAsync(token);
      string newMessage = "";

      if (context.Intents.Intents.Count > 0) {
        // add newMessage with intent.
        foreach (var intent in context.Intents.Intents)
          newMessage += intent.Name.Replace("_", " ") + " ";
      }

      // Send message with all entities
      if (context.Intents.Entities != null)
        foreach (KeyValuePair<string, List<Entities>> kvp in context.Intents.Entities) {
          kvp.Value.ForEach(v => {
            newMessage += v.Value + " ";
          });
        }

      newMessage += message;

      // Send new message with data and return intents.
      Message response = await _restApi.MessageAsync(newMessage);

      MessageDTO messagesDTO = new();
      messagesDTO.msgDTOs = new();
      messagesDTO.contexts = new();
      messagesDTO = await LoopIntentAndEntityAsync(response, appName);

      return JsonConvert.SerializeObject(messagesDTO);
    }

    /// <summary>
    /// Convert response from API to DTO
    /// </summary>
    /// <param name="response">Response of API</param>
    /// <param name="app">Name app for filter</param>
    /// <returns>Return DTO with missing entities or messages</returns>
    private async Task<MessageDTO> LoopIntentAndEntityAsync(Message response, string app)
    {
      // convert entities from response to entity.
      List<Entity> listEntity = ConvertToEntity(response);

      MessageDTO messageDTO = new();
      messageDTO.msgDTOs = new();
      messageDTO.contexts = new();

      // loop from intents
      foreach (var intent in response.Intents) {
        var sys_menu = await _db.FindAsync(f => f.Nombre == intent.Name && f.App == app).Result.FirstOrDefaultAsync();
        if (sys_menu != null) {

          // Save context (New document)
          Sys_Context context = new();
          context.App = app;
          context.Collection = sys_menu.Collection;
          context.Intents = response;
          context = await _context.CreateContextAsync(context);

          // Add Id to response
          Sys_ContextDTO sys_ContextDTO = new();
          sys_ContextDTO.Id = context.Id!;
          sys_ContextDTO.Collection = context.Collection;
          sys_ContextDTO.Text = context.Intents.Text;
          messageDTO.contexts.Add(sys_ContextDTO);

          // If have entity 
          if (sys_menu.Entities != null && sys_menu.Entities.Count > 0) {
            // Find the ones that don't match
            var exceptEntities = sys_menu.Entities
                  .Select(e => e.EntityID)
                  .Except(listEntity
                    .Select(l => l.EntityID)).ToList();
            if (exceptEntities.Count > 0) {
              // Missing entities to complete
              var missEntity = sys_menu.Entities.Where(e => exceptEntities.Any(a => a == e.EntityID)).ToList();
              missEntity.ForEach(m => {
                if (m.Required.HasValue) {
                  MsgDTO msgDTO = new();
                  msgDTO.MessageID = m.EntityID;
                  msgDTO.Content = m.Message;
                  messageDTO.msgDTOs.Add(msgDTO);
                }
              });
            }
          }
        }
      }
      return messageDTO;
    }


    /// <summary>
    /// Convert entities from response to Entity(Model)
    /// </summary>
    /// <param name="response">Response where contains entities</param>
    /// <returns>Return List of model Entity</returns>
    private List<Entity> ConvertToEntity(Message response)
    {
      List<Entity> listEntity = new();
      if (response.Entities != null && response.Entities.Count > 0)
        foreach (KeyValuePair<string, List<Entities>> kvp in response.Entities) {
          kvp.Value.ForEach(v => {
            Entity entity = new();
            entity = _mapper.Map<Entity>(v);
            entity.Alias = "";
            entity.Required = false;
            listEntity.Add(entity);
          });
        }
      return listEntity;
    }


    /// <summary>
    /// Detect if is greeting
    /// </summary>
    /// <param name="message">Reponse whre contains intents</param>
    /// <returns>True if intent is greeting</returns>
    private bool IsGreeting(Message message)
    {
      return (from a in message.Intents
              where a.Id == "1275683856349570" || a.Name == "saludos"
              select a).FirstOrDefault() != null;
      ;
    }
  }
}

