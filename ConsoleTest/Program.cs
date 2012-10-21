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
            bool run = true;
            IRCClientConnection connection = new IRCClientConnection(args[0], Convert.ToUInt16(args[1]), 
                new ImplicitSynchronisationContext());
            connection.ReceivedMessages.Subscribe(PrintMessage);
            connection.Connect().Subscribe(
                _ => {},
                e => _logger.ErrorException("Error connecting.", e),
                () => connection.Login(args[2], args[3], args[4], args[5]).Subscribe()
            );

            Console.CancelKeyPress += delegate
            {
                connection.Dispose();
                run = false;
            };

            while(run)
            {
                Thread.Sleep(50);
            }
        }

        private static void PrintMessage(IReceiveMessage message)
        {
            _logger.Info(message.Type.ToString() + "," + message.ReplyType.ToString() + " - " + 
                message.Sender.ToString() + " :: " + message.Receiver + " :: " + message.Contents);
        }
    }

    public class ImplicitSynchronisationContext : SynchronizationContext
    {
        private readonly SynchronizationContext m_SyncContext;

        public ImplicitSynchronisationContext()
        {
            m_SyncContext = SynchronizationContext.Current;
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            if(m_SyncContext != null)
            {
                m_SyncContext.Post(d, state);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(_ => d(state));
            }
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            if(m_SyncContext != null)
            {
                m_SyncContext.Send(d, state);
            }
            else
            {
                d(state);
            }
        }
    }

}
