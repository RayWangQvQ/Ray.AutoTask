using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray.AutoTask
{
    public class TaskOptions
    {
        public string Name { get; set; }

        public string Api { get; set; }

        public string Placeholder { get; set; }

        public string BodyJsonTemplate { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}
