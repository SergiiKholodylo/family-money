using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyMoneyLib.UWP.Storages
{
    public class DbStorage<T> : IDbStorage<T> where T : IIdBased, new()
    {
        private readonly List<T> _storage = new List<T>();
        public long Add(T t)
        {
            var id = GetId();
            t.Id = id;
            _storage.Add(t);
            return id;
        }

        public T Get(long id)
        {
            var found = _storage.FirstOrDefault(x => x.Id == id);
            if (found == null)
                return new T();
            return found;

        }

        public void Update(T t)
        {
            Delete(t.Id);
            _storage.Add(t);
        }

        public void Delete(long id)
        {
            var toDelete = Get(id);
            _storage.Remove(toDelete);
        }

        public IEnumerable<T> GetAll()
        {
            return _storage;
        }

        private long GetId()
        {
            var rnd = new Random();
            while (true)
            {
                var newId = (long)rnd.Next(0xFFFF);
                if (_storage.Exists(x => x.Id == newId))
                    continue;
                return newId;
            }
        }
    }
}
