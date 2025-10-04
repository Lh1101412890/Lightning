using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

using Lightning.Cryptography;

namespace LightningTime
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ApplyInfoModel> ApplyInfos { get; set; }
        private static string FilePath => "ApplyInfos.xml";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e) => SaveXml();

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyInfos = [];
            ReadXml();
            dataGrid.DataContext = ApplyInfos;
        }

        private void NewCode_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(guidBox.Text) || !long.TryParse(timeBox.Text, out long time))
            {
                return;
            }
            bool forever = foreverBox.IsChecked == true;
            long second = time * 24 * 3600;
            string value = "Product=" + (productBox.SelectedItem as ComboBoxItem).Content + ",Forever=" + forever + ",Time=" + second.ToString();
            string iv = NewIV();
            string key = guidBox.Text.Replace("-", "");
            codeBox.Text = iv + AESHelper.AesEncrypt(value, key, iv);
        }

        private void SaveCode_Click(object sender, RoutedEventArgs e)
        {
            if (long.TryParse(timeBox.Text, out long time) && foreverBox.IsChecked.HasValue)
            {
                if (guidBox.Text != "" && codeBox.Text != "" && (foreverBox.IsChecked.Value || time > 0))
                {
                    if (!ApplyInfos.Exists(x => x.Code == codeBox.Text))
                    {
                        ApplyInfos.Insert(0, new ApplyInfoModel()
                        {
                            Note = note.Text,
                            Guid = guidBox.Text,
                            Product = productBox.Text,
                            IsForever = foreverBox.IsChecked.Value,
                            Days = time,
                            Date = DateTime.Now.ToShortDateString(),
                            Code = codeBox.Text,
                        });
                    }
                }
                dataGrid.DataContext = null;
                dataGrid.DataContext = !string.IsNullOrEmpty(guidBox.Text) ? [.. ApplyInfos.Where(x => x.Guid == guidBox.Text)] : ApplyInfos;
            }
        }

        private void GuidBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.DataContext = null;
            dataGrid.DataContext = !string.IsNullOrEmpty(guidBox.Text) ? [.. ApplyInfos.Where(x => x.Guid == guidBox.Text)] : ApplyInfos;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => Clipboard.SetDataObject((dataGrid.SelectedItem as ApplyInfoModel).Code);

        /// <summary>
        /// 生成向量IV
        /// </summary>
        /// <returns></returns>
        private static string NewIV()
        {
            Random random = new();
            int[] iv = new int[16];
            for (int i = 0; i < 16; i++)
            {
                iv[i] = random.Next(64);
            }

            string ivStr = "";
            foreach (int i in iv)
            {
                ivStr += GetBase64(i);
            }
            return ivStr;
        }

        /// <summary>
        /// 0-64对应的Base64字符
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static string GetBase64(int i) => i switch
        {
            0 => "A",
            1 => "B",
            2 => "C",
            3 => "D",
            4 => "E",
            5 => "F",
            6 => "G",
            7 => "H",
            8 => "I",
            9 => "J",
            10 => "K",
            11 => "L",
            12 => "M",
            13 => "N",
            14 => "O",
            15 => "P",
            16 => "Q",
            17 => "R",
            18 => "S",
            19 => "T",
            20 => "U",
            21 => "V",
            22 => "W",
            23 => "X",
            24 => "Y",
            25 => "Z",
            26 => "a",
            27 => "b",
            28 => "c",
            29 => "d",
            30 => "e",
            31 => "f",
            32 => "g",
            33 => "h",
            34 => "i",
            35 => "j",
            36 => "k",
            37 => "l",
            38 => "m",
            39 => "n",
            40 => "o",
            41 => "p",
            42 => "q",
            43 => "r",
            44 => "s",
            45 => "t",
            46 => "u",
            47 => "v",
            48 => "w",
            49 => "x",
            50 => "y",
            51 => "z",
            52 => "0",
            53 => "1",
            54 => "2",
            55 => "3",
            56 => "4",
            57 => "5",
            58 => "6",
            59 => "7",
            60 => "8",
            61 => "9",
            62 => "+",
            63 => "/",
            _ => "L",
        };

        public void ReadXml()
        {
            if (File.Exists(FilePath))
            {
                XmlDocument document = new();
                document.Load(FilePath);//加载xml文件
                XmlElement root = document.DocumentElement;//根
                foreach (XmlNode node in root.ChildNodes)
                {
                    ApplyInfos.Add(new ApplyInfoModel()
                    {
                        Note = node["Note"].InnerText,
                        Guid = node["Guid"].InnerText,
                        Product = node["Product"].InnerText,
                        Date = node["Date"].InnerText,
                        IsForever = bool.Parse(node["IsForever"].InnerText),
                        Days = long.Parse(node["Days"].InnerText),
                        Code = node["Code"].InnerText,
                    });
                }
            }
        }

        public void SaveXml()
        {
            //通过xml模板文件创建内存xml
            XmlDocument document = new();
            document.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<AppInfos>
</AppInfos>");
            XmlElement root = document.DocumentElement;

            //内存解析FansModels至xml
            foreach (var info in ApplyInfos)
            {
                XmlNode node = document.CreateElement(FilePath);

                XmlNode note = document.CreateElement(nameof(info.Note));
                note.InnerText = info.Note;
                node.AppendChild(note);

                XmlNode guid = document.CreateElement(nameof(info.Guid));
                guid.InnerText = info.Guid;
                node.AppendChild(guid);

                XmlNode product = document.CreateElement(nameof(info.Product));
                product.InnerText = info.Product;
                node.AppendChild(product);

                XmlNode date = document.CreateElement(nameof(info.Date));
                date.InnerText = info.Date;
                node.AppendChild(date);

                XmlNode isForever = document.CreateElement(nameof(info.IsForever));
                isForever.InnerText = info.IsForever.ToString();
                node.AppendChild(isForever);

                XmlNode days = document.CreateElement(nameof(info.Days));
                days.InnerText = info.Days.ToString();
                node.AppendChild(days);

                XmlNode code = document.CreateElement(nameof(info.Code));
                code.InnerText = info.Code.ToString();
                node.AppendChild(code);

                root.AppendChild(node);
            }

            //保存xml文件
            document.Save("ApplyInfos.xml");
        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = e.OriginalSource as MenuItem;
            Label label = menu.Header as Label;
            switch (label.Content.ToString())
            {
                case "删除":
                    List<ApplyInfoModel> applyInfoModels = [.. dataGrid.SelectedItems.Cast<ApplyInfoModel>()];
                    foreach (var item in applyInfoModels)
                    {
                        ApplyInfos.Remove(item);
                    }
                    dataGrid.DataContext = null;
                    dataGrid.DataContext = !string.IsNullOrEmpty(guidBox.Text) ? [.. ApplyInfos.Where(x => x.Guid == guidBox.Text)] : ApplyInfos;
                    break;
                default:
                    break;
            }
        }
    }
}