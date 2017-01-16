using System;
using System.Threading;
using System.Windows.Threading;
using Pokemon3D.Common.Diagnostics;

namespace Pokemon3D.Common
{
    public class JobSystem
    {
        private readonly Dispatcher _mainThreadDispatcher;

        public JobSystem()
        {
            _mainThreadDispatcher = Dispatcher.CurrentDispatcher;
        }

        public void EnsureExecutedInMainThread(Action action)
        {
            if (_mainThreadDispatcher.Thread == Thread.CurrentThread)
            {
                action();
            }
            else
            {
                _mainThreadDispatcher.Invoke(action);
            }
        }

        public void ExecuteBackgroundJob(Action action, Action onFinished = null)
        {
            ThreadPool.QueueUserWorkItem(s =>
            {
                try
                {
                    action();
                    onFinished?.Invoke();
                }
                catch (Exception ex)
                {
                    GameLogger.Instance.Log(ex);
                }
            });
        }
    }
}
