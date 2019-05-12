using DbEntities.Models;
using GPGDesign.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NToastNotify;
using System;
using System.Threading;

namespace GPGDesign.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IToastNotification toastNotification;
        private readonly IStringLocalizer<HomeController> _localizer;

        public BaseController(IToastNotification toastNotification, IStringLocalizer<HomeController> homeLocalizer)
        {
            this.toastNotification = toastNotification;
            _localizer = homeLocalizer;
        }

        protected void ShowNotification(string message, ToastrSeverity severity)
        {
            switch (severity)
            {
                case ToastrSeverity.Success:
                    toastNotification.AddSuccessToastMessage(message);
                    break;
                case ToastrSeverity.Info:
                    toastNotification.AddInfoToastMessage(message);
                    break;
                case ToastrSeverity.Warning:
                    toastNotification.AddWarningToastMessage(message);
                    break;
                case ToastrSeverity.Error:
                    toastNotification.AddErrorToastMessage(message);
                    break;
                case ToastrSeverity.Alert:
                    toastNotification.AddAlertToastMessage(message);
                    break;
            }
        }

        protected string ByteArrayToBase64(byte[] image, string imageType)
        {
            string result = string.Empty;

            if (image != null)
            {
                result = image != null ? imageType + "," + Convert.ToBase64String(image) : null;
            }

            return result;
        }

        protected byte[] Base64ToByteArray(string image)
        {
            return !string.IsNullOrEmpty(image) ? Convert.FromBase64String(image.Split(',')[1]) : null;
        }

        protected string GetCategoryDescription(Category category)
        {
            var result = string.Empty;

            if (Thread.CurrentThread.CurrentCulture.Name.Equals("en"))
            {
                result = category.EnDescription;
            }
            else if (Thread.CurrentThread.CurrentCulture.Name.Equals("de"))
            {
                result = category.DeDescription;
            }
            else
            {
                result = category.BgDescription;
            }

            return result;
        }

        protected void InitNavLabels()
        {
            ViewData["HomeNavLabel"] = _localizer["HomeNavLabel"];
            ViewData["ContactUsNavLabel"] = _localizer["ContactUsNavLabel"];
        }
    }
}