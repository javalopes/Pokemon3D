using System;

namespace Pokemon3D.Common.DataHandling
{
    internal class DataRequest
    {
        public string[] ResourcePath { get; }
        public Action<DataLoadResult[]> OnEnded { get; set; }

        public DataRequest(string[] resourcePath, Action<DataLoadResult[]> onEnded)
        {
            ResourcePath = resourcePath;
            OnEnded = onEnded;
        }

        public void NotifyEnded(DataLoadResult[] result)
        {
            OnEnded(result);
        }
    }
}