using MongoDB.Driver;
using RentAdvertisementSystem.Database;
using RentAdvertisementSystem.Models;

namespace RentAdvertisementSystem.Services
{
    public class RentService
    {
        private readonly MongoDbClient _dbClient;
        public RentService(MongoDbClient dbClient)
        {
            _dbClient = dbClient;
        }
        public RentItem GetRentItemById(string id)
        {
            return _dbClient.GetItemById<RentItem>(id);
        }
        public bool PostRentItem(RentItem rentItem)
        {
           return _dbClient.SaveItem<RentItem>(rentItem);
        }

        public bool UpdateRentItem(RentItem rentItem)
        {
            return _dbClient.SaveItem<RentItem>(rentItem);
        }
        
        public bool DeleteRentItemById(string id)
        {
            return _dbClient.DeleteItemById<RentItem>(id);
        }

        public List<RentItem> GetAllRentItems()
        {
            return _dbClient.GetItems<RentItem>();
        }

        public List<RentItem> GetUserRentItems(string userId)
        {
            var filter = Builders<RentItem>.Filter.Eq("UserId", userId);
            return _dbClient.GetItemsByFilterDefinition<RentItem>(filter);
        }

        public List<RentItem> GetRentItemsByLocation(string location)
        {
            Console.WriteLine(@"location \n");
            var filter = Builders<RentItem>.Filter.Eq("Location", location);
            return _dbClient.GetItemsByFilterDefinition<RentItem>(filter);
        }
    }
}