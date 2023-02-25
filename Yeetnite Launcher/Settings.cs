using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace Yeetnite_Launcher;

internal abstract class Settings
{
    private static SettingsSchema? _settings;

    public static void Init()
    {
        if (File.Exists("settings.json"))
        {
            Sync();
        }
        else
        {
            File.Create("settings.json").Close();
            _settings = new SettingsSchema();
            Save();
        }
    }

    public static string Username()
    {
        Debug.Assert(_settings != null);
        return _settings.Username;
    }

    public static void Username(string username)
    {
        Debug.Assert(_settings != null);

        _settings.Username = username;
        Save();
    }

    public static string AccessToken()
    {
        Debug.Assert(_settings != null);
        return _settings.AccessToken;
    }

    public static void AccessToken(string accessToken)
    {
        Debug.Assert(_settings != null);

        _settings.AccessToken = accessToken;
        Save();
    }

    public static List<FortniteEntrySchema> FortniteEntries()
    {
        Debug.Assert(_settings != null);
        return _settings.FortniteEntries;
    }

    public static void FortniteEntries(List<FortniteEntrySchema> fortniteEntries)
    {
        Debug.Assert(_settings != null);

        _settings.FortniteEntries = fortniteEntries;
        Save();
    }

    public static List<string> FortniteVersionsStored()
    {
        Debug.Assert(_settings != null);
        return _settings.FortniteVersionsStored;
    }

    public static void FortniteVersionsStored(List<string> fortniteVersionsStored)
    {
        Debug.Assert(_settings != null);

        _settings.FortniteVersionsStored = fortniteVersionsStored;
        Save();
    }

    public static void AddFortniteEntry(FortniteEntrySchema fortniteEntry)
    {
        Debug.Assert(_settings != null);

        _settings.FortniteEntries.Add(fortniteEntry);
        _settings.FortniteVersionsStored.Add(fortniteEntry.Version);
        Save();
    }

    public static void RemoveFortniteEntry(FortniteEntrySchema fortniteEntry)
    {
        Debug.Assert(_settings != null);

        _settings.FortniteEntries.Remove(fortniteEntry);
        Save();
    }

    public static void RemoveFortniteEntryAtIndex(int index)
    {
        Debug.Assert(_settings != null);

        _settings.FortniteEntries.RemoveAt(index);
        Save();
    }

    public static int FortniteSelectedIndex()
    {
        Debug.Assert(_settings != null);
        return _settings.FortniteSelectedIndex;
    }

    public static void FortniteSelectedIndex(int selectedIndex)
    {
        Debug.Assert(_settings != null);

        _settings.FortniteSelectedIndex = selectedIndex;
        Save();
    }

    private static void Sync()
    {
        using StreamReader file = new("settings.json");
        _settings = JsonConvert.DeserializeObject<SettingsSchema>(file.ReadToEnd());
    }

    private static void Save()
    {
        using StreamWriter file = new("settings.json");
        file.Write(JsonConvert.SerializeObject(_settings));
    }

    public static void Clear()
    {
        _settings = new SettingsSchema();
        Save();
    }
}