using System;
using System.IO;

using Microsoft.Win32;

namespace Lightning.Information
{
    public static class PCInfo
    {
        static PCInfo()
        {
            RegistryKey registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography");
            string guid = registry.GetValue("MachineGuid").ToString();
            Guid = new Guid(guid);
            registry.Close();
            registry.Dispose();
        }

        /// <summary>
        /// 本机唯一标识码Guid
        /// </summary>
        public static Guid Guid { get; }

        /// <summary>
        /// 我的文档文件夹
        /// </summary>
        public static DirectoryInfo MyDocumentsDirectoryInfo => new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
    }
}
