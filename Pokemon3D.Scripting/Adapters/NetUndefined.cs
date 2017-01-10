namespace Pokemon3D.Scripting.Adapters
{
    /// <summary>
    /// .Net equivalent of the undefined value.
    /// </summary>
    public class NetUndefined
    {
        private NetUndefined() { }

        private static NetUndefined _instance;

        public static NetUndefined Instance
        {
            get { return _instance ?? (_instance = new NetUndefined()); }
        }
    }
}
