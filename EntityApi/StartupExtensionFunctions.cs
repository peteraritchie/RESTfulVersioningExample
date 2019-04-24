using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using PRI.ProductivityExtensions.ReflectionExtensions;

namespace EntityApi
{
	/// TODO: break out into shared library
	public static class StartupExtensionFunctions
	{
		public static ApiVersioningOptions AddConventionsFromAssembly(this ApiVersioningOptions options)
		{
			var versionedControllers = from type in typeof(Startup).Assembly.GetTypes()
				where type.IsPublic && !type.IsAbstract
				                    && typeof(ControllerBase).IsAssignableFrom(type)
				                    && type.HasAttribute<ApiVersionAttribute>()
				select type;

			foreach (var classType in versionedControllers)
			{
				var apiVersionAttributes = classType.GetCustomAttributes<ApiVersionAttribute>();
				foreach (var apiVersionAttribute in apiVersionAttributes)
				{
					foreach (var apiVersion in apiVersionAttribute.Versions)
						options.Conventions
							.Controller(classType)
							.HasApiVersion(apiVersion);
				}
			}

			return options;
		}
	}
}