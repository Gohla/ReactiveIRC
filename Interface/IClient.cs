using System;
using System.Threading;

namespace ReactiveIRC.Interface
{
    public interface IClient
    {
        IClientConnection CreateClientConnection(String address, ushort port, SynchronizationContext context);
        IReceiveMessage CreateReceiveMessage(IClientConnection connection, String contents, DateTime date, 
            IMessageTarget sender, IMessageTarget receiver, ReceiveType type, ReplyType replyType);
        ISendMessage CreateSendMessage(IClientConnection connection, String contents, SendType type,
            params IMessageTarget[] receivers);
    }
}
