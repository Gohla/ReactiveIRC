using System;
using System.Reactive;
using ReactiveIRC.Protocol;

namespace ReactiveIRC.Connection
{
    public class IRCConnection : RawConnection
    {
        public IRCConnection(String address, ushort port)
            : base(address, port)
        {

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
            return WriteRaw
            (
                MessageSender.Nick(nickname)
              , MessageSender.User(username, 0, realname)
            );
        }

        public IObservable<Unit> Login(String nickname, String username, String realname, String password)
        {
            return WriteRaw
            (
                MessageSender.Pass(password)
              , MessageSender.Nick(nickname)
              , MessageSender.User(username, 0, realname)
            );
        }
    }
}
