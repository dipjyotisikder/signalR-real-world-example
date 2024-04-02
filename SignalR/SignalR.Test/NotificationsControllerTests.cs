using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SignalR.SelfHosted.Notification;

namespace SignalR.SelfHosted.Test
{
    public class NotificationsControllerTests
    {
        private readonly Mock<IHubContext<NotificationHub>> _hubContext;

        public NotificationsControllerTests()
        {
            _hubContext = new Mock<IHubContext<NotificationHub>>();
        }

        [Fact]
        public void CreateGroup_WhenInvalidGroupName_ReturnsBadRequest()
        {
            // Arrange
            string groupName = "";

            // Act
            var notificationController = new NotificationsController(_hubContext.Object);
            var result = notificationController.CreateGroup(groupName);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var objectResult = result as BadRequestObjectResult;
            objectResult.StatusCode.Should().Be(400);
            objectResult.Value.Should().Be("Invalid/empty group name.");

            _hubContext
                .Verify(x => x.Clients
                    .Groups(It.IsAny<IReadOnlyList<string>>())
                    .SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), times: Times.Never);
        }

        [Fact]
        public void CreateGroup_WhenValidGroupName_ReturnsOk()
        {
            // Arrange
            string groupName = "chat_room";

            // Act
            var notificationController = new NotificationsController(_hubContext.Object);
            var result = notificationController.CreateGroup(groupName);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var objectResult = result as OkObjectResult;
            objectResult.StatusCode.Should().Be(200);
            objectResult.Value.Should().BeOfType<HashSet<string>>();

            _hubContext
                .Verify(x => x.Clients
                    .Groups(It.IsAny<IReadOnlyList<string>>())
                    .SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), times: Times.Never);
        }

        [Fact]
        public void GetGroups_ReturnsOk()
        {
            // Arrange

            // Act
            var notificationController = new NotificationsController(_hubContext.Object);
            var result = notificationController.GetGroups();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var objectResult = result as OkObjectResult;
            objectResult.StatusCode.Should().Be(200);
            objectResult.Value.Should().BeOfType<HashSet<string>>();

            _hubContext
                .Verify(x => x.Clients
                    .Groups(It.IsAny<IReadOnlyList<string>>())
                    .SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), times: Times.Never);
        }

        [Fact]
        public async Task SendAll_ReturnsOk()
        {
            // Arrange
            var mockHubClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClientProxy
                .Setup(x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            mockHubClients.Setup(x => x.Groups(It.IsAny<IReadOnlyList<string>>())).Returns(mockClientProxy.Object);
            _hubContext.Setup(x => x.Clients).Returns(mockHubClients.Object);

            // Act
            var notificationController = new NotificationsController(_hubContext.Object);
            var result = await notificationController.SendAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var objectResult = result as OkObjectResult;
            objectResult.StatusCode.Should().Be(200);
            objectResult.Value.Should().Be("Sent to all connected clients.");

            _hubContext
                .Verify(x => x.Clients.Groups(It.IsAny<IReadOnlyList<string>>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), times: Times.Once);
        }

        [Fact]
        public async Task SendToGroup_WhenInvalidGroupName_ReturnsBadRequest()
        {
            // Arrange
            string groupName = "";

            // Act
            var notificationController = new NotificationsController(_hubContext.Object);
            var result = await notificationController.SendToGroup(groupName);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var objectResult = result as BadRequestObjectResult;
            objectResult.StatusCode.Should().Be(400);
            objectResult.Value.Should().Be("Invalid/empty group name.");

            _hubContext
                .Verify(x => x.Clients
                    .Groups(It.IsAny<IReadOnlyList<string>>())
                    .SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), times: Times.Never);
        }

        [Fact]
        public async Task SendToGroup_WhenValidGroupName_ReturnsOk()
        {
            // Arrange
            string groupName = "chat_room";

            var mockHubClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClientProxy
                .Setup(x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            mockHubClients.Setup(x => x.Groups(It.IsAny<IReadOnlyList<string>>())).Returns(mockClientProxy.Object);
            _hubContext.Setup(x => x.Clients).Returns(mockHubClients.Object);

            // Act
            var notificationController = new NotificationsController(_hubContext.Object);
            var result = await notificationController.SendToGroup(groupName);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var objectResult = result as OkObjectResult;
            objectResult.StatusCode.Should().Be(200);
            objectResult.Value.Should().Be($"Sent to all connected clients of {groupName} group.");

            _hubContext
                .Verify(x => x.Clients.Groups(It.IsAny<IReadOnlyList<string>>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), times: Times.Once);
        }
    }
}