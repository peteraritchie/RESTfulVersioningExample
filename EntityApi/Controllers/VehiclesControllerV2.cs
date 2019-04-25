using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using Domain.Primitives;
using EntityApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntityApi.Controllers
{
	/// <summary>
	/// A version that just returns an extra vehicle
	/// </summary>
	[Route("api/vehicles")]
	[ApiController]
	[ApiVersion("2.0")]
	//[Produces("application/vnd.null.entity+json; v=2.0", "application/vnd.null.entity+xml; v=2.0")]
	public class VehiclesControllerV2 : VehiclesControllerBase
	{
		public VehiclesControllerV2(IVehicleRepositorical repository) : base(repository)
		{
		}

		/// <inheritdoc />
		/// GET api/vehicles
		[HttpGet]
		public override ActionResult<IEnumerable<VehicleInfo>> Get()
		{
			return new OkObjectResult(repository.Vehicles.Append(new Vehicle {VehicleIdentifier = "version 2"})
				.Select(v => new VehicleInfo(v)).ToArray());
		}
	}
}
