using Collette.Utilities;
using Collette.Utilities.Core.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Collette.Index
{
    public class ADISource : IDataSource
    {
        public string APISource(string url, string market)
        {
            HttpClient client = new HttpClient();
            var result = client.GetRequestAsync(url + "/" + market);

            return JsonConvert.SerializeObject(new { key = "ADI," + market, data = result });
        }

        public string DBSource(string connectionString, string market)
        {
            throw new NotImplementedException();
        }
    }
}
