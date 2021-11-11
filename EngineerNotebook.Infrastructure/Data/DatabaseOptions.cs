namespace EngineerNotebook.Infrastructure.Data;

public interface IDatabaseOptions
{
    string Host { get; set; }
    string DatabaseName { get; set; }
    int Port { get; set; }
    string FullConnectionString { get; }
}