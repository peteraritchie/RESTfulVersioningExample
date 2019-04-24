using System.Collections.Generic;
using Domain.Abstractions;
using EntityApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntityApi.Controllers
{
	/// See <see cref="EntityApi.Startup.ConfigureServices"/> for where/how 
	/// an instance of VehicleController is composed at run time.
	[Route("api/[controller]")]
	[ApiController]
	[ApiVersion("1.0")]
	[Produces("application/vnd.null.entity+json; v=1.0")]
	public class VehiclesController : VehiclesControllerBase
	{
		public VehiclesController(IVehicleRepositorical repository) : base(repository)
		{
		}
	}
}