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
        public string Name { get; set; }

        public IEnumerable<GalleryImage> Images { get; set; }
    }
}
