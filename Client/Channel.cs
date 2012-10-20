﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Gohla.Shared;
using NLog;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Client
{
    public class Channel : IChannel
    {
        protected static readonly Logger _logger = NLog.LogManager.GetLogger("Channel");

        private KeyedCollection<String, IChannelUser> _users = new KeyedCollection<String, IChannelUser>();
        private ObservableProperty<String> _topic = new ObservableProperty<String>(String.Empty);

        public IObservable<IMessage> Messages { get; private set; }
        public IObservable<IReceiveMessage> ReceivedMessages { get; private set; }
        public IObservable<ISendMessage> SentMessages { get; private set; }

        public IObservableCollection<IChannelUser> Users { get { return _users; } }
        public Mode Modes { get; private set; }
        public ObservableProperty<String> Topic { get { return _topic; } }

        public IClientConnection Connection { get; private set; }
        public MessageTargetType Type { get { return MessageTargetType.Channel; } }
        public ObservableProperty<String> Name { get; private set; }

        public String Key { get { return Name; } }

        public Channel(IClientConnection connection, String name)
        {
            Connection = connection;
            Name = new ObservableProperty<String>(name);
            Modes = new Mode();

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

        public IChannelUser GetUser(String nickname)
        {
            if(_users.Contains(nickname))
                return _users[nickname];

            IUser user = Connection.GetUser(nickname);
            return AddUser(user);
        }

        internal IChannelUser AddUser(IUser user)
        {
            if(_users.Contains(user.Name))
            {
                _logger.Error("Trying to add user " + user.Name + " to channel " + Name + 
                    ", but user is already in this channel.");
                return _users[user.Name];
            }

            ChannelUser channelUser = new ChannelUser(Connection, this, user);
            _users.Add(channelUser);
            return channelUser;
        }

        internal bool RemoveUser(String nickname)
        {
            if(!_users.Contains(nickname))
            {
                _logger.Error("Trying to remove user " + nickname + " from channel " + Name +
                    ", but user does not exist in this channel.");
                return false;
            }

            return _users.Remove(nickname);
        }

        internal void ChangeName(String oldNickname, String newNickname)
        {
            if(_users.Contains(newNickname))
            {
                _logger.Error("Changing nickname " + oldNickname + " into " + newNickname +
                    ", but a channel user with that nickname already exists. Some observable subscriptions may be lost.");

                _users.Remove(oldNickname);
                return;
            }

            IChannelUser channelUser = _users[oldNickname];
            _users.ChangeItemKey(channelUser, newNickname);
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
