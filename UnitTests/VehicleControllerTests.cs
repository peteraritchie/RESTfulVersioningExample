using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Domain.Primitives;
using EntityApi.Controllers;
using EntityApi.Exceptions;
using EntityApi.Models;
using Microsoft.AspNetCore.Http;
#if !PUT_CREATE_SUPPORTED
using System.Linq;
using System.Net;
#endif
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;




namespace UnitTests
{
	public class VehicleControllerTests
	{
		[Fact /*[RFC2616.10.2.1]*/]
		public void GetVehiclesResultsInOk()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.SetupGet(m => m.Vehicles).Returns(new[] {new Vehicle()});
			var controller = new VehiclesController(mock.Object);
			var result = controller.Get();
			var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleInfo>>>(result);
			var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
		}

		[Fact]
		public void GetVehiclesResultsInCorrectVehicleCount()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.SetupGet(m => m.Vehicles).Returns(new[] {new Vehicle()});
			var controller = new VehiclesController(mock.Object);
			var result = controller.Get();
			var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleInfo>>>(result);
			var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
			var sequence = Assert.IsAssignableFrom<IEnumerable<VehicleInfo>>(okResult.Value);
			Assert.Single(sequence);
		}

		// Get Response Content
		[Fact /*[RFC2616.10.2.1]*/]
		public void GetVehicleWithValidVehicleIdResultsInOk()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.Setup(m => m.FindById(It.Is<string>(s => s == "1")))
				.Returns(new Vehicle {VehicleIdentifier = "10", ModelIdentifier = "TestName"});
			var controller = new VehiclesController(mock.Object);
			var result = controller.Get("1");
			var actionResult = Assert.IsType<ActionResult<VehicleInfo>>(result);
			var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

			var vehicle = Assert.IsType<VehicleInfo>(okResult.Value);
			Assert.Equal("TestName", vehicle.ModelIdentifier);
			Assert.Equal("10", vehicle.VehicleIdentifier);
		}

		[Fact /*[RFC2616.10.4.5]*/]
		public void GetVehicleWithInvalidVehicleIdResultsInNotFound()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.Setup(m => m.FindById(It.IsAny<string>()))
				.Returns(default(Vehicle));
			var controller = new VehiclesController(mock.Object);
			var result = controller.Get("1");
			var actionResult = Assert.IsType<ActionResult<VehicleInfo>>(result);
			var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
		}

		[Theory /*[RFC2616.10.4.1]*/]
		[InlineData(default(string))]
		[InlineData("")]
		// TODO: other invalid types if necessary
		public void GetVehicleWithMalformedVehicleIdResultsInBadRequest(string id)
		{
			var mock = new Mock<IVehicleRepositorical>();
			var controller = new VehiclesController(mock.Object);
			var result = controller.Get(id);
			var actionResult = Assert.IsType<ActionResult<VehicleInfo>>(result);
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

			// TODO: Error object
		}

		[Theory /*[RFC2616.10.4.1]*/]
		[InlineData(default(string))]
		[InlineData("")]
		// TODO: other invalid types if necessary
		public void PutVehicleWithMalformedVehicleIdResultsInBadRequest(string id)
		{
			var mock = new Mock<IVehicleRepositorical>();
			var controller = new VehiclesController(mock.Object);
			var result = controller.Put(id, new VehicleInfo());
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);

			// TODO: Error object
		}

		[Theory /*[RFC2616.10.4.1]*/]
		[InlineData(default(VehicleInfo))]
		// TODO: other invalid types if necessary
		public void PutVehicleWithMalformedVehicleResultsInBadRequest(VehicleInfo Vehicle)
		{
			var mock = new Mock<IVehicleRepositorical>();
			var controller = new VehiclesController(mock.Object);
			var result = controller.Put("1", Vehicle);
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);

			// TODO: Error object
		}

		// Put update, NoContent
		[Fact /*[RFC2616.9.6]*/]
		public void PutUpdateVehicleIdResultsInNoContent()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.Setup(m => m.FindById(It.Is<string>(s => s == "2")))
				.Returns(new Vehicle { MakeIdentifier = "TestName", VehicleIdentifier = "2" });
			Vehicle updatedVehicle = null;
			mock.Setup(m => m.ReplaceExistingVehicle(It.Is<string>(s => s == "2"), It.IsAny<Vehicle>()))
				.Callback((string i, Vehicle p) => { updatedVehicle = p; });
			var controller = new VehiclesController(mock.Object);
			var result = controller.Put("2", new VehicleInfo {ModelIdentifier = "name 2", VehicleIdentifier = "10.1"});
			var noContentResult = Assert.IsType<NoContentResult>(result);

			// todo: validate result content
			//var Vehicle = Assert.IsType<Vehicle>(noContentResult.Value);
			//Assert.Equal("name 2", Vehicle.VehicleIdentifier);
			//Assert.Equal("10.1", Vehicle.VehicleIdentifier);
			Assert.NotNull(updatedVehicle);
		}

