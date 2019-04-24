using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using Domain.Primitives;

namespace EntityApi.Controllers
{
	public class CollectionStubVehiclesRepository : IVehicleRepositorical
	{
		private IEnumerable<Vehicle> vehicles;

		public CollectionStubVehiclesRepository()
		{
			vehicles = new[] {new Vehicle(),};
		}

		public CollectionStubVehiclesRepository(IEnumerable<Vehicle> vehicles)
		{
			this.vehicles = vehicles;
		}

		public Vehicle FindById(string id)
		{
			return vehicles.SingleOrDefault(v =>
				id.Equals(v.VehicleIdentifier, StringComparison.InvariantCultureIgnoreCase));
		}

		public IEnumerable<Vehicle> Vehicles => vehicles;

		public string AddNewVehicle(Vehicle vehicle)
		{
			vehicles = vehicles.Append(vehicle);
			return vehicle.VehicleIdentifier;
		}

		public void ReplaceExistingVehicle(string id, Vehicle vehicle)
		{
			vehicles = vehicles.Where(v => v.VehicleIdentifier != id).Append(vehicle);
		}

		public void AddNewVehicle(string id, Vehicle domainVehicle)
		{
			domainVehicle.VehicleIdentifier = id;
			vehicles = vehicles.Append(domainVehicle);
		}
	}
}