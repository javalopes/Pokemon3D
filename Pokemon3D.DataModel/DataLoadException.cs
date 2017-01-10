using System;

namespace Pokemon3D.DataModel
{
    /// <summary>
    /// An exception thrown when an error occurs while loading data into a data model.
    /// </summary>
    public class DataLoadException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DataLoadException"/> class.
        /// </summary>
        /// <param name="inner">The inner exception.</param>
        /// <param name="jsonData">The data that the serializer was trying to load.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="dataType"></param>
        public DataLoadException(Exception inner, string jsonData, Type targetType, DataType dataType)
            : base("An exception occurred trying to read data into an internal format. Please check that the input data is correct.", inner)
        {
            Data.Add("Target type", targetType.Name);
            Data.Add("Data type", dataType.ToString());
            Data.Add("Data", jsonData);
        }
    }
}
