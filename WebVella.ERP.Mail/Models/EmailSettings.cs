using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Mail.Models
{
    public class EmailSettings
    {
        public string Server { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}
