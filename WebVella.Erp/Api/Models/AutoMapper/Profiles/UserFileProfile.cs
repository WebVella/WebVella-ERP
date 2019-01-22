using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
	public class UserFileProfile : Profile
	{
		public UserFileProfile()
		{
			CreateMap<EntityRecord, UserFile>().ConvertUsing(source => EntityRecordConvert(source));
		}

		private static UserFile EntityRecordConvert(EntityRecord inputObj)
		{
			
			if (inputObj == null)
				return null;

			var outputObj = new UserFile();
			outputObj.Id = (Guid)inputObj["id"];
			outputObj.Alt = (string)inputObj["alt"];
			outputObj.Caption = (string)inputObj["caption"];
			outputObj.CreatedOn = (DateTime)inputObj["created_on"];
			outputObj.Height = (decimal)inputObj["height"];
			outputObj.Name = (string)inputObj["name"];
			outputObj.Path = (string)inputObj["path"];
			outputObj.Size = (decimal)inputObj["size"];
			outputObj.Type = (string)inputObj["type"];
			outputObj.Width = (decimal)inputObj["width"];
			outputObj.Alt = (string)inputObj["alt"];
			outputObj.Alt = (string)inputObj["alt"];
			return outputObj;
		}

	}
}
