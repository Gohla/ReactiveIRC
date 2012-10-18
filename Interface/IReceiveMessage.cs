namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing a received IRC message.
    /// </summary>
    public interface IReceiveMessage : IMessage
    {
        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        ReceiveMessageType Type { get; }
    }
}
