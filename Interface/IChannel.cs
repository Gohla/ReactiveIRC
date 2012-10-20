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
        /// Gets the topic of the channel. Initially set to empty string. Subscribe to receive changes.
        /// </summary>
        ObservableProperty<String> Topic { get; }

        /// <summary>
        /// Gets the user that set the topic. Initially set to null. Subscribe to receive changes.
        /// </summary>
        ObservableProperty<IUser> TopicSetBy { get; }

        /// <summary>
        /// Gets the date when this topic was set. Initially set to 0. Subscribe to receive changes.
        /// </summary>
        ObservableProperty<uint> TopicSetDate { get; }

        /// <summary>
        /// Gets the date at which the channel was created. Initially set to 0. Subscribe to receive changes.
        /// </summary>
        ObservableProperty<uint> CreatedDate { get; }

        /// <summary>
        /// Gets the channel user with given nickname.
        /// </summary>
        IChannelUser GetUser(String nickname);
    }
}
