using System;
using System.Threading.Tasks;
using MaicoLand.Models;
using MaicoLand.Models.StructureType;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

}
