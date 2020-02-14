using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Collette.Utilities
{
    public static class HttpClientExtension
    {
        public static async Task<T> GetRequestAsync<T>(this HttpClient httpClient, string url)
        {
            //httpClient = new HttpClient();
            PollyPolicy<T> polly = new PollyPolicy<T>();

            return await polly.RetryPolicy.ExecuteAsync(async () =>
            {
                var result = await httpClient.GetAsync(url);

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return default(T);
                }

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(resultString);
            });

        }

        public static async Task<string> GetRequestAsync(this HttpClient httpClient, string url)
        {
            //httpClient = new HttpClient();
            PollyPolicy<string> polly = new PollyPolicy<string>();

            return await polly.RetryPolicy.ExecuteAsync(async () =>
            {
                var result = await httpClient.GetAsync(url);

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return default(string);
                }

                var resultString = await result.Content.ReadAsStringAsync();
                return resultString;
            });

        }
    }
}
