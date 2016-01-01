using System;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// An exception during a data request operation.
    /// </summary>
    class DataRequestException : Exception
    {
        /// <summary>
        /// The type of error that occurred.
        /// </summary>
        public DataRequestErrorType ErrorType { get; private set; }

        /// <summary>
        /// The request object that caused the exception.
        /// </summary>
        public object Request { get; private set; }

        public DataRequestException(object request, DataRequestErrorType errorType, Exception inner)
            : base("An error occurred during a data request.", inner)
        {
            Request = request;
            ErrorType = errorType;

            Data.Add("RequestErrorType", ErrorType.ToString());
        }
    }
}
