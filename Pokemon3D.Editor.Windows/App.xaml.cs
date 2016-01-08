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
            var applicationViewModel = new ApplicationViewModel(new PlatformServiceImp());
            MainWindow = new MainWindow();
            MainWindow.DataContext = applicationViewModel;
            MainWindow.Show();
        }
    }
}
