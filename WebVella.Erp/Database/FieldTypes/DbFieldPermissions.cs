using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace WebVella.Erp.Database
{
	public class DbFieldPermissions
    {
        [JsonProperty(PropertyName = "can_read")]
        public List<Guid> CanRead { get; set; }

        [JsonProperty(PropertyName = "can_update")]
        public List<Guid> CanUpdate { get; set; }

        public DbFieldPermissions()
        {
            CanRead = new List<Guid>();
            CanUpdate = new List<Guid>();
        }
    }
}