using Microsoft.Extensions.Configuration;
using Collette.Utilities;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Collette.Utilities.Core;
using System.Dynamic;
using System.ComponentModel;
using System;
using System.IO;
using ObjectsComparer;

namespace Collette.Index
{
    public sealed class ADI : BaseIndex
    {
        private IADIRepository _dataSource;
        //private System.Collections.IComparer _compare;

        public ADI(IConfiguration config, IADIRepository dataSource ) : base(config) //System.Collections.IComparer compare
        {
            _dataSource = dataSource;
            //_compare = compare;
        }

        protected override JArray Compare(IDictionary<string,string> readResultData)
        {
            JArray changedList = null;
            string text = "[";
            JArray change = null;
            foreach (KeyValuePair<string, string> item in readResultData)
            {
                string path = @"E:\Projects\Collette Index\ColletteFiles\";
                string fileName = item.Key;
                string filePath = path + fileName + ".json";
                string fileContent = File.ReadAllText(filePath);

                if (fileName.Contains("API"))
                {
                    text = text + " {\"key\": \""+fileName+"\",\"data\":";

                    string apiJsonText = "[";


                    var oldData = Newtonsoft.Json.JsonConvert.DeserializeObject<ShopTourDateItem[]>(fileContent);
                    var newData = Newtonsoft.Json.JsonConvert.DeserializeObject<ShopTourDateItem[]>(item.Value);

                    var oldDataObj = oldData.ToList().OrderBy(x => x.TourId).ThenBy(x => x.PackageId).ThenBy(x => x.PackageDateId).ToList();

                    var newDataObj = newData.ToList().OrderBy(x => x.TourId).ThenBy(x => x.PackageId).ThenBy(x => x.PackageDateId).ToList();

                    apiJsonText = apiJsonText + APIComparer(oldDataObj, newDataObj);
                    apiJsonText += "]";
                    text = text + apiJsonText + "}";
                    File.WriteAllText(filePath, item.Value);
                    //update file in the end
                }

                if (fileName.Contains("DB"))
                {
                    text = text + " {\"key\": \"" + fileName + "\",\"data\":";
                    string dbJsonText = "[";


                    foreach (JToken dbJToken in JsonConvert.DeserializeObject<JArray>(fileContent))
                    {
                        foreach (JProperty dbItem in dbJToken)
                        {
                            var dbItemCounter = 0;
                            foreach (var compareDbJToken in JsonConvert.DeserializeObject<JArray>(item.Value))
                            {
                                foreach (var compareDbItem in compareDbJToken)
                                {
                                    //New table added is yet to be done
                                     if (dbItem.Name == ((JProperty)compareDbItem).Name)
                                     {
                                         var oldData = Newtonsoft.Json.JsonConvert.DeserializeObject<ShopTabData[]>(dbItem.First.ToString());
                                         var newData = Newtonsoft.Json.JsonConvert.DeserializeObject<ShopTabData[]>(compareDbItem.First.ToString());

                                         var oldDataObj = oldData.ToList().OrderBy(x => x.Name).ToList();
                                         var newDataObj = newData.ToList().OrderBy(x => x.Name).ToList();

                                         var dbComparerResult = DBComparer(oldDataObj, newDataObj);
                                         if (dbComparerResult != "")
                                         {
                                             AddComma(dbJsonText);
                                             dbJsonText += "{\"" + ((JProperty)compareDbItem).Name + "\":[" + dbComparerResult + "]}";
                                         }
                                         dbItemCounter++;
                                         continue;
                                     }
                                }
                            }
                            if(dbItemCounter == 0)
                            {
                                //delete operation of table
                                dbJsonText = AddComma(dbJsonText);
                                dbJsonText = dbJsonText + "{\"" + dbItem.Name + "\":"+"[]}";
                            }

                        }
                        dbJsonText = AddComma(dbJsonText);
                    }
                    RemoveComma(dbJsonText);
                    dbJsonText += "]";
                    text += dbJsonText + "}";
                    File.WriteAllText(filePath, item.Value);
                }
                text = AddComma(text);

            }
            text = RemoveComma(text);
            text += "]";
            change = JArray.Parse(text);
            //_compare.CompareFileWithString(filePath, listObject);


            return changedList;
        }

