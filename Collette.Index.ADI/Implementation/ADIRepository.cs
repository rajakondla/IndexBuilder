using Collette.Utilities;
using Collette.Utilities.Core.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Collette.Index
{
    public class ADIRepository : IADIRepository
    {
        public string GetAPIData(string url, string market)
        {
            HttpClient client = new HttpClient();
            var result = client.GetRequestAsync(url + "/" + market);

            //   var text = "{\"result\" : [" + "{ \"packageId\":\"11833\" , \"packageDateId\":\"68827\" , \"tourId\":\"98\", \"tourDate\":\"2020-02-24T00:00:00\" , \"departureDate\":\"2020-02-20T00:00:00\",\"allotmentAvailable\":\"44\",\"saleStatusId\":\"15\",\"singlePrice\":\"5999\",\"doublePrice\":\"3999\",\"priceInterAir\":\"100\",\"pointsOverrideSingle\":\"0\",\"pointsOverrideDouble\":\"0\",\"priceAdditionalSupplements\":\"0\",\"priceGround\":\"0\",\"salesCurrencyId\":\"20\"}," + "{  \"packageId\":\"11199\" , \"packageDateId\":\"63469\" , \"tourId\":\"563\", \"tourDate\":\"2020-02-20T00:00:00\" , \"departureDate\":\"2020-02-19T00:00:00\",\"allotmentAvailable\":\"45\",\"saleStatusId\":\"4\",\"singlePrice\":\"3449\",\"doublePrice\":\"2699\",\"priceInterAir\":\"100\",\"pointsOverrideSingle\":\"0\",\"pointsOverrideDouble\":\"0\",\"priceAdditionalSupplements\":\"0\",\"priceGround\":\"0\",\"salesCurrencyId\":\"20\"}," + "{  \"packageId\":\"11621\" , \"packageDateId\":\"67409\" , \"tourId\":\"532\", \"tourDate\":\"2020-02-20T00:00:00\" , \"departureDate\":\"2020-02-19T00:00:00\",\"allotmentAvailable\":\"42\",\"saleStatusId\":\"15\",\"singlePrice\":\"1999\",\"doublePrice\":\"1399\",\"priceInterAir\":\"100\",\"pointsOverrideSingle\":\"0\",\"pointsOverrideDouble\":\"0\",\"priceAdditionalSupplements\":\"0\",\"priceGround\":\"0\",\"salesCurrencyId\":\"20\"} ]}";
            var text = "[{ \"packageId\":\"11833\" , \"packageDateId\":\"68827\" , \"tourId\":\"98\", \"tourDate\":\"2020-02-24T00:00:00\" , \"departureDate\":\"2020-02-20T00:00:00\",\"allotmentAvailable\":\"44\",\"saleStatusId\":\"15\",\"singlePrice\":\"5999\",\"doublePrice\":\"3999\",\"priceInterAir\":\"100\",\"pointsOverrideSingle\":\"0\",\"pointsOverrideDouble\":\"0\",\"priceAdditionalSupplements\":\"0\",\"priceGround\":\"0\",\"salesCurrencyId\":\"20\"}," + "{  \"packageId\":\"11199\" , \"packageDateId\":\"63469\" , \"tourId\":\"563\", \"tourDate\":\"2020-02-20T00:00:00\" , \"departureDate\":\"2020-02-19T00:00:00\",\"allotmentAvailable\":\"45\",\"saleStatusId\":\"4\",\"singlePrice\":\"3449\",\"doublePrice\":\"2699\",\"priceInterAir\":\"100\",\"pointsOverrideSingle\":\"0\",\"pointsOverrideDouble\":\"0\",\"priceAdditionalSupplements\":\"0\",\"priceGround\":\"0\",\"salesCurrencyId\":\"20\"}," + "{  \"packageId\":\"11621\" , \"packageDateId\":\"67409\" , \"tourId\":\"532\", \"tourDate\":\"2020-02-20T00:00:00\" , \"departureDate\":\"2020-02-19T00:00:00\",\"allotmentAvailable\":\"42\",\"saleStatusId\":\"15\",\"singlePrice\":\"1999\",\"doublePrice\":\"1399\",\"priceInterAir\":\"100\",\"pointsOverrideSingle\":\"0\",\"pointsOverrideDouble\":\"0\",\"priceAdditionalSupplements\":\"0\",\"priceGround\":\"0\",\"salesCurrencyId\":\"20\"}," +
"{  \"packageId\":\"116211\" , \"packageDateId\":\"674091\" , \"tourId\":\"5321\", \"tourDate\":\"2020-02-20T00:00:00\" , \"departureDate\":\"2020-02-19T00:00:00\",\"allotmentAvailable\":\"42\",\"saleStatusId\":\"15\",\"singlePrice\":\"1999\",\"doublePrice\":\"1399\",\"priceInterAir\":\"100\",\"pointsOverrideSingle\":\"0\",\"pointsOverrideDouble\":\"0\",\"priceAdditionalSupplements\":\"0\",\"priceGround\":\"0\",\"salesCurrencyId\":\"20\"}," +
"{  \"packageId\":\"116213\" , \"packageDateId\":\"674093\" , \"tourId\":\"5323\", \"tourDate\":\"2020-02-20T00:00:00\" , \"departureDate\":\"2020-02-19T00:00:00\",\"allotmentAvailable\":\"42\",\"saleStatusId\":\"15\",\"singlePrice\":\"1999\",\"doublePrice\":\"1399\",\"priceInterAir\":\"100\",\"pointsOverrideSingle\":\"0\",\"pointsOverrideDouble\":\"0\",\"priceAdditionalSupplements\":\"0\",\"priceGround\":\"0\",\"salesCurrencyId\":\"20\"} ]";      //    JObject obj = JObject.Parse(text);

            //return JsonConvert.SerializeObject(new { key = "ADI_API_" + market, data = obj });
            return text;
        }

        public string GetWeatherData(string connectionString, string market)
        {

            SqlConnection SQLConnection = new SqlConnection(connectionString);
            SQLConnection.Open();
            SqlCommand command = new SqlCommand("exec getData", SQLConnection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            adapter.SelectCommand = command;
            adapter.Fill(ds);
            adapter.Dispose();
            command.Dispose();
            SQLConnection.Close();

            //return JsonConvert.SerializeObject(new { key = "ADI_DB_" + market, data = ds });
            return "[{\"Table1\":[{\"Name\":\"Index22\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"},{\"Name\":\"Global\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"},{\"Name\":\"Collete\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"},{\"Name\":\"Test\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"}],\"Table123\":[{\"Name\":\"Index123\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"},{\"Name\":\"Global\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"},{\"Name\":\"Collete\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"},{\"Name\":\"Test\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"},{\"Name\":\"Sample\",\"Scope\":\"or\",\"DepartmentId\":12,\"Requester\":\"Samp\"}]}]";
            //return JsonConvert.SerializeObject(ds);

        }

      

    }
}
