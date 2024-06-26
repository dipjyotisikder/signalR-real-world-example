namespace SignalR.UnitTests;

/*    public class NotificationsServiceUnitTests
    {
        private readonly Mock<IHubContext<ApplicationHub>> _hubContext;

        public NotificationsServiceUnitTests()
        {
            _hubContext = new Mock<IHubContext<ApplicationHub>>();
        }

        [Fact]
        public async Task SendToAllAsyncReturnsTask()
        {
            // Arrange
            var mockHubClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClientProxy
                .Setup(x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockHubClients.Setup(x => x.All).Returns(mockClientProxy.Object);
            mockHubClients.Setup(x => x.Groups(It.IsAny<IReadOnlyList<string>>())).Returns(mockClientProxy.Object);

            _hubContext.Setup(x => x.Clients).Returns(mockHubClients.Object);

            // Act
            var notificationService = new HubService(_hubContext.Object);
            var result = async () => await notificationService.SendToAllAsync();

            // Assert
            result.Should().NotBeNull();
            await result.Should().NotThrowAsync();

            _hubContext.Verify(
                expression: x => x.Clients.All.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()),
                times: Times.Once);

            _hubContext.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SendToGroupAsync_ReturnsTask()
        {
            // Arrange
            string groupName = "chat_room";

            var mockHubClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClientProxy
                .Setup(x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockHubClients.Setup(x => x.All).Returns(mockClientProxy.Object);
            mockHubClients.Setup(x => x.Groups(It.IsAny<IReadOnlyList<string>>())).Returns(mockClientProxy.Object);
            _hubContext.Setup(x => x.Clients).Returns(mockHubClients.Object);

            // Act
            var notificationService = new HubService(_hubContext.Object);
            var result = async () => await notificationService.SendToGroupAsync(groupName);

            // Assert
            result.Should().NotBeNull();
            await result.Should().NotThrowAsync();

            _hubContext.Verify(
                expression: x => x.Clients.Groups(It.IsAny<IReadOnlyList<string>>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()),
                times: Times.Once);

            _hubContext.VerifyNoOtherCalls();
        }

    }*/