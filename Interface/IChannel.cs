using System;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    /// <summary>
    /// Interface representing channels on an IRC server.
    /// </summary>
    public interface IChannel : IMessageTarget, IComparable<IChannel>, IEquatable<IChannel>, IKeyedObject<String>
    {
        /// <summary>
        /// Gets an observable stream of received and sent messages for this channel.
        /// </summary>
        IObservable<IMessage> Messages { get; }

        /// <summary>
        /// Gets an observable stream of received messages for this channel from the server.
        /// </summary>
        IObservable<IReceiveMessage> ReceivedMessages { get; }

        /// <summary>
        /// Gets an observable stream of all sent messages for this channel by the client.
        /// </summary>
        IObservable<ISendMessage> SentMessages { get; }

        /// <summary>
        /// Gets the users in this channel.
        /// </summary>
        IObservableCollection<IChannelUser> Users { get; }

        /// <summary>
        /// Gets the modes of the channel.
        /// </summary>
        Mode Modes { get; }

        /// <summary>
        /// Gets the topic of the channel. Subscribe to receive changes.
        /// </summary>
        ObservableProperty<String> Topic { get; }

        /// <summary>
        /// Gets the channel user with given nickname.
        /// </summary>
        IChannelUser GetUser(String nickname);
    }
}
