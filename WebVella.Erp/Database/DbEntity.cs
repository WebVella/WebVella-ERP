using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Database
{
    public class DbEntity : DbDocumentBase
    {
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		
		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "label_plural")]
		public string LabelPlural { get; set; }

		[JsonProperty(PropertyName = "system")]
		public bool System { get; set; }

		[JsonProperty(PropertyName = "icon_name")]
		public string IconName { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

		[JsonProperty(PropertyName = "record_permissions")]
		public DbRecordPermissions RecordPermissions { get; set; } = new DbRecordPermissions();

		[JsonProperty(PropertyName = "fields")]
		public List<DbBaseField> Fields { get; set; } = new List<DbBaseField>();

		[JsonProperty(PropertyName = "record_screen_id_field")]
		public Guid? RecordScreenIdField { get; set; } //If null the ID field of the record is used as ScreenId
    }

    public class DbRecordPermissions
    {
		[JsonProperty(PropertyName = "can_read")]
		public List<Guid> CanRead { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "can_create")]
		public List<Guid> CanCreate { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "can_update")]
		public List<Guid> CanUpdate { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "can_delete")]
		public List<Guid> CanDelete { get; set; } = new List<Guid>();
    }
}