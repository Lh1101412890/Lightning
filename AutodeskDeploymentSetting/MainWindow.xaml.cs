using System;
using System.IO;
using System.Windows;

namespace AutodeskDeploymentSetting
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 1、将Release的程序复制到部署目录下，并将批处理文件重命名为“第2步，点我安装.bat”
    /// 2、在部署目录下image文件夹中，创建“Path.txt”文件，并写入初始路径如“D:\CAD2023”
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //当前运行的卸载程序路径,在.net Framework中FriendlyName有后缀名（.exe）,在.net 8中不带后缀名
            string current = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName;
            FileInfo fileInfo = new FileInfo(current);
            DirectoryInfo directory = fileInfo.Directory;

            string path = directory.FullName + "\\image\\Path.txt";
            string old = File.ReadAllText(path);
            File.WriteAllText(path, directory.FullName);

            string bat = directory.FullName + "\\第2步，点我安装.bat";
            string v = File.ReadAllText(bat);
            v = v.Replace(old, directory.FullName);
            File.WriteAllText(bat, v);

            string collection = directory.FullName + "\\image\\Collection.xml";
            string collectionData = File.ReadAllText(collection);
            collectionData = collectionData.Replace(old, directory.FullName);
            File.WriteAllText(collection, collectionData);

            string odisver = directory.FullName + "\\image\\odisver.xml";
            string odisverData = File.ReadAllText(odisver);
            odisverData = odisverData.Replace(old, directory.FullName);
            File.WriteAllText(odisver, odisverData);

            MessageBox.Show("重定向完成");
            Close();
        }
    }
}