using System;

namespace Pokemon3D.DataModel
{
    public static class DataLoadExceptionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="jsonData">The data that the serializer was trying to load.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="dataType"></param>
        public static DataLoadException AddData(this DataLoadException exception, string jsonData, Type targetType, DataType dataType)
        {
            exception.Data.Add("Target type", targetType.Name);
            exception.Data.Add("Data type", dataType.ToString());
            exception.Data.Add("Data", jsonData);

            return exception;
        }
    }

    /// <summary>
    /// An exception thrown when an error occurs while loading data into a data model.
    /// </summary>
    public class DataLoadException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DataLoadException"/> class.
        /// </summary>
        /// <param name="inner">The inner exception.</param>
        
        public DataLoadException(Exception inner)
            : base("An exception occurred trying to read data into an internal format. Please check that the input data is correct.", inner)
        {
        }
    }
}
