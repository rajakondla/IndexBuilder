using Microsoft.Extensions.Configuration;
using Collette.Utilities;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Collette.Utilities.Core;

namespace Collette.Index
{
    public sealed class ADI : BaseIndex
    {
        private IADIRepository _dataSource;
        private IComparer _compare;

        public ADI(IConfiguration config, IADIRepository dataSource, IComparer compare) : base(config)
        {
            _dataSource = dataSource ?? new ADIRepository();
            _compare = compare ?? new Comparer();
        }

        protected override JObject Compare(string market)
        {
            string apiFileName = "ADI_API_Collette";
            string dbFileName = "ADI_DB_Collette";

            //await foreach(var get in _compare.Compare(base.Data))
            //{

            //}
            //base.Data.CompareTo("");

            _compare.CompareFileWithString("", null);
            return null;
        }

        protected override IndexSource ReadConfiguration()
        {
            return base.Configuration.GetSection("Indexes:ADI").Get<IndexSource>();
        }

        protected override JObject ReadData(string market)
        {
            List<string> results = new List<string>();
            //var type = base.IndexConfiguration.DataSources.Where(x => x.Type == "API").SingleOrDefault();

            //HttpClient client = new HttpClient();
            //var result = client.GetRequestAsync<string>(type.Location);
            //json += result;
            JObject data = null;

            foreach (var getSource in base.IndexConfiguration.DataSources)
            {
                switch (getSource.Type)
                {
                    case "DB":
                        {
                           // _dataSource.DBSource("", market);
                            continue;
                        }
                    case "API":
                        {
                            _dataSource.APISource("", market);
                            continue;
                        }
                }
            }
            // convert results to jobject
            return data;
        }

        protected override IIndexModel BuildModel(JObject jObj)
        {
            throw new System.NotImplementedException();
        }

        protected override JObject QueueObject
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}
