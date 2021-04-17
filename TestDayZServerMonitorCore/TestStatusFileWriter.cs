using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestStatusFileWriter
    {
        private MockLogger logger = new MockLogger();
        private readonly string[] filenames = new string[4];

        [TestInitialize]
        public void Initialize()
        {
            for (int i = 0; i < filenames.Length; i++)
            {
                filenames[i] = Path.GetTempFileName();
                File.Delete(filenames[i]);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            foreach (string filename in filenames)
            {
                File.Delete(filename);
            }
        }

        [TestMethod]
        public void WriteStatusWritesServerStatusToFileUsingContentFormat()
        {
            StatusFileSetting[] statusFiles = {
                new StatusFileSetting {
                    Pathname = filenames[0],
                    Content = "CONTENT"
                }
            };

            StatusFileWriter writer = new StatusFileWriter(statusFiles, logger);
            writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);

            Assert.AreEqual("CONTENT", File.ReadAllText(filenames[0]));

            Assert.IsTrue(logger.NothingLogged);
        }

        [TestMethod]
        public void WriteStatusInterpolatesSupportedParametersIntoContentFormat()
        {
            StatusFileSetting[] statusFiles = {
                new StatusFileSetting {
                    Pathname = filenames[0],
                    Content = "foo %N bar %A baz %P buz %M"
                }
            };

            StatusFileWriter writer = new StatusFileWriter(statusFiles, logger);
            writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);

            Assert.AreEqual(
                "foo SERVER-NAME bar SERVER-ADDRESS baz 42 buz 69",
                File.ReadAllText(filenames[0]));
        }

        [TestMethod]
        public void WriteStatusIgnoresUnsupportedParameterSpecifiers()
        {
            StatusFileSetting[] statusFiles = {
                new StatusFileSetting {
                    Pathname = filenames[0],
                    Content = "%N %Z %Y %X %% %A"
                }
            };

            StatusFileWriter writer = new StatusFileWriter(statusFiles, logger);
            writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);

            Assert.AreEqual(
                "SERVER-NAME %Z %Y %X %% SERVER-ADDRESS",
                File.ReadAllText(filenames[0]));

            Assert.IsTrue(logger.NothingLogged);
        }

        [TestMethod]
        public void WriteStatusTreatsTrailingPercentAsLiteral()
        {
            StatusFileSetting[] statusFiles = {
                new StatusFileSetting {
                    Pathname = filenames[0],
                    Content = "49%"
                }
            };

            StatusFileWriter writer = new StatusFileWriter(statusFiles, logger);
            writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);

            Assert.AreEqual("49%", File.ReadAllText(filenames[0]));

            Assert.IsTrue(logger.NothingLogged);
        }

        [TestMethod]
        public void WriteStatusTreatsFormatSpecifiersCaseSensitively()
        {
            StatusFileSetting[] statusFiles = {
                new StatusFileSetting {
                    Pathname = filenames[0],
                    Content = "%n %a %p %m"
                }
            };

            StatusFileWriter writer = new StatusFileWriter(statusFiles, logger);
            writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);

            Assert.AreEqual("%n %a %p %m", File.ReadAllText(filenames[0]));
        }

        [TestMethod]
        public void WriteStatusWritesServerStatusToAllEnabledStatusFiles()
        {
            StatusFileSetting[] statusFiles = {
                new StatusFileSetting {
                    Pathname = filenames[0],
                    Content = "%N"
                },
                new StatusFileSetting {
                    Pathname = filenames[1],
                    Content = "%A"
                },
                new StatusFileSetting {
                    Pathname = filenames[2],
                    Content = "%P"
                },
                new StatusFileSetting {
                    Pathname = filenames[3],
                    Content = "%M"
                }
            };

            StatusFileWriter writer = new StatusFileWriter(statusFiles, logger);
            writer.WriteStatus("ServerAddress", "ServerName", 12, 21);

            Assert.AreEqual("ServerName", File.ReadAllText(filenames[0]));
            Assert.AreEqual("ServerAddress", File.ReadAllText(filenames[1]));
            Assert.AreEqual("12", File.ReadAllText(filenames[2]));
            Assert.AreEqual("21", File.ReadAllText(filenames[3]));
        }

        [TestMethod]
        public void WriteStatusDoesNothingWhenStatusFilesIsEmpty()
        {
            StatusFileWriter writer = new StatusFileWriter(new StatusFileSetting[] {}, logger);

            writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);

            Assert.IsFalse(File.Exists(filenames[0]));
            Assert.IsFalse(File.Exists(filenames[1]));
            Assert.IsFalse(File.Exists(filenames[2]));
            Assert.IsFalse(File.Exists(filenames[3]));

            Assert.IsTrue(logger.NothingLogged);
        }

        [TestMethod]
        public void WriteStatusDoesNothingWhenStatusFileIsDisabled()
        {
            StatusFileWriter writer = new StatusFileWriter(new StatusFileSetting[] {
                new StatusFileSetting {
                    Pathname = null,
                    Content = "CONTENT"
                }
            }, logger);

            writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);

            Assert.IsFalse(File.Exists(filenames[0]));
            Assert.IsFalse(File.Exists(filenames[1]));
            Assert.IsFalse(File.Exists(filenames[2]));
            Assert.IsFalse(File.Exists(filenames[3]));

            Assert.IsTrue(logger.NothingLogged);
        }

        [TestMethod]
        public void WriteStatusLogsErrorWhenWritingStatusFileThrowsException()
        {
            StatusFileSetting[] statusFiles = {
                new StatusFileSetting {
                    Pathname = filenames[0],
                    Content = "CONTENT"
                }
            };
            StatusFileWriter writer = new StatusFileWriter(statusFiles, logger);

            using (FileStream fs = File.OpenWrite(filenames[0]))
            {
                writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);
            }

            Assert.AreEqual(1, logger.ErrorTexts.Count);
            Assert.AreEqual($"Failed to write status to {filenames[0]}", logger.ErrorTexts[0]);
            Assert.AreEqual(1, logger.ErrorExceptions.Count);
            Assert.IsNotNull(logger.ErrorExceptions[0]);
        }

        [TestMethod]
        public void WriteStatusContinuesWritingOtherStatusFilesAfterError()
        {
            StatusFileSetting[] statusFiles = {
                new StatusFileSetting {
                    Pathname = filenames[0],
                    Content = "SUCCESS 1"
                },
                new StatusFileSetting {
                    Pathname = filenames[1],
                    Content = "DON'T CARE"
                },
                new StatusFileSetting {
                    Pathname = filenames[2],
                    Content = "SUCCESS 2"
                }
            };
            StatusFileWriter writer = new StatusFileWriter(statusFiles, logger);

            using (FileStream fs = File.OpenWrite(filenames[1]))
            {
                writer.WriteStatus("SERVER-ADDRESS", "SERVER-NAME", 42, 69);
            }

            Assert.AreEqual("SUCCESS 1", File.ReadAllText(filenames[0]));
            Assert.AreEqual("SUCCESS 2", File.ReadAllText(filenames[2]));
        }
    }
}
