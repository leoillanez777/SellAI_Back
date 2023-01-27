using System;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.AI;

namespace SellAI.Services
{
  public class InterpreterService : IInterpreter {

    private readonly IMongoClient _client;
    private readonly IMongoCollection<Sys_Menu> _db;
    private readonly IRestApi _restApi;
    private readonly IClaim _claim;

    public InterpreterService(IMongoClient client, IOptions<ContextMongoDB> options, IRestApi restApi, IClaim claim)
    {
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Sys_Menu>(options.Value.SysMenuCollectionName);
      _restApi = restApi;
      _claim = claim;
    }


    /// <summary>
    /// Método para responder pero sin contexto todavía.
    /// </summary>
    /// <param name="message">texto del usuario</param>
    /// <returns>Respuesta al usuario, con contexto creado.</returns>
    public async Task<string> SendMessageAsync(string message, ClaimsIdentity identity, string id = "")
    {


      // Send message and return intents.
      Message response = await _restApi.MessageAsync(message);

      //Debo generar el tipo de respuesta según los intents.

      // Traer de sys_menu cuales son los entities que falta.
      // guardar de forma temporal el proceso en base de dato.
      // volver a preguntar por la intención desde la base de datos.

      // Get App & Roles from user.
      var rolesApp = _claim.GetRoleAndApp(identity);
      foreach (var intent in response.Intents) {
        var sys_menu = await _db.FindAsync(f => f.Nombre == intent.Name).Result.FirstOrDefaultAsync();

        switch (intent.Name.ToLower()) {
          case "crear_producto":
            break;
          case "crear_persona":
            // Debo buscar en la base de datos y que devuelva a que collection grabar.
            // Que datos debo pedir: ejemplo cliente, productos, etc. (buscar entities)
            // establecer conversación para que me devuelva los datos que necesitos.
            // los entity buscar en mongodb o wit.ai?
            // una vez completados todos los datos grabar en collection.
            // Indicar si se genera pdf o no, desde la collection menu.
            break;
          case "crear_presupuesto":
            break;
        }
      }


      return "Respuesta en formato json.";
    }


    /// <summary>
    /// Método para responder en un contexto.
    /// Ya que faltaba algunos datos para completar el comando.
    /// </summary>
    /// <param name="message">texto del usuario</param>
    /// <param name="token">Id del contexto</param>
    /// <returns>Devuelve respuesta al usuario.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<string> SendResponseAsync(string message, string token)
    {
      throw new NotImplementedException();
    }
  }
}

