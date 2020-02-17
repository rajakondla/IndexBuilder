using System;
using System.Collections.Generic;
using System.Text;

namespace Collette.Index
{
    public interface IADIRepository
    {
        string DBSource(string connectionString, string market);
        string APISource(string url, string market);
    }
}
