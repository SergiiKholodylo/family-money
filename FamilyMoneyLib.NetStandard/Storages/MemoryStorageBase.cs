using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryStorageBase
    {
        private readonly List<IIdAble> _storage = new List<IIdAble>();

        private static long _counter = 0;

        public IIdAble Create(IIdAble idAble)
        {
            if (IsExists(idAble))
            {
                Delete(idAble);
            }
            else
            {
                if (idAble.Id == 0)
                    idAble.Id = ++_counter;
            }
            _storage.Add(idAble);
            return idAble;
        }

        public void Delete(IIdAble idAble)
        {
            var categoryToDelete = _storage.Where(x => x.Id == idAble.Id).ToArray();
            foreach (var category1 in categoryToDelete)
            {
                _storage.Remove(category1);
            }
        }

        public void DeleteAllData()
        {
            _storage.Clear();
        }

        public IEnumerable<IIdAble> GetAll()
        {
            return _storage.ToArray();
        }

        private bool IsExists(IIdAble idAble)
        {
            return (idAble.Id != 0 && _storage.Any(x => x.Id == idAble.Id));
        }
    }
}
