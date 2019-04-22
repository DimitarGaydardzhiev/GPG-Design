using DbEntities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DbEntities.Models
{
    public class GalleryImage : IBase
    {
        [Key]
        public int Id { get; set; }
        
        public string Description { get; set; }

        public byte[] Image { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public bool ShowOnMainPage { get; set; }
    }
}
