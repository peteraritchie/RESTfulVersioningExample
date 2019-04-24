using System.Runtime.Serialization;

namespace EntityApi.Models
{
	[DataContract]
	public class ErrorInfo
	{
		[DataMember(Name = "code")] public string Code { get; set; }
		[DataMember(Name = "message")] public ErrorMessage Message { get; set; }
	}
}