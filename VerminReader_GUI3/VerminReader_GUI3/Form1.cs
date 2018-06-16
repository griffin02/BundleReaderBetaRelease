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
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace VerminReader_GUI3
{
    public partial class Form1 : Form
    {
        BundleReader myReader = new BundleReader();
        VT2Reader vt2Reader;
        public TreeNode node;
        private Thread searcherThread;
        VerminData.BundleData myBundles = new VerminData.BundleData();

        public Form1()
        {
            InitializeComponent();

            if (contextMenuStrip1.Items.Count == 0)
            {
                contextMenuStrip1.Items.Add("Extract File", null, ChildContextMenuExtract_Click);
                contextMenuStrip1.Items.Add("Replace File", null, ChildContextMenuReplace_Click);

                contextMenuStrip1.Items.Add("Unpack Bundle", null, ParentContextMenuUnpack_Click);
                contextMenuStrip1.Items.Add("Repack Bundle", null, ParentContextMenuRepack_Click);
                contextMenuStrip1.Items.Add("Search File", null, ParentContextMenuSearch_Click);
                contextMenuStrip1.Items.Add("Close Bundle", null, ParentContextMenuClose_Click);
            }
        }

        private void GetVermintideDirectoriesFromSteam()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Valve\\Steam");
            if (key != null)
            {
                Object o = key.GetValue("InstallPath");
                string fileName = o.ToString() + "\\steamapps\\libraryfolders.vdf";
                var lines = File.ReadLines(fileName);
                string VT1Folder = "none";
                string VT2Folder = "none";

                if (Directory.Exists(o.ToString() + "\\SteamApps\\common\\Warhammer End Times Vermintide\\bundle"))
                {
                    VT1Folder = o.ToString() + "\\SteamApps\\common\\Warhammer End Times Vermintide\\bundle";
                }
                if (Directory.Exists(o.ToString() + "\\SteamApps\\common\\Warhammer Vermintide 2\\bundle"))
                {
                    VT2Folder = o.ToString() + "\\SteamApps\\common\\Warhammer Vermintide 2\\bundle";
                }

                foreach (var line in lines)
                {
                    if (line.Contains("\\"))
                    {
                        string folder = line.Replace(@"\\", @"\");
                        folder = folder.Substring(folder.IndexOf(@"\") - 2, folder.LastIndexOf("\"") - (folder.IndexOf(@"\") - 2));
                        if (Directory.Exists(folder + "\\SteamApps\\common\\Warhammer End Times Vermintide\\bundle"))
                        {
                            VT1Folder = folder + "\\SteamApps\\common\\Warhammer End Times Vermintide\\bundle";
                        }
                        if (Directory.Exists(folder + "\\SteamApps\\common\\Warhammer Vermintide 2\\bundle"))
                        {
                            VT2Folder = folder + "\\SteamApps\\common\\Warhammer Vermintide 2\\bundle";
                        }
                    }
                }
                myReader.settings["VT1GameDir"] = VT1Folder;
                myReader.settings["VT2GameDir"] = VT2Folder;
                myReader.settings["DecompLua"] = "true";
                myReader.settings["BundleHistory"] = "true";
                File.WriteAllLines("settings.ini", myReader.settings.Select(x => x.Key + "=" + x.Value).ToArray());
            }
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
                    myReader.settings[kvp.Key] = kvp.Value;
                }
            }
            else
            {
                GetVermintideDirectoriesFromSteam();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myReader.AppDir = Application.StartupPath;
            vt2Reader = new VT2Reader(myReader);

            LoadSettings();

            LoadBundleHistoryOnStartup();
        }

        private void LoadBundleHistory(string mode)
        {
            treeView1.Nodes.Clear();
            if(mode == "VT1")
            {
                for (int i = 0; i < myBundles.Vt1bundles.Count(); i++)
                {
                    treeView1.Nodes.Add(myBundles.Vt1bundles[i].ToString());
                }
            }
            else if(mode == "VT2")
            {
                for(int i = 0; i < myBundles.Vt2bundles.Count(); i++)
                {
                    treeView1.Nodes.Add(myBundles.Vt2bundles[i].ToString());
                }
            }
        }

        private void SaveBundlesBeforeClosing()
        {
            if(myReader.Mode == "VT2")
            {
                if(myBundles.Vt2bundles.Count() == 0 && treeView1.Nodes.Count > 0)
                {
                    for (int i = 0; i < treeView1.Nodes.Count; i++)
                    {
                        myBundles.Vt2bundles.Add(treeView1.Nodes[i].Text);
                    }
                }
                else if (myBundles.Vt2bundles.Count() > 0 && treeView1.Nodes.Count > 0)
                {
                    myBundles.Vt2bundles.Clear();
                    for (int i = 0; i < treeView1.Nodes.Count; i++)
                    {
                        myBundles.Vt2bundles.Add(treeView1.Nodes[i].Text);
                    }
                }
            }
            else if(myReader.Mode == "VT1")
            {
                if(myBundles.Vt1bundles.Count() == 0 && treeView1.Nodes.Count > 0)
                {
                    for (int i = 0; i < treeView1.Nodes.Count; i++)
                    {
                        myBundles.Vt1bundles.Add(treeView1.Nodes[i].Text);
                    }
                }
                else if (myBundles.Vt1bundles.Count() > 0 && treeView1.Nodes.Count > 0)
                {
                    myBundles.Vt1bundles.Clear();
                    for (int i = 0; i < treeView1.Nodes.Count; i++)
                    {
                        myBundles.Vt1bundles.Add(treeView1.Nodes[i].Text);
                    }
                }
            }
        }

        private void LoadBundleHistoryOnStartup()
        {
            if (File.Exists("bundleHistory.dat"))
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(VerminData.BundleData));
                var subReq = myBundles;

                using (var sww = new StringReader(File.ReadAllText("bundleHistory.dat")))
                {
                    using (XmlTextReader reader = new XmlTextReader(sww))
                    {
                        myBundles = (VerminData.BundleData)xsSubmit.Deserialize(reader);
                    }
                }
            }
        }

        private void SaveBundleHistory()
        {
            SaveBundlesBeforeClosing();
            XmlSerializer xsSubmit = new XmlSerializer(typeof(VerminData.BundleData));
            var subReq = myBundles;
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(sww)) // XmlWriter.Create(sww))
                {
                    writer.Formatting = Formatting.Indented;
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString(); // Your XML
                }
            }
            File.WriteAllText("bundleHistory.dat", xml);
        }

        private void vT2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(myReader.AppDir + "\\settings.ini"))
            {
                LoadSettings();
                if (myReader.settings["VT2GameDir"] != "none")
                {
                    if (vT1ToolStripMenuItem.Checked)
                    {
                        vT1ToolStripMenuItem.Checked = false;

                        if (myReader.settings["BundleHistory"] == "true")
                        {
                            myBundles.Vt1bundles.Clear();
                            for (int i = 0; i < treeView1.Nodes.Count; i++)
                            {
                                myBundles.Vt1bundles.Add(treeView1.Nodes[i].Text);
                            }

                            treeView1.Nodes.Clear();

                            for (int i = 0; i < myBundles.Vt2bundles.Count(); i++)
                            {
                                treeView1.Nodes.Add(myBundles.Vt2bundles[i]);
                            }
                        }
                        else
                        {
                            treeView1.Nodes.Clear();
                        }
                    }
                    else
                    {
                        if (myReader.settings["BundleHistory"] == "true")
                        {
                            LoadBundleHistory("VT2");
                        }
                    }
                    myReader.Mode = "VT2";
                    myReader.directory = myReader.settings["VT2GameDir"];

                    if (!Directory.Exists(myReader.directory + "\\packed"))
                    {
                        Directory.CreateDirectory(myReader.directory + "\\packed");
                    }
                    if (!Directory.Exists(myReader.directory + "\\unpacked"))
                    {
                        Directory.CreateDirectory(myReader.directory + "\\unpacked");
                    }
                }
                else
                {
                    if (vT1ToolStripMenuItem.Checked)
                    {
                        vT1ToolStripMenuItem.Checked = false;
                    }
                    vT2ToolStripMenuItem.Checked = false;
                    MessageBox.Show("VT2 Bundle Directory not configured");
                }
            }
            else
            {
                if (vT1ToolStripMenuItem.Checked)
                {
                    vT1ToolStripMenuItem.Checked = false;
                }
                vT2ToolStripMenuItem.Checked = false;
                MessageBox.Show("Please configure settings first");
            }
        }

        private void vT1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(myReader.AppDir + "\\settings.ini"))
            {
                LoadSettings();
                if (myReader.settings["VT1GameDir"] != "none")
                {
                    if (vT2ToolStripMenuItem.Checked)
                    {
                        vT2ToolStripMenuItem.Checked = false;

                        if (myReader.settings["BundleHistory"] == "true")
                        {
                            myBundles.Vt2bundles.Clear();
                            for (int i = 0; i < treeView1.Nodes.Count; i++)
                            {
                                myBundles.Vt2bundles.Add(treeView1.Nodes[i].Text);
                            }

                            treeView1.Nodes.Clear();

                            for (int i = 0; i < myBundles.Vt1bundles.Count(); i++)
                            {
                                treeView1.Nodes.Add(myBundles.Vt1bundles[i]);
                            }
                        }
                        else
                        {
                            treeView1.Nodes.Clear();
                        }
                    }
                    else
                    {
                        if (myReader.settings["BundleHistory"] == "true")
                        {
                            LoadBundleHistory("VT1");
                        }
                    }
                    myReader.Mode = "VT1";
                    myReader.directory = myReader.settings["VT1GameDir"];

                    if (!Directory.Exists(myReader.directory + "\\packed"))
                    {
                        Directory.CreateDirectory(myReader.directory + "\\packed");
                    }
                    if (!Directory.Exists(myReader.directory + "\\unpacked"))
                    {
                        Directory.CreateDirectory(myReader.directory + "\\unpacked");
                    }
                }
                else
                {
                    if (vT2ToolStripMenuItem.Checked)
                    {
                        vT2ToolStripMenuItem.Checked = false;
                    }
                    vT1ToolStripMenuItem.Checked = false;
                    MessageBox.Show("VT1 Bundle Directory not configured");
                }
            }
            else
            {
                if (vT2ToolStripMenuItem.Checked)
                {
                    vT2ToolStripMenuItem.Checked = false;
                }
                vT1ToolStripMenuItem.Checked = false;
                MessageBox.Show("Please configure settings first");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myReader.Mode != null)
            {
                StringBuilder tempName = new StringBuilder();
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.InitialDirectory = myReader.directory;
                openFile.Filter = "Bundle | *.*";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    int index = openFile.FileName.LastIndexOf('\\');
                    tempName.Append(openFile.FileName.Substring(index + 1, openFile.FileName.Length - (index + 1)));
                }
                openFile.Dispose();
                if (tempName.ToString().Length > 1)
                {
                    TreeNode mainFile = new TreeNode(tempName.ToString());
                    if (mainFile.ToString() != String.Empty)
                    {
                        treeView1.Nodes.Add(mainFile);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a mode first");
            }
        }

        private void settingsStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings mySettings = new Settings(myReader);
            if(mySettings.ShowDialog() == DialogResult.OK)
            {

            }
            mySettings.Dispose();
        }

        private void UpdateParentNode()
        {
            treeView1.BeginUpdate();
            for (int i = 0; i < myReader.fileCount; i++)
            {
                if (myReader.file_name_hashes[i].Any(x => Char.IsDigit(x)) && !myReader.file_name_hashes[i].Any(x => Char.IsPunctuation(x)))
                {
                    myReader.file_name_hashes[i] = UInt64.Parse(myReader.file_name_hashes[i]).ToString("X");
                }
                if (myReader.file_ext_hashes[i].Any(x => Char.IsDigit(x)) && !myReader.file_ext_hashes[i].Any(x => Char.IsPunctuation(x)))
                {
                    myReader.file_ext_hashes[i] = UInt64.Parse(myReader.file_ext_hashes[i]).ToString("X");
                }
                treeView1.Nodes[treeView1.SelectedNode.Index].Nodes.Add(myReader.file_name_hashes[i] + "." + myReader.file_ext_hashes[i]);
            }
            treeView1.EndUpdate();
            treeView1.Nodes[treeView1.SelectedNode.Index].Expand();
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(e.X, e.Y);
                node = treeView1.GetNodeAt(p);
                if (node != null)
                {
                    contextMenuStrip1.Show(treeView1, p);
                    treeView1.SelectedNode = node;
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Parent == null)
            {
                if (treeView1.SelectedNode.Nodes.Count == 0)
                {
                    try
                    {
                        if (File.Exists(myReader.directory + "\\unpacked\\" + treeView1.SelectedNode.Text))
                        {
                            if (myReader.Mode == "VT2")
                            {
                                vt2Reader.GetBundleHashes(treeView1.SelectedNode.Text);
                            }
                            else if (myReader.Mode == "VT1")
                            {
                                vt2Reader.GetBundleHashes(treeView1.SelectedNode.Text);
                            }
                            UpdateParentNode();
                        }
                        else
                        {
                            int res = 0;
                            if (myReader.Mode == "VT2")
                            {
                               res = vt2Reader.GetTempBundleHashes(treeView1.SelectedNode.Text);
                            }
                            else if (myReader.Mode == "VT1")
                            {
                                res = vt2Reader.GetTempBundleHashes(treeView1.SelectedNode.Text);
                            }
                            if(res == 1)
                            {
                                UpdateParentNode();
                            }
                            else if(res == -1)
                            {
                                treeView1.SelectedNode.Remove();
                            }
                            
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                }
                else
                {
                    treeView1.SelectedNode.Expand();
                }
            }
            else
            {
                if (File.Exists(myReader.directory + "\\unpacked\\" + treeView1.SelectedNode.Parent.Text))
                {
                    vt2Reader.openedBundle = treeView1.SelectedNode.Parent.Text;
                    if (myReader.Mode == "VT2")
                    {
                        myReader.unpacked_file = File.ReadAllBytes(myReader.settings["VT2GameDir"] + "\\unpacked\\" + treeView1.SelectedNode.Parent.Text);
                        textBox1.Text = vt2Reader.GetFileInfo(treeView1.SelectedNode.Index);
                        GC.Collect();
                    }
                    else if(myReader.Mode == "VT1")
                    {
                        myReader.unpacked_file = File.ReadAllBytes(myReader.settings["VT1GameDir"] + "\\unpacked\\" + treeView1.SelectedNode.Parent.Text);
                        textBox1.Text = vt2Reader.GetFileInfo(treeView1.SelectedNode.Index);
                        GC.Collect();
                    }
                }
                else
                {
                    MessageBox.Show(null, "Bundle has not been unpacked yet", "Bundle Error!");
                }
            }
        }

        private void ParentContextMenuUnpack_Click(object sender, EventArgs e)
        {
            if (node != null)
            {
                if (treeView1.SelectedNode != null && treeView1.SelectedNode.Parent == null)
                {
                    myReader.UnpackBundle(treeView1.SelectedNode.Text);
                    MessageBox.Show("Bundle unpacked");
                }
                else
                {
                    MessageBox.Show(null, "Please highlight bundle to unpack", "Bundle Error");
                }
            }
        }

        private void ParentContextMenuRepack_Click(object sender, EventArgs e)
        {
            if (node != null)
            {
                if (treeView1.SelectedNode != null && treeView1.SelectedNode.Parent == null)
                {
                    myReader.RepackBundle(treeView1.SelectedNode.Text);
                    MessageBox.Show("Bundle repacking finished");
                }
                else
                {
                    MessageBox.Show(null, "Please highlight bundle to repack", "Bundle Error");
                }
            }
        }

        private void ParentContextMenuClose_Click(object sender, EventArgs e)
        {
            if (node != null)
            {
                node.Remove();
            }
            node = null;
        }

        private void ParentContextMenuSearch_Click(object sender, EventArgs e)
        {
            if(node != null)
            {
                string findString = String.Empty;
                SearchBox search = new SearchBox();
                if(search.ShowDialog() == DialogResult.OK)
                {
                    findString = search.textBox1.Text;
                }
                search.Dispose();
                if(findString != String.Empty)
                {
                    for(int i = 0; i < node.Nodes.Count; i++)
                    {
                        if(node.Nodes[i].Text.Contains(findString))
                        {
                            treeView1.SelectedNode = node.Nodes[i];
                            treeView1.SelectedNode.EnsureVisible();
                        }
                    }
                }
            }
        }

        private void ChildContextMenuExtract_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(myReader.directory + "\\unpacked\\" + node.Parent.Text))
                {
                    if (node.Parent != null)
                    {
                        myReader.unpacked_file = File.ReadAllBytes(myReader.directory + "\\unpacked\\" + node.Parent.Text);
                        string extractedFileName = myReader.directory + "\\" + node.Parent.Text + "_extract\\" + node.Text.Replace('/', '\\').ToString();
                        vt2Reader.openedBundle = node.Parent.Text;
                        if(myReader.Mode == "VT2")
                        {
                            vt2Reader.ExtractFile(extractedFileName, node.Index);
                        }
                        else if(myReader.Mode == "VT1")
                        {
                            vt2Reader.ExtractFile(extractedFileName, node.Index);
                        }
                        myReader.unpacked_file = null;
                        GC.Collect();
                        MessageBox.Show("File extraction complete");
                    }
                }
                else
                {
                    MessageBox.Show(null, "Bundle has not been unpacked yet", "Bundle Error!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }; //myReader.unpacked_file = null; GC.Collect(); }
            node = null;
        }

        private void ChildContextMenuReplace_Click(object sender, EventArgs e)
        {
            MessageBox.Show(null, "This Feature Is A Work In Progress", "Warning");
            if (File.Exists(myReader.directory + "\\unpacked\\" + node.Parent.Text))
            {
                if (node.Parent != null)
                {
                    List<byte> replacementFile = new List<byte>();
                    OpenFileDialog replaceDiag = new OpenFileDialog();
                    vt2Reader.openedBundle = node.Parent.Text;
                    if (replaceDiag.ShowDialog() == DialogResult.OK)
                    {
                        if(replaceDiag.FileName != null)
                            replacementFile = File.ReadAllBytes(replaceDiag.FileName).ToList<byte>();
                    }
                    replaceDiag.Dispose();
                    if (replacementFile.Count > 0)
                    {
                        string modFileName = myReader.directory + "\\unpacked\\" + node.Parent.Text;
                        myReader.unpacked_file = File.ReadAllBytes(myReader.directory + "\\unpacked\\" + node.Parent.Text);
                        if (myReader.Mode == "VT2")
                        {
                            vt2Reader.ReplaceFile(modFileName, replacementFile, node.Index);
                        }
                        else if (myReader.Mode == "VT1")
                        {
                            vt2Reader.ReplaceFile(modFileName, replacementFile, node.Index);
                        }
                        myReader.unpacked_file = null;
                        GC.Collect();
                        MessageBox.Show("File has been replaced in bundle");
                    }
                }
            }
            else
            {
                MessageBox.Show(null, "Bundle has not been unpacked yet", "Bundle Error!");
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (node != null)
            {
                contextMenuStrip1.Items[0].Enabled = node.Parent != null;
                contextMenuStrip1.Items[2].Enabled = node.Parent == null;

                if (contextMenuStrip1.Items[0].Enabled)
                {
                    contextMenuStrip1.Items[0].Visible = true;
                    contextMenuStrip1.Items[1].Visible = true;
                    contextMenuStrip1.Items[2].Visible = false;
                    contextMenuStrip1.Items[3].Visible = false;
                    contextMenuStrip1.Items[4].Visible = false;
                    contextMenuStrip1.Items[5].Visible = false;
                }
                if (contextMenuStrip1.Items[2].Enabled)
                {
                    contextMenuStrip1.Items[0].Visible = false;
                    contextMenuStrip1.Items[1].Visible = false;
                    contextMenuStrip1.Items[2].Visible = true;
                    contextMenuStrip1.Items[3].Visible = true;
                    contextMenuStrip1.Items[4].Visible = true;
                    contextMenuStrip1.Items[5].Visible = true;
                }
            }
        }

        private void hashCalcBtn_Click(object sender, EventArgs e)
        {
            if(hashInputbox.Text != String.Empty)
            {
                hashOutputbox.Text = myReader.CreateHash(hashInputbox.Text);
            }
        }

        private void hashLookupBtn_Click(object sender, EventArgs e)
        {
            if (hashInputbox.Text != String.Empty)
            {
                hashOutputbox.Text = myReader.HashLookUp(hashInputbox.Text);
            }
        }

        private void searchBundlesBtn_Click(object sender, EventArgs e)
        {
            if (searchInputbox.Text != String.Empty)
            {
                searchDisplaybox.Items.Clear();
                searcherThread = new Thread(() => myReader.SearchBundles(searchInputbox.Text, this));
                searcherThread.Start();
            }
        }

        private void searchDisplaybox_DoubleClick(object sender, EventArgs e)
        {
            if(searchDisplaybox.SelectedItem != null)
            {
                treeView1.Nodes.Add(searchDisplaybox.SelectedItem.ToString());
            }
        }

        private void cancelSearchBtn_Click(object sender, EventArgs e)
        {
            searcherThread.Abort();
            label5.Text = "Search Aborted";
            Process[] proc = Process.GetProcessesByName("VerminUnpacker");
            if(proc.Count() > 0)
            {
                proc[0].Kill();
                searcherThread.Abort();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myReader.settings["BundleHistory"] == "true")
            {
                SaveBundleHistory();
            }
        }
    }
}
