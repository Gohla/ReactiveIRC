using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Gohla.Shared;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Client
{
    public class User : IUser
    {
        private KeyedCollection<String, IChannel> _channels = new KeyedCollection<String, IChannel>();
        private BehaviorSubject<bool> _away = new BehaviorSubject<bool>(false);
        private BehaviorSubject<String> _name;

        public IObservable<IMessage> Messages { get; private set; }
        public IObservable<IReceiveMessage> ReceivedMessages { get; private set; }
        public IObservable<ISendMessage> SentMessages { get; private set; }

        public IObservableCollection<IChannel> Channels { get { return _channels; } }

        public IObservable<bool> Away { get { return _away; } }

        public IClientConnection Connection { get; private set; }
        public MessageTargetType Type { get { return MessageTargetType.User; } }
        public IObservable<String> Name { get { return _name; } }

        public String Key { get { return Name.FirstAsync().Wait(); } }

        public User(IClientConnection connection, String name)
        {
            Connection = connection;
            _name = new BehaviorSubject<String>(name);

            Messages = connection.Messages
                .Where(m => m.Receivers.Contains(this))
                ;
            ReceivedMessages = connection.ReceivedMessages
                .Where(m => m.Receivers.Contains(this))
                ;
            SentMessages = connection.SentMessages
                .Where(m => m.Receivers.Contains(this))
                ;
        }

        public int CompareTo(IUser other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            int result = 0;
            result = this.Name.FirstAsync().Wait().CompareTo(other.Name.FirstAsync().Wait());
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
                EqualityComparer<String>.Default.Equals(this.Name.FirstAsync().Wait(), other.Name.FirstAsync().Wait())
             && EqualityComparer<IClientConnection>.Default.Equals(this.Connection, other.Connection)
             ;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Name.FirstAsync().Wait());
                hash = hash * 23 + EqualityComparer<IClientConnection>.Default.GetHashCode(this.Connection);
                return hash;
            }
        }

        public override string ToString()
        {
            return this.Name.FirstAsync().Wait();
        }
    }
}
