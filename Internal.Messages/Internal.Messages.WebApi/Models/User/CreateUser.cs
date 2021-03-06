using System.ComponentModel.DataAnnotations;

namespace Internal.Messages.WebApi.Models.User
{
    public class CreateUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
