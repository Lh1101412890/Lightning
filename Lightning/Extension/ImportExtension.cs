using System;
using System.Runtime.InteropServices;

namespace Lightning.Extension
{
    public static partial class ImportExtension
    {
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        public static IntPtr FindWindow(string lpClassName, string lpWindowName) => FindWindowW(lpClassName, lpWindowName);

        public static unsafe void Free(void* p) => Free_Inner(p);

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong) => SetWindowLongPtrW(hWnd, nIndex, dwNewLong);

        public static int SetFocus(IntPtr hWnd) => SetFocus_Inner(hWnd);

        public static IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam) => SendMessage_Inner(hWnd, Msg, wParam, lParam);

        public static IntPtr GetModuleHandle(string name) => GetModuleHandle_Inner(name);

        public static int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId)
            => SetWindowsHookEx_Inner(idHook, lpfn, hInstance, threadId);

        public static bool UnhookWindowsHookEx(int idHook) => UnhookWindowsHookEx_Inner(idHook);

        public static int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState)
            => ToAscii_Inner(uVirtKey, uScanCode, lpbKeyState, lpwTransKey, fuState);

        public static int GetKeyboardState(byte[] pbKeyState) => GetKeyboardState_Inner(pbKeyState);

        public static int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam)
            => CallNextHookEx_Inner(idHook, nCode, wParam, lParam);
    }

    public static partial class ImportExtension
    {
#if N8
        [LibraryImport("user32.dll", EntryPoint = "FindWindowW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial IntPtr FindWindowW(string lpClassName, string lpWindowName);

        [LibraryImport("LightningCore.dll", EntryPoint = "Free")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static unsafe partial void Free_Inner(void* p);

        [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        private static partial IntPtr SetWindowLongPtrW(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [LibraryImport("user32.dll", EntryPoint = "SetFocus")]
        private static partial int SetFocus_Inner(IntPtr hWnd);

        [LibraryImport("user32.dll", EntryPoint = "SendMessageW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial IntPtr SendMessage_Inner(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        // 使用 WINDOWS API 函数代替获取当前实例的函数,防止钩子失效
        [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial IntPtr GetModuleHandle_Inner(string name);

        // 使用此功能，安装一个钩子
        [LibraryImport("user32.dll", EntryPoint = "SetWindowsHookExW")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
        public static partial int SetWindowsHookEx_Inner(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        // 调用此函数卸载钩子
        [LibraryImport("user32.dll", EntryPoint = "UnhookWindowsHookEx")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool UnhookWindowsHookEx_Inner(int idHook);

        /// <summary>
        /// 将虚拟键码和键盘状态翻译为对应的 ANSI 字符。
        /// 注意：ToAscii只进行 ANSI 翻译，不支持 Unicode 或复杂输入法/死键。需处理 Unicode 或 IME 的场景请使用 ToUnicode/ToUnicodeEx。
        /// </summary>
        /// <param name="uVirtKey">[In] 虚拟键码（Virtual-Key code，例如 VK_A）。</param>
        /// <param name="uScanCode">[In] 硬件扫描码（scan code）。</param>
        /// <param name="lpbKeyState">[In] 长度为256 的字节数组，表示每个虚拟键的当前状态（通常由 GetKeyboardState 填充）。</param>
        /// <param name="lpwTransKey">[Out] 输出缓冲区，用于接收翻译得到的字符（通常写入1 或2 个字节）。</param>
        /// <param name="fuState">[In] 指示是否有菜单处于活动状态（0 或1）。</param>
        /// <returns>写入到 lpwTransKey 的字符数量（0、1 或2）。</returns>
        [LibraryImport("user32.dll", EntryPoint = "ToAscii")]
        private static partial int ToAscii_Inner(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        /// <summary>
        /// 获取按键的状态
        /// </summary>
        /// <param name="pbKeyState">[Out] 将状态写入到pbKeyState中</param>
        /// <returns></returns>
        [LibraryImport("user32.dll", EntryPoint = "GetKeyboardState")]
        private static partial int GetKeyboardState_Inner(byte[] pbKeyState);

        // 使用此功能，通过信息钩子继续下一个钩子
        [LibraryImport("user32.dll", EntryPoint = "CallNextHookEx")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
        private static partial int CallNextHookEx_Inner(int idHook, int nCode, Int32 wParam, IntPtr lParam);

#else
        [DllImport("user32.dll", EntryPoint = "FindWindowW", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowW(string lpClassName, string lpWindowName);

        [DllImport("LightningCore.dll", EntryPoint = "Free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void Free_Inner(void* p);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtrW(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetFocus")]
        private static extern int SetFocus_Inner(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage_Inner(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        // 使用WINDOWS API函数代替获取当前实例的函数,防止钩子失效
        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW")]
        private static extern IntPtr GetModuleHandle_Inner(string name);

        //使用此功能，安装一个钩子
        [DllImport("user32.dll", EntryPoint = "SetWindowsHookExW", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx_Inner(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //调用此函数卸载钩子
        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx_Inner(int idHook);

        /// <summary>
        /// 将虚拟键码和键盘状态翻译为对应的 ANSI 字符。
        /// 注意：ToAscii只进行 ANSI 翻译，不支持 Unicode 或复杂输入法/死键。需处理 Unicode 或 IME 的场景请使用 ToUnicode/ToUnicodeEx。
        /// </summary>
        /// <param name="uVirtKey">[In] 虚拟键码（Virtual-Key code，例如 VK_A）。</param>
        /// <param name="uScanCode">[In] 硬件扫描码（scan code）。</param>
        /// <param name="lpbKeyState">[In] 长度为256 的字节数组，表示每个虚拟键的当前状态（通常由 GetKeyboardState 填充）。</param>
        /// <param name="lpwTransKey">[Out] 输出缓冲区，用于接收翻译得到的字符（通常写入1 或2 个字节）。</param>
        /// <param name="fuState">[In] 指示是否有菜单处于活动状态（0 或1）。</param>
        /// <returns>写入到 lpwTransKey 的字符数量（0、1 或2）。</returns>
        [DllImport("user32", EntryPoint = "ToAscii")]
        private static extern int ToAscii_Inner(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        /// <summary>
        /// 获取按键的状态
        /// </summary>
        /// <param name="pbKeyState">[Out] 将状态写入到pbKeyState中</param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "GetKeyboardState")]
        private static extern int GetKeyboardState_Inner(byte[] pbKeyState);

        //使用此功能，通过信息钩子继续下一个钩子
        [DllImport("user32.dll", EntryPoint = "CallNextHookEx", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx_Inner(int idHook, int nCode, Int32 wParam, IntPtr lParam);

#endif
    }
}