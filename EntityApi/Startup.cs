using Domain.Abstractions;
using EntityApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Versioning;
using Domain.Primitives;
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddTransient(c =>
				new VehiclesController(new CollectionStubVehiclesRepository(new[]
				{
					new Vehicle {VehicleIdentifier = "1"},
					new Vehicle {VehicleIdentifier = "2"}
				})));
			services.AddSingleton<IVehicleRepositorical>(
				new CollectionStubVehiclesRepository(new[]
				{
					new Vehicle {VehicleIdentifier = "3"},
					new Vehicle {VehicleIdentifier = "4"}
				}));

			services.AddApiVersioning(options =>
			{
				options.ApiVersionReader = new MediaTypeApiVersionReader("v");
				options.ReportApiVersions = true;
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.Conventions
					.Controller<VehiclesController>()
					.HasApiVersion(1, 0);
				options.Conventions
					.Controller<VehiclesControllerV2>()
					.HasApiVersion(2, 0);
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