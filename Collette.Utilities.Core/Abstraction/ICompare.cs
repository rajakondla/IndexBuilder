using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collette.Utilities.Core.Abstraction
{
    public interface ICompare
    {
        Task<string> CompareFileWithString(string filePath, JObject data);
    }
}
