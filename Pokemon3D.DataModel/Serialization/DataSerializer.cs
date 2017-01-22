namespace Pokemon3D.DataModel.Serialization
{
    interface DataSerializer<T>
    {
        T FromString(string data);

        T FromByteArray(byte[] data);

        string ToString(DataModel<T> dataModel);
    }
}
