using Pokemon3D.Editor.Windows.View3D;
using System;
using System.Threading;
using System.Windows;

namespace Pokemon3D.Editor.Windows
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public D3D11Host View3D { get { return View3DControl; } }
    }
}
