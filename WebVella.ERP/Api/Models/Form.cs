using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class Form
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public IList<FormField> Fields { get; set; }
    }

    public class FormField
    {
        public Guid? Id { get; set; }

        public Guid? EntityId { get; set; }

        public FormColumns? Column { get; set; }

        public int? Position { get; set; }
    }

    public class FormList
    {
        public Guid Offset { get; set; }

        public List<Form> Forms { get; set; }
    }

    public class FormResponse : BaseResponseModel
    {
        public Form Object { get; set; }
    }

    public class FormListResponse : BaseResponseModel
    {
        public FormList Object { get; set; }
    }
}