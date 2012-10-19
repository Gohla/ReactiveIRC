using System;
using System.Threading;
using NLog;
using ReactiveIRC.Client;
using ReactiveIRC.Interface;

namespace ReactiveIRC.ConsoleTest
{
    public class Program
    {
        protected static readonly Logger _logger = NLog.LogManager.GetLogger("Program");

        public static void Main(String[] args)
        {
            IRCClientConnection connection = new IRCClientConnection(args[0], Convert.ToUInt16(args[1]));
            connection.ReceivedMessages.Subscribe(PrintMessage);
            connection.Connect().Subscribe(
                _ => {},
                e => _logger.ErrorException("Error connecting.", e),
                () => connection.Login(args[2], args[3], args[4], args[5]).Subscribe()
            );

            while(true)
            {
                Thread.Sleep(100);
            }
        }

        private static void PrintMessage(IReceiveMessage message)
        {
            _logger.Info(message.Type.ToString() + "," + message.ReplyType.ToString() + " - " + 
                message.Sender.ToString() + " :: " + message.Receivers.ToString(", ") + " :: " + message.Contents);
        }
    }
}
