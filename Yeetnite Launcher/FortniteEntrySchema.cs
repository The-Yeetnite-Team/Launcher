namespace Yeetnite_Launcher
{
    internal class FortniteEntrySchema
    {

        public FortniteEntrySchema()
        {
            Version = string.Empty;
            InstallPath = string.Empty;
        }

        public FortniteEntrySchema(string Version, string InstallPath)
        {
            this.Version = Version;
            this.InstallPath = InstallPath;
        }

        public string Version { get; set; }
        public string InstallPath { get; set; }
    }
}
