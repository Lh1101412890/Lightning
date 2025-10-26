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
        /// 将original目录下的所有指定后缀名的文件复制到target目录下
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target">目标位置</param>
        /// <param name="ext">例如".dll"，默认为"*"</param>
        /// <param name="IncludingChildren">是否包括所有子文件夹</param>
        public static void XCopy(DirectoryInfo original, DirectoryInfo target, string ext = "*", bool IncludingChildren = true)
        {
            foreach (FileInfo file in original.GetFiles())
            {
                if (ext == "*" || file.Extension == ext)
                {
                    file.CopyTo(new FileInfo(target.FullName + "\\" + file.Name));
                }
            }
            if (!IncludingChildren)
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
}