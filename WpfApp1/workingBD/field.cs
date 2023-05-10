using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workingBD
{
    public class field
    {
        public int id { get; set; }
        public string title { get; set; }
        public string values { get; set; }

        public field(int id, string title, string values)
        {
            this.id = id;
            this.title = title;
            this.values = values;
        }
    }
}
