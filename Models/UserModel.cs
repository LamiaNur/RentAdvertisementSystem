using RentAdvertisementSystem.Database;

namespace RentAdvertisementSystem.Models
{
    public class UserModel : IRepositoryItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public UserModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}