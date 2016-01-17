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

                    var list = new List<DataRequestResult>();
                    foreach(var resourcePath in request.ResourcePath)
                    {
                        var data = OnRequestData(resourcePath);
                        list.Add(new DataRequestResult(data, RequestState.Success));
                    }
                    request.NotifyEnded(list.ToArray());
                }
            }
            _isWorkerThreadRunning = false;
        }

        protected abstract byte[] OnRequestData(string resourcePath);

        protected void LoadAsync(string[] resourcePaths, Action<DataRequestResult[]> onEnded)
        {
            EnsureWorkerThreadIsRunning();
            lock (_lockObject)
            {
                _dataRequests.Enqueue(new DataRequest(resourcePaths, onEnded));
            }
        }

        protected void LoadAsync(string resourcePath, Action<DataRequestResult> onEnded)
        {
            LoadAsync(new[] { resourcePath }, a => OnDelegateMultiToSingleCast(a, onEnded));
        }

        private void OnDelegateMultiToSingleCast(DataRequestResult[] requests, Action<DataRequestResult> onEnded)
        {
            onEnded(requests[0]);
        }
    }
}
