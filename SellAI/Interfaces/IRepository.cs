using System;
using System.Linq.Expressions;

namespace SellAI.Interfaces
{
  public interface IRepository<T>
  {
    IQueryable<T> GetAll();
    Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T obj);
    Task<T> UpdateAsync(T obj);
    Task DeleteAsync(Expression<Func<T, bool>> predicate);
  }
}

