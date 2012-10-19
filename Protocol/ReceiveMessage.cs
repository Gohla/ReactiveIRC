using System;
using ReactiveIRC.Interface;

namespace ReactiveIRC.Protocol
{
    public class ReceiveMessage : Message, IReceiveMessage
    {
        public String Contents { get; private set; }
        public IMessageTarget Sender { get; private set; }
        public ReceiveMessageType Type { get; private set; }

        public ReceiveMessage(IClientConnection connection, String contents, IMessageTarget sender, 
            ReceiveMessageType type, params IMessageTarget[] receivers) :
            base(connection, receivers)
        {
            Contents = contents;
            Sender = sender;
            Type = type;
        }
    }
}
