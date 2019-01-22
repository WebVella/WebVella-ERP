using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Services
{
	public class ThemeService : BaseService
	{
		Theme theme = new Theme();

		public Theme Get()
		{
			Guid themeId = new WebSettingsService().Get().ThemeId;

			if (themeId == Guid.Empty)
			{
				//Default theme applies
				return theme;
			}

			return theme;
		}

		public string GenerateStyleFrameworkContent()
		{
			var cssContent = "";
			var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string file = FileService.GetEmbeddedTextResource("bootstrap.css", "WebVella.Erp.Web.Theme");
			cssContent = ApplyThemeSettingsToString(file);
			return cssContent;
		}

		public string GenerateStylesContent()
		{
			var cssContent = "";
			var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string file = FileService.GetEmbeddedTextResource("styles.css", "WebVella.Erp.Web.Theme");
			cssContent = ApplyThemeSettingsToString(file);
			return cssContent;
		}

		public string ApplyThemeSettingsToString(string input) {
			var themeSettings = new ThemeService().Get();
			var output = input;

			foreach (PropertyInfo propertyInfo in themeSettings.GetType().GetProperties()) {
				if (propertyInfo.Name != "Id" && propertyInfo.Name != "Name" && propertyInfo.Name != "Label" && propertyInfo.Name != "Description")
				{
					if (propertyInfo.CustomAttributes != null && propertyInfo.CustomAttributes.Any()
						&& propertyInfo.CustomAttributes.First().ConstructorArguments != null && propertyInfo.CustomAttributes.First().ConstructorArguments.Any()) {
						var tag = "--" + propertyInfo.CustomAttributes.First().ConstructorArguments[0].Value;
						var settingValue = themeSettings.GetType().GetProperty(propertyInfo.Name).GetValue(themeSettings, null);
						if (settingValue != null && settingValue is String) { 
							output = output.Replace(tag, (string)settingValue);
						}
					}
				}
			}


			return output;
		}


	}
}
