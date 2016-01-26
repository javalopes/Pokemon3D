using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Used to control switching between multiple control groups on the same screen.
    /// </summary>
    class MasterControlGroup
    {
        private List<ControlGroup> _groups = new List<ControlGroup>();

        public void AddGroup(ControlGroup group)
        {
            _groups.Add(group);
            group.Activated += GroupActivated;
        }

        public void GroupActivated(ControlGroup activatedGroup)
        {
            foreach (var group in _groups)
                if (group != activatedGroup)
                    group.Active = false;
        }
    }
}
