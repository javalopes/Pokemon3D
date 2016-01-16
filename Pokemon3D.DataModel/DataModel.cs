using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Pokemon3D.DataModel.Serialization;

namespace Pokemon3D.DataModel
{
    /// <summary>
    /// The base data model class.
    /// </summary>
    [DataContract(Namespace = "")]
    public abstract class DataModel<T> : ICloneable
    {
        /// <summary>
        /// Creates a data model of a specific type, loaded from a file.
        /// </summary>
        /// <typeparam name="T">The return type of the data model.</typeparam>
        public static T FromFile(string fileName)
        {
            if (File.Exists(fileName))
                return FromString(File.ReadAllText(fileName));
            else
                throw new FileNotFoundException("The data file does not exist.", fileName);
        }

        /// <summary>
        /// Creates a data model of a specific type, loaded from a file.
        /// </summary>
        /// <typeparam name="T">The return type of the data model.</typeparam>
        /// <param name="dataType">The data type of the data provided in the file.</param>
        public static T FromFile(string fileName, DataType dataType)
        {
            if (File.Exists(fileName))
                return FromString(File.ReadAllText(fileName), dataType);
            else
                throw new FileNotFoundException("The data file does not exist.", fileName);
        }

        /// <summary>
        /// Creates a data model of a specific type.
        /// </summary>
        /// <typeparam name="T">The return type of the data model.</typeparam>
        /// <param name="data">The data input string.</param>
        public static T FromString(string data)
        {
            return SerializerFactory.GetSerializer<T>(data).FromString(data);
        }

        /// <summary>
        /// Creates a data model of a specific type.
        /// </summary>
        /// <typeparam name="T">The return type of the data model.</typeparam>
        /// <param name="data">The data input string.</param>
        /// <param name="dataType">The data type of the data provided.</param>
        public static T FromString(string data, DataType dataType)
        {
            return SerializerFactory.GetSerializer<T>(dataType).FromString(data);
        }

        public static T FromByteArray(byte[] data)
        {
            return SerializerFactory.GetJsonSerializer<T>().FromByteArray(data);
        }

        /// <summary>
        /// Saves the content of this data model to a file.
        /// </summary>
        /// <param name="filename">The file to save the content to.</param>
        public void ToFile(string filename, DataType dataType)
        {
            string content = ToString();
            File.WriteAllText(filename, content);
        }

        /// <summary>
        /// Converts the contents of this data model to data of a specific type.
        /// </summary>
        public string ToString(DataType dataType)
        {
            return SerializerFactory.GetSerializer<T>(dataType).ToString(this);
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a member of the given enum type.
        /// </summary>
        protected static TEnum ConvertStringToEnum<TEnum>(string enumString) where TEnum : struct, IComparable
        {
            TEnum result;
            if (Enum.TryParse(enumString, true, out result)) return result;

            return default(TEnum);
        }

        /// <summary>
        /// Converts a <see cref="string"/> array to an array of the given enum type.
        /// </summary>
        protected static TEnum[] ConvertStringCollectionToEnumCollection<TEnum>(ICollection<string> enumMemberArray) where TEnum : struct, IComparable
        {
            TEnum[] enumArr = new TEnum[enumMemberArray.Count];

            for (int i = 0; i < enumMemberArray.Count; i++)
                enumArr[i] = ConvertStringToEnum<TEnum>(enumMemberArray.ElementAt(i));

            return enumArr;
        }

        /// <summary>
        /// Converts an enum member array to a <see cref="string"/> array (using ToString).
        /// </summary>
        protected static string[] ConvertEnumCollectionToStringCollection<TEnum>(ICollection<TEnum> enumArray)
        {
            string[] stringArr = new string[enumArray.Count];

            for (int i = 0; i < enumArray.Count; i++)
                stringArr[i] = enumArray.ElementAt(i).ToString();

            return stringArr;
        }

        /// <summary>
        /// Base cloning methods for data models. Use <see cref="CloneModel"/> for non-object typing instead.
        /// </summary>
        public abstract object Clone();

        /// <summary>
        /// Creates a shallow copy of the data model.
        /// </summary>
        public T CloneModel() { return (T)Clone(); }
    }
}
