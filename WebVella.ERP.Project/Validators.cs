using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Project
{
    public class Validators
    {
		RecordManager recMan;
		EntityManager entityManager;
		EntityRelationManager entityRelationManager;
		SecurityManager secMan;

		public Validators()
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entityManager = new EntityManager();
			entityRelationManager = new EntityRelationManager();
		}

		public static List<ErrorModel> ValidateTask(List<ErrorModel> currentErrors, EntityRecord taskObject, Guid recordId) {
			var errorList = currentErrors;

			return errorList;
		}

    }
}
