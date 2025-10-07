using System;
using System.Diagnostics;

using Lightning.Manager;

using Microsoft.Win32;

namespace Lightning.Extension
{
    public static class GodExtension
    {
        /// <summary>
        /// 获取注册表中指定位置的值
        /// </summary>
        /// <param name="god">对应的插件类型</param>
        /// <param name="loction">对应产品下的位置，例如LightningCAD项下</param>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>没有注册表时返回false, value为null; 成功时true</returns>
        public static bool GetValue(this God god, string loction, string key, out object value)
        {
            string str = $"Software\\Lightning\\{god.ProductName}" + (loction == null ? "" : $"\\{loction}");
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(str, false))
            {
                switch (registry)
                {
                    case null:
                        value = null;
                        return false;
                    default:
                        value = registry.GetValue(key);
                        return true;
                }
            }
        }

        /// <summary>
        /// 设置注册表中指定位置的值
        /// </summary>
        /// <param name="god">对应的插件类型</param>
        /// <param name="loction">对应产品下的位置，例如LightningCAD项下</param>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>未安装插件时返回false并不设置，否则进行设置</returns>
        public static bool SetValue(this God god, string loction, string key, object value)
        {
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey($"Software\\Lightning\\{god.ProductName}", true))
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
        /// <param name="god"></param>
        /// <param name="message">提示信息</param>
        /// <param name="time">显示时长（秒）</param>
        /// <param name="always">是否一直显示</param>
        public static void ShowMessage(this God god, string message, int time = 0, bool always = false)
        {
            // 启动消息窗口
            Process[] processes = Process.GetProcessesByName("LightningMessage");
            if (processes.Length == 0)
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                {
                    FileName = god.GetFileInfo("LightningMessage.exe").FullName,
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

            IntPtr intPtr = LTools.FindMessageWindow();
            intPtr.Send_PAINT();
        }

    }
}