using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Collette.Utilities
{
    public class Queue
    {
        public static void Push(JObject jObj)
        {
            //File.AppendAllText(@"C:\MyProject\Notification\Sample.json", jObj.ToString());
            File.AppendAllText(@"E:\Projects\Collette Index\ColletteFiles\sample.json", jObj.ToString());

        }
    }
}
