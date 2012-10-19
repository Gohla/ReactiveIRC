using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Gohla.Shared;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Client
{
    public class Channel : IChannel
    {
        private KeyedCollection<String, IChannelUser> _users = new KeyedCollection<String, IChannelUser>();
        private ObservableProperty<String> _topic = new ObservableProperty<String>(String.Empty);

        public IObservable<IMessage> Messages { get; private set; }
        public IObservable<IReceiveMessage> ReceivedMessages { get; private set; }
        public IObservable<ISendMessage> SentMessages { get; private set; }

        public IObservableCollection<IChannelUser> Users { get { return _users; } }
        public ObservableProperty<String> Topic { get { return _topic; } }

        public IClientConnection Connection { get; private set; }
        public MessageTargetType Type { get { return MessageTargetType.Channel; } }
        public ObservableProperty<String> Name { get; private set; }

        public String Key { get { return Name; } }

        public Channel(IClientConnection connection, String name)
        {
            Connection = connection;
            Name = new ObservableProperty<String>(name);

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

        internal void AddUser(IUser user)
        {
            _users.Add(new ChannelUser(Connection, this, user));
        }

        internal void RemoveUser(String user)
        {
            _users.Remove(user);
        }

        internal void ChangeNickname(String oldname, String nickname)
        {
            IChannelUser channelUser = _users[oldname];
            _users.ChangeItemKey(channelUser, nickname);
        }

        public int CompareTo(IChannel other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            int result = 0;
            result = this.Name.Value.CompareTo(other.Name);
            if(result == 0)
                result = this.Connection.CompareTo(other.Connection);
            return result;
        }

        public override bool Equals(object other)
        {
            if(ReferenceEquals(other, null))
                return false;

            return Equals(other as IChannel);
        }

        public bool Equals(IChannel other)
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
