using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestQueryServer
    {
        private byte[] serverRequest;
        private byte[] serverResponse;
        private bool serverCompleted;
        private Socket serverSocket;
        private int serverPort;

        [TestInitialize]
        public void Initialize()
        {
            serverRequest = null;
            serverResponse = null;
            serverCompleted = false;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            serverPort = ((IPEndPoint)serverSocket.LocalEndPoint).Port;

            AsyncCallback receiveCallback = new AsyncCallback(HandleReceiveFrom);

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[1400];
            Array.Fill<byte>(buffer, 0);
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
                serverRequest = buffer;

                if (serverResponse != null)
                {
                    AsyncCallback sendCallback = new AsyncCallback(HandleSendTo);

                    socket.BeginSendTo(
                        serverResponse, 0, serverResponse.Length, SocketFlags.None, endPoint,
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
            serverCompleted = true;
            socket.Close();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("Cleaning up");
            if (serverSocket != null)
            {
                serverSocket.Dispose();
            }
            Console.WriteLine("Done Cleaning up");
        }

        [TestMethod]
        public async Task QueryReturnsServerInfoWhenSuccessful()
        {
            serverResponse = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF,  // Response header
                0x49, // A2S_INFO header
                0x11, // Protocol
                0x53, 0x65, 0x72, 0x76, 0x65, 0x72, 0x20, 0x4E, 0x61, 0x6D, 0x65, 0x00, // Name
                0x6D, 0x61, 0x70, 0x00, // Map
                0x66, 0x6F, 0x6C, 0x64, 0x65, 0x72, 0x00, // Folder
                0x67, 0x61, 0x6D, 0x65, 0x00, // Game
                0x12, 0x34, // ID
                0x2A, // Players
                0x3C, // Max Players
                0x00, // Bots
                0x65, // Server type
                0x77, // Environment
                0x00, // Visibility
                0x01 // VAC
            };

            byte[] result = await QueryServer.Query("127.0.0.1", serverPort, 100, 100);

            Assert.IsTrue(serverCompleted);
            CollectionAssert.AreEqual(
                new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45,
                    0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
                },
                serverRequest);
            CollectionAssert.AreEqual(serverResponse, result);
        }

        [TestMethod]
        public async Task QueryReturnsNullOnServerTimeout()
        {
            Assert.IsNull(await QueryServer.Query("127.0.0.1", serverPort, 100, 100));
        }
    }
}