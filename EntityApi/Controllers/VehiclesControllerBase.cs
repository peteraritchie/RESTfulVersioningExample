using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using EntityApi.Exceptions;
using EntityApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntityApi.Controllers
{
	public abstract class VehiclesControllerBase : ControllerBase
	{
		protected IVehicleRepositorical repository;
		protected const string GetByIdRouteName = "GetProduct";

		protected VehiclesControllerBase(IVehicleRepositorical repository)
		{
			this.repository = repository;
		}

		/// GET api/vehicles
		[HttpGet]
		public virtual ActionResult<IEnumerable<VehicleInfo>> Get()
		{
			// TODO: implement Get using repository
			return Ok(repository.Vehicles.Select(v => new VehicleInfo(v)));
		}

		/// GET api/vehicles/5
		[HttpGet("{id}", Name = VehiclesController.GetByIdRouteName)]
		public virtual ActionResult<VehicleInfo> Get(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
				return BadRequest(ErrorResponseInfo.BadRequestError(1, "missing product id", nameof(id)));
			var vehicle = repository.FindById(id);
			if (vehicle == null) return new NotFoundResult();

			return Ok(new VehicleInfo(vehicle));
		}

		/// POST api/vehicles
		[HttpPost]
		public virtual IActionResult Post([FromBody] VehicleInfo vehicle)
		{
			if (vehicle == null)
				return BadRequest(ErrorResponseInfo.BadRequestError(1, "missing product", nameof(vehicle)));
			var vehicleId = repository.AddNewVehicle(vehicle.ToVehicle());
			vehicle.VehicleIdentifier = vehicleId;
			return CreatedAtRoute(VehiclesController.GetByIdRouteName,
				new {id = vehicleId}, vehicle);
		}

		/// PUT api/vehicles/5
		[HttpPut("{id}")]
		public virtual IActionResult Put(string id, [FromBody] VehicleInfo vehicle)
		{
			// TODO: implement Put(string, VehicleInfo) properly
			if (vehicle == null)
				return BadRequest(ErrorResponseInfo.BadRequestError(1, "missing product", nameof(vehicle)));
			if (string.IsNullOrWhiteSpace(id))
				return BadRequest(ErrorResponseInfo.BadRequestError(1, "missing product id", nameof(id)));
			var existing = repository.FindById(id);

			try
			{
				if (existing == null)
				{
					repository.AddNewVehicle(id, vehicle.ToVehicle());
					return CreatedAtRoute(VehiclesController.GetByIdRouteName,
						new {id}, vehicle);
				}

				repository.ReplaceExistingVehicle(id, vehicle.ToVehicle());
			}
			catch (VehicleConstraintViolatedException exception)
			{
				return BadRequest(ErrorResponseInfo.BadRequestError(2, $"invalid vehicle body {exception.Message}",
					nameof(vehicle)));
			}

			return NoContent();
		}

		/// DELETE api/vehicles/5
		[HttpDelete("{id}")]
		public virtual IActionResult Delete(int id)
		{
			// TODO: implement Delete
			return Ok();
		}
	}
}