using System;
using System.Collections.Generic;
using System.Text;

namespace Collette.Index
{
    public interface IADIRepository
    {
        string GetWeatherData(string connectionString, string market);
        string GetAPIData(string url, string market);
    }
}
