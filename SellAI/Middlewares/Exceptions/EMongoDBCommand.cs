using System;
namespace SellAI.Middlewares.Exceptions;

public class EMongoDBCommand : Exception {
  public EMongoDBCommand() { }

  public EMongoDBCommand(string message) : base(message) { }

  public EMongoDBCommand(string message, Exception innerException) : base(message, innerException) { }
}

