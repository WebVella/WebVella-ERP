using Newtonsoft.Json;
using System;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models
{
    public class SystemSettings
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        public SystemSettings()
        {

        }

        public SystemSettings(DbSystemSettings settings)
        {
            Id = settings.Id;
            Version = settings.Version;
        }
    }
}
