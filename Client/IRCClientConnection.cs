using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Gohla.Shared;
using ReactiveIRC.Interface;
using ReactiveIRC.Protocol;
using NLog;

namespace ReactiveIRC.Client
{
    public class IRCClientConnection : RawClientConnection, IClientConnection
    {
        protected static readonly Logger _logger = NLog.LogManager.GetLogger("IRCClientConnection");
        protected static readonly String _initialNickname = "***initial***";

        private MessageSender _messageSender;
        private MessageReceiver _messageReceiver;
        private User _me;
        private Network _network;
        private KeyedCollection<String, IChannel> _channels = new KeyedCollection<String, IChannel>();
        private KeyedCollection<String, IUser> _users = new KeyedCollection<String, IUser>();
        private Subject<IMessage> _messages = new Subject<IMessage>();
        private Subject<IReceiveMessage> _receivedMessages = new Subject<IReceiveMessage>();
        private Subject<ISendMessage> _sentMessages = new Subject<ISendMessage>();

        public IUser Me { get { return _me; } }
        public INetwork Network { get { return _network; } }
        public IObservableCollection<IChannel> Channels { get { return _channels; } }
        public IObservableCollection<IUser> Users { get { return _users; } }
        public IObservable<IMessage> Messages { get { return _messages; } }
        public IObservable<IReceiveMessage> ReceivedMessages { get { return _receivedMessages; } }
        public IObservable<ISendMessage> SentMessages { get { return _sentMessages; } }

        public IRCClientConnection(String address, ushort port)
            : base(address, port)
        {
            _me = GetUser(_initialNickname) as User;

            _messageSender = new MessageSender(this);
            _messageReceiver = new MessageReceiver(this);

            RawMessages
                .Select(r => _messageReceiver.Receive(r))
                .Subscribe(_receivedMessages)
                ;
            _receivedMessages.Subscribe(_messages);
            _sentMessages.Subscribe(_messages);

            _receivedMessages
                .Where(m => m.Type == ReceiveType.Ping)
                .Subscribe(HandlePing)
                ;
            _receivedMessages
                .Where(m => m.Type == ReceiveType.Join)
                .Subscribe(HandleJoin)
                ;
            _receivedMessages
                .Where(m => m.Type == ReceiveType.Part)
                .Subscribe(HandlePart)
                ;
            _receivedMessages
                .Where(m => m.Type == ReceiveType.Kick)
                .Subscribe(HandleKick)
                ;
            _receivedMessages
                .Where(m => m.Type == ReceiveType.Quit)
                .Subscribe(HandleQuit)
                ;
            _receivedMessages
                .Where(m => m.Type == ReceiveType.TopicChange)
                .Subscribe(HandleTopicChange)
                ;
            _receivedMessages
                .Where(m => m.Type == ReceiveType.ChannelModeChange)
                .Subscribe(HandleChannelModeChange)
                ;
            _receivedMessages
                .Where(m => m.Type == ReceiveType.UserModeChange)
                .Subscribe(HandleUserModeChange)
                ;

            _receivedMessages
                .Where(m => m.ReplyType == ReplyType.Welcome)
                .Subscribe(HandleWelcome)
                ;
            _receivedMessages
                .Where(m => m.ReplyType == ReplyType.ChannelModeIs)
                .Subscribe(HandleChannelModeIs)
                ;
            _receivedMessages
                .Where(m => m.ReplyType == ReplyType.Topic)
                .Subscribe(HandleTopic)
                ;
            _receivedMessages
                .Where(m => m.ReplyType == ReplyType.NamesReply)
                .Subscribe(HandleNamesReply)
                ;
            _receivedMessages
                .Where(m => m.ReplyType == ReplyType.Away)
                .Subscribe(HandleAway)
                ;
            _receivedMessages
                .Where(m => m.ReplyType == ReplyType.UnAway)
                .Subscribe(HandleUnAway)
                ;
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
            );
        }

        public IObservable<Unit> Login(String nickname, String username, String realname, String password)
        {
            return Send
            (
                _messageSender.Pass(password)
              , _messageSender.Nick(nickname)
              , _messageSender.User(username, 0, realname)
            );
        }

        public INetwork GetNetwork(String networkName)
        {
            return new Network(this, networkName);
        }

        public IChannel GetChannel(String channelName)
        {
            if(_channels.Contains(channelName))
                return _channels[channelName];

            Channel channel = new Channel(this, channelName);
            _channels.Add(channel);
            return channel;
        }

        public IUser GetUser(String nickname)
        {
            if(_users.Contains(nickname))
                return _users[nickname];

            User user = new User(this, nickname);
            _users.Add(user);
            return user;
        }

