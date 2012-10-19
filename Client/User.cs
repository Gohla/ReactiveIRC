using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Gohla.Shared;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Client
{
    public class User : IUser
    {
        private KeyedCollection<String, IChannel> _channels = new KeyedCollection<String, IChannel>();
        private ObservableProperty<bool> _away = new ObservableProperty<bool>(false);
        private ObservableProperty<String> _name;

        public IObservable<IMessage> Messages { get; private set; }
        public IObservable<IReceiveMessage> ReceivedMessages { get; private set; }
        public IObservable<ISendMessage> SentMessages { get; private set; }

        public IObservableCollection<IChannel> Channels { get { return _channels; } }

        public ObservableProperty<bool> Away { get { return _away; } }

        public IClientConnection Connection { get; private set; }
        public MessageTargetType Type { get { return MessageTargetType.User; } }
        public ObservableProperty<String> Name { get { return _name; } }

        public String Key { get { return Name; } }

        public User(IClientConnection connection, String name)
        {
            Connection = connection;
            _name = new ObservableProperty<String>(name);

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

        internal void AddChannel(IChannel channel)
        {
            _channels.Add(channel);
        }

        internal void RemoveChannel(String channel)
        {
            _channels.Remove(channel);
        }

        internal void SetNickname(String nickname)
        {
            _name.Value = nickname;
        }

        public int CompareTo(IUser other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            int result = 0;
            result = this.Name.Value.CompareTo(other.Name);
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
                EqualityComparer<String>.Default.Equals(this.Name, other.Name)
             && EqualityComparer<IClientConnection>.Default.Equals(this.Connection, other.Connection)
             ;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Name);
                hash = hash * 23 + EqualityComparer<IClientConnection>.Default.GetHashCode(this.Connection);
                return hash;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
