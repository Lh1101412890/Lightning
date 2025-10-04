using System;
using System.Runtime.InteropServices;

namespace Lightning.Extension
{
    /// <summary>
    /// 窗口句柄扩展
    /// </summary>
    public static partial class IntPtrExtension
    {
        /// <summary>
        /// 使hWnd窗口成为活动窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static int Focus(this IntPtr hWnd)
        {
            return SetFocus_Inner(hWnd);
        }

        /// <summary>
        /// 设置窗口拥有者
        /// </summary>
        /// <param name="hWnd">被设置窗口句柄</param>
        /// <param name="hWndOwner">窗口拥有者的句柄</param>
        /// <returns>如果设置成功返回上一个窗口拥有者的句柄，否则返回0</returns>
        internal static IntPtr SetOwner(this IntPtr hWnd, IntPtr hWndOwner)
        {
            return SetWindowLongPtrImp_Inner(hWnd, -8, hWndOwner);
        }

        /// <summary>
        /// 向窗口发送消息
        /// </summary>
        /// <param name="hwnd">接收窗口句柄</param>
        /// <param name="Msg">消息</param>
        /// <param name="wParam">默认填IntPtr.Zero</param>
        /// <param name="lParam">默认填IntPtr.Zero</param>
        public static void SendMessage(this IntPtr hwnd, int Msg, IntPtr wParam, IntPtr lParam)
        {
            SendMessage_Inner(hwnd, Msg, wParam, lParam);
        }

        /// <summary>
        /// 向窗口发送PAINT消息
        /// </summary>
        /// <param name="hwnd">接收窗口句柄</param>
        public static void Send_PAINT(this IntPtr hwnd)
        {
            const int WM_PAINT = 0x000F;
            SendMessage_Inner(hwnd, WM_PAINT, IntPtr.Zero, IntPtr.Zero);
        }
    }

    public partial class IntPtrExtension
    {
#if N8
        [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        private static partial IntPtr SetWindowLongPtrImp_Inner(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [LibraryImport("user32.dll", EntryPoint = "SetFocus")]
        private static partial int SetFocus_Inner(IntPtr hWnd);

        [LibraryImport("user32.dll", EntryPoint = "SendMessageW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial IntPtr SendMessage_Inner(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

#else
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtrImp_Inner(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetFocus")]
        private static extern int SetFocus_Inner(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage_Inner(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
#endif
    }
}