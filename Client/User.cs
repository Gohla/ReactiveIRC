using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Gohla.Shared;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Client
{
    public class User : IUser
    {
        private KeyedCollection<String, IChannel> _channels = new KeyedCollection<String, IChannel>();
        private BehaviorSubject<bool> _away = new BehaviorSubject<bool>(false);

        public IObservable<IMessage> Messages { get; private set; }
        public IObservable<IReceiveMessage> ReceivedMessages { get; private set; }
        public IObservable<ISendMessage> SentMessages { get; private set; }

        public IObservableCollection<IChannel> Channels { get { return _channels; } }

        public IObservable<bool> Away { get { return _away; } }

        public IClientConnection Connection { get; private set; }
        public IIdentity Identity { get; private set; }
        public MessageTargetType Type { get { return MessageTargetType.User; } }

        public IIdentity Key { get { return Identity; } }
        String IKeyedObject<String>.Key { get { return Identity.Name; } }

        public User(IClientConnection connection, IIdentity identity)
        {
            Connection = connection;
            Identity = identity;

            // TODO: set messages
        }

        public int CompareTo(IUser other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            int result = 0;
            result = this.Identity.CompareTo(other.Identity);
            return result;
        }

        public override bool Equals(object other)
        {
            if(ReferenceEquals(other, null))
                return false;

            return Equals(other as IUser);
        }

        public bool Equals(IUser other)
        {
            if(ReferenceEquals(other, null))
                return false;

            return
                EqualityComparer<IIdentity>.Default.Equals(this.Identity, other.Identity)
             && EqualityComparer<IClientConnection>.Default.Equals(this.Connection, other.Connection)
             ;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + EqualityComparer<IIdentity>.Default.GetHashCode(this.Identity);
                hash = hash * 23 + EqualityComparer<IClientConnection>.Default.GetHashCode(this.Connection);
                return hash;
            }
        }

        public override string ToString()
        {
            return this.Identity.Name;
        }
    }
}
