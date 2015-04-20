using System;
using System.Collections.Generic;

namespace WebVella.ERP
{
    public enum FormLayouts
    {
        OneColumn = 1,
        TwoColumns
    }

    public enum FormColumns
    {
        Left = 1,
        Right
    }

    public interface IFormField
    {
        Guid Id { get; set; }

        Guid EntityId { get; set; }

        FormColumns Column { get; set; }

        int Position { get; set; }
    }

    public interface IForm
    {
        string Name { get; set; }

        string DisplayName { get; set; }

        IList<IFormField> Fields { get; set; }
    }
}