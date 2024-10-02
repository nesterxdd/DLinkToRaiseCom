using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLinkToRaiseCom
{
    static public class TaskUtils
    {
        public static void DeletePorts(ref List<Tag> tags, List<Tag> deletedtags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                int count = 0;
                for(int j = 0; j < tags[i].ports.Count; j++)
                {
                    count += tags[i].ports[j].Item1.Count;
                }
                if (count == 1 && Convert.ToInt32(tags[i].tag) > 513)
                {
                    Tag temp = new Tag(tags[i].ports, tags[i].tag, tags[i].name);
                    deletedtags.Add(temp);
                    tags.Remove(tags[i]);
                    i--;
                }


            }
        }

        public static void GetAllPorts(List<Tag> tags, List<int> ports)
        {
            for (int i = 0; i < tags.Count; i++)
            {

                for (int j = 0; j < tags[i].ports.Count; j++)
                {
                    for(int k = 0; k < tags[i].ports[j].Item1.Count; k++)
                    {
                        if (!ports.Contains(tags[i].ports[j].Item1[k]))
                        {
                            ports.Add(tags[i].ports[j].Item1[k]);
                        }
                    }
                    
                }


            }
            ports.Sort();
        }
    }
}
