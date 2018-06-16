using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Data.HashFunction;
using System.Windows.Forms;

namespace VerminReader_GUI3
{
    public class VT2Reader
    {
        public BundleReader thisReader;
        public MurmurHash2 Hasher = new MurmurHash2();
        public int filePadding = 0x14;
        public string openedBundle = String.Empty;

        public VT2Reader(BundleReader reader)
        {
            thisReader = reader;
        }

        enum FileHashes : UInt64
        {
            animation = 0x931E336D7646CC26,
            animation_curves = 0xDCFB9E18FFF13984,
            apb = 0x3EED05BA83AF5090,
            bik = 0xAA5965F03029FA18,
            bones = 0x18DEAD01056B72E9,
            config = 0x82645835E6B73232,
            crypto = 0x69108DED1E3E634B,
            data = 0x8FD0D44D20650B68,
            entity = 0x9831CA893B0D087D,
            flow = 0x92D3EE038EEB610D,
            font = 0x9EFE0A916AAE7880,
            ini = 0xD526A27DA14F1DC5,
            ivf = 0xFA4A8E091A91201E,
            level = 0x2A690FD348FE9AC5,
            lua = 0xA14E8DFA2CD117E2,
            material = 0xEAC0B497876ADEDF,
            mod = 0x3FCDD69156A46417,
            mouse_cursor = 0xB277B11FE4A61D37,
            navdata = 0x169DE9566953D264,
            network_config = 0x3B1FA9E8F6BAC374,
            package = 0xAD9C6D9ED1E5E77A,
            particles = 0xA8193123526FAD64,
            physics_properties = 0xBF21403A3AB0BBB1,
            render_config = 0x27862FE24795319C,
            scene = 0x9D0A795BFE818D19,
            shader = 0xCCE8D5B5F5AE333F,
            shader_library = 0xE5EE32A477239A93,
            shader_library_group = 0x9E5C3CC74575AEB5,
            shading_environment = 0xFE73C7DCFF8A7CA5,
            shading_environment_mapping = 0x250E0A11AC8E26F8,
            sound_environment = 0xD8B27864A97FFDD7,
            state_machine = 0xA486D4045106165C,
            strings = 0xD972BAB10B40FD3,
            surface_properties = 0xAD2D3FA30D9AB394,
            texture = 0xCD4238C6A0C69E32,
            timpani_bank = 0x99736BE1FFF739A4,
            timpani_master = 0xA3E6C59A2B9C6C,
            tome = 0x19C792357C99F49B,
            unit = 0xE0A48D0BE9A7453F,
            vector_field = 0xF7505933166D6755,
            wav = 0x786F65C00A816B19,
            wwise_bank = 0x535A7BD3E650D799,
            wwise_dep = 0xAF32095C82F2B070,
            wwise_metadata = 0xD50A8B7E1C82B110,
            wwise_stream = 0x504B55235D21440E,
        };

