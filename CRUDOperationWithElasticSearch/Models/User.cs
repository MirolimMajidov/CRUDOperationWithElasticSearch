using System.Text.Json.Serialization;

namespace MyUser.Models
{
    public class User : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public string FullName { get; }

        public string Username { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<Backpack> Backpacks { get; set; } = new List<Backpack>();
    }
}
