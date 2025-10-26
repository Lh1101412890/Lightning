using System;
using System.Diagnostics;
using System.IO;

using Lightning.Extension;

using Microsoft.Win32;

namespace Lightning.Manager
{
    public class God
    {
        public God(GodEnum god)
        {
            Type = god;
            using (RegistryKey registry = Registry.LocalMachine.OpenSubKey($"Software\\Lightning\\{ProductName}"))
            {
                switch (registry)
                {
                    case null:
                        BaseDir = $"D:\\Visual Studio 2022 Projects\\{ProductName}\\Data";
                        break;
                    default:
                        BaseDir = registry.GetValue("Folder").ToString();
                        registry.Close();
                        break;
                }
            }
        }

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

        /// <summary>
        /// 获取注册表中指定位置的值
        /// </summary>
        /// <param name="loction">对应产品下的位置，例如LightningCAD项下</param>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>没有注册表时返回false, value为null; 成功时true</returns>
        public bool GetValue(string loction, string key, out object value)
        {
            string str;
            switch (loction)
            {
                case null:
                    str = $"Software\\Lightning\\{ProductName}";
                    break;
                default:
                    str = $"Software\\Lightning\\{ProductName}\\{loction}";
                    break;
            }
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(str, false))
            {
                if (registry is null)
                {
                    // 注册表路径不存在时就没有安装插件
                    value = null;
                    return false;
                }
                else
                {
                    value = registry.GetValue(key);
                    registry.Close();
                    return true;
                }
            }
        }

        /// <summary>
        /// 设置注册表中指定位置的值
        /// </summary>
        /// <param name="loction">对应产品下的位置，例如LightningCAD项下</param>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>未安装插件时返回false并不设置，否则进行设置</returns>
        public bool SetValue(string loction, string key, object value)
        {
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey($"Software\\Lightning\\{ProductName}", true))
            {
                if (registry == null)
                {
                    // 注册表路径不存在时就没有安装插件
                    return false;
                }
                if (loction == null)
                {
                    registry.SetValue(key, value);
                }
                else
                {
                    using (RegistryKey registryKey = registry.OpenSubKey(loction, true) ?? registry.CreateSubKey(loction, true))
                    {
                        registryKey.SetValue(key, value);
                        registryKey.Close();
                    }
                }
                registry.Close();
                return true;
            }
        }

        /// <summary>
        /// 用消息窗口显示消息
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="time">显示时长（秒）</param>
        /// <param name="always">是否一直显示</param>
        public void ShowMessage(string message, int time = 0, bool always = false)
        {
            // 启动消息窗口
            Process[] processes = Process.GetProcessesByName("LightningMessage");
            if (processes.Length == 0)
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                {
                    FileName = GetFileInfo("LightningMessage.exe").FullName,
                };
                process.StartInfo = processStartInfo;
                process.Start();
            }

            // 设置注册表中的消息内容
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey($"Software", true))
            {
                using (RegistryKey key = registry.OpenSubKey("Lightning", true) ?? registry.CreateSubKey("Lightning", true))
                {
                    key.SetValue("Text", message);
                    key.SetValue("Time", time);
                    key.SetValue("Always", always.ToString());
                    key.Close();
                }
                registry.Close();
            }

            IntPtr intPtr = ImportExtension.FindWindow(null, "LightningMessage");
            // 发送PAINT消息让窗口刷新
            int WM_PAINT = 0x000F;
            intPtr.SendMessage(WM_PAINT, IntPtr.Zero, IntPtr.Zero);
        }

        private GodEnum Type { get; }

        private string BaseDir { get; }
    }
}