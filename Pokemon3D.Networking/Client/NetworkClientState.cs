﻿namespace Pokemon3D.Networking.Client
{
    public enum NetworkClientState
    {
        Disconnected,
        Connecting,
        Connected,
        CheckContentFolderCorrect,
        DownloadingContent,
        CheckContentFolderFinished,
        ConnectionFailed,
    }
}