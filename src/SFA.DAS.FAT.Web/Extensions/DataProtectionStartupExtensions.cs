using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FAT.Domain.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.FAT.Web.Extensions
{
    public static class DataProtectionStartupExtensions
    {
        public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            
            var redisConfiguration = configuration.GetSection(nameof(FindApprenticeshipTrainingWeb))
                .Get<FindApprenticeshipTrainingWeb>();

            if (redisConfiguration != null)
            {
                var redisConnectionString = redisConfiguration.RedisConnectionString;
                var dataProtectionKeysDatabase = redisConfiguration.DataProtectionKeysDatabase;

                var redis = ConnectionMultiplexer
                    .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

                services.AddDataProtection()
                    .SetApplicationName("das-find-apprenticeship-training")
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
            }
        }
    }
}
