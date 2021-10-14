using CutMkv.Properties;
using System;
using System.Configuration;
using System.Runtime.ExceptionServices;
using System.Windows;

namespace CutMkv
{
    public partial class App : Application
    {
        [STAThread()]
        public static void Main()
        {
            if (!ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).HasFile)
                Settings.Default.Upgrade();

            App app = new App();
            app.InitializeComponent();
            app.Run();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            StartupUri = new Uri("View/MainWindow.xaml", UriKind.Relative);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {

        }

        private static void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs ex)
        {

        }
    }
}
