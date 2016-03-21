using System;
using AutoMapper;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class ErpRoleConverter : ITypeConverter<EntityRecord, ErpRole>
    {
        public ErpRole Convert(ResolutionContext context)
        {
            var src = (EntityRecord)context.SourceValue;

            if (src == null)
                return null;

            ErpRole dest = new ErpRole();
            dest.Id = (Guid)src["id"];
            dest.Name = (string)src["name"];
            return dest;
        }
    }
}