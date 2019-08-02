﻿using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    public class SqLiteBarCodeStorage:IBarCodeStorage
    {
        private const string BarCodeTableStructure = "id INTEGER PRIMARY KEY," +
                                                      "code TEXT NOT NULL, " +
                                                      "isWeight NOT NULL," +
                                                      "numberOfDigits INTEGER, "+
                                                      "transactionId INTEGER";

        private readonly SqLiteTable _table = new SqLiteTable("familyMoney.db", "Barcode",
            $"({BarCodeTableStructure})");

        private readonly IBarCodeFactory _barCodeFactory;
        private readonly ITransactionStorage _transactionStorage;

        public SqLiteBarCodeStorage(IBarCodeFactory factory, ITransactionStorage storage)
        {
            _barCodeFactory = factory;
            _transactionStorage = storage;
        }

        public IBarCode CreateBarCode(IBarCode barCode)
        {
            _table.InitializeDatabase();
            var id = _table.AddData(ObjectToIBarCodeConvertor.ConvertToKeyValuePair(barCode));
            barCode.Id = id;
            return barCode;
        }

        public void DeleteBarCode(IBarCode barCode)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(barCode.Id);
        }

        public IEnumerable<IBarCode> GetAllBarCodes()
        {
            _table.InitializeDatabase();
            var barCodes = _table.SelectAll().ToArray();
            return barCodes.Select(objects => ObjectToIBarCodeConvertor.Convert(objects, _barCodeFactory, _transactionStorage)).ToList();
        }

        public ITransaction GetBarCodeTransaction(string barCode)
        {
            var foundBarCodes = GetAllBarCodes().FirstOrDefault(x => x.GetProductBarCode().Equals(barCode));
            return foundBarCodes?.Transaction;
        }

        public void UpdateBarCode(IBarCode barCode)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToIBarCodeConvertor.ConvertToKeyValuePair(barCode),barCode.Id);
        }

        public void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
        }
    }

    public static class ObjectToIBarCodeConvertor
    {
        public static IEnumerable<KeyValuePair<string, object>> ConvertToKeyValuePair(IBarCode barCode)
        {
            var returnList = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("transactionId", barCode.Transaction?.Id),
                new KeyValuePair<string, object>("code", barCode.Code),
                new KeyValuePair<string, object>("isWeight", barCode.IsWeight?1:0),
                new KeyValuePair<string, object>("numberOfDigits", barCode.NumberOfDigitsForWeight),
            };
            return returnList;
        }

        public static IBarCode Convert(IDictionary<string, object> line, IBarCodeFactory barCodeFactory,
            ITransactionStorage transactionStorage)
        {
            var code = line["code"].ToString();
            var isWeight = ((long)line["isWeight"] == 0);
            var numberOfDigits = System.Convert.ToInt32( (long)line["numberOfDigits"]);
            var transactionId = (line["transactionId"] is System.DBNull) ? 0 : (long)line["transactionId"]; 

            var barCode = barCodeFactory.CreateBarCode(code, isWeight, numberOfDigits);

            if(transactionId!=0)
                barCode.Transaction = transactionStorage.GetAllTransactions().FirstOrDefault(x=>x.Id == transactionId);
            barCode.Id = (long)line["id"];

            return barCode;
        }
    }
}