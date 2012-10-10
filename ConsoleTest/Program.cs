using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveIRC.Connection;

namespace ReactiveIRC.ConsoleTest
{
    public class Program
    {
        public static void Main(String[] args)
        {
            IRCConnection connection = new IRCConnection(args[0], Convert.ToUInt16(args[1]));
            connection.RawMessages.Subscribe(s => Console.WriteLine(s));
            connection.Connect().Subscribe(_ => { }, e => Console.WriteLine(e.Message), () => Console.WriteLine("Connected!"));

            while(true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
