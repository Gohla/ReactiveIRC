namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing an IRC message that can be sent.
    /// </summary>
    public interface ISendMessage : IMessage
    {
        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        SendMessageType Type { get; }
    }
}
