using System;
using System.IO;

using Lightning.Information;

using Microsoft.Win32;

namespace Lightning.Manager
{
    public partial class God
    {
        /// <summary>
        /// "Lightning"
        /// </summary>
        public static string Brand => "Lightning";

        /// <summary>
        /// LightningCAD、LightningRevit、LightningOffice
        /// </summary>
        public string ProductName => Brand + Type.ToString();

        /// <summary>
        /// 错误日志文件路径
        /// </summary>
        public string ErrorLog => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Brand, ProductName, "ErrorLog.txt");

        /// <summary>
        /// 获取随包安装文件路径
        /// </summary>
        /// <param name="lastPath"></param>
        /// <returns></returns>
        public FileInfo GetFileInfo(string lastPath) => new FileInfo(Path.Combine(BaseDir, lastPath));

        internal GodEnum Type { get; }

        private string BaseDir { get; }

        public unsafe God(GodEnum god)
        {
            Type = god;
            using (RegistryKey registry = Registry.LocalMachine.OpenSubKey($"Software\\Lightning\\{ProductName}"))
            {
                BaseDir = registry == null ? $"D:\\Visual Studio 2022 Projects\\{ProductName}\\Data" : registry.GetValue("Folder").ToString();
            }
        }

        /// <summary>
        /// 是否是机械革命 Mechrevo极光pro16
        /// </summary>
        public static bool IsGod => PCInfo.Guid.ToString() == "cbd95518-368e-4133-9d75-eb9d7ac94af3";

    }
}