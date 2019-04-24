using System.Net;
using System.Runtime.Serialization;

namespace EntityApi.Models
{
	[DataContract]
	public class ErrorResponseInfo
	{
		[DataMember(Name = "error")] public ErrorInfo Error { get; set; }

		public static ErrorResponseInfo BadRequestError(int subCode, string messageText, params string[] targets)
		{
			return new ErrorResponseInfo
			{
				Error = new ErrorInfo
				{
					Code = $"{(int) HttpStatusCode.BadRequest}.{subCode}",
					Message = new ErrorMessage
					{
						Text = messageText,
						Targets = targets
					}
				}
			};
		}
	}
}