using System;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace ReactiveIRC.Connection
{
    public class Connection : IDisposable
    {
        private Socket _socket;

        public String Address { get; private set; }
        public ushort Port { get; private set; }

        public IObservable<String> RawMessages { get; private set; }

        public Connection(String address, ushort port)
        {
            if(String.IsNullOrEmpty(address))
                throw new ArgumentNullException("address");

            Address = address;
            Port = port;
        }

        public void Dispose()
        {
            if(_socket == null)
                return;

            _socket.Close();
            _socket = null;
        }

        public IObservable<Unit> Connect()
        {
            if(_socket != null)
                return Observable.Throw<Unit>(new InvalidOperationException("Already connected or connecting."));

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            return _socket
                .ConnectObservable(Address, Port)
                .Do(_ => { }, ConnectedError, Connected)
                ;
        }

        public IObservable<Unit> Disconnect()
        {
            if(_socket == null)
                return Observable.Throw<Unit>(new InvalidOperationException("No connection."));

            return _socket.
                DisconnectObservable(false)
                .Do(_ => { }, DisconnectedError, Disconnected)
                ;
        }

        private void Connected()
        {
            RawMessages = _socket.ReceiveUntilCompleted(SocketFlags.None)
                .SelectMany(x => Encoding.UTF8.GetString(x).ToCharArray())
                .Scan(String.Empty, (a, b) => (a.EndsWith("\r\n") ? "" : a) + b)
                .Where(x => x.EndsWith("\r\n"))
                .Select(buffered => String.Join("", buffered))
                .Select(a => a.Replace("\n", ""));
        }

        private void ConnectedError(Exception e)
        {
            _socket.Close();
            _socket = null;
        }

        private void Disconnected()
        {
            _socket.Close();
            _socket = null;
        }

        private void DisconnectedError(Exception e)
        {
            _socket.Close();
            _socket = null;
        }
    }
}
