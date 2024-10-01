using MongoDB.Driver;

namespace E_commerce_system.Data
{
    //MongoDbService class to connect to the MongoDB database
    public class MongoDbService
    {
        //Configuration property to access the configuration
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase? _database;

        //Constructor to initialize the database
        public MongoDbService(IConfiguration configuration) {
            _configuration = configuration;
            var connectionString = _configuration.GetConnectionString("DbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        
        }

        //Database property to access the database
        public IMongoDatabase? Database => _database;

    }
}
