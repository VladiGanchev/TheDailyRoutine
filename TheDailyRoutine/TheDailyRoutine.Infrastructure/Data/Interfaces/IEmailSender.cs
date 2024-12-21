using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Infrastructure.Data.Interfaces
{
    public interface IEmailSender
    { 
            Task SendEmailAsync(string email, string subject, string message);
    }
}
