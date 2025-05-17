using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntities;

namespace Data.Repositories
{
    public abstract class InMemoryRepository<T> where T : IdObject
    {
        protected readonly Dictionary<Guid, T> _store = new Dictionary<Guid, T>();

        public virtual void Save(T entity)
        {
            _store[entity.Id] = entity;
        }

        public virtual T Get(Guid id)
        {
            _store.TryGetValue(id, out var entity);
            return entity;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _store.Values.ToList();
        }

        public virtual void Delete(T entity)
        {
            _store.Remove(entity.Id);
        }

        public virtual void DeleteAll()
        {
            _store.Clear();
        }
    }
}
