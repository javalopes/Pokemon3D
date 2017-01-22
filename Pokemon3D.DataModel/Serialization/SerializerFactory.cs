namespace Pokemon3D.DataModel.Serialization
{
    internal static class SerializerFactory
    {
        /// <summary>
        /// Returns a data serializer based on the data.
        /// </summary>
        public static DataSerializer<T> GetSerializer<T>(string data)
        {
            // this method tries to identify the type of data, if it's either Xml or Json.
            // Json does not have "comment outside of model" support, so we can check if it either stars with the object/array notation:

            string trimmed = data.Trim();
            if (trimmed.StartsWith("[") || trimmed.StartsWith("{"))
            {
                return GetSerializer<T>(DataType.Json);
            }
            // otherwise we assume it's Xml:
            else
            {
                return GetSerializer<T>(DataType.Xml);
            }
        }

        /// <summary>
        /// Returns the json serializer
        /// </summary>
        public static DataSerializer<T> GetJsonSerializer<T>()
        {
            return new JsonDataSerializer<T>();
        }

        /// <summary>
        /// Returns the appropriate data serializer.
        /// </summary>
        public static DataSerializer<T> GetSerializer<T>(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Json:
                    return new JsonDataSerializer<T>();
                case DataType.Xml:
                    return new XmlDataSerializer<T>();
                default:
                    return null;
            }
        }
    }
}
