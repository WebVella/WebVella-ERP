using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.Extensions.PlatformAbstractions;

namespace WebVella.ERP.Plugins
{
	public class PluginAssemblyProvider : DefaultAssemblyProvider
	{
		private readonly IAssemblyProvider[] additionalProviders;
		private readonly string[] referenceAssemblies;

		public PluginAssemblyProvider( ILibraryManager libraryManager,
			IAssemblyProvider[] additionalProviders = null, 
			string[] referenceAssemblies = null) : base(libraryManager)
		{
			this.additionalProviders = additionalProviders;
			this.referenceAssemblies = referenceAssemblies;
		}

		protected override HashSet<string> ReferenceAssemblies 	=> 
			referenceAssemblies == null ? base.ReferenceAssemblies : new HashSet<string>(referenceAssemblies);

		protected override IEnumerable<Library> GetCandidateLibraries()
		{
			var baseCandidates = base.GetCandidateLibraries();
			
			if (additionalProviders == null) 
				return baseCandidates;

			IEnumerable<Library> libs = additionalProviders.SelectMany(
				provider => provider.CandidateAssemblies.Select( x => 
					new Library(x.FullName, null,  Path.GetDirectoryName(x.Location), null,
						Enumerable.Empty<string>(), new[] { new AssemblyName(x.FullName) })));

			return baseCandidates.Concat(libs);
		}
	}
}
