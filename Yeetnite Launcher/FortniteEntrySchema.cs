namespace Yeetnite_Launcher
{
    internal class FortniteEntrySchema
    {
        public FortniteEntrySchema(string version, string installPath)
        {
            Version = version;
            InstallPath = installPath;
        }

        public string Version { get; }
        public string InstallPath { get; }
    }
}
