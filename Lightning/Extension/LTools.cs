using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Lightning.Extension
{
    public static partial class LTools
    {
        /// <summary>
        /// 获取指针s指向的字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static unsafe string GetString(char* s)
        {
            int length = 0;
            for (length = 0; s[length] != 0; length++)
            {
            }
            return Marshal.PtrToStringUni((IntPtr)s, length);
        }

        /// <summary>
        /// 查找LightningMessage窗口的句柄
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns></returns>
        public static IntPtr FindMessageWindow()
        {
            return FindWindow(null, "LightningMessage");
        }

        /// <summary>
        /// 释放指针p指向的内存
        /// </summary>
        /// <param name="p"></param>
        public static unsafe void Free(void* p)
        {
            _Free(p);
        }

        public static void XCopy(DirectoryInfo original, DirectoryInfo target, string ext, bool hasDir)
        {
            foreach (FileInfo file in original.GetFiles())
            {
                if (ext == "*" || file.Extension == ext)
                {
                    file.CopyTo(new FileInfo(target.FullName + "\\" + file.Name));
                }
            }
            if (!hasDir)
            {
                return;
            }
            foreach (DirectoryInfo directoryInfo in original.GetDirectories())
            {
                DirectoryInfo directory = new DirectoryInfo(target.FullName + "\\" + directoryInfo.Name);
                if (!directory.Exists)
                {
                    directory.Create();
                }
                XCopy(directoryInfo, directory, ext, true);
            }
        }

    }

    public static partial class LTools
    {
#if N8
        [LibraryImport("user32.dll", EntryPoint = "FindWindowW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial IntPtr FindWindow(string lpClassName, string lpWindowName);

        [LibraryImport("LightningCore.dll", EntryPoint = "Free")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static unsafe partial void _Free(void* p);
#else
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("LightningCore.dll", EntryPoint = "Free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void _Free(void* p);
#endif
    }
}