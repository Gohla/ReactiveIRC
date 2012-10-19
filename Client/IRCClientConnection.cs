using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Gohla.Shared;
using ReactiveIRC.Interface;
using ReactiveIRC.Protocol;

namespace ReactiveIRC.Client
{
    public class IRCClientConnection : RawClientConnection, IClientConnection
    {
        private MessageSender _messageSender;
        private MessageReceiver _messageReceiver;
        private IUser _me;
        private KeyedCollection<String, IChannel> _channels = new KeyedCollection<String, IChannel>();
        private KeyedCollection<String, IUser> _users = new KeyedCollection<String, IUser>();
        private KeyedCollection<String, IChannel> _knownChannels = new KeyedCollection<String, IChannel>();
        private KeyedCollection<String, IUser> _knownUsers = new KeyedCollection<String, IUser>();
        private Subject<IMessage> _messages = new Subject<IMessage>();
        private Subject<IReceiveMessage> _receivedMessages = new Subject<IReceiveMessage>();
        private Subject<ISendMessage> _sentMessages = new Subject<ISendMessage>();

        public IUser Me { get { return _me; } }
        public IObservableCollection<IChannel> Channels { get { return _channels; } }
        public IObservableCollection<IUser> Users { get { return _users; } }
        public IObservableCollection<IChannel> KnownChannels { get { return _knownChannels; } }
        public IObservableCollection<IUser> KnownUsers { get { return _knownUsers; } }
        public IObservable<IMessage> Messages { get { return _messages; } }
        public IObservable<IReceiveMessage> ReceivedMessages { get { return _receivedMessages; } }
        public IObservable<ISendMessage> SentMessages { get { return _sentMessages; } }

        public IRCClientConnection(String address, ushort port)
            : base(address, port)
        {
            _messageSender = new MessageSender(this);
            _messageReceiver = new MessageReceiver(this);

            RawMessages
                .Select(r => _messageReceiver.Receive(r))
                .Subscribe(_receivedMessages)
                ;
            _receivedMessages.Subscribe(_messages);
            _sentMessages.Subscribe(_messages);
        }

        public IObservable<Unit> Send(params ISendMessage[] messages)
        {
            messages.Do(m => _sentMessages.OnNext(m));

            return Observable.Merge(
                messages
                    .Select(m => WriteRaw(m.Raw))
            );
        }

        public IObservable<Unit> Login(String nickname)
        {
            return Login(nickname, nickname, nickname);
        }

        public IObservable<Unit> Login(String nickname, String username)
        {
            return Login(nickname, username, username);
        }

        public IObservable<Unit> Login(String nickname, String username, String realname)
        {
            return Send
            (
                _messageSender.Nick(nickname)
              , _messageSender.User(username, 0, realname)
            ).Finally(() => HandleLoginSent(nickname));
        }

        public IObservable<Unit> Login(String nickname, String username, String realname, String password)
        {
            return Send
            (
                _messageSender.Pass(password)
              , _messageSender.Nick(nickname)
              , _messageSender.User(username, 0, realname)
            ).Finally(() => HandleLoginSent(nickname));
        }

        public INetwork GetNetwork(String network)
        {
            return new Network(this, network);
        }

        public IChannel GetChannel(String name)
        {
            if(_knownChannels.Contains(name))
                return _knownChannels[name];

            Channel channel = new Channel(this, name);
            _knownChannels.Add(channel);
            return channel;
        }

        public IUser GetUser(String nickname)
        {
            if(_knownUsers.Contains(nickname))
                return _knownUsers[nickname];

            User user = new User(this, nickname);
            _knownUsers.Add(user);
            return user;
        }

        private void HandleLoginSent(String nickname)
        {
            _me = GetUser(nickname);
            _users.Add(_me);
        }

        public int CompareTo(IClientConnection other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            int result = 0;
            result = this.Address.CompareTo(other.Address);
            if(result == 0)
                result = this.Port.CompareTo(other.Port);
            return result;
        }

        public override bool Equals(object other)
        {
            if(ReferenceEquals(other, null))
                return false;

            return Equals(other as IClientConnection);
        }

        public bool Equals(IClientConnection other)
        {
            if(ReferenceEquals(other, null))
                return false;

            return
                EqualityComparer<String>.Default.Equals(this.Address, other.Address)
             && EqualityComparer<ushort>.Default.Equals(this.Port, other.Port)
             ;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + EqualityComparer<String>.Default.GetHashCode(this.Address);
                hash = hash * 23 + EqualityComparer<ushort>.Default.GetHashCode(this.Port);
                return hash;
            }
        }

        public override string ToString()
        {
            return this.Address.ToString() + ":" + this.Port.ToString();
        }
    }
}