        protected override IndexSource ReadConfiguration()
        {
            return base.Configuration.GetSection("Indexes:ADI").Get<IndexSource>();
        }

        protected override IDictionary<string,string> ReadData(string market)
        {
            IDictionary<string, string> readDataResult = new Dictionary<string, string>();
            string resultData = "";
            string key = "";

            foreach (var getSource in base.IndexConfiguration.DataSources)
            {
                switch (getSource.Type)
                {
                    case "DB":
                        {
                            resultData = _dataSource.GetWeatherData(getSource.Location, market);
                            key = "ADI_" + getSource.Type + "_" + market;
                            readDataResult.Add(key, resultData);
                            continue;
                        }
                    case "API":
                        {
                            resultData = _dataSource.GetAPIData(getSource.Location, market);
                            key = "ADI_" + getSource.Type + "_" + market;
                            readDataResult.Add(key, resultData);
                            continue;
                        }
                }
            }

            return readDataResult;
        }

        protected override IIndexModel BuildModel(JArray jObj)
        {
            throw new System.NotImplementedException();

            //JObject.Parse()
        }

        public class ShopTourDateItem
        {
            public ShopTourDateItem()
            { }

            public int PackageId { get; set; }
            public int PackageDateId { get; set; }
            public int TourId { get; set; }
            public DateTime TourDate { get; set; }
            public DateTime DepartureDate { get; set; }
            public int AllotmentAvailable { get; set; }
            public int SaleStatusId { get; set; }
            public double SinglePrice { get; set; }
            public double DoublePrice { get; set; }
            public double PriceInterAir { get; set; }
            public double PriceAir { get; set; }
            public int PointsOverrideSingle { get; set; }
            public int PointsOverrideDouble { get; set; }
            public double PriceAdditionalSupplements { get; set; }
            public double PriceGround { get; set; }
            public int SalesCurrencyId { get; set; }
        }

        public class ShopTabData
        {
            public string Name { get; set; }

            public string Scope { get; set; }

            public string DepartmentId { get; set; }

            public string Requester { get; set; }

        }

