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

namespace VerminReader_GUI3
{
    public partial class Settings : Form
    {
        BundleReader thisReader;

        public Settings()
        {
            InitializeComponent();
        }

        public Settings(BundleReader reader)
        {
            InitializeComponent();
            thisReader = reader;

            thisReader.settings["VT1GameDir"] = "none";
            thisReader.settings["VT2GameDir"] = "none";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllLines("settings.ini", thisReader.settings.Select(x => x.Key + "=" + x.Value).ToArray());
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string folder = String.Empty;
            FolderBrowserDialog getDir = new FolderBrowserDialog();
            getDir.Description = "Select VT1 Bundle Directory";
            if (getDir.ShowDialog() == DialogResult.OK)
            {
                folder = getDir.SelectedPath;
            }
            textBox1.Text = folder;
            thisReader.settings["VT1GameDir"] = folder;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string folder = String.Empty;
            FolderBrowserDialog getDir = new FolderBrowserDialog();
            getDir.Description = "Select VT2 Bundle Directory";
            if (getDir.ShowDialog() == DialogResult.OK)
            {
                folder = getDir.SelectedPath;
            }
            textBox2.Text = folder;
            thisReader.settings["VT2GameDir"] = folder;
        }

        private void LoadSettings()
        {
            if (File.Exists("settings.ini"))
            {
                string path = "settings.ini";

                var query = (from line in File.ReadAllLines(path)
                             let values = line.Split('=')
                             select new { Key = values[0], Value = values[1] });

                
                foreach (var kvp in query)
                {
                    if(kvp.Key == "VT2GameDir")
                    {
                        textBox2.Text = kvp.Value;
                        thisReader.settings["VT2GameDir"] = kvp.Value;
                    }
                    else if(kvp.Key == "VT1GameDir")
                    {
                        textBox1.Text = kvp.Value;
                        thisReader.settings["VT1GameDir"] = kvp.Value;
                    }
                    else if(kvp.Key == "DecompLua")
                    {
                        comboBox1.Text = kvp.Value;
                        thisReader.settings["DecompLua"] = kvp.Value;
                    }
                    else if(kvp.Key == "BundleHistory")
                    {
                        comboBox2.Text = kvp.Value;
                        thisReader.settings["BundleHistory"] = kvp.Value;
                    }
                }
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            thisReader.settings["DecompLua"] = comboBox1.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            thisReader.settings["BundleHistory"] = comboBox2.Text;
        }
    }
}
