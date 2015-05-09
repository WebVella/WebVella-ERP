using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Api.Models
{
    public class User : EntityRecord
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Username { get; set; }

        string Email { get; set; }

        string Password { get; set; }

        public bool? Enabled { get; set; }

        public bool? Verified { get; set; }

        DateTime LastLoggedIn { get; set; }
    }
}
