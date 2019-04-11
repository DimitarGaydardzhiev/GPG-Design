using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GPGDesign.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]        
        public string EnDescription { get; set; }

        [Required]        
        public string DeDescription { get; set; }

        [Required]        
        public string BgDescription { get; set; }

        public IList<ImageViewModel> Images { get; set; }
    }
}
