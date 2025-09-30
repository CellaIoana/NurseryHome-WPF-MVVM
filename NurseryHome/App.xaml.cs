using System.Configuration;
using System.Data;
using System.Windows;
using NurseryHome.Views;

namespace NurseryHome
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var loginWindow = new LoginView();
            loginWindow.Show();
        }

    }

}
