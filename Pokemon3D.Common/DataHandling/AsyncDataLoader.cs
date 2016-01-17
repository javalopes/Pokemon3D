using System;
using System.Collections.Generic;
using System.Threading;

namespace Pokemon3D.Common.DataHandling
{
    public abstract class AsyncDataLoader
    {
        private bool _isWorkerThreadRunning;
        private readonly object _lockObject = new object();
        private readonly Queue<DataRequest> _dataRequests = new Queue<DataRequest>();

        private void EnsureWorkerThreadIsRunning()
        {
            if (_isWorkerThreadRunning) return;

            _isWorkerThreadRunning = true;
            ThreadPool.QueueUserWorkItem(OnLoadResources);
        }

        private void OnLoadResources(object state)
        {
            while (true)
            {
                DataRequest request;
                lock (_lockObject)
                {
                    if (_dataRequests.Count == 0) break;
                    request = _dataRequests.Dequeue();

                    var list = new List<DataLoadResult>();
                    foreach(var resourcePath in request.ResourcePath)
                    {
                        list.Add(OnRequestData(resourcePath));
                    }
                    request.NotifyEnded(list.ToArray());
                }
            }
            _isWorkerThreadRunning = false;
        }

        protected abstract DataLoadResult OnRequestData(string resourcePath);

        protected void LoadAsync(string[] resourcePaths, Action<DataLoadResult[]> onEnded)
        {
            EnsureWorkerThreadIsRunning();
            lock (_lockObject)
            {
                _dataRequests.Enqueue(new DataRequest(resourcePaths, onEnded));
            }
        }

        protected void LoadAsync(string resourcePath, Action<DataLoadResult> onEnded)
        {
            LoadAsync(new[] { resourcePath }, a => OnDelegateMultiToSingleCast(a, onEnded));
        }

        private void OnDelegateMultiToSingleCast(DataLoadResult[] requests, Action<DataLoadResult> onEnded)
        {
            onEnded(requests[0]);
        }
    }
}
