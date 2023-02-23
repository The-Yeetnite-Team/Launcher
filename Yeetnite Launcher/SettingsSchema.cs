using System.Collections.Generic;

namespace Yeetnite_Launcher
{
    internal class SettingsSchema
    {
        public SettingsSchema()
        {
            Username = string.Empty;
            AccessToken = string.Empty;
            FortniteEntries = new List<FortniteEntrySchema> { };
            FortniteVersionsStored = new List<string> { };
            FortniteSelectedIndex = -1;
        }

        public string Username { get; set; }

        public string AccessToken { get; set; }

        public List<FortniteEntrySchema> FortniteEntries { get; set; }

        public List<string> FortniteVersionsStored { get; set; }

        public int FortniteSelectedIndex { get; set; }
    }
}
