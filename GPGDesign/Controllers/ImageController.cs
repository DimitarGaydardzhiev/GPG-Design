using DataLayer;
using DbEntities.Models;
using GPGDesign.Enums;
using GPGDesign.Models;
using GPGDesign.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPGDesign.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IRepository<GalleryImage> imageRepository;
        IStringLocalizer<HomeController> _localizer;

        public ImageController(
            IToastNotification toastNotification,
            IRepository<GalleryImage> categoryRepository,
            IStringLocalizer<HomeController> homeLocalizer)
            : base(toastNotification, homeLocalizer)
        {
            this.imageRepository = categoryRepository;
            _localizer = homeLocalizer;
        }

        [HttpGet]
        [Authorize]
        public IActionResult MainPageImages()
        {
            base.InitNavLabels();

            var result = this.imageRepository.All()
                .Select(i => new ImageViewModel()
                {
                    Src = ByteArrayToBase64(i.Image, "data:image/png;base64"),
                    Id = i.Id,
                    ShowOnMainPage = i.ShowOnMainPage,
                    EnDescription = i.EnDescription,
                    BgDescription = i.BgDescription,
                    DeDescription = i.DeDescription
                }).ToList();

            return View(result);
        }

        [HttpPost]
        [Authorize]
        public IActionResult SaveMainPageImages(IEnumerable<ImageViewModel> images)
        {
            try
            {
                foreach (var image in images)
                {
                    var galleryImage = this.imageRepository.Find(image.Id);
                    galleryImage.ShowOnMainPage = image.ShowOnMainPage;
                    galleryImage.EnDescription = image.EnDescription;
                    galleryImage.BgDescription = image.BgDescription;
                    galleryImage.DeDescription = image.DeDescription;
                    this.imageRepository.Update(galleryImage);
                }

                this.imageRepository.SaveChanges();
                ShowNotification(Messages.SuccessAdd, ToastrSeverity.Success);
            }
            catch (Exception ex)
            {
                ShowNotification(ex.Message, ToastrSeverity.Error);
                return BadRequest();
            }

            return this.RedirectToAction("MainPageImages");
        }
    }
}