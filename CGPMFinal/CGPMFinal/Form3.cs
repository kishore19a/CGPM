using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CGPMFinal
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
                        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            StreamReader read = new StreamReader("E:/_Acad_7th_Sem/ME735 Computer Graphics and Product Modeling/CGPMFinal1/CGPMFinal/CGPMFinal/bin/Debug/example.txt");
            txtDisplay.Text = read.ReadToEnd();
            read.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string initPath = Path.GetTempPath() + @"\FQUL";
                s.InitialDirectory = Path.GetFullPath(initPath);
                s.RestoreDirectory = true;
                File.WriteAllText(saveFileDialog1.FileName, txtDisplay.Text);
                this.Close();
            }
        }
    }
}
