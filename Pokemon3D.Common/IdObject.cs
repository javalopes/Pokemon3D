namespace Pokemon3D.Common
{
    public abstract class IdObject
    {
        private static readonly object _lockObject = new object();

        private static int _currentMaxId = 0;

        public int Id { get; }

        protected IdObject()
        {
            lock (_lockObject)
            {
                Id = ++_currentMaxId;
            }
        }
    }
}
