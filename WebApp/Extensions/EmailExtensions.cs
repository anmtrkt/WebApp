using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Security;


namespace WebApp.Extensions
{
    public static class EmailExtensions
    {
        public static bool Sending(this IConfiguration configuration, string email, string header, string body)
        {

            var client = new SmtpClient("smtp.gmail.com", 587) //configuration.GetSection("SMTP:Host").Get<string>(), configuration.GetSection("SMTP:Port").Get<int>())
            {
                Credentials = new NetworkCredential("anmtrkt2@gmail.com", "zogx yxbk umpl uyqa"),//configuration.GetSection("SMTP:Email").Get<string>(), configuration.GetSection("SMTP:Password").Get<SecureString>()),
                EnableSsl = true
            };
            try
            {
                client.Send("anmtrkt2@gmail.com", email, header, body);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
   

    }
}

