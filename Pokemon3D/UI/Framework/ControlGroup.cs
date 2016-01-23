using Pokemon3D.Common.Input;
using Pokemon3D.GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework
{
    class ControlGroup
    {
        private List<Control> _controls;

        public ControlGroup()
        {
            _controls = new List<Control>();
        }

        public void SelectionChange(int change)
        {
            if (!_controls.Any(x => x.Selected))
            {
                _controls[0].Select();
            }
            else
            {
                int currentIndex = _controls.IndexOf(_controls.Single(x => x.Selected));
                currentIndex += change;
                while (currentIndex < 0)
                    currentIndex += _controls.Count;
                while (currentIndex >= _controls.Count)
                    currentIndex -= _controls.Count;

                _controls[currentIndex].Select();
            }
        }

        public void SetSelection(int index)
        {
            _controls[index].Select();
        }

        public void Update()
        {
            if (GameController.Instance.InputSystem.Up(true, DirectionalInputTypes.All))
            {
                SelectionChange(-1);
            }
            if (GameController.Instance.InputSystem.Down(true, DirectionalInputTypes.All))
            {
                SelectionChange(1);
            }

            _controls.ForEach(b => b.Update());
        }

        public void Draw()
        {
            _controls.ForEach(b => b.Draw());
        }

        public void ForEach(Action<Control> action)
        {
            _controls.ForEach(action);
        }

        public void Add(Control control)
        {
            control.Group = this;
            _controls.Add(control);
        }

        public void AddRange(Control[] controls)
        {
            foreach (var control in controls)
                Add(control);
        }
    }
}
