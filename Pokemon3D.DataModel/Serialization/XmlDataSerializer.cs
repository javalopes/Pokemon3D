using System;
using System.Runtime.Serialization;
using System.IO;

namespace Pokemon3D.DataModel.Serialization
{
    /// <summary>
    /// Serializes and deserializes xml data.
    /// </summary>
    class XmlDataSerializer<T> : DataSerializer<T>
    {
        private const string XmlSchemaInstance = " xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"";

        public T FromByteArray(byte[] data)
        {
            throw new NotImplementedException();
        }

        public T FromString(string data)
        {
            // We create a new Xml serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractSerializer(typeof(T), new DataContractSerializerSettings() { SerializeReadOnlyTypes = true });
            var memStream = new MemoryStream();

            // Create StreamWriter to the memory stream, which writes the input string to the stream.
            var sw = new StreamWriter(memStream);
            sw.Write(data);
            sw.Flush();

            // Reset the stream's position to the beginning:
            memStream.Position = 0;

            try
            {
                // Create and return the object:
                T returnObj = (T)serializer.ReadObject(memStream);
                return returnObj;
            }
            catch (Exception ex)
            {
                // Exception occurs while loading the object due to malformed Xml.
                // Throw exception and move up to handler class.
                throw new DataLoadException(ex, data, typeof(T), DataType.Xml);
            }
        }

        public string ToString(DataModel<T> dataModel)
        {
            // We create a new Xml serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractSerializer(dataModel.GetType(), new DataContractSerializerSettings() { SerializeReadOnlyTypes = true });
            var memStream = new MemoryStream();

            // Write the data to the stream.
            serializer.WriteObject(memStream, dataModel);

            // Reset the stream's position to the beginning:
            memStream.Position = 0;

            // Create stream reader, read string and return it.
            var sr = new StreamReader(memStream);
            var returnXml = sr.ReadToEnd();

            // we want to remove the instance note on this, so we do this the dirty way, as there is no other way...
            return returnXml.Replace(XmlSchemaInstance, "");
        }
    }
}
