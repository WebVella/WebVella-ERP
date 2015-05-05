//Test model for an Area

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Web.Models
{
    public class Area
    {
		public Area() {
			Id = Guid.Empty;
			Name = String.Empty;
			Label = String.Empty;
			Weight = 0;
            Color = "indigo";
            IconName = "database";
			Sections = new List<AreaSection>();
		}

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		//Unique technical name of the entity, e.g. order
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		//Human readable identifier of a single instance of this entity, e.g. Order
		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		//Weight- way of sorting ares in the list, Bigger weight sinks the element down the list
		[JsonProperty(PropertyName = "weight")]
		public int Weight { get; set; }

		//Area color - string, should be one of the implemented in a theme or module css include
		[JsonProperty(PropertyName = "color")]
		public string Color { get; set; }

        //One of the available FontAwesome Icons
        [JsonProperty(PropertyName = "iconName")]
        public string IconName { get; set; }

        //Area Entity sections (grouping). Entities that need to be directly linked to the area must have section with id = Guid.Empty
        [JsonProperty(PropertyName = "sections")]
		public List<AreaSection> Sections { get; set; }

	}

	public class AreaSection
	{
		public AreaSection()
		{
			Id = Guid.Empty;
			Name = String.Empty;
			Label = String.Empty;
			Weight = 0;
			Entities = new List<Entity>();
		}

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		//Unique technical name of the entity, e.g. order
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		//Human readable identifier of a single instance of this entity, e.g. Order
		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		//Weight- way of sorting ares in the list, Bigger weight sinks the element down the list
		[JsonProperty(PropertyName = "weight")]
		public int Weight { get; set; }

		//Entities attached to the  section. Entities that need to be directly linked to the area must have section with id = Guid.Empty
		[JsonProperty(PropertyName = "entities")]
		public List<Entity> Entities { get; set; }

	}
}