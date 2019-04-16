using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GPGDesign.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string EnName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string DeName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string BgName { get; set; }

        [Required]        
        public string EnDescription { get; set; }

        [Required]        
        public string DeDescription { get; set; }

        [Required]        
        public string BgDescription { get; set; }

        public IList<ImageViewModel> Images { get; set; }
        
        public string Thumbnail { get; set; }

        public int NumberOfImages { get; set; }

        public string HasThumbnail { get; set; }        
    }
}
