using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLinkToRaiseCom
{
    public class Tag
    {

        public List<Tuple<List<int>, string>> ports { get; set; }

        public string tag { get; set; }

        public string name { get; set; }

        public Tag()
        { 
            ports = new List<Tuple<List<int>, string>>();
        }

        public Tag( List<Tuple<List<int>, string>> ports, string tag, string name)
        {
            this.ports = ports;
            this.tag = tag;
            this.name = name;
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", tag, ports[0].Item1);
        }
    }
}