        private string APIComparer(List<ShopTourDateItem> oldDataObj, List<ShopTourDateItem> newDataObj)
        {
            int counter1 = 0, counter2 = 0;
            var key1 = ""; var key2 = "";
            string text = "";
            var comparer = new ObjectsComparer.Comparer();
            while (counter1 < oldDataObj.Count() && counter2 < newDataObj.Count())
            {
                var objI1 = oldDataObj[counter1];
                key1 = $"{objI1.TourId}|{objI1.PackageId}|{objI1.PackageDateId}";
                var objI2 = newDataObj[counter2];
                key2 = $"{objI2.TourId}|{objI2.PackageId}|{objI2.PackageDateId}";

                IEnumerable<Difference> differences;

                var res = key1.CompareTo(key2);

                if (res < 0) // key1 is before key2 ...key1 can be incremented
                {
                    //deleted records
                    var deletedRecord = oldDataObj[counter1];
                    key1 = $"{deletedRecord.TourId}|{deletedRecord.PackageId}|{deletedRecord.PackageDateId}";
                    text = text + "{\"PrimaryKey\":\"" + key1;
                    text = text + "\",\"Operation\" : \"Delete\"},";
                    counter1++;
                    continue;
                }
                else if (res > 0) // key2 is before key1 ...key2 can be incremented
                {
                    //new records
                    var newRecord = newDataObj[counter2];
                    key2 = $"{newRecord.TourId}|{newRecord.PackageId}|{newRecord.PackageDateId}";
                    text = text + "{\"PrimaryKey\":\"" + key2 + "\"," + "\"TourDate\":\"" + newRecord.TourDate + "\"," + "\"DepartureDate\":\"" + newRecord.DepartureDate + "\"," + "\"AllotmentAvailable\":\"" + newRecord.AllotmentAvailable + "\"," + "\"SaleStatusId\":\"" + newRecord.SaleStatusId + "\"," + "\"SinglePrice\":\"" + newRecord.SinglePrice + "\"," + "\"DoublePrice\":\"" + newRecord.DoublePrice + "\"," + "\"PriceInterAir\":\"" + newRecord.PriceInterAir + "\"," + "\"PriceAir\":\"" + newRecord.PriceAir + "\"," + "\"PointsOverrideSingle\":\"" + newRecord.PointsOverrideSingle + "\"," + "\"PointsOverrideDouble\":\"" + newRecord.PointsOverrideDouble + "\"," + "\"PriceAdditionalSupplements\":\"" + newRecord.PriceAdditionalSupplements + "\"," + "\"PriceGround\":\"" + newRecord.PriceGround + "\"," + "\"SalesCurrencyId\":\"" + newRecord.SalesCurrencyId;
                    text = text + "\",\"Operation\" : \"Insert\"},";
                    counter2++;
                    continue;
                }
                else
                {
                    var isEqual = comparer.Compare(oldDataObj[counter1], newDataObj[counter2], out differences);

                    if (!isEqual)
                    {
                        int count = differences.Count();
                        int counter = 1;
                        text = text + "{\"PrimaryKey\":\"" + key1 + "\",";
                        foreach (var diff in differences)
                        {
                            text = text + "\"" + diff.MemberPath + "\" :" + "\"" + diff.Value2 + "\"" + ((counter != count) ? "," : ",\"Operation\" : \"Update\"},");
                            counter = counter + 1;
                        }
                    }
                    counter1++;
                    counter2++;
                }
            }
            while (counter1 < oldDataObj.Count())
            {
                var obj = oldDataObj[counter1];
                key1 = $"{obj.TourId}|{obj.PackageId}|{obj.PackageDateId}";
                text = text + "{\"PrimaryKey\":\"" + key1;
                text = text + "\",\"Operation\" : \"Delete\"},";
                counter1++;
            }
            while (counter2 < newDataObj.Count())
            {
                var obj = newDataObj[counter2];
                key2 = $"{obj.TourId}|{obj.PackageId}|{obj.PackageDateId}";
                text = text + "{\"PrimaryKey\":\"" + key2 + "\"," + "\"TourDate\":\"" + obj.TourDate + "\"," + "\"DepartureDate\":\"" + obj.DepartureDate + "\"," + "\"AllotmentAvailable\":\"" + obj.AllotmentAvailable + "\"," + "\"SaleStatusId\":\"" + obj.SaleStatusId + "\"," + "\"SinglePrice\":\"" + obj.SinglePrice + "\"," + "\"DoublePrice\":\"" + obj.DoublePrice + "\"," + "\"PriceInterAir\":\"" + obj.PriceInterAir + "\"," + "\"PriceAir\":\"" + obj.PriceAir + "\"," + "\"PointsOverrideSingle\":\"" + obj.PointsOverrideSingle + "\"," + "\"PointsOverrideDouble\":\"" + obj.PointsOverrideDouble + "\"," + "\"PriceAdditionalSupplements\":\"" + obj.PriceAdditionalSupplements + "\"," + "\"PriceGround\":\"" + obj.PriceGround + "\"," + "\"SalesCurrencyId\":\"" + obj.SalesCurrencyId;
                text = text + "\",\"Operation\" : \"Insert\"},";
                counter2++;
            }

            return (text == "") ? text : text.Remove(text.Length - 1, 1);
        }