#if PUT_CREATE_SUPPORTED
		[Fact/*[RFC2616.9.6]*/]
		public void PutVehicleWithNewVehicleIdResultsInCreate()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.Setup(m => m.FindById(It.Is<string>(s => s == "2"))).Returns(default(Vehicle));
			Vehicle newVehicle = null;
			mock.Setup(m => m.AddNewVehicle(It.Is<string>(s => s == "2"), It.IsAny<Vehicle>()))
				.Callback((string i, Vehicle p) => { newVehicle = p; });

			var controller = new VehiclesController(mock.Object)
			{
				ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()}
			};
			var result = controller.Put("2", new Vehicle {VehicleIdentifier = "name 2", VehicleIdentifier = "10.1"});

			var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);

			var Vehicle = Assert.IsType<Vehicle>(createdAtRouteResult.Value);
			Assert.Equal("name 2", Vehicle.VehicleIdentifier);
			Assert.Equal("10.1", Vehicle.VehicleIdentifier);
			Assert.NotNull(newVehicle);
		}

#else

		[Fact /*[RFC2616.9.6]*/]
		public void PutVehicleWithNewVehicleIdResultsInCreate()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.Setup(m => m.FindById(It.Is<string>(s => s == "2")))
				.Returns(default(Vehicle));
			Vehicle newVehicle = null;
			mock.Setup(m => m.AddNewVehicle(It.Is<string>(s => s == "2"), It.IsAny<Vehicle>()))
				.Callback((string _, Vehicle p) => { newVehicle = p; });

			var controller = new VehiclesController(mock.Object)
			{
				ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()}
			};
			var result = controller.Put("2", new VehicleInfo
			{
				ModelIdentifier = "name 2", VehicleIdentifier = "10.1"
			});

			var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
			var vehicleInfo = Assert.IsType<VehicleInfo>(createdAtRouteResult.Value);
			Assert.Equal("name 2", vehicleInfo.ModelIdentifier);
			Assert.Equal("10.1", vehicleInfo.VehicleIdentifier);
			Assert.NotNull(newVehicle);
			//Assert.Equal((int) HttpStatusCode.MethodNotAllowed, createdAtRouteResult.StatusCode);
			//Assert.True(controller.Response.Headers.ContainsKey("Allow"));
			//Assert.Equal(new[] {"GET", "POST"},
			//	controller.Response.Headers["Allow"].Select(e => e)
			//		.Select(e => e.Split(", ", StringSplitOptions.RemoveEmptyEntries)).SelectMany(e => e).ToArray());
		}
