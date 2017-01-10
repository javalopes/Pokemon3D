﻿namespace Pokemon3D.Common
{
    public abstract class IdObject
    {
        private static readonly object LockObject = new object();

        private static int _currentMaxId = 0;

        public int Id { get; }

        protected IdObject()
        {
            lock (LockObject)
            {
                Id = ++_currentMaxId;
            }
        }
    }
}