        private string DBComparer(List<ShopTabData> oldDataObj, List<ShopTabData> newDataObj)
        {

            int counter1 = 0, counter2 = 0;
            var key1 = ""; var key2 = "";
            string text = "";
            var comparer = new ObjectsComparer.Comparer();
            while (counter1 < oldDataObj.Count() && counter2 < newDataObj.Count())
            {
                var objI1 = oldDataObj[counter1];
                key1 = $"{objI1.Name}";
                var objI2 = newDataObj[counter2];
                key2 = $"{objI2.Name}";

                IEnumerable<Difference> differences;

                var res = key1.CompareTo(key2);

                if (res < 0) // key1 is before key2 ...key1 can be incremented
                {
                    //deleted records
                    var deletedRecord = oldDataObj[counter1];
                    key1 = $"{deletedRecord.Name}";
                    text = text + "{\"Name\":\"" + key1;
                    text = text + "\",\"Operation\" : \"Delete\"},";
                    counter1++;
                    continue;
                }
                else if (res > 0) // key2 is before key1 ...key2 can be incremented
                {
                    //new records
                    var newRecord = newDataObj[counter2];
                    key2 = $"{newRecord.Name}";
                    text = text + "{\"Name\":\"" + key2 + "\"," + "\"Requester\":\"" + newRecord.Requester ;// + "\"DepartureDate\":\"" + newRecord.DepartureDate + "\"," + "\"AllotmentAvailable\":\"" + newRecord.AllotmentAvailable + "\"," + "\"SaleStatusId\":\"" + newRecord.SaleStatusId + "\"," + "\"SinglePrice\":\"" + newRecord.SinglePrice + "\"," + "\"DoublePrice\":\"" + newRecord.DoublePrice + "\"," + "\"PriceInterAir\":\"" + newRecord.PriceInterAir + "\"," + "\"PriceAir\":\"" + newRecord.PriceAir + "\"," + "\"PointsOverrideSingle\":\"" + newRecord.PointsOverrideSingle + "\"," + "\"PointsOverrideDouble\":\"" + newRecord.PointsOverrideDouble + "\"," + "\"PriceAdditionalSupplements\":\"" + newRecord.PriceAdditionalSupplements + "\"," + "\"PriceGround\":\"" + newRecord.PriceGround + "\"," + "\"SalesCurrencyId\":\"" + newRecord.SalesCurrencyId;
                    text = text + "\",\"Operation\" : \"Insert\"},";
                    counter2++;
                    continue;
                }
                else
                {
                    var isEqual = comparer.Compare(oldDataObj[counter1], newDataObj[counter2], out differences);

                    if (!isEqual)
                    {
                        int count = differences.Count();
                        int counter = 1;
                        text = text + "{\"Name\":\"" + key1 + "\",";
                        foreach (var diff in differences)
                        {
                            text = text + "\"" + diff.MemberPath + "\" :" + "\"" + diff.Value2 + "\"" + ((counter != count) ? "," : ",\"Operation\" : \"Update\"},");
                            counter = counter + 1;
                        }
                    }
                    counter1++;
                    counter2++;
                }
            }
            while (counter1 < oldDataObj.Count())
            {
                var obj = oldDataObj[counter1];
                key1 = $"{obj.Name}";
                text = text + "{\"Name\":\"" + key1;
                text = text + "\",\"Operation\" : \"Delete\"},";
                counter1++;
            }
            while (counter2 < newDataObj.Count())
            {
                var obj = newDataObj[counter2];
                key2 = $"{obj.Name}";
                text = text + "{\"Name\":\"" + key2 + "\"," + "\"Requester\":\"" + obj.Requester;// + "\"DepartureDate\":\"" + obj.DepartureDate + "\"," + "\"AllotmentAvailable\":\"" + obj.AllotmentAvailable + "\"," + "\"SaleStatusId\":\"" + obj.SaleStatusId + "\"," + "\"SinglePrice\":\"" + obj.SinglePrice + "\"," + "\"DoublePrice\":\"" + obj.DoublePrice + "\"," + "\"PriceInterAir\":\"" + obj.PriceInterAir + "\"," + "\"PriceAir\":\"" + obj.PriceAir + "\"," + "\"PointsOverrideSingle\":\"" + obj.PointsOverrideSingle + "\"," + "\"PointsOverrideDouble\":\"" + obj.PointsOverrideDouble + "\"," + "\"PriceAdditionalSupplements\":\"" + obj.PriceAdditionalSupplements + "\"," + "\"PriceGround\":\"" + obj.PriceGround + "\"," + "\"SalesCurrencyId\":\"" + obj.SalesCurrencyId;
                text = text + "\",\"Operation\" : \"Insert\"},";
                counter2++;
            }

            return (text=="")?text:text.Remove(text.Length - 1, 1);
        }

        private string AddComma(string text)
        {
            if (text[text.Length - 1] == '}' || text[text.Length - 1] == ']')
                text += ",";
            return text;
        }

        private string RemoveComma(string text)
        {
            if (text[text.Length - 1] == ',')
                text = text.Remove(text.Length - 1, 1);
            return text;
        }
    }
}