        private void AddUserToChannel(String nickname, String channelName)
        {
            User user = GetChannel(nickname) as User;
            Channel channel = GetUser(channelName) as Channel;
            AddUserToChannel(user, channel);
        }

        private void AddUserToChannel(User user, Channel channel)
        {
            channel.AddUser(user);
            user.AddChannel(channel);
        }

        private void RemoveUserFromChannel(String nickname, String channelName)
        {
            User user = GetChannel(nickname) as User;
            Channel channel = GetUser(channelName) as Channel;
            RemoveUserFromChannel(user, channel);
        }

        private void RemoveUserFromChannel(User user, Channel channel)
        {
            channel.RemoveUser(user.Name);
            user.RemoveChannel(channel.Name);
        }

        private void ChangeNickname(User user, String nickname)
        {
            if(_users.Contains(nickname))
            {
                _logger.Error("Changing nickname of " + user.Name + " into " + nickname +
                    ", but a user with that nickname already exists. Some observable subscriptions may be lost.");

                if(user.Equals(_me))
                    _me = _users[nickname] as User;
                user.Channels.Cast<Channel>().Do(c => c.ChangeName(user.Name, nickname));
                _users.Remove(user);
                return;
            }

            _users.ChangeItemKey(user, nickname);
            user.Channels.Cast<Channel>().Do(c => c.ChangeName(user.Name, nickname));
            user.Name.Value = nickname;
        }

        private void HandlePing(IReceiveMessage message)
        {
            Send(_messageSender.Pong(message.Contents)).Subscribe();
        }

        private void HandleJoin(IReceiveMessage message)
        {
            if(message.Receivers.Count != 1)
            {
                _logger.Error("Join message with no or more than one receiver.");
                return;
            }

            User user = message.Sender as User;
            Channel channel = message.Receivers.First() as Channel;
            AddUserToChannel(user, channel);
        }

        private void HandlePart(IReceiveMessage message)
        {
            if(message.Receivers.Count != 1)
            {
                _logger.Error("Part message with no or more than one receiver.");
                return;
            }

            User user = message.Sender as User;
            Channel channel = message.Receivers.First() as Channel;
            RemoveUserFromChannel(user, channel);
        }

        private void HandleKick(IReceiveMessage message)
        {
            HandlePart(message);
        }

        private void HandleQuit(IReceiveMessage message)
        {
            User user = message.Sender as User;
            user.Channels.Cast<Channel>().Do(c => RemoveUserFromChannel(user, c));
            _users.Remove(user);
        }

        private void HandleTopicChange(IReceiveMessage message)
        {
            if(message.Receivers.Count != 1)
            {
                _logger.Error("Topic change message with no or more than one receiver.");
                return;
            }

            Channel channel = message.Receivers.First() as Channel;
            channel.Topic.Value = message.Contents;
        }

        private void HandleChannelModeChange(IReceiveMessage message)
        {
            if(message.Receivers.Count != 1)
            {
                _logger.Error("Channel mode change message with no or more than one receiver.");
                return;
            }

            Channel channel = message.Receivers.First() as Channel;

            String[] split = message.Contents.Split(new[] { ' ' }, 2);
            if(split.Length == 1)
            {
                channel.Modes.ParseAndApply(split[0]);
            }
            else
            {
                ModeChange[] changes = Mode.Parse(split[0]);
                String[] nicknames = split[1].Split(' ');

                if(changes.Length != nicknames.Length)
                {
                    _logger.Error("Length of changes does not match length of nicknames.");
                    return;
                }

                for(int i = 0; i < changes.Length; ++i)
                {
                    ChannelUser channelUser = channel.GetUser(nicknames[i]) as ChannelUser;
                    channelUser.Modes.Apply(changes[i]);
                }
            }
        }

        private void HandleUserModeChange(IReceiveMessage message)
        {
            if(message.Receivers.Count != 1)
            {
                _logger.Error("User mode change message with no or more than one receiver.");
                return;
            }

            User user = message.Receivers.First() as User;
            user.Modes.ParseAndApply(message.Contents);
        }

        private void HandleWelcome(IReceiveMessage message)
        {
            String nickname = message.Contents.Split(new[] { ' ' }, 2)[0];
            ChangeNickname(_me, nickname);
            _network = message.Sender as Network;
        }

        private void HandleChannelModeIs(IReceiveMessage message)
        {

        }

        private void HandleTopic(IReceiveMessage message)
        {

        }

        private void HandleNamesReply(IReceiveMessage message)
        {

        }

        private void HandleAway(IReceiveMessage message)
        {

        }

        private void HandleUnAway(IReceiveMessage message)
        {

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
