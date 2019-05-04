using DbEntities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DbEntities.Models
{
    public class GalleryImage : IBase
    {
        [Key]
        public int Id { get; set; }
        
        public string CategoryDescription { get; set; }

        public string EnDescription { get; set; }

        public string DeDescription { get; set; }

        public string BgDescription { get; set; }

        public byte[] Image { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public bool ShowOnMainPage { get; set; }
    }
}
