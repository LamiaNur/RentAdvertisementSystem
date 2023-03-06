using MongoDB.Driver;
using Newtonsoft.Json;

namespace RentAdvertisementSystem.Database
{
    public class MongoDbClient
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient _mongoCilent;
        private readonly IMongoDatabase _database;
        
        public MongoDbClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _mongoCilent = new MongoClient(_configuration["DatabaseConfig:ConnectionString"]);
            _database = _mongoCilent.GetDatabase(_configuration["DatabaseConfig:DatabaseName"]);
        }
        
        private IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }

        public bool SaveItem<T>(T item) where T : class, IRepositoryItem
        {
            try
            {
                var collection = GetCollection<T>();
                var filter = Builders<T>.Filter.Eq("Id", item.Id);
                collection.ReplaceOne(filter, item, new ReplaceOptions {
                    IsUpsert = true
                });
                Console.WriteLine($"Successfully Save Item : {JsonConvert.SerializeObject(item)}\n");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Save Item : {JsonConvert.SerializeObject(item)}\n");
                return false;
            }
        }

        public T GetItemById<T>(string id) where T : class, IRepositoryItem
        {
            try
            {
                var collection = GetCollection<T>();
                var filter = Builders<T>.Filter.Eq("Id", id);
                var items = collection.Find<T>(filter);
                var item = items.FirstOrDefault<T>();
                Console.WriteLine($"Successfully Get Item : {JsonConvert.SerializeObject(item)}\n");
                return item;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Get Item, id : {id}\n");
                return null;
            }
        }

        public List<T> GetItems<T>() where T : class, IRepositoryItem
        {
            try
            {
                var collection = GetCollection<T>();
                var filter = Builders<T>.Filter.Empty;
                var itemsCursor = collection.Find<T>(filter);
                var items = itemsCursor.ToList<T>();
                Console.WriteLine($"Successfully Get items, Count: {JsonConvert.SerializeObject(items.Count)}\n");
                return items;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Get Items\n");
                return null;
            }
        }

        public T GetItemByFilterDefinition<T>(FilterDefinition<T> filterDefinition) where T : class, IRepositoryItem
        {
            try
            {
                var collection = GetCollection<T>();
                var items = collection.Find<T>(filterDefinition);
                var item = items.FirstOrDefault<T>();
                 Console.WriteLine($"Successfully Get Item by filter : {JsonConvert.SerializeObject(item)}\n");
                return item;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem Get Item by fiter \n");
                return null;
            }
        }

        public List<T> GetItemsByFilterDefinition<T>(FilterDefinition<T> filterDefinition) where T : class, IRepositoryItem
        {
            try
            {
                var collection = GetCollection<T>();
                var itemsCursor = collection.Find<T>(filterDefinition);
                var items = itemsCursor.ToList<T>();
                
                Console.WriteLine($"Successfully Get Items by filter count: {JsonConvert.SerializeObject(items.Count)}\n");
                
                return items;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem Get Items by fiter \n");
                return null;
            }
        }

        public bool DeleteItemById<T>(string id) where T : class, IRepositoryItem
        {
            try
            {
                var collection = GetCollection<T>();
                var filter = Builders<T>.Filter.Eq("Id", id);
                var res = collection.DeleteOne(filter);
                Console.WriteLine($"Successfully Item Deleted, Id: {JsonConvert.SerializeObject(id)}\n");
                return res == null? false : res.DeletedCount > 0;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Delete Item, Id : {JsonConvert.SerializeObject(id)}\n");
                return false;
            }
        }

    }
}