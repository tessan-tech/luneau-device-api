using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceApi
{
    public class Status
    {
        public Status(string name, object payload)
        {
            Name = name;
            Payload = payload;
        }

        public string Name { get; set; }
        public object Payload { get; set; }
    }
}
