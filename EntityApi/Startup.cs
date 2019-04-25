using Domain.Abstractions;
using EntityApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Versioning;
using Domain.Primitives;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;

namespace EntityApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(options =>
			{
				options.RespectBrowserAcceptHeader = true;
				options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
				var xmlSerializerOutputFormatter = new XmlSerializerOutputFormatter();
				xmlSerializerOutputFormatter.SupportedMediaTypes.Add("application/vnd.null.entity+xml");
				xmlSerializerOutputFormatter.SupportedMediaTypes.Add("application/vnd.null.entity+xml; v=1.0");
				xmlSerializerOutputFormatter.SupportedMediaTypes.Add("application/vnd.null.entity+xml; v=2.0");
				options.OutputFormatters.Add(xmlSerializerOutputFormatter);
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddSingleton<IVehicleRepositorical>(
				new CollectionStubVehiclesRepository(new[]
				{
					new Vehicle {VehicleIdentifier = "3"},
					new Vehicle {VehicleIdentifier = "4"}
				}));
			var defaultVersion = new ApiVersion(1, 0);
			services.AddApiVersioning(options =>
			{
				options.ApiVersionReader = new MediaTypeApiVersionReader();
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.ReportApiVersions = true;
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.Conventions
					.Controller<VehiclesControllerV2>()
					.HasApiVersion(2, 0);
				options.Conventions
					.Controller<VehiclesController>()
					.HasApiVersion(defaultVersion);

				options.DefaultApiVersion = defaultVersion;
				//options.AddConventionsFromAssembly();
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}