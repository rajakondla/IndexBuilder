
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collette.Index
{
    public abstract class BaseIndex
    {
        public BaseIndex(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; set; }

        //protected string IndexType { get; set; }

        public string[] Markets { get; set; }

        public string[] Fields { get; set; }

        protected IndexSource IndexConfiguration { get; set; }

        protected JObject Data { get; set; }

        protected abstract IndexSource ReadConfiguration();

        protected abstract JObject ReadData(string market);

        protected abstract JObject Compare(string market);

        protected abstract IIndexModel BuildModel(JObject jObj);

        private bool BuildIndex(IIndexModel model)
        {
            // Indexer.UpdateIndex(Type,jobj);
            return true;
        }

        protected abstract JObject QueueObject { get; set; }

        private void AddToQueue(JObject obj)
        {

        }

        public void Process()
        {
            IndexConfiguration = ReadConfiguration();

            var markets = Configuration.GetSection("Markets").Get<string[]>();

            markets = markets.Where(x => Markets == null || (Markets.Contains(x))).Select(x => x).ToArray();

            foreach (var market in markets)
            {
                if (IndexConfiguration.DataSources.Length > 0)
                {
                    Data = ReadData(market);

                    if (Data != null && Data.Count > 0)
                    {
                        var result = Compare(market);

                        if (result != null)
                        {
                            var model = BuildModel(result);
                            if (model != null)
                            {
                                var isIndexBuild = BuildIndex(model);
                                if (isIndexBuild)
                                {
                                    AddToQueue(QueueObject);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
