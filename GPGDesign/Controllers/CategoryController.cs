﻿using DataLayer;
using DbEntities.Models;
using GPGDesign.Enums;
using GPGDesign.ImageUtils;
using GPGDesign.Models;
using GPGDesign.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public CategoryController(
            IRepository<Category> categoryRepository,
            IToastNotification toastNotification,
            IRepository<GalleryImage> galleryImageRepository)
            : base(toastNotification)
        {
            this.categoryRepository = categoryRepository;
            this.galleryImageRepository = galleryImageRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CategoryViewModel model, IFormFile file)
        {
            //if (!ModelState.IsValid)
            //    return View(model);

            if (this.categoryRepository.All().Any(c => c.EnName == model.EnName))
            {
                ShowNotification(Messages.ObjectAlreadyExists, ToastrSeverity.Error);
                return View();
            }

            if (this.categoryRepository.All().Any(c => c.DeName == model.DeName))
            {
                ShowNotification(Messages.ObjectAlreadyExists, ToastrSeverity.Error);
                return View();
            }

            if (this.categoryRepository.All().Any(c => c.BgName == model.BgName))
            {
                ShowNotification(Messages.ObjectAlreadyExists, ToastrSeverity.Error);
                return View();
            }

            string img = string.Empty;

            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    img = ImageWorker.ResizeImage(ms);
                }
            }


            this.categoryRepository.Add(new Category()
            {
                EnName = model.EnName,
                DeName = model.DeName,
                BgName = model.BgName,
                EnDescription = model.EnDescription,
                DeDescription = model.DeDescription,
                BgDescription = model.BgDescription,
                Thumbnail = Base64ToByteArray(img)

            });

            this.categoryRepository.SaveChanges();
            ShowNotification(Messages.SuccessAdd, ToastrSeverity.Success);
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
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
                    NumberOfImages = category.Images.Count()
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
        public IActionResult GetById(int id)
        {
            var category = categoryRepository.All()
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                ShowNotification(Messages.ObjectNotFound, ToastrSeverity.Error);
                return View();
            }

            var images = new List<ImageViewModel>();
            foreach (var item in category.Images)
            {
                images.Add(new ImageViewModel()
                {
                    Id = item.Id,
                    Description = item.Description,
                    Src = ByteArrayToBase64(item.Image, "data:image/png;base64")
                });
            }

            var result = new CategoryViewModel()
            {
                EnName = category.EnName,
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