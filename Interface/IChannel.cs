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
        /// Gets an observable stream of topics. Sends current value on subscribe.
        /// </summary>
        IObservable<String> Topic { get; }
    }
}
