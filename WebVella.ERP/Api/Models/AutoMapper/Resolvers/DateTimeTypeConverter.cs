using System;
using AutoMapper;

namespace WebVella.ERP.Api.Models.AutoMapper.Resolvers
{
    public class DateTimeTypeConverter : TypeConverter<DateTimeOffset, DateTime>
    {
        protected override DateTime ConvertCore(DateTimeOffset source)
        {
            return source.DateTime;
        }
    }
}