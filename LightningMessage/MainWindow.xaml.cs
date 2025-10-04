using System;
using System.Diagnostics;
using System.IO;
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
                Process.Start("Explorer", "https://space.bilibili.com/191930682");
                ShowMessage("", 0);
            };
        }

        private void MessageView_SourceInitialized(object sender, EventArgs e)
        {
            Left = SystemParameters.WorkArea.Width / 2 - 250;
            Top = 0;
            Visibility = Visibility.Hidden;

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            //添加窗口重绘消息处理事件
            hwndSource?.AddHook(WndProc);

            //显示消息
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

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_PAINT = 0x000F;
            // Windows重绘消息
            if (msg == WM_PAINT)
            {
                //读取xml文件
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
            return IntPtr.Zero;
        }

        private class MessageStruct
        {
            public string Message;
            public int Time;
            public bool Always;
            public MessageStruct(string message, int time, bool always)
            {
                Message = message;
                Time = time;
                Always = always;
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
            }
            return new MessageStruct(message, time, always);
        }

        private Timer timer = null;
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="time"></param>
        /// <param name="always"></param>
        public void ShowMessage(string message, int time, bool always = false)
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
                    if (time == 0)
                    {
                        Visibility = Visibility.Hidden;
                        messageBox.Text = "";
                    }
                    else
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
                    }
                }
            });
        }

        /// <summary>
        /// 设置消息缓存
        /// </summary>
        /// <param name="message"></param>
        /// <param name="time"></param>
        /// <param name="always"></param>
        public static void SetMessage(string message, int time, bool always)
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

        private void MenuItem_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}