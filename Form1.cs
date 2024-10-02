using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLinkToRaiseCom
{
    public partial class Form1 : Form
    {
        private string File;
        private string FileResult;
        

        List<Tag> tags = new List<Tag>();

        List<int> ports = new List<int>();

        List<Tag> deleted = new List<Tag>();

        public Form1()
        {
            
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void Run_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open initial data file for first park";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "All FIles|*.*|Text Files|*.txt|Word Documents|*.doc|Cfg files (*.*)|*.cfg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File = openFileDialog1.FileName;
                IOUtils.Read(File, tags);
                TaskUtils.DeletePorts(ref tags, deleted);

                TaskUtils.GetAllPorts(tags, ports);

                saveFileDialog1.Title = "Save your result file";
                saveFileDialog1.FileName = "";
                saveFileDialog1.Filter = "Text Files|*.txt|Word Documents|*.doc";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter wr = new StreamWriter(saveFileDialog1.FileName, false)) { }
                    FileResult = saveFileDialog1.FileName;

                    WritePorts(FileResult, ports, "All ports");
                    WritePorts(FileResult, deleted, "Deleted tags");
                    WritePorts(FileResult, tags, "All vlans");
                    WriteTags(FileResult, "", tags, ports);

                }
            }

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
