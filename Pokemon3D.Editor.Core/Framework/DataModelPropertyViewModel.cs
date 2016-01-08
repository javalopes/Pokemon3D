using Pokemon3D.Editor.Core.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Editor.Core.Framework
{
    public abstract class DataModelPropertyViewModel : ViewModel
    {
        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set { SetProperty(ref _caption, value); }
        }
    }
}
