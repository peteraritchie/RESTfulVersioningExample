using System.Runtime.Serialization;
using Domain.Primitives;

namespace EntityApi.Models
{
	/// <summary>
	/// The abstraction that represents vehicle information used within API
	/// requests and responses for this service.
	/// </summary>
	/// <remarks>
	/// The "Info" suffix signifies that this class has no behavior
	/// and represents just information about the prefix.  In this case Info
	/// about a Vehicle.
	/// </remarks>
	[DataContract]
	public class VehicleInfo
	{
		public VehicleInfo()
		{
		}

		public VehicleInfo(Vehicle vehicle)
		{
			VehicleIdentifier = vehicle.VehicleIdentifier;
			ModelIdentifier = vehicle.ModelIdentifier;
			ModelYearIdentifier = vehicle.ModelYearIdentifier;
			MakeIdentifier = vehicle.MakeIdentifier;
		}

		/// <summary/>
		/// <remarks>
		/// The "Identifier suffix signifies that this property contains
		/// a value unique to a context (in this case, all vehicles)
		/// with unknown, or external, value constraints  and contains
		/// the identifier for the prefix (Vehicle)
		/// </remarks>
		[DataMember(Name = "vehicleIdentifier")]
		public string VehicleIdentifier { get; set; }

		[DataMember(Name = "makeIdentifier")] public string MakeIdentifier { get; set; }

		[DataMember(Name = "modelYearIdentifier")]
		public int ModelYearIdentifier { get; set; }

		[DataMember(Name = "modelIdentifier")] public string ModelIdentifier { get; set; }

		public Vehicle ToVehicle()
		{
			return new Vehicle
			{
				MakeIdentifier = MakeIdentifier,
				ModelIdentifier = ModelIdentifier,
				ModelYearIdentifier = ModelYearIdentifier,
				VehicleIdentifier = VehicleIdentifier,
			};
		}
	}
}