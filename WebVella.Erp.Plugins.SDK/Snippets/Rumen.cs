using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Web.Models;


public class Rumen : ICodeVariable
{
	public object Evaluate(BaseErpPageModel pageModel)
	{
		try
		{
			if (pageModel == null)
				return "";

			return "Success DS boz";
		}
		catch (Exception ex)
		{
			return "Error: " + ex.Message;
		}
	}
}

