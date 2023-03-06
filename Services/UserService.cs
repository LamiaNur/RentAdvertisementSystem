using MongoDB.Driver;
using RentAdvertisementSystem.Database;
using RentAdvertisementSystem.Models;

namespace RentAdvertisementSystem.Services
{
    public class UserService
    {
        private readonly MongoDbClient _dbClient;
        public UserService(MongoDbClient dbClient)
        {
            _dbClient = dbClient;
        }
        
        public bool RegisterUser(UserModel userModel)
        {
            var user = GetUserByEmail(userModel.Email);
            if (user != null) return false;
            return _dbClient.SaveItem<UserModel>(userModel);
        }

        public bool CanLogIn(LogInModel logInModel)
        {
            var emailFilter = Builders<UserModel>.Filter.Eq("Email", logInModel.Email);
            var passwordFilter = Builders<UserModel>.Filter.Eq("Password", logInModel.Password);
            var filter = Builders<UserModel>.Filter.And(emailFilter, passwordFilter);
            
            var userModel = _dbClient.GetItemByFilterDefinition<UserModel>(filter);
            
            if (userModel == null) 
            {
                Console.WriteLine($"{logInModel.Email} can't log in\n");
                return false;
            }
            Console.WriteLine($"{logInModel.Email} can log in\n");

            return true;
        }

        public UserModel GetUserByEmail(string email)
        {
            var emailFilter = Builders<UserModel>.Filter.Eq("Email", email);
            var userModel = _dbClient.GetItemByFilterDefinition<UserModel>(emailFilter);
            return userModel;
        }
    }
}