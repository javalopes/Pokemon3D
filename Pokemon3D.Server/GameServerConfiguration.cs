namespace Pokemon3D.Server
{
    public class GameServerConfiguration
    {
        public string MasterServerUrl { get; set; }

        public bool IsPrivate { get; set; }

        public string Name { get; set; }

        public int MaxPlayerCount { get; set; } = 1;
    }
}