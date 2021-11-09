namespace EngineerNotebook.PublicApi
{
    public class ConnectionString
    {
        public string Server { get; init; }
        public string Database { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public int Port { get; init; }

        public string RootConnectionString => $"server={Server};port={Port};user={Username};password={Password};";
        public string FullConnectionString => $"server={Server};port={Port};database={Database};user={Username};password={Password};";

        public ConnectionString(string server, int port, string database, string username, string password)
        {
            this.Server = server;
            this.Database = database;
            this.Port = port;
            this.Username = username;
            this.Password = password;
        }

        public ConnectionString() { }
    }
}
