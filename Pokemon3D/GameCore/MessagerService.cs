using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.GameCore
{
    class MessagerService
    {
        private MessageData _activeMessage;

        public void ShowMessage(MessageData messageData)
        {
            _activeMessage = messageData;
        }

        public void Update(GameTime time)
        {
            
        }
    }

    internal class MessageData
    {
        public string Text { get; set; }

        public Action OnFinished { get; set; }
    }
}
