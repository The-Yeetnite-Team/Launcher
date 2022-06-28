using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace Yeetnite_Launcher
{
    internal class Settings
    {
        public static SettingsSchema? settings;

        public static void Init()
        {
            if (File.Exists("settings.json"))
                Sync();
            else
            {
                File.Create("settings.json").Close();
                settings = new();
                Save();
            }
        }

        public static string Username()
        {
            Debug.Assert(settings != null);
            return (settings != null) ? settings.Username : string.Empty;
        }

        public static void Username(string username)
        {
            Debug.Assert(settings != null);
            if (settings == null) return;

            settings.Username = username;
            Save();
        }

        public static string AccessToken()
        {
            Debug.Assert(settings != null);
            return (settings != null) ? settings.AccessToken : string.Empty;
        }

        public static void AccessToken(string accessToken)
        {
            Debug.Assert(settings != null);
            if (settings == null) return;

            settings.AccessToken = accessToken;
            Save();
        }

        public static List<FortniteEntrySchema> FortniteEntries()
        {
            Debug.Assert(settings != null);
            return (settings != null) ? settings.FortniteEntries : new List<FortniteEntrySchema> { };
        }

        public static void FortniteEntries(List<FortniteEntrySchema> fortniteEntries)
        {
            Debug.Assert(settings != null);
            if (settings == null) return;

            settings.FortniteEntries = fortniteEntries;
            Save();
        }

        public static void AddFortniteEntry(FortniteEntrySchema fortniteEntry)
        {
            settings?.FortniteEntries.Add(fortniteEntry);
            Save();
        }

        public static void RemoveFortniteEntry(FortniteEntrySchema fortniteEntry)
        {
            settings?.FortniteEntries.Remove(fortniteEntry);
            Save();
        }

        public static void RemoveFortniteEntryAtIndex(int index)
        {
            settings?.FortniteEntries.RemoveAt(index);
            Save();
        }

        public static int FortniteSelectedIndex()
        {
            Debug.Assert(settings != null);
            return (settings != null) ? settings.FortniteSelectedIndex : -1;
        }

        public static void ForntiteSelectedIndex(int selectedIndex)
        {
            Debug.Assert(settings != null);
            if (settings == null) return;

            settings.FortniteSelectedIndex = selectedIndex;
            Save();
        }

        public static void Sync()
        {
            using StreamReader file = new("settings.json");
            settings = JsonConvert.DeserializeObject<SettingsSchema>(file.ReadToEnd());
        }

        public static void Save()
        {
            using StreamWriter file = new("settings.json");
            file.Write(JsonConvert.SerializeObject(settings));
        }

        public static void Clear()
        {
            settings = new();
            Save();
        }
    }
}
