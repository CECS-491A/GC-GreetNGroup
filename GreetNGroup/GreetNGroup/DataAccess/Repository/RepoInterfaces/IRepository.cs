using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GreetNGroup.DataAccess.Repository.RepoInterfaces
{
    /*
     * This will be following the generic Repository Pattern
     * https://deviq.com/repository-pattern/
     *
     * Creates a generic interface of type T where T is a class
     */
    public interface IRepository<T> where T : class
    {
        // Finds via id -- returns T
        T Get(int id);
        // Return all available
        IEnumerable<T> GetAll();
        // Finds via an expression of func -- so we may filter our search via lambda statements
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        // Adds an entity
        void Add(T entity);

        // Deletes an entity
        void Delete(T entity);
    }
}
