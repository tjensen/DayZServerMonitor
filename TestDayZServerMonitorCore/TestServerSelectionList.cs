﻿using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestServerSelectionList
    {
        private readonly ComboBox comboBox = new ComboBox();
        private ServerSelectionList list;
        private string filename;

        [TestInitialize]
        public void Initialize()
        {
            list = new ServerSelectionList(comboBox);
            filename = Path.GetTempFileName();
        }

        [TestCleanup]
        public void Cleanup()
        {
            comboBox.Dispose();

            try
            {
                File.Delete(filename);
            }
            catch (DirectoryNotFoundException error)
            {
                Console.WriteLine("Unable to delete {0}: {1}", filename, error);
            }
        }

        [TestMethod]
        public void ContainsOnlyLatestServerSourceEntriesForStableAndExperimentalByDefault()
        {
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("Most Recent (Stable)", list[0].DisplayName);
            Assert.IsInstanceOfType(list[0].GetSource(), typeof(LatestServerSource));
            Assert.AreEqual("Most Recent (Experimental)", list[1].DisplayName);
            Assert.IsInstanceOfType(list[1].GetSource(), typeof(LatestServerSource));
        }

        [TestMethod]
        public void SubscriptReturnsNullIfIndexIsOutOfRange()
        {
            Assert.IsNull(list[-1]);
            Assert.IsNull(list[2]);
        }

        [TestMethod]
        public void SaveServerAddsSavedServerToListAndReturnsItsIndex()
        {
            ServerSelectionItem item = list.SaveServer(new Server("1.2.3.4", 5678));

            Assert.AreEqual(list[2], item);

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("1.2.3.4:5678", list[2].DisplayName);
            Assert.IsInstanceOfType(list[2].GetSource(), typeof(SavedServerSource));
        }

        [TestMethod]
        public void SaveServerDoesNotChangeSelectedIndex()
        {
            comboBox.SelectedIndex = 1;

            list.SaveServer(new Server("1.2.3.4", 5678));

            Assert.AreEqual(1, comboBox.SelectedIndex);
        }

        [TestMethod]
        public void SaveServerAddsServerWithNameWhenSpecified()
        {
            ServerSelectionItem item = list.SaveServer(new Server("1.2.3.4", 5678), "SERVER NAME");

            Assert.AreEqual(list[2], item);

            Assert.AreEqual("SERVER NAME (1.2.3.4:5678)", list[2].DisplayName);
        }

        [TestMethod]
        public void SaveServerWithNameDoesNotChangeSelectedIndex()
        {
            comboBox.SelectedIndex = 1;

            list.SaveServer(new Server("1.2.3.4", 5678), "SERVER NAME");

            Assert.AreEqual(1, comboBox.SelectedIndex);
        }

        [TestMethod]
        public void SaveServerInsertsNewServerAsFirstSavedServer()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");

            Assert.AreEqual(4, list.Count);
            Assert.AreEqual("SAVED SECOND (5.6.7.8:4321)", list[2].DisplayName);
            Assert.AreEqual("SAVED FIRST (1.2.3.4:5678)", list[3].DisplayName);
        }

        [TestMethod]
        public void SaveProfileAddsLatestServerSourceForGivenFilenameAndDoesNotChangeSelectedIndex()
        {
            comboBox.SelectedIndex = 1;

            ServerSelectionItem item = list.SaveProfile(@"X:\path\to\some.DayZProfile");

            Assert.AreEqual(1, comboBox.SelectedIndex);

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(@"Most Recent (X:\path\to\some.DayZProfile)", list[2].DisplayName);
            Assert.IsInstanceOfType(list[2].GetSource(), typeof(LatestServerSource));

            Assert.AreEqual(list[2], item);
        }

        [TestMethod]
        public void SaveServerReplacesExistingSavedServerWithSameAddressButNewName()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");
            list.SaveProfile(@"X:\path\to\some.DayZProfile");
            comboBox.SelectedIndex = 3;

            list.SaveServer(new Server("1.2.3.4", 5678), "UPDATED NAME");

            Assert.AreEqual(4, comboBox.SelectedIndex);

            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("UPDATED NAME (1.2.3.4:5678)", list[2].DisplayName);
            Assert.AreEqual(@"Most Recent (X:\path\to\some.DayZProfile)", list[3].DisplayName);
            Assert.AreEqual("SAVED SECOND (5.6.7.8:4321)", list[4].DisplayName);
        }

        [TestMethod]
        public void SaveServerUpdatesSelectedIndexWhenReplacingSelectedItem()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");
            comboBox.SelectedIndex = 3;

            list.SaveServer(new Server("1.2.3.4", 5678), "UPDATED NAME");

            Assert.AreEqual(2, comboBox.SelectedIndex);
        }

        [TestMethod]
        public void SaveServerReplacesExistingSavedServerWithSameAddressButNoName()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");
            list.SaveServer(new Server("1.2.3.4", 5678));

            Assert.AreEqual(4, list.Count);
            Assert.AreEqual("1.2.3.4:5678", list[2].DisplayName);
            Assert.AreEqual("SAVED SECOND (5.6.7.8:4321)", list[3].DisplayName);
        }

        [TestMethod]
        public void SaveServerWithNameUpdatesSelectedIndexWhenReplacingSelectedServer()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");
            comboBox.SelectedIndex = 3;

            list.SaveServer(new Server("1.2.3.4", 5678));

            Assert.AreEqual(2, comboBox.SelectedIndex);
        }

        [TestMethod]
        public void SaveProfileMovesMatchingServerSourceIfOneAlreadyExistsWithTheSameFilename()
        {
            list.SaveProfile(@"X:\path\to\some.DayZProfile");
            list.SaveProfile(@"Z:\path\to\some\other.DayZProfile");
            comboBox.SelectedIndex = 2;

            ServerSelectionItem item = list.SaveProfile(@"X:\path\to\some.DayZProfile");

            Assert.AreEqual(3, comboBox.SelectedIndex);

            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(@"Most Recent (X:\path\to\some.DayZProfile)", list[2].DisplayName);
            Assert.IsInstanceOfType(list[2].GetSource(), typeof(LatestServerSource));
            Assert.AreEqual(@"Most Recent (Z:\path\to\some\other.DayZProfile)", list[3].DisplayName);
            Assert.IsInstanceOfType(list[3].GetSource(), typeof(LatestServerSource));

            Assert.AreEqual(list[2], item);
        }

        [TestMethod]
        public void SaveProfileUpdatesSelectedIndexWhenReplacingSelectedItem()
        {
            list.SaveProfile(@"X:\path\to\some.DayZProfile");
            list.SaveProfile(@"Z:\path\to\some\other.DayZProfile");
            comboBox.SelectedIndex = 3;

            list.SaveProfile(@"X:\path\to\some.DayZProfile");

            Assert.AreEqual(2, comboBox.SelectedIndex);
        }

        [TestMethod]
        public void PromoteMovesItemToTheTopOfTheListAndDoesNotChangeTheSelectedItem()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveProfile(@"X:\path\to\some.DayZProfile");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");
            comboBox.SelectedIndex = 2;

            ServerSelectionItem item = list.Promote(3);

            Assert.AreEqual(3, comboBox.SelectedIndex);

            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("Most Recent (Stable)", list[0].DisplayName);
            Assert.AreEqual("Most Recent (Experimental)", list[1].DisplayName);
            Assert.AreEqual(@"Most Recent (X:\path\to\some.DayZProfile)", list[2].DisplayName);
            Assert.AreEqual("SAVED SECOND (5.6.7.8:4321)", list[3].DisplayName);
            Assert.AreEqual("SAVED FIRST (1.2.3.4:5678)", list[4].DisplayName);

            Assert.AreEqual(list[2], item);
        }

        [TestMethod]
        public void PromoteReturnsNullAndDoesNothingIfIndexIsTooLow()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveProfile(@"X:\path\to\some.DayZProfile");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");

            Assert.IsNull(list.Promote(1));

            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("Most Recent (Stable)", list[0].DisplayName);
            Assert.AreEqual("Most Recent (Experimental)", list[1].DisplayName);
            Assert.AreEqual("SAVED SECOND (5.6.7.8:4321)", list[2].DisplayName);
            Assert.AreEqual(@"Most Recent (X:\path\to\some.DayZProfile)", list[3].DisplayName);
            Assert.AreEqual("SAVED FIRST (1.2.3.4:5678)", list[4].DisplayName);
        }

        [TestMethod]
        public void PromoteUpdatesSelectedIndexIfPromotedItemWasSelected()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveProfile(@"X:\path\to\some.DayZProfile");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");
            comboBox.SelectedIndex = 3;

            list.Promote(3);

            Assert.AreEqual(2, comboBox.SelectedIndex);
        }

        [TestMethod]
        public void ResetRemovesAllEntriesExceptForTheOnesForStableAndExperimental()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED FIRST");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED SECOND");

            list.Reset();

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("Most Recent (Stable)", list[0].DisplayName);
            Assert.AreEqual("Most Recent (Experimental)", list[1].DisplayName);
        }

        [TestMethod]
        public void ContentsCanBeSavedAndRestoredFromAFilename()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SERVER ONE");
            list.SaveProfile(@"X:\path\to\some.DayZProfile");
            list.SaveServer(new Server("5.6.7.8", 4321), "SERVER TWO");

            list.SaveToFilename(filename);

            list.Reset();

            list.LoadFromFilename(filename);

            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("Most Recent (Stable)", list[0].DisplayName);
            Assert.AreEqual("Most Recent (Experimental)", list[1].DisplayName);
            Assert.AreEqual("SERVER TWO (5.6.7.8:4321)", list[2].DisplayName);
            Assert.AreEqual(@"Most Recent (X:\path\to\some.DayZProfile)", list[3].DisplayName);
            Assert.AreEqual("SERVER ONE (1.2.3.4:5678)", list[4].DisplayName);
        }

        [TestMethod]
        public void RestoringFromFilenameReplacesContents()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED ONE");
            list.SaveProfile(@"X:\path\to\some.DayZProfile");
            list.SaveServer(new Server("5.6.7.8", 4321), "SAVED TWO");

            list.SaveToFilename(filename);

            list.Reset();
            list.SaveServer(new Server("5.5.5.5", 6666), "REPLACED");
            list.SaveProfile(@"Z:\replaced\saved\profile");

            list.LoadFromFilename(filename);

            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("Most Recent (Stable)", list[0].DisplayName);
            Assert.AreEqual("Most Recent (Experimental)", list[1].DisplayName);
            Assert.AreEqual("SAVED TWO (5.6.7.8:4321)", list[2].DisplayName);
            Assert.AreEqual(@"Most Recent (X:\path\to\some.DayZProfile)", list[3].DisplayName);
            Assert.AreEqual("SAVED ONE (1.2.3.4:5678)", list[4].DisplayName);
        }

        [TestMethod]
        public void IndexRemovableReturnsFalseWhenIndexIsLessThanTwo()
        {
            Assert.IsFalse(list.IndexRemovable(1));
        }

        [TestMethod]
        public void IndexRemovableReturnsTrueWhenIndexIsGreaterThanOrEqualToTwo()
        {
            Assert.IsTrue(list.IndexRemovable(2));
        }

        [TestMethod]
        public void RemoveIndexRemovesItemFromList()
        {
            list.SaveServer(new Server("1.2.3.4", 5678), "SAVED");

            list.RemoveIndex(2);

            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void RemoveIndexDoesNothingIfIndexIsNotRemovable()
        {
            list.RemoveIndex(0);

            Assert.AreEqual(2, list.Count);
        }
    }
}
