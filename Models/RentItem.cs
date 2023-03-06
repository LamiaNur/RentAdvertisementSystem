using RentAdvertisementSystem.Database;

namespace RentAdvertisementSystem.Models
{
    public class RentItem : IRepositoryItem
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime PostedAt { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string UserId { get; set; }
        public RentItem()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}