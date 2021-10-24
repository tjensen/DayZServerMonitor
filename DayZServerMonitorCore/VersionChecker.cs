using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class VersionChecker
    {
        private static readonly string CHECK_URL = "https://tjensen.github.io/DayZServerMonitor/version.txt";

        private readonly HttpClient client;
        private readonly ILogger logger;

        public VersionChecker(HttpClient client, ILogger logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task<Version> Check()
        {
            try
            {
                var response = await client.GetAsync(CHECK_URL);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (Version.TryParse(content, out Version version))
                    {
                        logger.Debug($"Got version {version} from {CHECK_URL}");

                        return version;
                    }
                    else
                    {
                        logger.Debug(
                            $"Unable to parse version returned by {CHECK_URL}: {version}");
                    }
                }
                else
                {
                    logger.Debug($"GET {CHECK_URL} returned status {response.StatusCode}");
                }
            }
            catch (Exception error)
            {
                logger.Debug($"Error making GET {CHECK_URL}: {error}");
            }

            return null;
        }
    }
}
