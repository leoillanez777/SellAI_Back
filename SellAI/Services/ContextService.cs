using System;
using SellAI.Models.AI.Objects;

namespace SellAI.Services
{
  public class ContextService
  {
    public ContextService()
    {
    }

    
    public void CreateContext(List<Intents> intents, List<Entities> entities)
    {
      // Crea en la base de datos y verifica si falta alguna entidad.
    }

    public void EditContext()
    {

    }

    private void VerifyEntity()
    {
      //Verifico si existe en la respuesta de wit.ai las entidades que debe llevar la intención.
      //Si esta todas las entidades debe crearse en la colección correspondiente.
      //y posiblemente mostrar el pdf.
    }
  }
}

