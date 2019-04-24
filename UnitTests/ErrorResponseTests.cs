using EntityApi.Models;
using Newtonsoft.Json;
using Xunit;

namespace UnitTests
{
	public class ErrorResponseTests
	{
		[Fact]
		public void JsonPropertyNamesAreCorrect()
		{
			var error = new ErrorResponseInfo
			{
				Error = new ErrorInfo
				{
					Code = "400.1",
					Message = new ErrorMessage
					{
						Text = "Field cannot be empty",
						Targets = new[] { "request.name" },
						Links = new HelpLinks
						{
							Help = new[]
							{
								new HelpLink {Href = "the URL", Name = "the name", Title = "the title"}
							}
						}
					}
				}
			};

			var json = JsonConvert.SerializeObject(error);

			Assert.NotNull(json);
			Assert.Equal(
				"{\"error\":{\"code\":\"400.1\",\"message\":{\"text\":\"Field cannot be empty\",\"targets\":[\"request.name\"],\"_links\":{\"help\":[{\"href\":\"the URL\",\"title\":\"the title\",\"name\":\"the name\"}]}}}}",
				json);
		}
	}
}
