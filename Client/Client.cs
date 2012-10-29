using System;
using System.Threading;
using ReactiveIRC.Interface;
using ReactiveIRC.Protocol;

namespace ReactiveIRC.Client
{
    public class Client : IClient
    {
        public IClientConnection CreateClientConnection(String address, ushort port, SynchronizationContext context)
        {
            return new IRCClientConnection(address, port, context, this);
        }

        public IReceiveMessage CreateReceiveMessage(IClientConnection connection, String contents, DateTime date,
            IMessageTarget sender, IMessageTarget receiver, ReceiveType type, ReplyType replyType)
        {
            return new ReceiveMessage(connection, contents, date, sender, receiver, type, replyType);
        }

        public ISendMessage CreateSendMessage(IClientConnection connection, String contents, SendType type, 
            params IMessageTarget[] receivers)
        {
            return new SendMessage(connection, contents, type, receivers);
        }
    }
}
