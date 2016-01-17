namespace Pokemon3D.Common.DataHandling
{
    public class DataRequestResult
    {
        public byte[] Data { get; }
        public RequestState State { get; }

        public DataRequestResult(byte[] data, RequestState state)
        {
            Data = data;
            State = state;
        }
    }
}