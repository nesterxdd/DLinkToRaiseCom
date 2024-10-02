using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Tracing;
using System.Windows.Forms;

namespace DLinkToRaiseCom
{
    public class IOUtils
    {
        public static void Read(string file, List<Tag> tags)
        {

            string line = "";

            using (StreamReader reader = new StreamReader(file))
            {


                while (!(line = reader.ReadLine()).Replace(" ", "").Equals("#VLAN", StringComparison.OrdinalIgnoreCase))
                {
                    if (line == null)
                    {
                        return;
                    }
                }
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("#"))
                    {
                        break;
                    }
                    string[] split = line.Split(' ');


                    if (line.Contains("default") ||
                        (split.Length > 6 && (!line.Contains("tagged") && !line.Contains("untagged")) ||
                        split.Length < 5) && (!line.Contains("tagged") && !line.Contains("untagged")))
                    {
                        continue;
                    }

                    if (split[0].Equals("create", StringComparison.OrdinalIgnoreCase))
                    {
                        Tag temp = new Tag();
                        temp.name = split[2];
                        temp.tag = split[4];
                        tags.Add(temp);
                    }
                    if (split[0].Equals("config", StringComparison.OrdinalIgnoreCase) && (line.Contains("tagged") || line.Contains("untagged")))
                    {

                        for (int i = 0; i < tags.Count; i++)
                        {
                            if (tags[i].name.Equals(split[2]))
                            {
                                List<int> ports = Ports(split);
                                string type = split[4];
                                Tuple<List<int>, string> temp = new Tuple<List<int>, string>(ports, type);
                                tags[i].ports.Add(temp);
                                break;
                            }
                        }

                    }
                }

            }

        }

        private static List<int> Ports(string[] split)
        {
            List<int> specefiedPorts = new List<int>();

            string[] tempPorts = split[5].Split(',');

            for (int i = 0; i < tempPorts.Length; i++)
            {
                if (tempPorts[i].Contains('-'))
                {
                    string[] range = tempPorts[i].Split('-');

                    for (int j = Convert.ToInt32(range[0]); j <= Convert.ToInt32(range[1]); j++)
                    {
                        specefiedPorts.Add(j);
                    }
                }
                else
                {
                    specefiedPorts.Add(Convert.ToInt32(tempPorts[i]));
                }

            }
            return specefiedPorts;
        }
    }
}
