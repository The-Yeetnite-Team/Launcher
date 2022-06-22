namespace Yeetnite_Launcher
{
    internal class SettingsSchema
    {
        private string _username;
        private string _accessToken;
        private string[] _fortniteEntries;

        public SettingsSchema()
        {
            _username = string.Empty;
            _accessToken = string.Empty;
            _fortniteEntries = System.Array.Empty<string>();
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        public string[] FortniteEntries
        {
            get { return _fortniteEntries; }
            set { _fortniteEntries = value; }
        }
    }
}
