using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
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
                                                      "description TEXT NOT NULL," + 
                                                      "parentCategory INTEGER, "+
                                                      "ownerId TEXT NOT NULL, " +
                                                      "baseId TEXT NOT NULL ";

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
            var lines = _table.SelectAll().ToArray();
            var withNoParents = lines.Select(objects => ObjectToICategoryConverter.Convert(objects, CategoryFactory)).ToArray();
            foreach (var line in lines)
            {
                ObjectToICategoryConverter.UpdateParents(line,withNoParents);
            }
            return withNoParents.ToList();
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
        public static ICategory Convert(IDictionary<string, object> line, ICategoryFactory categoryFactory)
        {
            var id = (long)line["id"];
            var name = line["name"].ToString();
            var description = line["description"].ToString();

            var account = categoryFactory.CreateCategory(name,description,id,null);

            return account;
        }

        public static void UpdateParents(IDictionary<string, object> line, ICategory[] withNoParents)
        {
            var id = (long)line["id"];
            var parentId = line["parentCategory"];
            if (parentId is System.DBNull) return;
            var category = withNoParents.FirstOrDefault(x => x.Id == id);
            var parentCategory = withNoParents.FirstOrDefault(x => x.Id == (long) parentId);
            if (category != null) category.Parent = parentCategory;
        }


        public static List<KeyValuePair<string, object>> ConvertForUpdateString(ICategory category)
        {
            var returnList = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", category.Name),
                new KeyValuePair<string, object>("description", category.Description),
                new KeyValuePair<string, object>("parentCategory", category.Parent?.Id),
                new KeyValuePair<string, object>("ownerId", category.OwnerId),
                new KeyValuePair<string, object>("baseId", category.BaseId),
            };
            return returnList;
        }

        public static List<KeyValuePair<string, object>> ConvertForInsertString(ICategory category)
        {
            var returnList = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", category.Name),
                new KeyValuePair<string, object>("description", category.Description),
                new KeyValuePair<string, object>("parentCategory", category.Parent?.Id),
                new KeyValuePair<string, object>("ownerId", category.OwnerId),
                new KeyValuePair<string, object>("baseId", category.BaseId),

            };
            return returnList;
        }

    }
}
