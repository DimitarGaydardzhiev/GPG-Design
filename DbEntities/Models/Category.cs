using DbEntities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbEntities.Models
{
    public class Category : IBase
    {
        public Category()
        {
            Images = new HashSet<GalleryImage>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string EnName { get; set; }

        [Required]
        public string DeName { get; set; }

        [Required]
        public string BgName { get; set; }

        public string EnDescription { get; set; }

        public string DeDescription { get; set; }

        public string BgDescription { get; set; }

        public IEnumerable<GalleryImage> Images { get; set; }
        
        public byte[] Thumbnail { get; set; }
    }
}
