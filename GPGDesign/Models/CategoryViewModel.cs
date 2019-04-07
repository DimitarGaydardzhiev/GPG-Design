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

        public IEnumerable<ImageViewModel> Images { get; set; }
    }
}
