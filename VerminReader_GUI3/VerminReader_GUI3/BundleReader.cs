using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Data.HashFunction;
using System.Windows.Forms;

namespace VerminReader_GUI3
{
    public class BundleReader
    {
        public VerminData verminData = new VerminData();
        public byte[] unpacked_file;
        public byte[] magicNumber = new byte[4];
        public List<string> file_name_hashes = new List<string>();
        public List<string> file_ext_hashes = new List<string>();
        public int fileCount;
        public string directory;
        public string AppDir;
        public Dictionary<string, string> settings = new Dictionary<string, string>();
        public string Mode;

        public string CreateHash(string input)
        {
            MurmurHash2 Hasher = new MurmurHash2();
            UInt64 tempHash = BitConverter.ToUInt64(Hasher.ComputeHash(input, 64), 0);
            return tempHash.ToString("X") + Environment.NewLine + BitConverter.ToString(BitConverter.GetBytes(tempHash)).Replace('-', ' ');
        }

        public string HashLookUp(string input)
        {
            string ret = "Hash Not Found";
            string output = String.Empty;
            Process unpacker = new Process();
            unpacker.StartInfo.FileName = AppDir + "\\VerminUnpacker.exe";
            unpacker.StartInfo.Arguments = "-lookup " + input;
            unpacker.StartInfo.UseShellExecute = false;
            unpacker.StartInfo.CreateNoWindow = true;
            unpacker.StartInfo.RedirectStandardOutput = true;
            unpacker.Start();

            while ((output = unpacker.StandardOutput.ReadLine()) != null)
            {
                if (output != "" && output.Contains("Found:"))
                {
                    ret = output.Substring(7, output.Length - 7);
                }
            }
            unpacker.WaitForExit();

            return ret;
        }

        public string FindExePath(string exe)
        {
            exe = Environment.ExpandEnvironmentVariables(exe);
            if (!File.Exists(exe))
            {
                if (Path.GetDirectoryName(exe) == String.Empty)
                {
                    foreach (string test in (Environment.GetEnvironmentVariable("PATH", System.EnvironmentVariableTarget.Machine) ?? "").Split(';'))
                    {
                        string path = test.Trim();
                        if (!String.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, exe)))
                            return Path.GetFullPath(path);
                    }
                    foreach (string test in (Environment.GetEnvironmentVariable("PATH", System.EnvironmentVariableTarget.User) ?? "").Split(';'))
                    {
                        string path = test.Trim();
                        MessageBox.Show(path);
                        if (!String.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, exe)))
                            return Path.GetFullPath(path);
                    }
                }
            }
            return Path.GetFullPath(exe);
        }

        public void UnpackBundle(string bundlename)
        {
            Process unpacker = new Process();
            unpacker.StartInfo.FileName = AppDir + "\\VerminUnpacker.exe";
            unpacker.StartInfo.Arguments = "-u " + "\"" + directory + "\" " + bundlename;
            unpacker.StartInfo.UseShellExecute = false;
            unpacker.StartInfo.CreateNoWindow = true;
            unpacker.Start();
            unpacker.WaitForExit();
        }

        public void RepackBundle(string bundlename)
        {
            Process unpacker = new Process();
            unpacker.StartInfo.FileName = AppDir + "\\VerminUnpacker.exe";
            unpacker.StartInfo.Arguments = "-p " + "\"" + directory + "\" " + bundlename;
            unpacker.StartInfo.UseShellExecute = false;
            unpacker.StartInfo.CreateNoWindow = true;
            unpacker.Start();
            unpacker.WaitForExit();
        }

        public void SearchBundles(string searchTerm, Form thisForm)
        {
            Form1 form = (Form1)thisForm;
            string output = String.Empty;
            Process unpacker = new Process();
            unpacker.StartInfo.FileName = AppDir + "\\VerminUnpacker.exe";
            unpacker.StartInfo.Arguments = "-find " + "\"" + directory + "\" " + searchTerm;
            unpacker.StartInfo.UseShellExecute = false;
            unpacker.StartInfo.CreateNoWindow = true;
            unpacker.StartInfo.RedirectStandardOutput = true;
            unpacker.Start();

            while ((output = unpacker.StandardOutput.ReadLine()) != null)
            {
                if (output != "" && output.Contains("Found in"))
                {
                    form.searchDisplaybox.Invoke((MethodInvoker)delegate
                    {
                        form.searchDisplaybox.Items.Add(output.Substring(9, output.Length - 9));
                        form.Update();
                    });
                    
                }
                else if (output != "" && output.Contains("Searching"))
                {
                    form.label5.Invoke((MethodInvoker)delegate
                    {
                        form.label5.Text = output;
                        form.Update();
                    });
                }


                form.Invoke((MethodInvoker)delegate
                {
                    form.Update();
                });
            }
            unpacker.WaitForExit();

            form.label5.Invoke((MethodInvoker)delegate
            {
                form.label5.Text = "Searching Completed";
                form.Update();
            });
        }

    }
}
