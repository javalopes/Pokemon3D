namespace Pokemon3D.FileSystem.Requests
{
    /// <summary>
    /// Different error types for a <see cref="DataRequest{T}"/>.
    /// </summary>
    enum DataRequestErrorType
    {
        JsonDataError,
        FileNotFound,
        FileReadError,
        MiscError,
        ServerError
    }
}
