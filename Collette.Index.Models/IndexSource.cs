using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Collette.Index
{
    public enum SourceType { DB, API };

    public class IndexSource
    {
        public Source[] DataSources { get; set; }

    }

    public class Source
    {
        public string Type { get; set; }
        public string Location { get; set; }
    }

    
}
