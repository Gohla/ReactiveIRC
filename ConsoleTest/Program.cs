using System;
using System.Threading;
using ReactiveIRC.Client;

namespace ReactiveIRC.ConsoleTest
{
    public class Program
    {
        public static void Main(String[] args)
        {
            IRCClientConnection connection = new IRCClientConnection(args[0], Convert.ToUInt16(args[1]));
            connection.RawMessages.Subscribe(s => Console.WriteLine(s));
            connection.Connect().Subscribe(
                _ => Console.WriteLine("Connected!"),
                e => Console.WriteLine(e.Message),
                () => connection.Login(args[2], args[3], args[4], args[5]).Subscribe(_ => { }, () => Console.WriteLine("Logged in!"))
            );

            while(true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
