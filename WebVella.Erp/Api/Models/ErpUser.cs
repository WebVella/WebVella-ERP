using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebVella.Erp.Api.Models
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
			Username = String.Empty;
            Enabled = true;
			Verified = true;
		}

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }
		
		[JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

		[JsonIgnore]
		//[JsonProperty(PropertyName = "password")]
		public string Password { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

		[JsonIgnore]
		//[JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }

		[JsonIgnore]
		//[JsonProperty(PropertyName = "verified")]
		public bool Verified { get; set; }

		[JsonProperty(PropertyName = "createdOn")]
		public DateTime CreatedOn { get; set; }

		[JsonProperty(PropertyName = "lastLoggedIn")]
        public DateTime? LastLoggedIn { get; set; }

		[JsonIgnore]
		//[JsonProperty(PropertyName = "roles")]
		public List<ErpRole> Roles { get; private set; } = new List<ErpRole>();

		[JsonProperty(PropertyName = "is_admin")]
		public bool IsAdmin { get { return Roles.Any(x => x.Id == SystemIds.AdministratorRoleId); } }
	
		[JsonProperty(PropertyName = "preferences")]
		public ErpUserPreferences Preferences { get; set; }
	}
}