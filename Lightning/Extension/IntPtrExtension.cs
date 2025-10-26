using System;

namespace Lightning.Extension
{
    /// <summary>
    /// 窗口句柄扩展
    /// </summary>
    public static class IntPtrExtension
    {
        /// <summary>
        /// 使hWnd窗口成为活动窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static int Focus(this IntPtr hWnd) => ImportExtension.SetFocus(hWnd);

        /// <summary>
        /// 设置窗口拥有者
        /// </summary>
        /// <param name="hWnd">被设置窗口句柄</param>
        /// <param name="hWndOwner">窗口拥有者的句柄</param>
        /// <returns>如果设置成功返回上一个窗口拥有者的句柄，否则返回0</returns>
        internal static IntPtr SetOwner(this IntPtr hWnd, IntPtr hWndOwner) => ImportExtension.SetWindowLongPtr(hWnd, -8, hWndOwner);

        /// <summary>
        /// 向窗口发送消息
        /// </summary>
        /// <param name="hwnd">接收窗口句柄</param>
        /// <param name="Msg">消息</param>
        /// <param name="wParam">默认填IntPtr.Zero</param>
        /// <param name="lParam">默认填IntPtr.Zero</param>
        public static void SendMessage(this IntPtr hwnd, int Msg, IntPtr wParam, IntPtr lParam)
            => ImportExtension.SendMessage(hwnd, Msg, wParam, lParam);

    }
}