        public void GetBundleHashes(string fileName)
        {
            thisReader.file_name_hashes.Clear();
            thisReader.file_ext_hashes.Clear();
            File.OpenRead(thisReader.directory + "\\" + fileName).Read(thisReader.magicNumber, 0, 4);
            
            using (FileStream fs = new FileStream(thisReader.directory + "\\unpacked\\" + fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] count = new byte[0x4];
                fs.Read(count, 0, 4);
                thisReader.fileCount = BitConverter.ToInt32(count, 0);
            }

            Process unpacker = new Process();
            unpacker.StartInfo.FileName = thisReader.AppDir + "\\VerminUnpacker.exe";
            if (BitConverter.ToUInt32(thisReader.magicNumber, 0) == 0xF0000005)
            {
                unpacker.StartInfo.Arguments = "-hash " + "\"" + thisReader.directory + "\" " + fileName;
            }
            else
            {
                unpacker.StartInfo.Arguments = "-vt1hash " + "\"" + thisReader.directory + "\" " + fileName;
            }
            unpacker.StartInfo.UseShellExecute = false;
            unpacker.StartInfo.CreateNoWindow = true;
            unpacker.Start();
            unpacker.WaitForExit();
            string jsonObject = File.ReadAllText(thisReader.AppDir + "\\" + fileName + "_HashDump.json");
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonObject));
            int addTo = 0;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType.ToString() == "PropertyName")
                    {
                        if (reader.Value.ToString() == "fileNameHash")
                        {
                            addTo = 1;
                        }

                        if (reader.Value.ToString() == "fileTypeHash")
                        {
                            addTo = 2;
                        }
                    }
                    else
                    {

                        if (addTo == 1)
                        {
                            thisReader.file_name_hashes.Add(reader.Value.ToString());
                        }
                        if (addTo == 2)
                        {
                            thisReader.file_ext_hashes.Add(reader.Value.ToString());
                        }
                    }
                }
            }
            File.Delete(thisReader.AppDir + "\\" + fileName + "_HashDump.json");
        }

        public int GetTempBundleHashes(string fileName)
        {
            thisReader.file_name_hashes.Clear();
            thisReader.file_ext_hashes.Clear();
            File.OpenRead(thisReader.directory + "\\" + fileName).Read(thisReader.magicNumber, 0, 4);

            Process unpacker = new Process();
            unpacker.StartInfo.FileName = thisReader.AppDir + "\\VerminUnpacker.exe";
            if (BitConverter.ToUInt32(thisReader.magicNumber, 0) == 0xF0000005)
            {
                unpacker.StartInfo.Arguments = "-temphash " + "\"" + thisReader.directory + "\" " + fileName;
            }
            else if (BitConverter.ToUInt32(thisReader.magicNumber, 0) == 0xF0000004)
            {
                unpacker.StartInfo.Arguments = "-vt1temphash " + "\"" + thisReader.directory + "\" " + fileName;
            }
            else
            {
                MessageBox.Show(null, "This is not a valid bundle file", "Error");
                return -1;
            }
            unpacker.StartInfo.UseShellExecute = false;
            unpacker.StartInfo.CreateNoWindow = true;
            unpacker.Start();
            unpacker.WaitForExit();
            string jsonObject = File.ReadAllText(thisReader.AppDir + "\\" + fileName + "_HashDump.json");
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonObject));
            int addTo = 0;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType.ToString() == "PropertyName")
                    {
                        if (reader.Value.ToString() == "fileNameHash")
                        {
                            addTo = 1;
                        }

                        if (reader.Value.ToString() == "fileTypeHash")
                        {
                            addTo = 2;
                        }
                    }
                    else
                    {

                        if (addTo == 1)
                        {
                            thisReader.file_name_hashes.Add(reader.Value.ToString());
                        }
                        if (addTo == 2)
                        {
                            thisReader.file_ext_hashes.Add(reader.Value.ToString());
                        }
                    }
                }
            }
            thisReader.fileCount = thisReader.file_name_hashes.Count;
            File.Delete(thisReader.AppDir + "\\" + fileName + "_HashDump.json");
            return 1;
        }

        public void SearchTempBundleHashes(string fileName, string searchTerm, ListBox thisBox)
        {
            List<string> file_name_hashes = new List<string>();
            List<string> file_ext_hashes = new List<string>();
            File.OpenRead(thisReader.directory + "\\" + fileName).Read(thisReader.magicNumber, 0, 4);

            Process unpacker = new Process();
            unpacker.StartInfo.FileName = thisReader.AppDir + "\\VerminUnpacker.exe";
            if (BitConverter.ToUInt32(thisReader.magicNumber, 0) == 0xF0000005)
            {
                unpacker.StartInfo.Arguments = "-temphash " + "\"" + thisReader.directory + "\" " + fileName;
            }
            else
            {
                unpacker.StartInfo.Arguments = "-vt1temphash " + "\"" + thisReader.directory + "\" " + fileName;
            }
            unpacker.StartInfo.UseShellExecute = false;
            unpacker.StartInfo.CreateNoWindow = true;
            unpacker.Start();
            unpacker.WaitForExit();
            string jsonObject = File.ReadAllText(thisReader.AppDir + "\\" + fileName + "_HashDump.json");
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonObject));
            int addTo = 0;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType.ToString() == "PropertyName")
                    {
                        if (reader.Value.ToString() == "fileNameHash")
                        {
                            addTo = 1;
                        }

                        if (reader.Value.ToString() == "fileTypeHash")
                        {
                            addTo = 2;
                        }
                    }
                    else
                    {

                        if (addTo == 1)
                        {
                            file_name_hashes.Add(reader.Value.ToString());
                        }
                        if (addTo == 2)
                        {
                            file_ext_hashes.Add(reader.Value.ToString());
                        }
                    }
                }
            }
            File.Delete(thisReader.AppDir + "\\" + fileName + "_HashDump.json");

            for(int i = 0; i < file_name_hashes.Count; i++)
            {
                if((file_name_hashes[i] + "." + file_ext_hashes[i]).Contains(searchTerm))
                {
                    thisBox.Invoke(new MethodInvoker(delegate
                    {
                        thisBox.Items.Add(fileName);
                    }));
                }
            }
            file_name_hashes.Clear();
            file_ext_hashes.Clear();
        }

        public int[] GetFileData(int fileIndex)
        {
            int[] retData = new int[2];
            thisReader.fileCount = BitConverter.ToInt32(thisReader.unpacked_file, 0);
            UInt32 headerCount = 0;
            UInt32 fileSize = 0;
            UInt32 realfileSize = 0;
            uint prevSize = 0;
            uint fakeCounter = 0;

            UInt64 fileName = 0;
            UInt64 extName = 0;
            UInt64 nxtfileNameHash = 0;
            UInt64 nxtfileTypeHash = 0;
            UInt64 curfileTypeHash = 0;
            UInt64 nxtfileName = 0;
            retData[0] = -1;
            retData[1] = -1;
            File.OpenRead(thisReader.directory + "\\" + openedBundle).Read(thisReader.magicNumber, 0, 4);
            if (BitConverter.ToUInt32(thisReader.magicNumber, 0) == 0xF0000005)
            {
                filePadding = 0x14;
            }
            else
            {
                filePadding = 0x10;
            }

            if (fileIndex == 0)
            {
                retData[0] = fileIndex;
                retData[1] = BitConverter.ToInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x1C));
                return retData;
            }
            else
            {
                for (int i = 0; i < (fileIndex + 1); i++)
                {
                    extName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((fileIndex * 0x10)));
                    fileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((fileIndex * 0x10) + 0x8));

                    nxtfileTypeHash = BitConverter.ToUInt64(thisReader.unpacked_file, 0x104 + ((i + 1) * filePadding));
                    curfileTypeHash = BitConverter.ToUInt64(thisReader.unpacked_file, 0x104 + (i * filePadding));
                    nxtfileNameHash = BitConverter.ToUInt64(thisReader.unpacked_file, 0x104 + ((i + 1) * filePadding) + 0x8);

                    headerCount = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((i * 0x10) + 0x10));
                    fileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((i * 0x10) + 0x1C));
                    UInt32 otherHeader = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((i * 0x10) + 0x14));
                    UInt32 isLua = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((i * 0x10) + 0x28));
                    UInt32 isDDS = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((i * 0x10) + 0x24));

                    realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((i * 0x10) + 0x1C));

                    if (extName == (UInt64)FileHashes.lua)
                    {
                        realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((i * 0x10) + 0x24));
                    }

                    if(curfileTypeHash == (UInt64)FileHashes.timpani_bank)
                    {
                        fileSize = 0x4;
                        realfileSize = 0x8;
                    }
                    
                    fakeCounter = prevSize;
                    if (curfileTypeHash == (UInt64)FileHashes.timpani_bank)
                    {
                        fakeCounter += (fileSize + 0x4);
                    }
                    else
                    {
                        fakeCounter += (fileSize + 0x14);
                    }

                    if (i < (thisReader.fileCount - 1))
                    {
                        if (fileSize > 0x10000)
                        {
                            nxtfileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fakeCounter) + (((i + 1) * 0x10) + 0x8));

                            if (nxtfileName != nxtfileNameHash)
                            {
                                for (int b = 0x10000; b < 0x100000; b += 0x10000)
                                {
                                    nxtfileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)(fakeCounter - b)) + (((i + 1) * 0x10) + 0x8));
                                    if (nxtfileName == nxtfileNameHash)
                                    {
                                        fileSize -= (uint)b;
                                        realfileSize -= (uint)b;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (i == (fileIndex))
                    {
                        retData[0] = (int)prevSize;
                        retData[1] = (int)realfileSize;
                        return retData;
                    }

                    if (curfileTypeHash == (UInt64)FileHashes.timpani_bank)
                    {
                        prevSize += (fileSize + 0x4);
                    }
                    else
                    {
                        prevSize += (fileSize + 0x14);
                    }
                }
            }

            return retData;
        }

        public void DecompilePackage(string filename)
        {
            string newFileName = filename.Substring(0, filename.Length - 8);
            byte[] newFile = File.ReadAllBytes(filename);
            StringBuilder decompPackage = new StringBuilder();

            UInt32 fileCount = BitConverter.ToUInt32(newFile, 0);
            List<string> Mods = new List<string>();
            List<string> Packages = new List<string>();
            List<string> Luas = new List<string>();
            List<string> Materials = new List<string>();

            for (int l = 0; l < fileCount; l++)
            {
                UInt64 fileExtHash = BitConverter.ToUInt64(newFile, 0x4 + (l * 0x10));
                UInt64 fileNameHash = BitConverter.ToUInt64(newFile, 0x4 + ((l * 0x10) + 0x8));

                if (fileExtHash == (UInt64)FileHashes.mod)
                {
                    Mods.Add(fileNameHash.ToString("X"));
                }

                if (fileExtHash == (UInt64)FileHashes.package)
                {
                    Packages.Add(fileNameHash.ToString("X"));
                }

                if (fileExtHash == (UInt64)FileHashes.lua)
                {
                    Luas.Add(fileNameHash.ToString("X"));
                }
                if (fileExtHash == (UInt64)FileHashes.texture)
                {
                    Materials.Add(fileNameHash.ToString("X"));
                }
            }

            for (int i = 0; i < Mods.Count; i++)
            {
                if (i == 0)
                {
                    decompPackage.AppendFormat("mod = [" + Environment.NewLine + "\t\"{0}\"" + Environment.NewLine, Mods[i]);
                }
                else
                {
                    decompPackage.AppendFormat("\t\"{0}\"" + Environment.NewLine, Mods[i]);
                }
            }
            decompPackage.AppendFormat("]" + Environment.NewLine + Environment.NewLine);

            for (int i = 0; i < Packages.Count; i++)
            {
                if (i == 0)
                {
                    decompPackage.AppendFormat("package = [" + Environment.NewLine + "\t\"{0}\"" + Environment.NewLine, Packages[i]);
                }
                else
                {
                    decompPackage.AppendFormat("\t\"{0}\"" + Environment.NewLine, Packages[i]);
                }
            }
            decompPackage.AppendFormat("]" + Environment.NewLine + Environment.NewLine);

            for (int i = 0; i < Luas.Count; i++)
            {
                if (i == 0)
                {
                    decompPackage.AppendFormat("lua = [" + Environment.NewLine + "\t\"{0}\"" + Environment.NewLine, Luas[i]);
                }
                else
                {
                    decompPackage.AppendFormat("\t\"{0}\"" + Environment.NewLine, Luas[i]);
                }
            }
            decompPackage.AppendFormat("]" + Environment.NewLine + Environment.NewLine);

            if (Materials.Count > 0)
            {
                decompPackage.AppendFormat("material = [" + Environment.NewLine + "\t\"{0}/*\"" + Environment.NewLine + "]", Packages[0]);
            }
            File.WriteAllText(newFileName + "_decomp.package", decompPackage.ToString());
        }

        public void HandleFileTypeExtraction(string filename, UInt64 fileTypeHash, int fileIndex, int prevSize, int size)
        {
            int startIndex = 0;
            UInt32 realfileSize = 0;

            int startOffset = 0x24;
            int sizeOffset = 0x1C;

            if (fileTypeHash == (UInt64)FileHashes.lua)
            {
                if (filePadding == 0x14)
                {
                    startOffset = 0x30;
                    sizeOffset = 0x24;
                }
                else
                {
                    startOffset = 0x2C;
                    sizeOffset = 0x24;
                }
            }

            if (fileTypeHash == (UInt64)FileHashes.mod)
            {
                startOffset = 0x34;
                sizeOffset = 0x24;
            }

            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }

            using (FileStream thisStream = new FileStream(filename, FileMode.Create))
            {
                if (fileIndex == 0)
                {
                    startIndex = (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + startOffset);
                    realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + sizeOffset));
                }
                else
                {
                    startIndex = (0x104 + (thisReader.fileCount * filePadding) + (int)prevSize) + ((fileIndex * 0x10) + startOffset);
                    realfileSize = Convert.ToUInt32(size);
                }

                for (int j = startIndex; j < (startIndex + realfileSize); j++)
                {
                    thisStream.WriteByte(thisReader.unpacked_file[j]);
                }
            }
        }

        public void DecompileLua(string filename, string newName)
        {
            string pythonExe = thisReader.FindExePath("python.exe");
            if (!pythonExe.Contains(thisReader.AppDir))
            {
                if (filePadding == 0x14)
                {
                    if(File.Exists(thisReader.AppDir + "\\ljd-vt2\\" + newName + "c"))
                    {
                        File.Delete(thisReader.AppDir + "\\ljd-vt2\\" + newName + "c");
                    }
                    File.Move(filename, thisReader.AppDir + "\\ljd-vt2\\" + newName + "c");
                }
                else
                {
                    if(File.Exists(thisReader.AppDir + "\\ljd-vt1\\" + newName + "c"))
                    {
                        File.Delete(thisReader.AppDir + "\\ljd-vt1\\" + newName + "c");
                    }
                    File.Move(filename, thisReader.AppDir + "\\ljd-vt1\\" + newName + "c");
                }
                Process unpacker = new Process();
                unpacker.StartInfo.FileName = "C:\\windows\\system32\\cmd.exe";
                if (filePadding == 0x14)
                {
                    unpacker.StartInfo.WorkingDirectory = thisReader.AppDir + "\\ljd-vt2";
                }
                else
                {
                    unpacker.StartInfo.WorkingDirectory = thisReader.AppDir + "\\ljd-vt1";
                }


                unpacker.StartInfo.Arguments = "/c \"" + pythonExe + " " + "main.py -f " + newName + "c\" --catchasserts > " + newName;
                unpacker.StartInfo.UseShellExecute = false;
                unpacker.StartInfo.CreateNoWindow = true;
                unpacker.Start();
                unpacker.WaitForExit();
                if (filePadding == 0x14)
                {
                    File.Move(thisReader.AppDir + "\\ljd-vt2\\" + newName, filename);
                    File.Delete(thisReader.AppDir + "\\ljd-vt2\\" + newName + "c");
                }
                else
                {
                    FileInfo FileVol = new FileInfo(thisReader.AppDir + "\\ljd-vt1\\" + newName);
                    if (FileVol.Length != 0)
                    {
                        File.Move(thisReader.AppDir + "\\ljd-vt1\\" + newName, filename);
                        File.Delete(thisReader.AppDir + "\\ljd-vt1\\" + newName + "c");
                    }
                    else
                    {
                        File.Move(thisReader.AppDir + "\\ljd-vt1\\" + newName + "c", filename);
                        File.Delete(thisReader.AppDir + "\\ljd-vt1\\" + newName);
                        MessageBox.Show("Decompiling of LUA file failed. Only original compiled LUA file produced.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Python.exe does not exist. Please install python to decompile. Lua will be extracted but not decompiled.");
            }
        }

        public string GetFileInfo(int fileIndex)
        {
            int[] fileData = GetFileData(fileIndex);
            StringBuilder testString = new StringBuilder();
            UInt64 fileName = 0;
            UInt64 extName = 0;
            UInt32 headerCount = 0;
            UInt32 fileSize = 0;

            UInt64 nxtfileNameHash = 0;
            UInt64 nxtfileTypeHash = 0;

            if (fileData[0] == 0)
            {
                extName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10)));
                fileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x8));

                headerCount = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x10));
                fileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x1C));
            }
            else
            {
                UInt64 fileTypeHash = BitConverter.ToUInt64(thisReader.unpacked_file, 0x104 + (fileIndex * filePadding));
                UInt64 fileNameHash = BitConverter.ToUInt64(thisReader.unpacked_file, 0x104 + (fileIndex * filePadding) + 0x8);

                nxtfileTypeHash = BitConverter.ToUInt64(thisReader.unpacked_file, 0x104 + ((fileIndex + 1) * filePadding));
                nxtfileNameHash = BitConverter.ToUInt64(thisReader.unpacked_file, 0x104 + ((fileIndex + 1) * filePadding) + 0x8);

                extName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10)));
                fileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x8));

                headerCount = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x10));
                fileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x1C));
                if(fileTypeHash == (UInt64)FileHashes.timpani_bank)
                {
                    fileSize = 0x8;
                }
            }

            testString.AppendFormat("FileName: {0}\r\n FileExt: {1}\r\n HeaderCount: {2}\r\n HeaderFileSize: {3}\r\n", fileName.ToString("X"), extName.ToString("X"), headerCount, fileSize.ToString("X")); // NextHash1: {4:X}\r\n NextHash2: {5:X}\r\n FakeSize: {6:X}", fileName.ToString("X"), extName.ToString("X"), headerCount, fileSize.ToString("X"), nxtfileNameHash, nxtfileName, fakeSize);
            return testString.ToString();
        }

        

        public void ExtractFile(string filename, int fileIndex)
        {
            int[] fileData = GetFileData(fileIndex);
            UInt64 fileName = 0;
            UInt64 extName = 0;

            if (fileData[0] == 0)
            {
                extName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10)));
                fileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x8));
                HandleFileTypeExtraction(filename, extName, fileIndex, 0, 0);
            }
            else if (fileData[0] > 0)
            {
                extName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10)));
                fileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x8));
                HandleFileTypeExtraction(filename, extName, fileIndex, fileData[0], fileData[1]);
            }
            if (thisReader.settings["DecompLua"] == "true")
            {
                if (filename.Contains(".lua"))
                {
                    string newName = filename.Substring(filename.LastIndexOf("\\") + 1, filename.Length - (filename.LastIndexOf("\\") + 1));
                    DecompileLua(filename, newName);
                }
                if(filename.Contains(".package"))
                {
                    DecompilePackage(filename);
                }
            }
        }

        public void ReplaceFile(string filename, List<byte> newfile, int fileIndex)
        {
            int[] fileData = GetFileData(fileIndex);
            UInt64 fileName = 0;
            UInt64 extName = 0;
            UInt32 realfileSize = 0;
            int counter = 0;
            bool isLuaCompiled = false;
            bool isModCompiled = false;

            int startIndex = 0;
            int headerSizeIndex = 0;
            int headerRealSizeIndex = 0;
            uint luaJunkDataSize = 0;
            int newSize = newfile.Count;
            byte[] newRealSize = BitConverter.GetBytes(newSize);
            byte[] newChuckSize = new byte[4];

            if(filePadding == 0x10)
            {
                if (filename.Contains(".lua"))
                {
                    MessageBox.Show(null, "Old Bundle Lua Format Not Supported", "ERROR");
                    return;
                }
                if (filename.Contains(".mod"))
                {
                    MessageBox.Show(null, "Old Bundle Mod Format Not Supported", "ERROR");
                    return;
                }
            }

            if (fileIndex == 0)
            {
                extName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10)));
                fileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x8));

                if(extName == (UInt64)FileHashes.lua)
                {
                    if (newfile[0] == 0x1B && newfile[1] == 0x4C)
                        isLuaCompiled = false;
                    else
                        isLuaCompiled = true;
                }

                if(extName == (UInt64)FileHashes.mod)
                {
                    if (newfile[0x4] == 0 && newfile[0x5] == 0 && newfile[0x6] == 0 && newfile[0x7] == 0 && newfile[0x8] == 0 && newfile[0x9] == 0 && newfile[0xA] == 0 && newfile[0xB] == 0 && newfile[0xC] == 0 && newfile[0xD] == 0 && newfile[0xE] == 0 && newfile[0xF] == 0)
                        isModCompiled = true;
                    else
                        isModCompiled = false;
                }

                realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x1C));
                startIndex = (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x24);
                headerSizeIndex = (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x1C);
                headerRealSizeIndex = (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x24);
                newChuckSize = BitConverter.GetBytes(newSize);

                if (extName == (UInt64)FileHashes.lua && !isLuaCompiled)
                {
                    realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x24));
                    startIndex = (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x30);
                    luaJunkDataSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x2C));
                    newChuckSize = BitConverter.GetBytes(newSize + (luaJunkDataSize + 0xC));
                }
                else if (extName == (UInt64)FileHashes.lua && isLuaCompiled)
                {
                    luaJunkDataSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x2C));
                    realfileSize -= luaJunkDataSize;
                    newChuckSize = BitConverter.GetBytes(newSize + luaJunkDataSize);
                    newRealSize = BitConverter.GetBytes(newSize - 0xC);
                    newfile[8] = BitConverter.GetBytes(luaJunkDataSize)[0];
                    newfile[9] = BitConverter.GetBytes(luaJunkDataSize)[1];
                    newfile[10] = BitConverter.GetBytes(luaJunkDataSize)[2];
                    newfile[11] = BitConverter.GetBytes(luaJunkDataSize)[3];
                }
                else if(extName == (UInt64)FileHashes.mod && !isModCompiled)
                {
                    startIndex = (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x34);
                    realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding)) + ((fileIndex * 0x10) + 0x24));
                    newChuckSize = BitConverter.GetBytes(newSize + 0x10);
                    newRealSize = BitConverter.GetBytes(newSize);
                }
                else if (extName == (UInt64)FileHashes.mod && isModCompiled)
                {
                    newRealSize = BitConverter.GetBytes(newSize - 0x10);
                }
            }
            else
            {
                extName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10)));
                fileName = BitConverter.ToUInt64(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x8));

                if (extName == (UInt64)FileHashes.lua)
                {
                    if (newfile[0] == 0x1B)
                        isLuaCompiled = false;
                    else
                        isLuaCompiled = true;
                }

                if (extName == (UInt64)FileHashes.mod)
                {
                    if (newfile[0x4] == 0 && newfile[0x5] == 0 && newfile[0x6] == 0 && newfile[0x7] == 0 && newfile[0x8] == 0 && newfile[0x9] == 0 && newfile[0xA] == 0 && newfile[0xB] == 0 && newfile[0xC] == 0 && newfile[0xD] == 0 && newfile[0xE] == 0 && newfile[0xF] == 0)
                        isModCompiled = true;
                    else
                        isModCompiled = false;
                }

                realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x1C));
                startIndex = (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x24);
                headerSizeIndex = (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x1C);
                headerRealSizeIndex = (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x24);
                newChuckSize = BitConverter.GetBytes(newSize);

                if(extName == (UInt64)FileHashes.lua && !isLuaCompiled)
                {
                    realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x24));
                    startIndex = (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x30);
                    luaJunkDataSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x2C));
                    newChuckSize = BitConverter.GetBytes(newSize + (luaJunkDataSize + 0xC));
                }
                else if(extName == (UInt64)FileHashes.lua && isLuaCompiled)
                {
                    luaJunkDataSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x2C));
                    realfileSize -= luaJunkDataSize;
                    newChuckSize = BitConverter.GetBytes(newSize + luaJunkDataSize);
                    newRealSize = BitConverter.GetBytes(newSize - 0xC);
                    newfile[8] = BitConverter.GetBytes(luaJunkDataSize)[0];
                    newfile[9] = BitConverter.GetBytes(luaJunkDataSize)[1];
                    newfile[10] = BitConverter.GetBytes(luaJunkDataSize)[2];
                    newfile[11] = BitConverter.GetBytes(luaJunkDataSize)[3];
                }
                else if (extName == (UInt64)FileHashes.mod && !isModCompiled)
                {
                    startIndex = (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x34);
                    realfileSize = BitConverter.ToUInt32(thisReader.unpacked_file, (0x104 + (thisReader.fileCount * filePadding) + (int)fileData[0]) + ((fileIndex * 0x10) + 0x24));
                    newChuckSize = BitConverter.GetBytes(newSize + 0x10);
                    newRealSize = BitConverter.GetBytes(newSize);
                }
                else if (extName == (UInt64)FileHashes.mod && isModCompiled)
                {
                    newRealSize = BitConverter.GetBytes(newSize - 0x10);
                }
            }

            List<byte> old_data = thisReader.unpacked_file.ToList<byte>();
            old_data.RemoveRange(startIndex, (int)realfileSize);
            old_data.InsertRange(startIndex, newfile);
            if (extName == (UInt64)FileHashes.lua || extName == (UInt64)FileHashes.mod)
            {
                foreach (byte b in newRealSize)
                {
                    old_data[headerRealSizeIndex + counter] = b;
                    counter++;
                }
            }
            counter = 0;
            foreach (byte b in newChuckSize)
            {
                old_data[headerSizeIndex + counter] = b;
                counter++;
            }
            counter = 0;
            thisReader.unpacked_file = old_data.ToArray();
            if (File.Exists(filename))
            {
                File.Move(filename, filename + ".bak");
            }
            File.WriteAllBytes(filename, thisReader.unpacked_file);
        }
    }
}
