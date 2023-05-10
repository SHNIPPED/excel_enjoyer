using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workingBD
{
    public class Values
    {
        public string id { get; set; }
        public string values { get; set; }

        public Values(string id, string values)
        {
            this.id = id;
            this.values = values;
        }
    }
}
