using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pokemon3D.Common.Diagnostics;

namespace Pokemon3D.GameCore
{
    class JobSystem
    {
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
