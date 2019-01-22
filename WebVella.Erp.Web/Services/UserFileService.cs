using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;
using WebVella.Erp.Utilities;

namespace WebVella.Erp.Web.Services
{
	public class UserFileService : BaseService
	{

		public List<UserFile> GetFilesList(string type = "", string search = "",  int sort = 1, int page = 1, int pageSize = 30)
		{
			//sort -> 1(created on), 2(filename alpha)
			//type -> image,document,audio,video
			var skipCount = (page-1)*pageSize;


			var listSorts = new List<QuerySortObject>();
			switch(sort) {
				case 1:
					listSorts.Add(new QuerySortObject("created_on", QuerySortType.Descending));
					break;
				case 2:
					listSorts.Add(new QuerySortObject("name", QuerySortType.Ascending));
					break;
			}

			var filters = new List<QueryObject>();
			if(!String.IsNullOrWhiteSpace(search)) {
				filters.Add(EntityQuery.QueryOR(EntityQuery.QueryContains("name",search),EntityQuery.QueryContains("alt",search),EntityQuery.QueryContains("caption",search)));
			}
			if(!String.IsNullOrWhiteSpace(type)) {
				filters.Add(EntityQuery.QueryContains("type",type));
			}
			var filterQuery = EntityQuery.QueryAND(filters.ToArray());
			
			EntityQuery query = new EntityQuery("user_file", UserFile.GetQueryColumns(), filterQuery, listSorts.ToArray(),skipCount,pageSize);
			QueryResponse response = RecMan.Find(query);
			if (!response.Success)
				throw new Exception(response.Message);

			var files = response.Object.Data.MapTo<UserFile>();

			return files;
		}

		public UserFile CreateUserFile(string path = "", string alt = "",  string caption = "")
		{
			var userFileRecord = new EntityRecord();
			if(path.StartsWith("/fs")) {
				path = path.Substring(3);
			}
			var tempFile = Fs.Find(path);
			if(tempFile == null) {
				throw new Exception("File not found on that path");
			}
			var newFileId = Guid.NewGuid();
			userFileRecord["id"] = newFileId;
			userFileRecord["alt"] = alt;
			userFileRecord["caption"] = caption;
			var fileKilobytes = Math.Round(((decimal)tempFile.GetBytes().Length / 1024),2);
			userFileRecord["size"] = fileKilobytes;
			userFileRecord["name"] = Path.GetFileName(path);
			var fileExtension = Path.GetExtension(path);
            var mimeType = MimeMapping.MimeUtility.GetMimeMapping(path);
            if (mimeType.StartsWith("image")) {
				var dimensionsRecord = Helpers.GetImageDimension(tempFile.GetBytes());
				userFileRecord["width"] = (decimal)dimensionsRecord["width"];
				userFileRecord["height"] = (decimal)dimensionsRecord["height"];
				userFileRecord["type"] = "image";
			}
			else if(mimeType.StartsWith("video")) {
				userFileRecord["type"] = "video";
			}
			else if(mimeType.StartsWith("audio")) {
				userFileRecord["type"] = "audio";
			}
			else if(fileExtension == ".doc" || fileExtension == ".docx"  || fileExtension == ".odt"  || fileExtension == ".rtf" 
			 || fileExtension == ".txt"  || fileExtension == ".pdf"  || fileExtension == ".html"  || fileExtension == ".htm"  || fileExtension == ".ppt" 
			  || fileExtension == ".pptx"  || fileExtension == ".xls"  || fileExtension == ".xlsx"  || fileExtension == ".ods"  || fileExtension == ".odp" ) {
				userFileRecord["type"] = "document";
			}
			else { 
				userFileRecord["type"] = "other";
			}

			var newFilePath = $"/file/{newFileId}/{Path.GetFileName(path)}";

			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				con.BeginTransaction();
				try
				{
					var file = Fs.Move(path,newFilePath,false);
					if(file == null) {
						throw new Exception("File move from temp folder failed");
					}

					userFileRecord["path"] = newFilePath;
					var response = RecMan.CreateRecord("user_file",userFileRecord);
					if(!response.Success)
						throw new Exception(response.Message);

					userFileRecord = response.Object.Data.First();
					con.CommitTransaction();
				}
				catch (Exception ex)
				{
					con.RollbackTransaction();
					throw ex;
				}
			}
			return userFileRecord.MapTo<UserFile>();
		}

	}
}