#endif

		[Fact /*[RFC2616.10.4.1]*/]
		public void PostWithMissingVehicleResultsInBadRequest()
		{
			var mock = new Mock<IVehicleRepositorical>();
			var controller = new VehiclesController(mock.Object);
			var result = controller.Post(null);
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
		}

		[Fact /*[RFC2616.9.5]*/]
		public void PostVehicleResultsInCreate()
		{
			var mock = new Mock<IVehicleRepositorical>();
			Vehicle newVehicle = null;
			mock.Setup(m => m.AddNewVehicle(It.IsAny<Vehicle>()))
				.Callback((Vehicle p) => { newVehicle = p; })
				.Returns("3");
			var controller = new VehiclesController(mock.Object);
			var result = controller.Post(new VehicleInfo {ModelIdentifier= "name 3", VehicleIdentifier = "10.2"});
			var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);

			var vehicle = Assert.IsType<VehicleInfo>(createdAtRouteResult.Value);
			Assert.Equal("name 3", vehicle.ModelIdentifier);
			Assert.Equal("3", vehicle.VehicleIdentifier);
			Assert.NotNull(newVehicle);
		}


		// post bad Vehicle object, Bad Constraint, BadRequest
#if false
		[Fact /*[RFC2616.10.4.1]*/]
		public void PostVehicleConstraintViolationResultsInBadRequest()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.Setup(m => m.FindById(It.Is<string>(s => s == "2"))).Returns(default(VehicleInfo));
			mock.Setup(m => m.AddNewVehicle(It.IsAny<VehicleInfo>())).Throws<VehicleConstraintViolatedException>();
			var controller = new VehiclesController(mock.Object);
			var result = controller.Post(new VehicleInfo {VehicleIdentifier = "name 3", VehicleIdentifier = "10.2"});
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);

			var errorResponse = Assert.IsType<ErrorResponseInfo>(badRequestObjectResult.Value); // TODO: Error object
		}
#endif
		// Put bad Vehicle object, Bad Constraint, BadRequest
		[Fact /*[RFC2616.10.4.1]*/]
		public void PutUpdateVehicleConstraintViolationResultsInBadRequest()
		{
			var mock = new Mock<IVehicleRepositorical>();
			mock.Setup(m => m.FindById(It.Is<string>(s => s == "2"))).Returns(new Vehicle{MakeIdentifier = "TestName", VehicleIdentifier = "10"});
			mock.Setup(m => m.ReplaceExistingVehicle(It.Is<string>(s => s == "2"), It.IsAny<Vehicle>()))
				.Throws<VehicleConstraintViolatedException>();
			var controller = new VehiclesController(mock.Object);
			var result = controller.Put("2", new VehicleInfo {ModelIdentifier = "name 2", VehicleIdentifier = "10.1"});
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);

			var errorResponse = Assert.IsType<ErrorResponseInfo>(badRequestObjectResult.Value);
		}

		// TODO: Post response content

#if PUT_CREATE_SUPPORTED
		// race condition (InvalidOperationException, NoContent);
		[Fact/*[RFC2616.9.5]*/]
		public void PutCreateInvalidOperationResultsInNoContent()
		{
			var mock = new Mock<IVehicleRepositorical>();
			Vehicle updatedVehicle = null;
			mock.Setup(m => m.FindById(It.Is<string>(s => s == "2"))).Returns(default(Vehicle));
			mock.Setup(m => m.AddNewVehicle(It.Is<string>(s => s == "2"), It.IsAny<Vehicle>())).Throws<InvalidOperationException>();
			mock.Setup(m => m.ReplaceExistingVehicle(It.Is<string>(s => s == "2"), It.IsAny<Vehicle>())).Callback((string i, Vehicle p) => { updatedVehicle
 = p; });
			var controller = new VehiclesController(mock.Object)
			{
				ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
			};
			var result = controller.Put("2", new Vehicle {VehicleIdentifier = "name 2", VehicleIdentifier = "10.1"});
			var noContentResult = Assert.IsType<NoContentResult>(result);
		}
#endif
		[Fact]
		public void Version10CausesNoException()
		{
			var mock = new Mock<IVehicleRepositorical>();
			var httpContext = new DefaultHttpContext();
			httpContext.Request.Headers["Accept"] = "application/json;v=1.0";
			var controller = new VehiclesController(mock.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = httpContext
				}
			};
			var result = controller.Get();
			var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleInfo>>>(result);
		}
	}
}
