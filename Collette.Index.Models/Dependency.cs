using System;
using System.Collections.Generic;
using System.Text;

namespace Collette.Index
{
    public class Condition
    {
        public Constraint[] Constraints { get; set; }
    }

    public class Constraint
    {
        public string Market { get; set; }
        public string[] Fields { get; set; }
    }
}
