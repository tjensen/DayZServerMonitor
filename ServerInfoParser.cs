using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZServerMonitor
{
    [Serializable]
    internal class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }

    internal class ServerInfoParser
    {
        private readonly List<byte> buffer;

        internal ServerInfoParser(byte[] buffer)
        {
            this.buffer = new List<byte>(buffer);
        }

        internal List<byte> GetBytes(int count)
        {
            if (buffer.Count < count)
            {
                throw new ParseException(string.Format("Insufficent bytes remaining: {0} < {1}", buffer.Count, count));
            }
            List<byte> result = buffer.GetRange(0, count);
            buffer.RemoveRange(0, count);
            return result;
        }

        internal byte GetByte()
        {
            return GetBytes(1)[0];
        }

        internal int GetShort()
        {
            List<byte> bytes = GetBytes(2);
            return bytes[0] + (bytes[1] << 8);
        }

        internal string GetString()
        {
            int pos = 0;
            while (pos < buffer.Count)
            {
                if (buffer[pos] == 0)
                {
                    break;
                }
                pos++;
            }
            string result = Encoding.ASCII.GetString(GetBytes(pos).ToArray());
            if (buffer.Count > 0 && buffer[0] == 0)
            {
                _ = GetByte(); // Ignore string terminator
            }
            return result;
        }
    }
}
