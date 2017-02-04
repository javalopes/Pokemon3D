using System;

namespace Pokemon3D.GameCore
{
    internal class MessageData
    {
        public string Text { get; set; }

        public Action OnFinished { get; set; }
    }
}