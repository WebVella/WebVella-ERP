using System;
using AutoMapper;

namespace WebVella.Erp.Api.Models.AutoMapper.Resolvers
{
    public class GuidToStringConverter : ITypeConverter<Guid, string>
    {
        public string Convert(Guid source, string destination, ResolutionContext context)
        {
            return source.ToString("N");
        }
    }
}