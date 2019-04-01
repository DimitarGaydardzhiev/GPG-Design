using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GPGDesign.Models.ContactModels;

namespace GPGDesign.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View(new EmailFormModel());
        }

        [HttpPost]
        public void SendEmail(EmailFormModel model)
        {

            if (ModelState.IsValid)
            {
                const string fromPassword = "Welkom2018!doepicshit";
                var fromAddress = new MailAddress("teofil.y@gmail.com", "From Name");
                var toAddress = new MailAddress("teofil.y@gmail.com, dimiter.gg@gmail.com", "To Name");
                var credential = new NetworkCredential
                {
                    UserName = "teofil.y@gmail.com",
                    Password = fromPassword
                };

                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                //var message = new MailMessage();
                //message.To.Add(new MailAddress("teofil.y@gmail.com"));
                //message.From = new MailAddress(model.FromEmail);


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
