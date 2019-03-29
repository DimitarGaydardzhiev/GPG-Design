using System.ComponentModel.DataAnnotations;

namespace GPGDesign.Models.ContactModels
{
    public class EmailFormModel
    {
        [Required]
        public string FromName { get; set; }

        [Required]
        public string FromEmail { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
