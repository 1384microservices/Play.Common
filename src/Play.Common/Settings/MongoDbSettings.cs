namespace Play.Common.Settings;

public class MongoDbSettings
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    private string connectionString;
    public string ConnectionString
    {
        get
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return $"mongodb://{Host}:{Port}";
            }
            return connectionString;
        }

        set
        {
            connectionString = value;
        }
    }

}
