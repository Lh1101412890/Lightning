using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using static Lightning.Extension.ImportExtension;

namespace Lightning.Extension
{
    /// <summary>
    /// 全局键盘钩子
    /// </summary>
    public class KeyboardHook
    {
        public event KeyEventHandler KeyDownEvent;
        public event KeyPressEventHandler KeyPressEvent;
        public event KeyEventHandler KeyUpEvent;
        private HookProc hookProc;
        private const int WM_KEYDOWN = 0x100; // KEYDOWN
        private const int WM_KEYUP = 0x101; // KEYUP
        private const int WM_SYSKEYDOWN = 0x104; // SYSKEYDOWN
        private const int WM_SYSKEYUP = 0x105; // SYSKEYUP
        private const int WH_KEYBOARD_LL = 13; // 全局钩子
        private int key = 0;

        public void Start()
        {
            // 创建HookProc实例，HookProc在类下面，方法执行完毕后不会被垃圾回收
            hookProc = new HookProc(KeyboardHookProc);
            // 获取当前主模块的句柄
            IntPtr intPtr = GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);
            // 安装钩子
            key = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, intPtr, 0);
        }

        public void Stop()
        {
            if (key != 0)
            {
                UnhookWindowsHookEx(key);
            }
        }

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // 侦听键盘事件
            if ((nCode >= 0) && (KeyDownEvent != null || KeyUpEvent != null || KeyPressEvent != null))
            {
                // 从回调传入的指针 `lParam` 中将非托管数据封送（marshal）为 `KeyboardHookStruct`结构。
                // 该结构包含此次键盘事件的详细信息：虚拟键码（vkCode）、硬件扫描码（scanCode）、标志（flags）、时间戳（time）和额外信息（dwExtraInfo）。
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                int vkCode = MyKeyboardHookStruct.vkCode;
                int scanCode = MyKeyboardHookStruct.scanCode;
                int flags = MyKeyboardHookStruct.flags;
                KeyEventArgs e = new KeyEventArgs((Keys)vkCode);
                // 触发 KeyDown
                if (KeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    KeyDownEvent(this, e);
                }
                // 键盘按下
                if (KeyPressEvent != null && wParam == WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    if (ToAscii(vkCode, scanCode, keyState, inBuffer, flags) == 1)
                    {
                        KeyPressEventArgs ev = new KeyPressEventArgs((char)inBuffer[0]);
                        KeyPressEvent(this, ev);
                    }
                }
                // 键盘抬起
                if (KeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    KeyUpEvent(this, e);
                }
            }
            return CallNextHookEx(key, nCode, wParam, lParam);
        }
    }
}