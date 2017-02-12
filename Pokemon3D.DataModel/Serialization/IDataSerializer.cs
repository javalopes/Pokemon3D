namespace Pokemon3D.DataModel.Serialization
{
    interface IDataSerializer<T>
    {
        T FromString(string data);

        T FromByteArray(byte[] data);

        string ToString(DataModel<T> dataModel);
    }
}
