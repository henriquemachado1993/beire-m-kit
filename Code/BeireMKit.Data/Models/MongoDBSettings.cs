namespace BeireMKit.Data.Models
{
    public class MongoDBSettings
    {
        public MongoDBSettings()
        {
        }

        public MongoDBSettings(string connection, string databaseName)
        {
            ConnectionString = connection;
            DatabaseName = databaseName;
        }

        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
