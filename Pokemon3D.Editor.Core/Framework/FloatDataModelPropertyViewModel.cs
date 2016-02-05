using Pokemon3D.DataModel;
using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class FloatDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<float>
    {
        public FloatDataModelPropertyViewModel(Action<float> updateModelValue, float value, string caption) 
            : base(updateModelValue, value, caption)
        {
        }
    }

    public class DoubleDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<double>
    {
        public DoubleDataModelPropertyViewModel(Action<double> updateModelValue, double value, string caption)
            : base(updateModelValue, value, caption)
        {
        }
    }

    public class ColorDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<ColorModel>
    {
        public ColorDataModelPropertyViewModel(ColorModel value, string caption)
            : base(a => { }, value, caption)
        {
        }
    }
}