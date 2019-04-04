using CSScriptLib;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Service
{
	public static class CodeEvalService
	{
		private static readonly Dictionary<string, object> scriptObjects = new Dictionary<string, object>();

		//private static string CalculateMD5Hash(string input)
		//{
		//	MD5 md5 = MD5.Create();
		//	byte[] inputBytes = Encoding.ASCII.GetBytes(input);
		//	byte[] hash = md5.ComputeHash(inputBytes);

		//	StringBuilder sb = new StringBuilder();
		//	for (int i = 0; i < hash.Length; i++)
		//		sb.Append(hash[i].ToString("X2"));
		//	return sb.ToString();
		//}

		private static ICodeVariable GetScriptObject(string sourceCode)
		{
			if (string.IsNullOrWhiteSpace(sourceCode))
				throw new ArgumentException("SourceCode is empty");

			//dublication of MD5 hash, so we stopped using it
			//string md5Key = CalculateMD5Hash(sourceCode);
			string md5Key = sourceCode;
			if (scriptObjects.ContainsKey(md5Key))
				return scriptObjects[md5Key] as ICodeVariable;

			CSScript.EvaluatorConfig.RefernceDomainAsemblies = true;
			ICodeVariable scriptObject = CSScript.Evaluator.LoadCode<ICodeVariable>(sourceCode);
			scriptObjects[md5Key] = scriptObject;
			return scriptObject;
		}

		public static object Evaluate(string sourceCode, BaseErpPageModel pageModel)
		{
			ICodeVariable script = GetScriptObject(sourceCode);
			return script.Evaluate(pageModel);
		}

		public static void Compile(string sourceCode)
		{
			ICodeVariable script = GetScriptObject(sourceCode);
		}
	}
}
