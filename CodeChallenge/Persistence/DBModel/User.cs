using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Persistence.DBModel
{
    public class User
    {
        [Required]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Type { get; set; }
    }
}
