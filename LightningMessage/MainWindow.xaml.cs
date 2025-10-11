using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Interop;

using Microsoft.Win32;

namespace LightningMessage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += MessageView_SourceInitialized;
            MouseLeftButtonUp += (s, e) =>
            {
                if (ReadMessage().Message == "Lightning插件作者：【不要干施工】，点击去b站充电，插件群：785371506！")
                {
                    Process.Start("Explorer", "https://space.bilibili.com/191930682");
                    ShowMessage("", 0);
                }
            };
        }

        private void MessageView_SourceInitialized(object sender, EventArgs e)
        {
            Left = SystemParameters.WorkArea.Width / 2 - 250;
            Top = 0;
            Visibility = Visibility.Hidden;

            // 获取窗口句柄
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            // 添加窗口消息处理钩子
            hwndSource?.AddHook(WndProc);

            Paint();
        }

        /// <summary>
        /// 窗口消息处理函数，用于拦截并处理特定的 Windows 消息。
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="msg">消息标识</param>
        /// <param name="wParam">消息参数</param>
        /// <param name="lParam">消息参数</param>
        /// <param name="handled">指示消息是否已被处理</param>
        /// <returns>处理结果指针</returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_PAINT = 0x000F; // Windows重绘消息常量
                                         // 判断是否为重绘消息
            if (msg == WM_PAINT)
            {
                // 执行自定义绘制逻辑
                Paint();
            }
            // 返回 IntPtr.Zero 表示未处理其他消息
            return IntPtr.Zero;
        }

        /// <summary>
        /// 读取消息并显示
        /// </summary>
        private void Paint()
        {
            MessageStruct messageStruct = ReadMessage();
            if (messageStruct != null)
            {
                ShowMessage(messageStruct.Message, messageStruct.Time, messageStruct.Always);
            }
            else
            {
                ShowMessage("", 0);
            }
        }

        /// <summary>
        /// 读取消息缓存
        /// </summary>
        /// <returns></returns>
        private MessageStruct ReadMessage()
        {
            string message;
            int time;
            bool always;
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey($"Software\\Lightning", false))
            {
                if (registry == null)
                {
                    return null; // 没有注册表项时返回null
                }
                message = (string)registry.GetValue("Text");
                time = (int)registry.GetValue("Time");
                always = bool.Parse(registry.GetValue("Always").ToString());
                registry.Close();
            }
            return new MessageStruct(message, time, always);
        }

        /// <summary>
        /// 设置消息缓存
        /// </summary>
        /// <param name="message"></param>
        /// <param name="time"></param>
        /// <param name="always"></param>
        private static void SetMessage(string message, int time, bool always)
        {
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey($"Software", true))
            {
                using (RegistryKey key = registry.OpenSubKey("Lightning", true) ?? registry.CreateSubKey("Lightning", true))
                {
                    key.SetValue("Text", message);
                    key.SetValue("Time", time);
                    key.SetValue("Always", always.ToString());
                    key.Close();
                }
                registry.Close();
            }
        }

        private Timer timer = null;
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="time"></param>
        /// <param name="always"></param>
        private void ShowMessage(string message, int time, bool always = false)
        {
            timer?.Stop();
            timer?.Dispose();
            Dispatcher.Invoke(() =>
            {
                if (always)
                {
                    Visibility = Visibility.Visible;
                    messageBox.Text = message;
                }
                else
                {
                    switch (time)
                    {
                        case 0:
                            Visibility = Visibility.Hidden;
                            messageBox.Text = "";
                            break;

                        default:
                            {
                                Visibility = Visibility.Visible;
                                messageBox.Text = message;
                                timer = new Timer
                                {
                                    Interval = time * 1000
                                };
                                timer.Elapsed += (sender, e) =>
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Visibility = Visibility.Hidden;
                                        messageBox.Text = "";
                                    });
                                    SetMessage("", 0, false);
                                    timer.Stop();
                                    timer.Dispose();
                                };
                                timer.Start();
                                break;
                            }
                    }
                }
            });
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    internal class MessageStruct
    {
        internal MessageStruct(string message, int time, bool always)
        {
            Message = message;
            Time = time;
            Always = always;
        }
        internal string Message;
        internal int Time;
        internal bool Always;
    }
}