using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using Domain.Primitives;

namespace EntityApi.Controllers
{
	public class CollectionStubVehiclesRepository : IVehicleRepositorical
	{
		public CollectionStubVehiclesRepository(int quantity)
		{
			Vehicles = Enumerable.Range(1, quantity).Select(i => new Vehicle
			{
				VehicleIdentifier = $"YV1RH{527362514776 + i}",
				MakeIdentifier = "Volvo",
				ModelIdentifier = "XL",
				ModelYearIdentifier = 2019

			});
		}

		public CollectionStubVehiclesRepository(IEnumerable<Vehicle> vehicles)
		{
			this.Vehicles = vehicles;
		}

		public Vehicle FindById(string id)
		{
			return Vehicles.SingleOrDefault(v =>
				id.Equals(v.VehicleIdentifier, StringComparison.InvariantCultureIgnoreCase));
		}

		public IEnumerable<Vehicle> Vehicles { get; private set; }

		public string AddNewVehicle(Vehicle vehicle)
		{
			Vehicles = Vehicles.Append(vehicle);
			return vehicle.VehicleIdentifier;
		}

		public void ReplaceExistingVehicle(string id, Vehicle vehicle)
		{
			Vehicles = Vehicles.Where(v => v.VehicleIdentifier != id).Append(vehicle);
		}

		public void AddNewVehicle(string id, Vehicle domainVehicle)
		{
			domainVehicle.VehicleIdentifier = id;
			Vehicles = Vehicles.Append(domainVehicle);
		}
	}
}