﻿using System;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace ReactiveIRC.Connection
{
    public class IRCConnection : IDisposable
    {
        private TcpClient _socket = new TcpClient();
        private Subject<String> _rawMessages = new Subject<String>();
        private IDisposable _rawMessagesSubscription;

        public String Address { get; private set; }
        public ushort Port { get; private set; }
        public IObservable<String> RawMessages { get { return _rawMessages; } }

        public IRCConnection(String address, ushort port)
        {
            if(String.IsNullOrEmpty(address))
                throw new ArgumentNullException("address");

            Address = address;
            Port = port;
        }

        public void Dispose()
        {
            Disconnected();
        }

        public IObservable<Unit> Connect()
        {
            if(_socket.Connected)
                return Observable.Throw<Unit>(new InvalidOperationException("Already connected or connecting."));

            return _socket.Client
                .ConnectObservable(Address, Port)
                .Do(_ => Connected(), ConnectedError)
                ;
        }

        public IObservable<Unit> Disconnect()
        {
            if(!_socket.Connected)
                return Observable.Throw<Unit>(new InvalidOperationException("No connection."));

            return _socket.Client
                .DisconnectObservable(true)
                .Do(_ => Disconnected(), DisconnectedError)
                ;
        }

        private void Write(String message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message + Environment.NewLine);
            _socket.Client
                .SendObservable(data, 0, data.Length, SocketFlags.None)
                .Subscribe()
                ;
        }

        private void Connected()
        {
            _rawMessagesSubscription = _socket.Client.ReceiveUntilCompleted(SocketFlags.None)
                .SelectMany(x => Encoding.UTF8.GetString(x).ToCharArray())
                .Scan(String.Empty, (a, b) => (a.EndsWith("\r\n") ? "" : a) + b)
                .Where(x => x.EndsWith("\r\n"))
                .Select(b => String.Join("", b).Replace("\n", ""))
                .Subscribe(_rawMessages);
                ;

            Write("USER Gohla swivvet.com  swivvet.com :Gohla");
            Write("NICK Gohla");
        }

        private void ConnectedError(Exception e)
        {
            _socket.Close();
        }

        private void Disconnected()
        {
            _socket.Close();
            _rawMessagesSubscription.Dispose();
            _rawMessagesSubscription = null;
        }

        private void DisconnectedError(Exception e)
        {
            Disconnected();
        }
    }
}
