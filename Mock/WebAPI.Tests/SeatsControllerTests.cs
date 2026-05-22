using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    [TestMethod]
    public void ReserveSeatOk()
    {
        Mock<SeatsService> mockService = new Mock<SeatsService>();
        Mock<SeatsController> mockController = new Mock<SeatsController>(mockService.Object) { CallBase = true };

        var seat = new Seat { Id = 1, Number = 1, ExamenUserId = "testuser" };

        mockController.Setup(c => c.UserId).Returns("testuser");
        mockService.Setup(s => s.ReserveSeat("testuser", 1)).Returns(seat);

        var actionResult = mockController.Object.ReserveSeat(1);
        var result = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(seat, result.Value);
    }

    [TestMethod]
    public void ReserveTakenSeat()
    {
        Mock<SeatsService> mockService = new Mock<SeatsService>();
        Mock<SeatsController> mockController = new Mock<SeatsController>(mockService.Object) { CallBase = true };

        mockController.Setup(c => c.UserId).Returns("testuser");
        mockService.Setup(s => s.ReserveSeat(It.IsAny<string>(), 1)).Throws(new SeatAlreadyTakenException());

        var result = mockController.Object.ReserveSeat(1);
        var actionResult = result.Result as UnauthorizedResult;

        Assert.IsNotNull(actionResult);
    }

    [TestMethod]
    public void ReserveNonExistingSeat()
    {
        Mock<SeatsService> mockService = new Mock<SeatsService>();
        Mock<SeatsController> mockController = new Mock<SeatsController>(mockService.Object) { CallBase = true };

        mockController.Setup(c => c.UserId).Returns("testuser");
        mockService.Setup(s => s.ReserveSeat(It.IsAny<string>(), 101)).Throws(new SeatOutOfBoundsException());

        var result = mockController.Object.ReserveSeat(101);
        var actionResult = result.Result as NotFoundObjectResult;

        Assert.IsNotNull(actionResult);
        Assert.AreEqual("Could not find 101", actionResult.Value);
    }

    [TestMethod]
    public void ReserveTwiceSeat()
    {
        Mock<SeatsService> mockService = new Mock<SeatsService>();
        Mock<SeatsController> mockController = new Mock<SeatsController>(mockService.Object) { CallBase = true };

        mockController.Setup(c => c.UserId).Returns("testuser");
        mockService.Setup(s => s.ReserveSeat(It.IsAny<string>(), 1)).Throws(new UserAlreadySeatedException());

        var result = mockController.Object.ReserveSeat(1);
        var actionResult = result.Result as BadRequestResult;

        Assert.IsNotNull(actionResult);
    }
}
