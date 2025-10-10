using System;
using System.IO;

using Microsoft.Win32;

namespace Lightning.Information
{
    public static class PCInfo
    {
        static PCInfo()
        {
            using (RegistryKey registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography"))
            {
                string guid = registry.GetValue("MachineGuid").ToString();
                Guid = new Guid(guid);
                registry.Close();
            }
        }

        /// <summary>
        /// 本机唯一标识码Guid
        /// </summary>
        public static Guid Guid { get; }

        /// <summary>
        /// 是否是机械革命 Mechrevo极光pro16
        /// </summary>
        public static bool IsGod => Guid.ToString() == "cbd95518-368e-4133-9d75-eb9d7ac94af3";

        /// <summary>
        /// 我的文档文件夹
        /// </summary>
        public static DirectoryInfo MyDocumentsDirectoryInfo => new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

    }
}