using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class QueryServer
    {
        private static async Task<T> WithTimeout<T>(Task<T> mainTask, int timeoutMilliseconds)
        {
            Task timeoutTask = Task.Delay(timeoutMilliseconds);
            Task result = await Task.WhenAny(mainTask, timeoutTask);
            if (result.Equals(mainTask))
            {
                return mainTask.Result;
            }
            throw new TimeoutException();
        }

        private static async Task SendQueryPacket(UdpClient client, int sendTimeout)
        {
            List<byte> request = new List<byte>(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x54 });
            request.AddRange(Encoding.UTF8.GetBytes("Source Engine Query"));
            request.Add(0);
            int sendResult = await WithTimeout(
                client.SendAsync(request.ToArray(), request.Count), sendTimeout);
            if (request.Count != sendResult)
            {
                Console.WriteLine(
                    "Send returned unexpected result: {0} != {1}", sendResult, request.Count);
            }
        }

        public static async Task<List<byte>> Query(string host, int port, int sendTimeout, int receiveTimeout)
        {
            try
            {
                // Send Query to DayZ server: https://developer.valvesoftware.com/wiki/Server_queries#A2S_INFO
                using (UdpClient client = new UdpClient(host, port))
                {
                    await SendQueryPacket(client, sendTimeout);
                    UdpReceiveResult response = await WithTimeout(client.ReceiveAsync(), receiveTimeout);
                    return new List<byte>(response.Buffer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error querying server: {0}", e);
                return null;
            }
        }
    }
}
