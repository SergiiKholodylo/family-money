﻿using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    public class SqLiteCategoryStorage : ICategoryStorage
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

        public ICategory CreateCategory(ICategory category)
        {
            _table.InitializeDatabase();
            category.Id = _table.AddData(ObjectToICategoryConverter.ConvertForInsertString(category));
            return category;

        }

        public void DeleteCategory(ICategory category)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(category.Id);
        }

        public void UpdateCategory(ICategory category)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToICategoryConverter.ConvertForUpdateString(category), category.Id);

        }

        public IEnumerable<ICategory> GetAllCategories()
        {
            _table.InitializeDatabase();
            var lines = _table.SelectAll();

            return lines.Select(ObjectToICategoryConverter.Convert).ToList();
        }

        public void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
        }
    }

    public static class ObjectToICategoryConverter
    {
        public static ICategory Convert(object[] line)
        {
            var factory = new RegularCategoryFactory();
            var account = factory.CreateCategory(line[1].ToString(), line[2].ToString());
            account.Id = (long)line[0];

            return account;
        }

        public static string ConvertForUpdateString(ICategory account)
        {
            var sqlDataString =
                $"name = '{account.Name}', description = '{account.Description}'";
            return sqlDataString;
        }

        public static string ConvertForInsertString(ICategory account)
        {
            var sqlDataString =
                $"NULL,'{account.Name}', '{account.Description}'";
            return sqlDataString;
        }
    }
}
