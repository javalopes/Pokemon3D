namespace Pokemon3D.Common.DataHandling
{
    public class DataLoadResult
    {
        public byte[] Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static DataLoadResult Succeeded(byte[] data)
        {
            return new DataLoadResult
            {
                Data = data,
                Success = true,
                ErrorMessage = string.Empty
            };
        }

        public static DataLoadResult Failed(string errorMessage)
        {
            return new DataLoadResult
            {
                Data = null,
                ErrorMessage = errorMessage,
                Success = false
            };
        }
    }
}