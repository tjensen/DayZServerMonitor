using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DayZServerMonitorCore
{
    public class MessageParser
    {
        private readonly List<byte> buffer;

        public MessageParser(byte[] buffer)
        {
            this.buffer = new List<byte>(buffer);
        }

        public List<byte> GetBytes(int count)
        {
            if (buffer.Count < count)
            {
                throw new ParseException(string.Format("Insufficient bytes remaining: {0} < {1}", buffer.Count, count));
            }
            List<byte> result = buffer.GetRange(0, count);
            buffer.RemoveRange(0, count);
            return result;
        }

        public byte GetByte()
        {
            return GetBytes(1)[0];
        }

        public short GetShort()
        {
            return BitConverter.ToInt16(GetBytes(2).ToArray(), 0);
        }

        public long GetLongLong()
        {
            return BitConverter.ToInt64(GetBytes(8).ToArray(), 0);
        }

        public ushort GetPort()
        {
            return (ushort)IPAddress.NetworkToHostOrder(GetShort());
        }

        public IPAddress GetIPAddress()
        {
            return new IPAddress(GetBytes(4).ToArray());
        }

        public string GetString()
        {
            int index = buffer.FindIndex((b) => b == 0);
            if (index < 0)
            {
                return Encoding.UTF8.GetString(GetBytes(buffer.Count).ToArray());
            }

            string result = Encoding.UTF8.GetString(GetBytes(index).ToArray());
            _ = GetByte();
            return result;
        }
    }
}
