using System;
using AutoMapper;

namespace WebVella.Erp.Api.Models.AutoMapper.Resolvers
{
    public class StringToGuidConverter : ITypeConverter<string, Guid>
    {
        public Guid Convert(string source, Guid destination, ResolutionContext context)
        {
            Guid guid;
            if (Guid.TryParse(source, out guid) == false)
                return Guid.Empty;
            return guid;
        }
    }

    public class StringToNullableGuidConverter : ITypeConverter<string, Guid?>
    {
        public Guid? Convert(string source, Guid? destination, ResolutionContext context)
        {
            Guid guid;
            if (Guid.TryParse(source, out guid) == false)
                return null;
            return guid;
        }
    }
}