using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Lightning.Extension
{
    public static class FileInfoExtension
    {
        public static BitmapImage ToBitmapImage(this FileInfo fileInfo)
        {
            Uri uri = new Uri(fileInfo.FullName);
            BitmapImage bitmapImage = new BitmapImage(uri);
            return bitmapImage;
        }

        /// <summary>
        /// 如果文件较新则将文件复制到新位置
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="target"></param>
        public static void CopyTo(this FileInfo fileInfo, FileInfo target)
        {
            if (target.Exists)
            {
                if (fileInfo.Length != target.Length || fileInfo.LastWriteTime != target.LastWriteTime)
                {
                    fileInfo.CopyTo(target.FullName, true);
                }
            }
            else
            {
                fileInfo.CopyTo(target.FullName);
            }
        }
    }
}