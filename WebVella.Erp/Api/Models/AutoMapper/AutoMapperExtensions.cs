using System;
using AutoMapper;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace WebVella.Erp.Api.Models.AutoMapper
{
	public static class AutoMapperExtensions
	{
		public static List<TResult> MapTo<TResult>(this IEnumerable self, object additionalArguments )
		{
			if (self == null)
				return default(List<TResult>); //throw new ArgumentNullException();

			return (List<TResult>)ErpAutoMapper.Mapper.Map(self, self.GetType(), typeof(List<TResult>), 
				(opts => { opts.Items.Add("additional_arguments", additionalArguments); } ) );
		}
		public static TResult MapToSingleObject<TResult>(this IEnumerable self, object additionalArguments)
		{
			if (self == null)
				return default(TResult); //throw new ArgumentNullException();

			return (TResult)ErpAutoMapper.Mapper.Map(self, self.GetType(), typeof(TResult),
				(opts => { opts.Items.Add("additional_arguments", additionalArguments); }));
		}


		public static List<TResult> MapTo<TResult>(this IEnumerable self)
		{
			if (self == null)
				return default(List<TResult>); //throw new ArgumentNullException();

			return (List<TResult>)ErpAutoMapper.Mapper.Map(self, self.GetType(), typeof(List<TResult>));
		}

		public static TResult MapToSingleObject<TResult>(this IEnumerable self)
		{
			if (self == null)
				return default(TResult); //throw new ArgumentNullException();

			return (TResult)ErpAutoMapper.Mapper.Map(self, self.GetType(), typeof(TResult));
		}

		public static List<TResult> MapSingleObjectToList<TResult>(this object self)
		{
			if (self == null)
				return default(List<TResult>); //throw new ArgumentNullException();

			return (List<TResult>)ErpAutoMapper.Mapper.Map(self, self.GetType(), typeof(List<TResult>));
		}

		public static TResult MapTo<TResult>(this object self)
		{
			if (self == null)
				return default(TResult); //throw new ArgumentNullException();

			return (TResult)ErpAutoMapper.Mapper.Map(self, self.GetType(), typeof(TResult));
		}

		public static TResult MapPropertiesToInstance<TResult>(this object self, TResult value)
		{
			if (self == null)
				return default(TResult); //throw new ArgumentNullException();

			return (TResult)ErpAutoMapper.Mapper.Map(self, value, self.GetType(), typeof(TResult));
		}

        public static TResult MapTo<TResult>(this object self, object additionalArguments)
        {
            if (self == null)
                return default(TResult); //throw new ArgumentNullException();

            return (TResult)ErpAutoMapper.Mapper.Map(self, self.GetType(), typeof(TResult),
                (opts => { opts.Items.Add("additional_arguments", additionalArguments); }));
        }
        //public static TResult DynamicMapTo<TResult>(this object self)
        //{
        //	if (self == null)
        //		return default(TResult); //throw new ArgumentNullException();

        //	return (TResult)Mapper.DynamicMap(self, self.GetType(), typeof(TResult));
        //}

        //public static List<TResult> DynamicMapTo<TResult>(this IEnumerable self)
        //{
        //	if (self == null)
        //		return default(List<TResult>); //throw new ArgumentNullException();

        //	return (List<TResult>)Mapper.DynamicMap(self, self.GetType(), typeof(List<TResult>));
        //}

        //public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        //{
        //	var sourceType = typeof(TSource);
        //	var destinationType = typeof(TDestination);
        //	var existingMaps = Mapper.Configuration.GetAllTypeMaps().First(x => x.SourceType.Equals(sourceType)
        //		&& x.DestinationType.Equals(destinationType));
        //	foreach (var property in existingMaps.GetUnmappedPropertyNames())
        //	{
        //		expression.ForMember(property, opt => opt.Ignore());
        //	}
        //	return expression;
        //}
    }
}