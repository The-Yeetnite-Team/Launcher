namespace Yeetnite_Launcher
{
    internal class User
    {
        private string Username;
        private string AccessToken;

        public User(string Username, string AccessToken)
        {
            this.Username = Username;
            this.AccessToken = AccessToken;
        }

        public void SetUsername(string Username)
        {
            this.Username = Username;
        }

        public string GetUsername()
        {
            return Username;
        }

        public void SetAccessToken(string AccessToken)
        {
            this.AccessToken = AccessToken;
        }

        public string GetAccessToken()
        {
            return AccessToken;
        }
    }
}
