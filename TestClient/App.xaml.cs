using System.Windows;

namespace TestClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow
            {
                DataContext = new ApplicationViewModel(new ApplicationModel())
            };
            MainWindow.Show();
        }
    }
}
