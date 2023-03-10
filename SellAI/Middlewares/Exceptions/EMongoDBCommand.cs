using System;
namespace SellAI.Middlewares.Exceptions;

public class EMongoDBQuery : Exception {
  public EMongoDBQuery() { }

  public EMongoDBQuery(string message) : base(message) { }

  public EMongoDBQuery(string message, Exception innerException) : base(message, innerException) { }
}

