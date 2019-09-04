using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.Storages.SQLite
{
    public class SqLiteBarCodeStorage:BarCodeStorageBase
    {
        private const string BarCodeTableStructure = "id INTEGER PRIMARY KEY," +
                                                      "code TEXT NOT NULL, " +
                                                      "isWeight NOT NULL," +
                                                      "numberOfDigits INTEGER, "+
                                                      "transactionId INTEGER";

        private readonly SqLiteTable _table = new SqLiteTable("familyMoney.db", "Barcode",
            $"({BarCodeTableStructure})");



        

        public SqLiteBarCodeStorage(IBarCodeFactory factory, ITransactionStorage storage):base(factory, storage)
        {
        }

        public override IBarCode CreateBarCode(IBarCode barCode)
        {
            _table.InitializeDatabase();
            if (barCode.Id == 0)
                barCode.Id = _table.AddData(ObjectToIBarCodeConvertor.ConvertToKeyValuePair(barCode));
            else
                _table.AddData(ObjectToIBarCodeConvertor.ConvertToKeyValuePairWithId(barCode));

            return barCode;
        }

        public override void DeleteBarCode(IBarCode barCode)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(barCode.Id);
            
        }

        public override IEnumerable<IBarCode> GetAllBarCodes()
        {
            _table.InitializeDatabase();
            var barCodes = _table.SelectAll().ToArray();
            return barCodes.Select(objects => ObjectToIBarCodeConvertor.Convert(objects, BarCodeFactory, _transactionStorage)).ToList();
        }



        public override void UpdateBarCode(IBarCode barCode)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToIBarCodeConvertor.ConvertToKeyValuePair(barCode),barCode.Id);
            
        }

        public override void DeleteAllData()
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

        public static IEnumerable<KeyValuePair<string, object>> ConvertToKeyValuePairWithId(IBarCode barCode)
        {
            var returnList = ConvertToKeyValuePair(barCode).ToList();
            returnList.Add(new KeyValuePair<string, object>("id", barCode.Id));
            return returnList;
        }

        public static IBarCode Convert(IDictionary<string, object> line, IBarCodeFactory barCodeFactory,
            ITransactionStorage transactionStorage)
        {
            var code = line["code"].ToString();
            var isWeight = ((long)line["isWeight"] == 1);
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
