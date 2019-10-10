using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;

namespace WebVella.Erp.Web
{
	internal class WebConfigurationOptions : IPostConfigureOptions<StaticFileOptions>
	{
		private readonly IWebHostEnvironment env;

		public WebConfigurationOptions(IWebHostEnvironment environment)
		{
			env = environment;
		}

		public void PostConfigure(string name, StaticFileOptions options)
		{
			options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();

			if (options.FileProvider == null && env.WebRootFileProvider == null)
				throw new InvalidOperationException("Missing FileProvider.");

			options.FileProvider = options.FileProvider ?? env.WebRootFileProvider;

			var filesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, "wwwroot");
			options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
		}
	}
}
