using Pokemon3D.Editor.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Editor.Core.DetailViewModels
{
    public abstract class DetailViewModel : ViewModel
    {
        internal abstract void OnActivate();

        internal abstract void OnDeactivate();
    }
}
