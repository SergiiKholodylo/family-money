using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class CategoryStorage
    {
        public IDbStorage<Category> DataBaseConnector = new DbStorage<Category>();

        public long AddCategory(Category category)
        {
            return DataBaseConnector.Add(category);
        }

        public Category GetCategory(long id)
        {
            return DataBaseConnector.Get(id);
        }

        public void UpdateCategory(Category category)
        {
            DataBaseConnector.Update(category);
        }

        public void DeleteCategory(long id)
        {
            DataBaseConnector.Delete(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return DataBaseConnector.GetAll();
        }

        public IEnumerable<Category> GetSubcategories(Category category)
        {
            return DataBaseConnector.GetAll().Where(x => x.ParentCategory == category);
        }
    }
}
