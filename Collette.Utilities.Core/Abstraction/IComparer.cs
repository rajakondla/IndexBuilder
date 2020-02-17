using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collette.Utilities
{
    public interface IComparer
    {
        Task<string> CompareFileWithString(string filePath, JObject data);
    }
}
