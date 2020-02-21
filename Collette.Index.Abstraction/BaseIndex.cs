
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Linq;
using Collette.Utilities;
using System.Collections.Generic;

namespace Collette.Index
{
    public abstract class BaseIndex
    {
        public BaseIndex(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; set; }

        public string[] Markets { get; set; }

        public string[] Fields { get; set; }

        protected IndexSource IndexConfiguration { get; set; }

        protected IDictionary<string,string> ReadDataResult { get; set; }
        protected abstract IndexSource ReadConfiguration();

        protected abstract IDictionary<string, string> ReadData(string market);
      
        protected abstract JArray Compare(IDictionary<string, string> readResultData);

        protected abstract IIndexModel BuildModel(JArray jObj);

        protected IIndexModel Model { get; set; }

        private bool BuildIndex(IIndexModel model)
        {
            // Indexer.UpdateIndex(Type,jobj);
            return true;
        }

        //protected abstract JObject QueueObject { get; set; }

        private void AddToQueue(JObject obj)
        {
            Queue.Push(obj);
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
                    ReadDataResult = ReadData(market);

                    if (ReadDataResult != null && ReadDataResult.Count > 0)
                    {
                        JArray result = Compare(ReadDataResult);

                        if (result != null)
                        {
                            Model = BuildModel(result);
                            //if (Model != null)
                            //{
                            //    var isIndexBuild = BuildIndex(Model);
                            //    if (isIndexBuild)
                            //    {
                            //        //AddToQueue(QueueObject);
                            //    }
                            //}
                        }
                    }
                }
            }
        }

        
    }
}
