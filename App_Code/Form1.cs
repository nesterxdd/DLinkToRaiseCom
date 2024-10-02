using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLinkToRaiseCom
{
    public partial class Form1 : Form
    {

        public void WritePorts(string file, List<int> list, string header)
        {

            using (StreamWriter writer = new StreamWriter(file, true))
            {
                writer.WriteLine(header + "\n");
                if (list.Count > 0)
                {

                    string temp = list[0].ToString() + " ";
                    writer.Write(temp);
                    for (int i = 1; i < list.Count; i++)
                    {
                        
                        if(temp.Length + ("," + list[i].ToString() + " " ).Length > 330)
                        {
                            temp += "\n," + list[i].ToString() + " ";
                        }
                        else
                        {
                            temp += "," + list[i].ToString() + " ";
                        }
                        
                        
                    }
                    writer.Write(temp);
                    writer.WriteLine("\n");
                }
                else
                {
                    writer.WriteLine("empty");
                }
            }
        }
        public void WritePorts(string file, List<Tag> list, string header)
        {

            using (StreamWriter writer = new StreamWriter(file, true))
            {
                writer.WriteLine(header + "\n");
                if (list.Count > 0)
                {

                   
                    writer.Write(list[0].tag);
                    
                    for (int i = 1; i < list.Count; i++)
                    {

                        writer.Write("," + list[i].tag);
                    }
                    writer.WriteLine("\n");
                }
                else
                {
                    writer.WriteLine("empty");
                }
            }
        }

        public void WriteTags(string file, string header, List<Tag> tags, List<int> ports)
        {
            using (StreamWriter writer = new StreamWriter(file, true))
            {
                writer.WriteLine(header);

                foreach (int port in ports)
                {
                    List<Tag> requiredTags = tags.Where(x => x.ports.Any(a => a.Item1.Contains(port))).ToList();
                    if (requiredTags.Count != 0)
                    {
                        string type = "";

                        List<Tag> tagged = SpecifiedType(requiredTags, port, "tagged");
                        List<Tag> untagged = SpecifiedType(requiredTags, port, "untagged");
                        string outputLine = "";
                        if (tagged.Count != 0)
                        {
                            outputLine = BlockCommand(tagged, port, "tagged");
                            writer.WriteLine(outputLine);
                        }
                        if(untagged.Count != 0)
                        {
                            outputLine = BlockCommand(untagged, port, "untagged");
                            writer.WriteLine(outputLine);
                        }
                        
                    }
                }
            }
        }

        private List<Tag> SpecifiedType(List<Tag> tags, int port, string type)
        {
            List<Tag> result = new List<Tag>();
            foreach (Tag t in tags)
            {
                foreach (Tuple<List<int>, string> ports in t.ports)
                {
                    if (ports.Item1.Contains(port) && ports.Item2 == type)
                    {
                        result.Add(t);
                    }
                }

            }
            return result;
        }

        private string BlockCommand(List<Tag> tags, int port, string type)
        {
            string typeCom = "";
            if (type == "tagged")
            {
                typeCom = "trunk";
            }
            else
            {
                typeCom = "access";
            }
            string switchports = String.Format("interface gigaethernet 1/1/{0}\n", port);
            switchports += String.Format("switchport {0} allowed vlan ", typeCom);
            foreach(Tag tag in tags)
            {
                switchports += tag.tag + ",";
            }
            switchports = switchports.Remove(switchports.Length - 1);
            switchports += "\nexit";
            return switchports;
        }

    }
}
