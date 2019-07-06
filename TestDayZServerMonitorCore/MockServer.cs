using System;
using System.Net;
using System.Net.Sockets;

namespace TestDayZServerMonitorCore
{
    internal class MockServer : IDisposable
    {
        private readonly Socket serverSocket;

        public byte[] Request { get; private set; }
        public byte[] Response { get; set; }
        public bool RequestCompleted { get; set; }
        public int Port { get; private set; }

        public MockServer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            Port = ((IPEndPoint)serverSocket.LocalEndPoint).Port;

            AsyncCallback receiveCallback = new AsyncCallback(HandleReceiveFrom);

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[1400];
            serverSocket.BeginReceiveFrom(
                buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, receiveCallback,
                new Tuple<Socket, byte[]>(serverSocket, buffer));
        }

        private void HandleReceiveFrom(IAsyncResult ar)
        {
            Tuple<Socket, byte[]> payload = (Tuple<Socket, byte[]>)ar.AsyncState;
            Socket socket = payload.Item1;
            byte[] buffer = payload.Item2;
            try
            {
                EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                int bytesReceived = socket.EndReceiveFrom(ar, ref endPoint);
                Array.Resize(ref buffer, bytesReceived);
                Request = buffer;

                if (Response != null)
                {
                    AsyncCallback sendCallback = new AsyncCallback(HandleSendTo);

                    socket.BeginSendTo(
                        Response, 0, Response.Length, SocketFlags.None, endPoint,
                        sendCallback, socket);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void HandleSendTo(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            _ = socket.EndSendTo(ar);
            RequestCompleted = true;
            socket.Close();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    serverSocket.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
