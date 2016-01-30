using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Input;
using Pokemon3D.GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Groups together controls and manages them.
    /// </summary>
    abstract class ControlGroup : GameObject
    {
        protected List<Control> _controls;

        public List<Control> Controls { get { return _controls; } }

        /// <summary>
        /// If this control group gets drawn.
        /// </summary>
        public bool Visible { get; set; } = false;

        private bool _isActive = false;

        /// <summary>
        /// If this control group gets updated.
        /// </summary>
        public bool Active
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                if (_isActive)
                {
                    _controls.ForEach(c => c.GroupActivated());
                    if (Activated != null)
                        Activated(this);
                }
                else
                {
                    _controls.ForEach(c => c.GroupDeactivated());
                    if (Deactivated != null)
                        Deactivated(this);
                }
            }
        }

        public event Action<ControlGroup> Activated;

        public event Action<ControlGroup> Deactivated;

        /// <summary>
        /// The action that occurs when the selection goes over the lower control limit.
        /// </summary>
        public Action RunOverLowerBound { get; set; } = null;
        /// <summary>
        /// The action that occurs when the selection goes over the upper control limit.
        /// </summary>
        public Action RunOverUpperBound { get; set; } = null;

        protected ControlGroup()
        {
            _controls = new List<Control>();
        }

        /// <summary>
        /// Moves the selection index by a certain amount (negative to move up).
        /// </summary>
        public void MoveSelection(int change)
        {
            if (!_controls.Any(x => x.Selected))
            {
                _controls[0].Select();
            }
            else
            {
                int currentIndex = _controls.IndexOf(_controls.Single(x => x.Selected));
                int previousIndex = currentIndex;

                currentIndex += change;
                while (currentIndex < 0)
                {
                    if (RunOverLowerBound == null)
                        currentIndex += _controls.Count;
                    else
                    {
                        RunOverLowerBound();
                        currentIndex = _controls.IndexOf(_controls.Single(x => x.Selected));
                    }
                }
                while (currentIndex >= _controls.Count)
                {
                    if (RunOverUpperBound == null)
                        currentIndex -= _controls.Count;
                    else
                    {
                        RunOverUpperBound();
                        currentIndex = _controls.IndexOf(_controls.Single(x => x.Selected));
                    }
                }

                if (previousIndex != currentIndex)
                    _controls[currentIndex].Select();

            }
        }

        /// <summary>
        /// Sets the selection index to a given index.
        /// </summary>
        public void SetSelection(int index)
        {
            _controls[index].Select();
        }

        public virtual void Update()
        {
            _controls.ForEach(b => b.Update());
        }

        public virtual void Draw(SamplerState samplerState = null, BlendState blendState = null)
        {
            InternalDraw(Game.SpriteBatch);
        }

        protected void InternalDraw(SpriteBatch spriteBatch)
        {
            if (Visible)
                _controls.ForEach(b => b.Draw(spriteBatch));
        }

        /// <summary>
        /// Performs an action on each control in the group.
        /// </summary>
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
