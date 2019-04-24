using System.Collections.Generic;
using Domain.Primitives;

namespace Domain.Abstractions
{
	public interface IVehicleRepositorical
	{
		Vehicle FindById(string id);
		IEnumerable<Vehicle> Vehicles { get; }
		string AddNewVehicle(Vehicle vehicle);
		void ReplaceExistingVehicle(string id, Vehicle vehicle);
		void AddNewVehicle(string id, Vehicle domainVehicle);
	}
}
