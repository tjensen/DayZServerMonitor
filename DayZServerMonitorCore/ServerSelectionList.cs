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
            public List<SavedSource> Servers = new List<SavedSource>();
        }

        private static readonly int SAVED_SERVER_INDEX = 2;

        private ComboBox comboBox;
        private ILogger logger;

        public ServerSelectionList(ComboBox comboBox, ILogger logger)
        {
            this.comboBox = comboBox;
            this.logger = logger;
            Insert(0, new LatestServerSource("Stable", ProfileParser.GetDayZFolder(), ProfileParser.GetProfileFilename()));
            Insert(1, new LatestServerSource("Experimental", ProfileParser.GetExperimentalDayZFolder(), ProfileParser.GetProfileFilename()));
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

        public ServerSelectionItem SaveServer(Server server)
        {
            return SaveServer(server, null);
        }

        public ServerSelectionItem SaveServer(Server server, string name)
        {
            int savedIndex = comboBox.SelectedIndex;
            object savedItem = comboBox.SelectedItem;
            comboBox.SelectedIndex = -1;
            int removedIndex = RemoveServer(server);
            ServerSelectionItem item = Insert(SAVED_SERVER_INDEX, new SavedServerSource(server, name));
            if (removedIndex == savedIndex)
            {
                comboBox.SelectedIndex = SAVED_SERVER_INDEX;
            }
            else
            {
                comboBox.SelectedItem = savedItem;
            }
            return item;
        }

        public ServerSelectionItem SaveProfile(string filename)
        {
            int savedIndex = comboBox.SelectedIndex;
            object savedItem = comboBox.SelectedItem;
            comboBox.SelectedIndex = -1;
            int removedIndex = RemoveProfile(filename);
            ServerSelectionItem item = Insert(SAVED_SERVER_INDEX, new LatestServerSource(filename, Path.GetDirectoryName(filename), Path.GetFileName(filename)));
            if (removedIndex == savedIndex)
            {
                comboBox.SelectedIndex = SAVED_SERVER_INDEX;
            }
            else
            {
                comboBox.SelectedItem = savedItem;
            }
            return item;
        }

        public ServerSelectionItem Promote(int index)
        {
            if (index <= SAVED_SERVER_INDEX)
            {
                return null;
            }

            int savedIndex = comboBox.SelectedIndex;
            object savedItem = comboBox.SelectedItem;
            comboBox.SelectedIndex = -1;
            ServerSelectionItem item = this[index];
            comboBox.Items.RemoveAt(index);
            comboBox.Items.Insert(SAVED_SERVER_INDEX, item);
            if (savedIndex == index)
            {
                comboBox.SelectedIndex = SAVED_SERVER_INDEX;
            }
            else
            {
                comboBox.SelectedItem = savedItem;
            }
            return item;
        }

        public void SaveToFilename(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SavedServers));
            using (TextWriter writer = new StreamWriter(filename))
            {
                SavedServers servers = new SavedServers();
                for (int index = SAVED_SERVER_INDEX; index < Count; index++)
                {
                    IServerSource item = this[index].GetSource();
                    servers.Servers.Add(item.Save());
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
                foreach (SavedSource source in servers.Servers)
                {
                    if (source.Filename == null)
                    {
                        Insert(Count, new SavedServerSource(source));
                    }
                    else
                    {
                        Insert(Count, new LatestServerSource(source));
                    }
                }
            }
        }

        private ServerSelectionItem Insert(int index, IServerSource item)
        {
            ServerSelectionItem result = new ServerSelectionItem(item);
            comboBox.Items.Insert(index, result);
            return result;
        }

        private int RemoveServer(Server server)
        {
            for (int index = SAVED_SERVER_INDEX; index < Count; index++)
            {
                if (this[index].GetSource() is SavedServerSource source)
                {
                    if (source.Address == server.Address)
                    {
                        comboBox.Items.RemoveAt(index);
                        return index;
                    }
                }
            }
            return -1;
        }

        private int RemoveProfile(string filename)
        {
            for (int index = SAVED_SERVER_INDEX; index < Count; index++)
            {
                if (this[index].GetSource() is LatestServerSource source)
                {
                    if (source.Modifier == filename)
                    {
                        comboBox.Items.RemoveAt(index);
                        return index;
                    }
                }
            }
            return -1;
        }
    }
}
