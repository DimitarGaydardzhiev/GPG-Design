using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GPGDesign.Models.ContactModels;
using Microsoft.Extensions.Localization;

namespace GPGDesign.Controllers
{
    public class ContactController : Controller
    {
        private readonly IStringLocalizer<ContactController> _localizer;

        public ContactController(IStringLocalizer<ContactController> localizer)
        {
            _localizer = localizer;
        }

        public ActionResult Contact()
        {
            ViewData["ContactLbl"] = _localizer["ContactLbl"];            
            ViewData["ContactTxt"] = _localizer["ContactTxt"];

            return View(new EmailFormModel());
        }

        [HttpPost]
        public void SendEmail(EmailFormModel model)
        {

            if (ModelState.IsValid)
            {
                const string fromPassword = "Gpg2018!";
                var fromAddress = new MailAddress("officegpgdesign@gmail.com", "From Name");
                var toAddress = new MailAddress("teofil.y@gmail.com", "To Name");
                var credential = new NetworkCredential
                {
                    UserName = "officegpgdesign@gmail.com",
                    Password = fromPassword
                };

                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = credential,
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Contact Email",
                    Body = string.Format(body, model.FromName, model.FromEmail, model.Message),
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                    Response.Redirect("/Home");

                }
            }
        }
    }
}
