using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Api.Models.AutoMapper
{
	public static class ErpAutoMapper
	{
		public static IMapper Mapper = null;

		public static void Initialize(MapperConfigurationExpression cfg)
		{
			Mapper = new Mapper(new MapperConfiguration(cfg));
		}
	}
}
