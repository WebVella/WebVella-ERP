using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Database;

namespace WebVella.ERP.Web.Services
{
	public class BaseService
	{
		protected RecordManager RecMan { get; private set; } = new RecordManager();
		protected EntityManager EntMan { get; private set; } = new EntityManager();
		protected SecurityManager SecMan{ get; private set; } = new SecurityManager();
		protected EntityRelationManager RelMan { get; private set; } = new EntityRelationManager();
		protected DbFileRepository Fs { get; private set; } = new DbFileRepository();
		
	}
}
