using Pokemon3D.Editor.Core;
using System.Windows;

namespace Pokemon3D.Editor.Windows
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            var applicationViewModel = new ApplicationViewModel(new PlatformServiceImp());

            MainWindow = mainWindow;
            MainWindow.DataContext = applicationViewModel;
            MainWindow.Show();
        }
    }
}
