using System;
using RestSharp;
using SellAI.Models.AI;

namespace SellAI.Interfaces {
  public interface IRestApi {
    /// <summary>
    /// Get intent and entities from message
    /// </summary>
    /// <param name="message">text to send</param>
    /// <returns>Model Message</returns>
    Task<Message> MessageAsync(string message);
    /// <summary>
    /// Create a new synonym for a keywords entity
    /// </summary>
    /// <param name="body">{"synonym":"Camembert city"}</param>
    /// <param name="entity">name of entity</param>
    /// <param name="keyword">name of keyword</param>
    /// <returns></returns>
    Task<string> CallPostEntityAsync(string body, string entity, string keyword);
    /// <summary>
    /// Add new values to a keywords entity (This only applies to keywords entities.)
    /// </summary>
    /// <param name="body">{"keyword": "Brussels", "synonyms": ["Brussels", "Capital of Europe"]}</param>
    /// <param name="entity">name of entity</param>
    /// <returns>json with id, name, roles, lookups and keywords with synonyms</returns>
    Task<string> CallPostEntityAsync(string body, string entity);
    /// <summary>
    /// Create a new entity
    /// </summary>
    /// <param name="body">{"name":"favorite_city","roles":[]}</param>
    /// <returns>json with id, roles, lookups and keywords</returns>
    Task<string> CallPostEntityAsync(string body);
    /// <summary>
    /// Remove a synonym from an entity
    /// </summary>
    /// <param name="entity">name of entity</param>
    /// <param name="keyword">name of keyword</param>
    /// <param name="synonym">name of synonyms to delete (ex: 'Camembert city')</param>
    /// <returns>{"deleted": "Camembert city"}</returns>
    Task<string> CallDeleteEntityAsync(string entity, string keyword, string synonym);
    /// <summary>
    /// Remove a given keyword from an entity
    /// </summary>
    /// <param name="entity">name of entity</param>
    /// <param name="keyword">name of keyword to delete (ex: 'Paris')</param>
    /// <returns>{"deleted": "Paris"}</returns>
    Task<string> CallDeleteEntityAsync(string entity, string keyword);
    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="entity">name of entity to delete (ex: 'favorite_city')</param>
    /// <returns>{"deleted": "favorite_city"}</returns>
    Task<string> CallDeleteEntityAsync(string entity);
  }
}

