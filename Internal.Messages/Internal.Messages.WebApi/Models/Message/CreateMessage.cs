using System.ComponentModel.DataAnnotations;
using Internal.Messages.WebApi.Attributes.Validation;

namespace Internal.Messages.WebApi.Models.Message
{
    public class CreateMessage
    {
        [Required]
        [IdValidation]
        public int UserId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        [IdValidation]
        public int ChannelId { get; set; }
    }
}
