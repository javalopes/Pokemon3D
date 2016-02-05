using Pokemon3D.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;

namespace Pokemon3D.Editor.Windows.CustomControls
{
    class ColorModelColorPicker : ColorPicker
    {
        public ColorModelColorPicker()
        {
            DataContextChanged += OnDataContextChanged;
            SelectedColorChanged += OnSelectedColorChanged;
        }

        private void OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var colorModel = DataContext as ColorModel;
            if (colorModel == null) return;
            if (!SelectedColor.HasValue) return;

            colorModel.Red = SelectedColor.Value.R;
            colorModel.Green = SelectedColor.Value.G;
            colorModel.Blue = SelectedColor.Value.B;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ColorModel)
            {
                var colorModel = (ColorModel)e.NewValue;
                SelectedColor = new System.Windows.Media.Color()
                {
                    A = 255,
                    R = colorModel.Red,
                    G = colorModel.Green,
                    B = colorModel.Blue
                };
            }
        }
    }
}
