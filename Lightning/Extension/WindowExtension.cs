using System;
using System.Windows;
using System.Windows.Interop;

using Lightning.Manager;

namespace Lightning.Extension
{
    /// <summary>
    /// 窗口扩展
    /// </summary>
    public static class WindowExtension
    {
        private const string group = "WindowsLocation";

        /// <summary>
        /// 将窗口位置保存到指定文件
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static void SetLocation(this Window window, God god)
        {
            god.SetValue(group, window.GetType().Name, $"{window.Left},{window.Top}");
        }

        /// <summary>
        /// 从指定文件获取位置，并设置到窗口
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static void GetLocation(this Window window, God god)
        {
            double left = (SystemParameters.WorkArea.Width - window.Width) / 2;
            double top = (SystemParameters.WorkArea.Height - window.Height) / 2;
            bool v = god.GetValue(group, window.GetType().Name, out object value);
            if (v && value != null)
            {
                string[] location = value.ToString().Split(',');
                left = double.Parse(location[0]);
                top = double.Parse(location[1]);
            }
            window.Left = left;
            window.Top = top;
        }

        /// <summary>
        /// 设置窗口拥有者
        /// </summary>
        /// <param name="window">被设置窗口句柄</param>
        /// <param name="hWndOwner">窗口拥有者的句柄</param>
        /// <returns>如果设置成功返回上一个窗口拥有者的句柄，否则返回0</returns>
        public static IntPtr SetOwner(this Window window, IntPtr hWndOwner)
        {
            return new WindowInteropHelper(window).Handle.SetOwner(hWndOwner);
        }

    }
}