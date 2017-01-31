using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {

        String[] configFile;
        Dictionary<string, string> dict = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();

            LoadStuff();

        }

        private void ResetTextBox()
        {
            this.textBox2.ResetText();
        }

        private void LoadStuff()
        {
            textBox2.Text = "Loading...";
            listBox2.ClearSelected();
            dict.Clear();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                configFile = System.IO.File.ReadAllLines(@"./mednafen-09x.cfg");
                foreach (String element in configFile)
                {
                    if (!element.StartsWith(";"))
                    {
                        //a comment

                        String[] splitElement = element.Split(' ');

                        if (splitElement.Length == 2)
                        {
                            try
                            {
                                Console.Write(splitElement[0] + " :: " + splitElement[1]);
                                dict.Add(splitElement[0], splitElement[1]);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        

                    }
                    
                }
                this.ResetTextBox();
            }).Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            string va = listBox2.GetItemText(listBox2.SelectedItem);


            if (dict.ContainsKey(va))
            {
                textBox3.Text = dict[va];
                // "tester" key exists and contains "testing" value
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox2.Items.AddRange(dict.Keys.Where(X => X.Contains(textBox2.Text)).ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t = 0;
            foreach (String s in configFile) {
                t++;
                if (s.StartsWith(listBox2.GetItemText(listBox2.SelectedItem))) {
                    break;
                }
            }
            using (StreamWriter writer = new StreamWriter(@"./mednafen-09x.cfg.backup"))
            {
                foreach (String s in configFile) {
                    writer.Write(s + "\r\n");
                }
            }
            configFile[t-1] = listBox2.GetItemText(listBox2.SelectedItem) + " " + textBox3.Text;
            using (StreamWriter writer = new StreamWriter(@"./mednafen-09x.cfg"))
            {
                foreach (String s in configFile)
                {
                    writer.Write(s + "\r\n");
                }
            }

            LoadStuff();

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals(Keys.Enter))
                button2.PerformClick();
        }
    }
}
