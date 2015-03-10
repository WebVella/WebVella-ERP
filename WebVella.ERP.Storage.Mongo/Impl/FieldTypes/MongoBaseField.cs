using System;

namespace WebVella.ERP
{
    public class MongoBaseField : IField
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Required { get; set; }

        public string PlaceholderText { get; set; }

        public string Description { get; set; }

        public string HelpText { get; set; }
    }
}