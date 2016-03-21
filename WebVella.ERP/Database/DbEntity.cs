using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Database
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

		[JsonProperty(PropertyName = "weight")]
		public decimal Weight { get; set; }

		[JsonProperty(PropertyName = "record_permissions")]
		public DbRecordPermissions RecordPermissions { get; set; }

		[JsonProperty(PropertyName = "fields")]
		public List<DbBaseField> Fields { get; set; }

		[JsonProperty(PropertyName = "record_lists")]
		public List<DbRecordList> RecordLists { get; set; }

		[JsonProperty(PropertyName = "record_views")]
		public List<DbRecordView> RecordViews { get; set; }

		[JsonProperty(PropertyName = "record_trees")]
		public List<DbRecordTree> RecordTrees { get; set; }

		public DbEntity()
        {
            Fields = new List<DbBaseField>();
            RecordLists = new List<DbRecordList>();
            RecordViews = new List<DbRecordView>();
            RecordPermissions = new DbRecordPermissions();
        }
    }

    public class DbRecordPermissions
    {
		[JsonProperty(PropertyName = "can_read")]
		public List<Guid> CanRead { get; set; }

		[JsonProperty(PropertyName = "can_create")]
		public List<Guid> CanCreate { get; set; }

		[JsonProperty(PropertyName = "can_update")]
		public List<Guid> CanUpdate { get; set; }

		[JsonProperty(PropertyName = "can_delete")]
		public List<Guid> CanDelete { get; set; }

        public DbRecordPermissions()
        {
            CanRead = new List<Guid>();
            CanCreate = new List<Guid>();
            CanUpdate = new List<Guid>();
            CanDelete = new List<Guid>();
        }
    }
}