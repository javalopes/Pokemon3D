using System;

namespace Pokemon3D.UI.Framework.Dialogs
{
    abstract class ModalDialog : ControlGroup
    {
        public event Action<ControlGroup> Shown;

        public event Action<ControlGroup> Closed;

        public virtual void Show()
        {
            Active = true;
            Visible = true;

            OnShow();
        }

        public virtual void Close()
        {
            Active = false;
            Visible = false;

            OnClose();
        }

        protected void OnShow()
        {
            if (Shown != null)
                Shown(this);
        }

        protected void OnClose()
        {
            if (Closed != null)
                Closed(this);
        }
    }
}
