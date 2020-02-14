using System;
using System.Collections.Generic;


namespace Collette.Index.Service
{
    public class Service
    {
        public static string GetFullPublisherName(string name)
        {
            return "Collette.Index." + name + ", Collette.Index.Publisher";
        }

        public static string GetFullIndexName(string name)
        {
            return "Collette.Index." + name + ", Collette.Index."+ name;
        }
    }
}
