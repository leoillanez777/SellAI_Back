using System;
using Newtonsoft.Json;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.Data;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Services;
public class AnalyzeActionService : IAnalyzeAction {

  public AnalyzeActionService()
  {
  }

  public ResponseAnalyze ActionAndEntity(Sys_Menu sys_menu, List<Entity> listEntity)
  {
    ResponseAnalyze response = new();
    response.AllFieldsComplete = true;

    switch (sys_menu.Accion) {
      case "create":

        // Find the ones that don't match
        var exceptEntities = sys_menu.Entities!
          .Select(e => e.RoleId).Except(listEntity.Select(l => l.RoleId)).ToList();

        if (exceptEntities.Count > 0) {
          // Missing entities to complete
          var missEntity = sys_menu.Entities!
                .Where(e => exceptEntities.Any(a => a == e.RoleId)).ToList();

          missEntity.ForEach(m => {
            if (m.Required.HasValue) {
              if (response.AllFieldsComplete) {
                response.Messages.Add("Por favor ingresar los siguientes datos: \n");
              }
              if (m.RoleId == null) {
                // Revisar por los roles...
              }
              else
                response.Messages.Add(m.Message!);
              response.AllFieldsComplete = false;
            }
          });
        }
        break;
      case "update":
        break;
      case "read":

        // find for entities.
        // UNDONE: how detect if conditions is "or" or "and"?

        // Get entities to use.
        var entityIds = listEntity.Select(entSel => entSel.RoleId).ToList();

        if (entityIds.Count > 0) {
          response.ReadDatas = new();
          response.ReadDatas.AddRange(entityIds.Select(id => {
            var sysEntity = sys_menu.Entities?.FirstOrDefault(e => e.RoleId == id);
            var witEntity = listEntity.FirstOrDefault(l => l.RoleId == id);
            SearchJson searchJson = new() { aggrega = "$match" };
            if (sysEntity != null && !string.IsNullOrEmpty(sysEntity.Search)) {
              searchJson = JsonConvert.DeserializeObject<SearchJson>(sysEntity.Search)!;
            }
            ReadData readData = new() {
              Command = searchJson.aggrega,
              FullPath = $"entities.{witEntity!.Name}:{witEntity!.Role}.body",
              ExtraCmd = searchJson.extra ?? null,
              CondExtra = searchJson.cond ?? null,
              Value = witEntity.Value!
            };
            return readData;
          }));
        }

        break;
    }

    return response;
  }
}

