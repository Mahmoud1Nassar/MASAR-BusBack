using MASAR.Model.DTO;
using MASAR.Model;
using Microsoft.AspNetCore.Identity;
using MASAR.Data;
using MASAR.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
namespace MASAR.Repositories.Services
{
    public class MailjetService
    {
        private readonly IConfiguration _configuration;
        private readonly MailjetClient _client;

        public MailjetService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new MailjetClient(
                _configuration["Mailjet:ApiKey"],
                _configuration["Mailjet:SecretKey"]);
        }

        public async Task<bool> SendEmailAsync(string toEmail,  string TextPart)
        {
            var request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "abdullahraghad616@gmail.com")
            .Property(Send.FromName, "MASAR ")
            .Property(Send.Subject, "Reset Password")
            .Property(Send.TextPart, "Your Code is : " + TextPart)
            .Property(Send.HtmlPart, "Your Code is : " + TextPart)
            .Property(Send.Recipients, new JArray {
            new JObject {
                {"Email", toEmail},
                 {"Name", "aya"}
            }
            });


            MailjetResponse response = await _client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
