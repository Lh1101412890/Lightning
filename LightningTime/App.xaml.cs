using System.Windows;

namespace LightningTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            mutex = new Mutex(true, nameof(LightningTime), out bool ret);
            if (!ret)
            {
                MessageBox.Show("程序已启动");
                Environment.Exit(0);
            }
        }
    }
}