using System;
using System.Text;
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

namespace SellAI.Services;

public class AnalyzeContextService : IAnalyzeContext {
  private readonly IMapper _mapper;
  private readonly ISysMenu _sysMenu;
  private readonly ISysContext _context;
  private readonly IAnalyzeAction _action;
  private readonly IData _data;

  public AnalyzeContextService(
    IMapper mapper,
    ISysMenu sysMenu,
    ISysContext sysContext,
    IAnalyzeAction action,
    IData data
  )
  {
    _mapper = mapper;
    _sysMenu = sysMenu;
    _context = sysContext;
    _action = action;
    _data = data;
  }

  public async Task<MessageDTO> GetMessagesAsync(Message response, RoleAppDTO roleApp, string previousIntentID = "", string previousContextID = "")
  {
    bool flagOutofScope = true;
    // convert entities from response to entity.
    List<Entity> listEntity = ConvertToEntity(response);

    MessageDTO messageDTO = new();

    // UNDONE: use previousIntentID for comparar response.
    // loop from intents
    if (response.Intents.Count > 0) {
      // UNDONE: I must consider the percentage of the intent.
      // UNDONE: In case the intention is very low in percentage, give an answer that the question is not understood.
      var intent = response.Intents[0];
      var sys_menu = await _sysMenu.GetIntentAsync(intent.Name, roleApp);
      if (sys_menu != null) {
        flagOutofScope = false;
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

        // Get messages and filters to apply
        ResponseAnalyze responseAnalyze = _action.ActionAndEntity(sys_menu, listEntity);

        switch (sys_menu.Accion) {
          case "create":
            // it's ok in response analyze.
            if (responseAnalyze.AllFieldsComplete) {
              // TODO: Set default values too.
              // TODO: Set display of sys_menu in response.Entities. 
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
            break;
          case "read":
            List<ReadDataDTO> readDataDTOs = await _data.ReadAsync(responseAnalyze.ReadDatas!, sys_menu.Collection, roleApp.App);
            string typeStructured = sys_menu.Tipo ?? "default";
            foreach (ReadDataDTO dataDTO in readDataDTOs) {
              StringBuilder sbMessage = new StringBuilder();
              Dictionary<string, string> row = new();
              foreach (KeyValuePair<string, List<EntitiesDTO>> kvpEntity in dataDTO.Entidades) {
                kvpEntity.Value.ForEach(k => {
                  switch (typeStructured) {
                    case "table":
                      // Search if exists column but I create it.
                      var findColumn = messageDTO.table.columns.Find(c => c.field == k.Role);
                      if (findColumn == null) {
                        messageDTO.table.columns.Add(new Models.DTOs.Object.ColumnDto {
                          field = k.Role,
                          header = k.Display ?? k.Role
                        });
                      }

                      if (row.ContainsKey(k.Role)) {
                        row[k.Role] = k.Body;
                      }
                      else {
                        row.Add(k.Role, k.Body);
                      }
                      
                      break;
                    default:
                      sbMessage.Append($"\n{k.Display ?? k.Role}:  {k.Body}");
                      break;
                  }
                });
              }
              messageDTO.table.rows.Add(row);
              if (sbMessage.Length > 0)
                messageDTO.messages.Add(sbMessage.ToString());
            }
            messageDTO.messages.Add(sys_menu.Display);
            break;
        }

      }
    }

    if (flagOutofScope) {
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
          entity.Alias = entity.Search = "";
          entity.Required = false;
          listEntity.Add(entity);
        });
      }
    return listEntity;
  }
}
