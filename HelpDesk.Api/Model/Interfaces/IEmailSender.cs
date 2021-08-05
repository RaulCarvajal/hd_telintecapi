using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Model.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string message, string path = "");
    }
}
