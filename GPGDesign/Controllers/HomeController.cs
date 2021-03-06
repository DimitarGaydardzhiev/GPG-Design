﻿using DataLayer;
using DbEntities.Models;
using GPGDesign.Enums;
using GPGDesign.Models;
using GPGDesign.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace GPGDesign.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<GalleryImage> _galleryImageRepository;

        public HomeController(IStringLocalizer<HomeController> homeLocalizer, IRepository<Category> categoryRepository, IToastNotification toastNotification, IRepository<GalleryImage> galleryImageRepository) 
            : base(toastNotification, homeLocalizer)
        {
            _localizer = homeLocalizer;
            _categoryRepository = categoryRepository;
            _galleryImageRepository = galleryImageRepository;
        }

        public IActionResult Index()
        {
            base.InitMainLabels();
            ViewData["HomeMainLbl"] = _localizer["HomeMainLbl"];
            ViewData["HomeWeOffer"] = _localizer["HomeWeOffer"];
            ViewData["AboutUsIntro"] = _localizer["AboutUsIntro"];
            ViewData["AboutUsEquipment"] = _localizer["AboutUsEquipment"];
            ViewData["AboutUsNext"] = _localizer["AboutUsNext"];
            ViewData["AboutUsLabel"] = _localizer["AboutUsLabel"];
            ViewData["Philosophy"] = _localizer["Philosophy"];
            ViewData["PhilosophyLabel"] = _localizer["PhilosophyLabel"];
            ViewData["Discuss"] = _localizer["Discuss"];
            ViewData["Make"] = _localizer["Make"];
            ViewData["Product"] = _localizer["Product"];
            ViewData["SeeMoreLabel"] = _localizer["SeeMoreLabel"];
            ViewData["MainImages"] = this.GetMainPageImages();

            var category = _categoryRepository.All();

            if (category == null)
            {
                ShowNotification(Messages.ObjectNotFound, ToastrSeverity.Error);
                return View();
            }

            var allCategories = new List<CategoryViewModel>();
            foreach (var item in category)
            {
                var catItem = new CategoryViewModel() { Id = item.Id, EnName = item.EnName, DeName = item.DeName, BgName = item.BgName, EnDescription = item.EnDescription, DeDescription = item.DeDescription, BgDescription = item.BgDescription, Thumbnail = ByteArrayToBase64(item.Thumbnail, "data:image/png;base64") };
                allCategories.Add(catItem);
            }

            return View(new ListCategoryViewModel() { AllCategories = allCategories });
        }

        public IActionResult Cookie()
        {
            base.InitMainLabels();
            ViewBag.CultureName = CultureInfo.CurrentCulture.Name;
            return View();
        }

        public IActionResult UpdateData()
        {
            return Ok();
        }

        private IEnumerable<ImageViewModel> GetMainPageImages()
        {
            var result = _galleryImageRepository.All()
                .Include(i => i.Category)
                .Where(i => i.ShowOnMainPage)
                .Select(i => new ImageViewModel()
                {
                    Src = ByteArrayToBase64(i.Image, "data:image/png;base64"),
                    CategoryId = i.CategoryId,
                    EnDescription = i.EnDescription,
                    DeDescription = i.DeDescription,
                    BgDescription = i .BgDescription,
                    // CategoryDescription = GetCategoryDescription(i.Category)
                });

            return result;
        }
    }
}
