using System;
using AutoMapper;
using iText.Commons.Actions.Contexts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.AI;
using SellAI.Models.AI.Objects;
using SellAI.Models.Data;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Services
{
  public class AnalyzeContextService : IAnalyzeContext
  {
    private readonly IMapper _mapper;
    private readonly ISysMenu _sysMenu;
    private readonly ISysContext _context;
    private readonly IData _data;

    public AnalyzeContextService(
      IMapper mapper,
      ISysMenu sysMenu,
      ISysContext sysContext,
      IData data
    )
    {
      _mapper = mapper;
      _sysMenu = sysMenu;
      _context = sysContext;
      _data = data;
    }

    public async Task<MessageDTO> GetMessagesAsync(Message response, RoleAppDTO roleApp,string previousIntentID = "", string previousContextID = "")
    {
      // convert entities from response to entity.
      List<Entity> listEntity = ConvertToEntity(response);

      MessageDTO messageDTO = new();
      messageDTO.messages = new();

      // HACK: use previousIntentID for comparar response.
      // loop from intents
      if (response.Intents.Count > 0) {
        // TODO: I must consider the percentage of the intent.
        // TODO: In case the intention is very low in percentage, give an answer that the question is not understood.
        var intent = response.Intents[0];
        var sys_menu = await _sysMenu.GetIntentAsync(intent.Name, roleApp);
        if (sys_menu != null) {

          // Save context
          Sys_Context context = new() {
            Id = previousContextID,
            Display = sys_menu.Display,
            Accion = sys_menu.Accion,
            Intents = response,
            App = roleApp.App
          };

          if (string.IsNullOrEmpty(previousContextID))
            context = await _context.CreateContextAsync(context);
          else
            context = await _context.UpdateContextAsync(context);

          // Add Id to response
          Sys_ContextDTO sys_ContextDTO = new() {
            Id = context.Id!,
            Text = context.Intents.Text,
            Created = false
          };
          messageDTO.contexts = sys_ContextDTO;

          // Only save if have entities. 
          if (sys_menu.Entities != null && sys_menu.Entities.Count > 0) {
            // Find the ones that don't match
            var exceptEntities = sys_menu.Entities
                  .Select(e => e.RolID)
                  .Except(listEntity
                    .Select(l => l.RolID)).ToList();
            bool allFieldsComplete = true;
            if (exceptEntities.Count > 0) {
              // Missing entities to complete
              var missEntity = sys_menu.Entities.Where(e => exceptEntities.Any(a => a == e.RolID)).ToList();
              missEntity.ForEach(m => {
                if (m.Required.HasValue) {
                  if (allFieldsComplete) {
                    messageDTO.messages.Add("Por favor ingresar los siguientes datos: \n");
                  }
                  if (m.RolID == null) {
                    //Revisar por los roles...
                  }
                  else
                    messageDTO.messages.Add(m.Message!);
                  allFieldsComplete = false;
                }
              });
            }


            if (allFieldsComplete) {
              // TODO: Get default values too.
              Datas datas = new() {
                Intencion = intent,
                Entidades = response.Entities!,
                App = roleApp.App
              };
              Response resp = await _data.CreateOrUpdateAsync(datas, roleApp, true);// previousIntentID for Update.
              if (resp.code == "ok") {
                messageDTO.messages.Add("✅ Creado correctamente!");
                messageDTO.contexts.Created = true;
              }
              else
                messageDTO.messages.Add($"⚠️ Hubo un error al grabar. Intente nuevamente más tarde. ({resp.error})");
            }
          }
        }
      }
      else {
        string outOfScope = await _sysMenu.GetOutOfScopeAsync();
        messageDTO.messages.Add(outOfScope);
      }
      return messageDTO;
    }

    /// <summary>
    /// Convert entities from response to Entity(Model)
    /// </summary>
    /// <param name="response">Response where contains entities</param>
    /// <returns>Return List of model Entity</returns>
    public List<Entity> ConvertToEntity(Message response)
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
  }
}

