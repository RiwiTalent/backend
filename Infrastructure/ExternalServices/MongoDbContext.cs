using MongoDB.Driver;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Infrastructure.ExternalServices
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IConfiguration configuration)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration.GetConnectionString("DbConnection")));

            settings.SslSettings = new SslSettings{
                CheckCertificateRevocation = false,
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            settings.ConnectTimeout = TimeSpan.FromSeconds(30);
            settings.SocketTimeout = TimeSpan.FromSeconds(30);

            //we realize the connection to Database
            var client = new MongoClient(settings);
            _database = client.GetDatabase(configuration["MongoDbSettings:Database"]);
        }

        //We define connection to Models
        public IMongoCollection<Coder> Coders => _database.GetCollection<Coder>("Coders");
        public IMongoCollection<Group> Groups => _database.GetCollection<Group>("Groups");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<CoderStatusHistory> CoderStatusHistories => _database.GetCollection<CoderStatusHistory>("CoderStatusHistories");
        public IMongoCollection<Technology> Technologies  => _database.GetCollection<Technology>("Technologies");
        public IMongoCollection<TermAndCondition> TermsAndConditions  => _database.GetCollection<TermAndCondition>("TermsAndConditions");
    }
}