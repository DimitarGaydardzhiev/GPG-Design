using GPGDesign.Enums;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;

namespace GPGDesign.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IToastNotification toastNotification;

        public BaseController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
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
    }
}