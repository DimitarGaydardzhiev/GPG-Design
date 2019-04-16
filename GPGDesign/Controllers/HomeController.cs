using DataLayer;
using DbEntities.Models;
using GPGDesign.Enums;
using GPGDesign.Models;
using GPGDesign.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using System.Collections.Generic;
using System.Linq;

namespace GPGDesign.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<GalleryImage> _galleryImageRepository;

        public HomeController(IStringLocalizer<HomeController> localizer, IRepository<Category> categoryRepository, IToastNotification toastNotification, IRepository<GalleryImage> galleryImageRepository) : base(toastNotification)
        {
            _localizer = localizer;
            _categoryRepository = categoryRepository;
            _galleryImageRepository = galleryImageRepository;
        }
        public IActionResult Index()
        {
            ViewData["HomeMainLbl"] = _localizer["HomeMainLbl"];
            ViewData["HomeWeOffer"] = _localizer["HomeWeOffer"];

            var category = _categoryRepository.All();

            if (category == null)
            {
                ShowNotification(Messages.ObjectNotFound, ToastrSeverity.Error);
                return View();
            }
            
            var allCategories = new List<CategoryViewModel>();
            foreach (var item in category)
            {
                var catItem = new CategoryViewModel() {Id = item.Id, EnName = item.EnName, DeName = item.DeName, BgName = item.BgName, EnDescription = item.EnDescription, DeDescription = item.DeDescription, BgDescription = item.BgDescription, Thumbnail = ByteArrayToBase64(item.Thumbnail, "data:image/png;base64") };
                allCategories.Add(catItem);
            }
            
            return View(new ListCategoryViewModel() { AllCategories = allCategories });
        }      
    }
}
