using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
    public class SystemSettings
    {
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        public SystemSettings()
        {

        }

        public SystemSettings(IStorageSystemSettings settings)
        {
            Version = settings.Version;
        }
    }
}
