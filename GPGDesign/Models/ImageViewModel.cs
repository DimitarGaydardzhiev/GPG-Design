using System.ComponentModel.DataAnnotations;

namespace GPGDesign.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }

        public string Src { get; set; }

        public string Description { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        [Display(Name = "Покажи на главната страница")]
        public bool ShowOnMainPage { get; set; }
    }
}
