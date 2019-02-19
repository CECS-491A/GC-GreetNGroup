using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GreetNGroup.DataAccess.Repository.RepoInterfaces;

namespace GreetNGroup.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // Reference to the context we will be using in our specific repository definitions
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context= context;
        }

        /*
         * For reference in the coming functions:
         * Set<T> is used for generality of the set retrieved
         * in the use of the dbcontext in this repository class
         *
         * Returns where pk matches id
         */
        public T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }

        /*
         * Returns a list of data from the context
         */
        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }

        // Finds via filter
        public IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        // Adds entity to set
        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        // Removes entity from set
        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
    }
}