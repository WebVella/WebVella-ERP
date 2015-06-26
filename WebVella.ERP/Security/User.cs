using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Security
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; }
        public bool Enabled { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}