namespace SignalR.Api.UserModule.Services
{
    /// <summary>
    /// Current User object.
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Id of current user.
        /// </summary>
        int Id { get; }
    }
}
