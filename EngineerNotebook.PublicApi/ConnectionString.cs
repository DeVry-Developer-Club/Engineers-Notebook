namespace EngineerNotebook.PublicApi
{
    public class ConnectionString : IDatabaseOptions
    {
        public string Host { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }

        public string FullConnectionString => $@"mongodb://{Host}:{Port}";

        public ConnectionString(string server, int port, string database, string username, string password)
        {
            this.Host = server;
            this.DatabaseName = database;
            this.Port = port;
            this.Username = username;
            this.Password = password;
        }

        public ConnectionString() { }
    }
}
