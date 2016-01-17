using System;

namespace Pokemon3D.Common.DataHandling
{
    internal class DataRequest
    {
        public string[] ResourcePath { get; }
        public Action<DataRequestResult[]> OnEnded { get; set; }

        public DataRequest(string[] resourcePath, Action<DataRequestResult[]> onEnded)
        {
            ResourcePath = resourcePath;
            OnEnded = onEnded;
        }

        public void NotifyEnded(DataRequestResult[] result)
        {
            OnEnded(result);
        }
    }
}