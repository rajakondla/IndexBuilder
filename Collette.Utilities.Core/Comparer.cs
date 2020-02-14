using Collette.Utilities.Core.Abstraction;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collette.Utilities
{
    public class Comparer:ICompare
    {
       public async Task<string> CompareFileWithString(string filePath,JObject data)
        {
            return "";
        }

    }
}
