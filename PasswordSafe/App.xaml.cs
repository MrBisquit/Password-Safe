using System.Configuration;
using System.Data;
using System.Windows;

namespace PasswordSafe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Globals.settings = Functions.Settings.GetSettings();
            if(string.IsNullOrEmpty(Globals.settings.LastOpenedSafe))
            {
                Windows.NewSafe newSafe = new Windows.NewSafe();
                newSafe.ShowDialog();
            } else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
