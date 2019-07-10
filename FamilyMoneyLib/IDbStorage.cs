using System.Collections.Generic;

namespace FamilyMoneyLib
{
    public interface IDbStorage<T> where T : IIdBased, new()
    {
        long Add(T t);
        void Delete(long id);
        T Get(long id);
        IEnumerable<T> GetAll();
        void Update(T t);
    }
}