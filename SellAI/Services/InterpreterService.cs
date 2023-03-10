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
using SellAI.Models.Data;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Services
{
  public class InterpreterService : IInterpreter {
    private readonly IRestApi _restApi;
    private readonly IAnalyzeContext _analyze;
    private readonly ISysContext _context;
    private readonly ISysMenu _sysMenu;

    public InterpreterService(
      IRestApi restApi,
      IAnalyzeContext analyze,
      ISysContext context,
      ISysMenu sysMenu
    )
    {
      _restApi = restApi;
      _analyze = analyze;
      _context = context;
      _sysMenu = sysMenu;
    }

    public async Task<string> SendMessageAsync(string message, RoleAppDTO roleApp)
    {
      // Send message and return intents.
      Message response = await _restApi.MessageAsync(message);

      MessageDTO messagesDTO = new();
      messagesDTO.messages = new();

      if (response != null) {
        // get it is a greeting.
        Sys_Menu greeting = null!;
        if (response.Intents.Count > 0)
          greeting = await _sysMenu.GetGreetingAsync(response.Intents[0].Name);

        if (greeting is not null) {
          messagesDTO.messages.Add(greeting.Mensaje!);
        }
        else {
          messagesDTO = await _analyze.GetMessagesAsync(response!, roleApp);
        }
      }
      return JsonConvert.SerializeObject(messagesDTO);
    }

    public async Task<string> SendResponseAsync(string message, string token, RoleAppDTO roleApp)
    {
      string newMessage = "", intentID = "", contextID = "";
      // UNDONE: I must take into account the action
      Sys_Context context = await _context.GetContextAsync(token);
      if (context != null) {
        contextID = context.Id!;
        // Get previous intent id.
        intentID = context.Intents.Intents[0].Id;
        // concatenate intent
        newMessage += context.Display + " ";
        // Send message with all entities
        if (context.Intents.Entities != null)
          foreach (KeyValuePair<string, List<Entities>> kvp in context.Intents.Entities) {
            kvp.Value.ForEach(v => {
              newMessage += v.Value + " ";
            });
          }
      }

      newMessage += message;
      // Send new message with data and return intents.
      Message response = await _restApi.MessageAsync(newMessage);

      MessageDTO messagesDTO = new();
      messagesDTO.messages = new();
      messagesDTO = await _analyze.GetMessagesAsync(response, roleApp, intentID, contextID);

      return JsonConvert.SerializeObject(messagesDTO);
    }
  }
}

