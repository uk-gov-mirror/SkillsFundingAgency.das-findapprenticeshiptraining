using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace SFA.DAS.FAT.Web.AcceptanceTests.Infrastructure
{
    public static class ConfigBuilder
    {
        public static IConfigurationRoot GenerateConfiguration()
        {
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new[]
                {
                    new KeyValuePair<string, string>("ConfigurationStorageConnectionString", "UseDevelopmentStorage=true;"),
                    new KeyValuePair<string, string>("ConfigNames", "SFA.DAS.FindApprenticeshipTraining.Web"),
                    new KeyValuePair<string, string>("Environment", "DEV"),
                    new KeyValuePair<string, string>("Version", "1.0"),

                    new KeyValuePair<string, string>("FindApprenticeshipTrainingApi:Key", "test"),
                    new KeyValuePair<string, string>("FindApprenticeshipTrainingApi:BaseUrl", "http://localhost:5003/"),
                    new KeyValuePair<string, string>("FindApprenticeshipTrainingApi:PingUrl", "http://localhost:5003/"),
                    new KeyValuePair<string, string>("FindApprenticeshipTrainingWeb:RedisConnectionString", ""),
                    new KeyValuePair<string, string>("FindApprenticeshipTrainingWeb:DataProtectionKeysDatabase", ""),
                    new KeyValuePair<string, string>("FindApprenticeshipTrainingWeb:ZendeskSectionId", "213452345"),
                    new KeyValuePair<string, string>("FindApprenticeshipTrainingWeb:ZendeskSnippetKey", "e0730bdd"),
                    new KeyValuePair<string, string>("FindApprenticeshipTrainingWeb:ZendeskCoBrowsingSnippetKey", "Qec2OgXsUy8")
                }
            };

            var provider = new MemoryConfigurationProvider(configSource);

            return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
        }
    }
}
