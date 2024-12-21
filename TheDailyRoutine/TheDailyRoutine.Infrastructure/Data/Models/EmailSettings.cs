using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Infrastructure.Data.Models
{
    public class EmailSettings
    {
        
            public string SMTPServer { get; set; }
            public int SMTPPort { get; set; }
            public string SenderName { get; set; }
            public string SenderEmail { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        
    }
}
