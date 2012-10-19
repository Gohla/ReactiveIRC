using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Gohla.Shared;
using ReactiveIRC.Interface;
using ReactiveIRC.Protocol;

namespace ReactiveIRC.Client
{
    public class IRCClientConnection : RawClientConnection, IClientConnection
    {
        private MessageSender _messageSender;

        public IUser Me { get { return null; } }
        public IObservableCollection<IChannel> Channels { get { return null; } }
        public IObservableCollection<IUser> Users { get { return null; } }
        public IObservable<IMessage> Messages { get { return null; } }
        public IObservable<IReceiveMessage> ReceivedMessages { get { return null; } }
        public IObservable<ISendMessage> SentMessages { get { return null; } }

        public IRCClientConnection(String address, ushort port)
            : base(address, port)
        {
            _messageSender = new MessageSender(this);
        }

        public IObservable<Unit> Send(params ISendMessage[] messages)
        {
            return Observable.Merge(messages.Select(m => WriteRaw(m.Raw)));
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

        public INetwork GetNetwork(string network)
        {
            throw new NotImplementedException();
        }

        public IChannel GetChannel(string channel)
        {
            throw new NotImplementedException();
        }

        public IUser GetUser(string nickname)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IClientConnection other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IClientConnection other)
        {
            throw new NotImplementedException();
        }
    }
}
