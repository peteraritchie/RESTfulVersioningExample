using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace EntityApi
{
	public class Program
	{
		[ExcludeFromCodeCoverage( /* specific run-time requirement */)]
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		/// <summary>
		/// Encapsulate the creation of a builder and invoking Build
		/// </summary>
		/// <remarks>
		/// This is done for more comprehensive unit testing
		/// </remarks>
		/// <param name="args">command-line args</param>
		/// <returns>The built web host</returns>
		[ExcludeFromCodeCoverage( /* specific run-time requirement */)]
		public static IWebHost BuildWebHost(string[] args) =>
			CreateWebHostBuilder(args).Build();

		/// <summary>
		/// ASP.NET-supplied method
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}