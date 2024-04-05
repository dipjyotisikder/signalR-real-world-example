using FluentAssertions;
using SignalR.SelfHosted.Notification.Services;

namespace SignalR.SelfHosted.UnitTests
{
    public class HubServiceUnitTests
    {
        [Fact]
        public void CreateGroup_ReturnsGroupList()
        {
            // Arrange
            string groupName = "";

            // Act
            var hubService = new HubService();
            var result = hubService.CreateGroup(groupName);

            // Assert
            result.Should().BeOfType<HashSet<string>>();
            result.Count.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public void GetGroups_ReturnsOk()
        {
            // Act
            var hubService = new HubService();
            var result = hubService.GetGroups();

            // Assert
            result.Should().BeOfType<HashSet<string>>();
            result.Count.Should().BeGreaterThanOrEqualTo(1);
        }
    }
}