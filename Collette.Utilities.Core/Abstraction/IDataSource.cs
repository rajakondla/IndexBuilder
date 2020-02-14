using System;
using System.Collections.Generic;
using System.Text;

namespace Collette.Utilities.Core.Abstraction
{
    public interface IDataSource
    {
        string DBSource(string connectionString,string market);
        string APISource(string url, string market);
    }
}
