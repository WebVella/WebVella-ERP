using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
	[Serializable]
	public class ErpUser
    {
        public ErpUser()
        {
            Id = Guid.Empty;
            Email = String.Empty;
            Password = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            Enabled = true;
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }
		
		[JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }

        [JsonProperty(PropertyName = "createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public Guid? CreatedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonProperty(PropertyName = "modifiedBy")]
        public Guid? ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "lastLoggedIn")]
        public DateTime? LastLoggedIn { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public List<ErpRole> Roles { get; set; }
    }
}