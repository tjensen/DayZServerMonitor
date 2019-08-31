using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DayZServerMonitorCore
{
    public class ServerSelectionList
    {
        public class SavedServers
        {
            public List<SavedServerSource> Servers = new List<SavedServerSource>();
        }

        private static readonly int SAVED_SERVER_INDEX = 2;

        private ComboBox comboBox;

        public ServerSelectionList(ComboBox comboBox, ILogger logger)
        {
            this.comboBox = comboBox;
            Insert(0, new LatestServerSource("Stable", ProfileParser.GetDayZFolder(), ProfileParser.GetProfileFilename(), logger));
            Insert(1, new LatestServerSource("Experimental", ProfileParser.GetExperimentalDayZFolder(), ProfileParser.GetProfileFilename(), logger));
            comboBox.SelectedIndex = 0;
        }

        public int Count => comboBox.Items.Count;

        public ServerSelectionItem this[int index]
        {
            get {
                if (index >= 0 && index < Count)
                {
                    return (ServerSelectionItem)comboBox.Items[index];
                }
                return null;
            }
        }

        public void Reset()
        {
            while (Count > SAVED_SERVER_INDEX)
            {
                comboBox.Items.RemoveAt(SAVED_SERVER_INDEX);
            }
        }

        public int SaveServer(Server server)
        {
            return SaveServer(server, null);
        }

        public int SaveServer(Server server, string name)
        {
            int savedIndex = comboBox.SelectedIndex;
            comboBox.SelectedIndex = -1;
            int removedIndex = Remove(server);
            Insert(SAVED_SERVER_INDEX, new SavedServerSource(server, name));
            if (removedIndex == savedIndex)
            {
                comboBox.SelectedIndex = SAVED_SERVER_INDEX;
            }
            else
            {
                comboBox.SelectedIndex = savedIndex;
            }
            return SAVED_SERVER_INDEX;
        }

        public void SaveToFilename(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SavedServers));
            using (TextWriter writer = new StreamWriter(filename))
            {
                SavedServers servers = new SavedServers();
                for (int index = SAVED_SERVER_INDEX; index < Count; index++)
                {
                    ServerSelectionItem item = this[index];
                    SavedServerSource source = (SavedServerSource)item.GetSource();
                    servers.Servers.Add(source);
                }
                serializer.Serialize(writer, servers);
            }
        }

        public void LoadFromFilename(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SavedServers));
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                SavedServers servers = (SavedServers)serializer.Deserialize(fs);
                Reset();
                foreach (SavedServerSource source in servers.Servers)
                {
                    Insert(Count, source);
                }
            }
        }

        private void Insert(int index, IServerSource item)
        {
            comboBox.Items.Insert(index, new ServerSelectionItem(item));
        }

        private int Remove(Server server)
        {
            for (int index = 2; index < Count; index++)
            {
                if (((SavedServerSource)this[index].GetSource()).Address == server.Address)
                {
                    comboBox.Items.RemoveAt(index);
                    return index;
                }
            }
            return -1;
        }
    }
}
