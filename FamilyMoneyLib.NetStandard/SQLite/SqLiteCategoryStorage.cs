using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    public class SqLiteCategoryStorage : CategoryStorageBase, ICategoryStorage
    {
        /*
         Account Table Structure

            id INTEGER PRIMARY KEY,
            name TEXT NOT NULL, 
            description TEXT NOT NULL 
         */


        // Exception Microsoft.Data.Sqlite.SqliteException

        private const string CategoryTableStructure = "id INTEGER PRIMARY KEY," +
                                                      "name TEXT NOT NULL, " +
                                                      "description TEXT NOT NULL";

        private readonly SqLiteTable _table = new SqLiteTable("familyMoney.db", "Category",
            $"({CategoryTableStructure})");

        public SqLiteCategoryStorage(ICategoryFactory categoryFactory) : base(categoryFactory)
        {
        }

        public override ICategory CreateCategory(ICategory category)
        {
            _table.InitializeDatabase();
            category.Id = _table.AddData(ObjectToICategoryConverter.ConvertForInsertString(category));
            return category;
        }

        public override void DeleteCategory(ICategory category)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(category.Id);
        }

        public override IEnumerable<ICategory> GetAllCategories()
        {
            _table.InitializeDatabase();
            var lines = _table.SelectAll();

            return lines.Select(objects => ObjectToICategoryConverter.Convert(objects,CategoryFactory)).ToList();
        }

        public override void UpdateCategory(ICategory category)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToICategoryConverter.ConvertForUpdateString(category), category.Id);

        }

        public void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
        }
    }

    public static class ObjectToICategoryConverter
    {
        public static ICategory Convert(object[] line, ICategoryFactory categoryFactory)
        {
            var account = categoryFactory.CreateCategory(line[1].ToString(), line[2].ToString());
            account.Id = (long)line[0];

            return account;
        }

        public static List<KeyValuePair<string, object>> ConvertForUpdateString(ICategory category)
        {
            var returnList = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", category.Name),
                new KeyValuePair<string, object>("description", category.Description),
            };
            return returnList;
        }

        public static List<KeyValuePair<string, object>> ConvertForInsertString(ICategory category)
        {
            var returnList = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", category.Name),
                new KeyValuePair<string, object>("description", category.Description),
            };
            return returnList;
        }

    }
}
