using DataLayer;
using DbEntities.Models;
using GPGDesign.Enums;
using GPGDesign.ImageUtils;
using GPGDesign.Models;
using GPGDesign.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GPGDesign.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<GalleryImage> galleryImageRepository;
        private readonly IStringLocalizer<HomeController> _localizer;

        public CategoryController(
            IRepository<Category> categoryRepository,
            IToastNotification toastNotification,
            IRepository<GalleryImage> galleryImageRepository,
            IStringLocalizer<HomeController> homeLocalizer)
            : base(toastNotification, homeLocalizer)
        {
            this.categoryRepository = categoryRepository;
            this.galleryImageRepository = galleryImageRepository;
            _localizer = homeLocalizer;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add(int? id)
        {
            base.InitMainLabels();
            if (id.HasValue)
            {
                var category = this.categoryRepository.All()
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    ShowNotification(Messages.ObjectNotFound, ToastrSeverity.Error);
                    return View();
                }

                var viewModel = new CategoryViewModel()
                {
                    EnName = category.EnName,
                    DeName = category.DeName,
                    BgName = category.BgName,
                    EnDescription = category.EnDescription,
                    DeDescription = category.DeDescription,
                    BgDescription = category.BgDescription,
                    Thumbnail = ByteArrayToBase64(category.Thumbnail, "jpeg")
                };

                return View(viewModel);
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CategoryViewModel model, IFormFile file)
        {
            //if (!ModelState.IsValid)
            //    return View(model);

            //if (this.categoryRepository.All().Any(c => c.EnName == model.EnName))
            //{
            //    ShowNotification(Messages.ObjectAlreadyExists, ToastrSeverity.Error);
            //    return View();
            //}

            //if (this.categoryRepository.All().Any(c => c.DeName == model.DeName))
            //{
            //    ShowNotification(Messages.ObjectAlreadyExists, ToastrSeverity.Error);
            //    return View();
            //}

            //if (this.categoryRepository.All().Any(c => c.BgName == model.BgName))
            //{
            //    ShowNotification(Messages.ObjectAlreadyExists, ToastrSeverity.Error);
            //    return View();
            //}

            base.InitMainLabels();
            string img = string.Empty;

            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    img = ImageWorker.ResizeImage(ms);
                }
            }

            var category = this.categoryRepository.FindOrCreate(model.Id);

            category.EnName = model.EnName;
            category.DeName = model.DeName;
            category.BgName = model.BgName;
            category.EnDescription = model.EnDescription;
            category.DeDescription = model.DeDescription;
            category.BgDescription = model.BgDescription;
            category.Thumbnail = Base64ToByteArray(img);

            try
            {
                this.categoryRepository.Save(category);
                ShowNotification(Messages.SuccessAdd, ToastrSeverity.Success);
            }
            catch (Exception)
            {
                string error = string.Join(Environment.NewLine, ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)));
                ShowNotification(error, ToastrSeverity.Error);
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            base.InitMainLabels();

            var categories = categoryRepository.All()
                .Include(c => c.Images);

            if (categories == null)
            {
                ShowNotification(Messages.ObjectNotFound, ToastrSeverity.Error);
                return View();
            }

            var result = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                result.Add(new CategoryViewModel()
                {
                    Id = category.Id,
                    BgName = category.BgName,
                    NumberOfImages = category.Images.Count(),
                    HasThumbnail = category.Thumbnail != null ? "ИМА" : "НЯМА"
                });
            }

            return View(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var category = categoryRepository.All()
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                ShowNotification(Messages.ObjectNotFound, ToastrSeverity.Error);
                return View();
            }

            this.categoryRepository.Delete(category);
            this.categoryRepository.SaveChanges();
            ShowNotification(Messages.SuccessDelete, ToastrSeverity.Success);
            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("get-category/{id}")]
        public IActionResult GetById(int id)
        {
            base.InitMainLabels();

            var category = categoryRepository.All()
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                ShowNotification(Messages.ObjectNotFound, ToastrSeverity.Error);
                return View(new CategoryViewModel());
            }

            var images = new List<ImageViewModel>();
            foreach (var item in category.Images)
            {
                images.Add(new ImageViewModel()
                {
                    Id = item.Id,
                    EnDescription = item.EnDescription,
                    BgDescription = item.BgDescription,
                    DeDescription = item.DeDescription,
                    Src = ByteArrayToBase64(item.Image, "data:image/png;base64")
                });
            }

            var result = new CategoryViewModel()
            {
                EnName = category.EnName,
                BgName = category.BgName,
                DeName = category.DeName,
                Images = images
            };

            return View(result);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UploadImages(CategoryViewModel model, List<IFormFile> files)
        {
            if (files.Count == 0)
            {
                ShowNotification(Messages.NoImagesSelected, ToastrSeverity.Error);
                return RedirectToAction("GetById", new { id = model.Id });
            }

            if (files.Any(f => !ImageWorker.IsImage(f)))
            {
                ShowNotification(Messages.UnsupportedImageType, ToastrSeverity.Error);
                return RedirectToAction("GetById", new { id = model.Id });
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        string img = ImageWorker.ResizeImage(ms);
                        //string s = Convert.ToBase64String(fileBytes);
                        galleryImageRepository.Add(new GalleryImage()
                        {
                            CategoryId = model.Id,
                            Image = Base64ToByteArray(img)
                        });
                    }
                }
            }

            galleryImageRepository.SaveChanges();
            ShowNotification(Messages.ImageUploadSuccess, ToastrSeverity.Success);
            return RedirectToAction("GetById", new { id = model.Id });
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteImages(IEnumerable<ImageViewModel> images, int id)
        {
            var imagesForDelete = galleryImageRepository
                .All()
                .Where(i => images.Any(x => x.Id == i.Id && x.IsSelected));


            if (imagesForDelete.Count() > 0)
            {
                try
                {
                    this.galleryImageRepository.DeleteRange(imagesForDelete);
                    this.galleryImageRepository.SaveChanges();
                    ShowNotification(Messages.SuccessDelete, ToastrSeverity.Success);
                }
                catch (Exception ex)
                {
                    ShowNotification(Messages.DeleteError, ToastrSeverity.Error);
                }

                return RedirectToAction("GetById", new { id });
            }

            ShowNotification(Messages.NoImagesSelected, ToastrSeverity.Error);
            return RedirectToAction("GetById", new { id });
        }
    }
}