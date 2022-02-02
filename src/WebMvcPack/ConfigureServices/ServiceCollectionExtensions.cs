using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddRedisSession(this IServiceCollection services, RedisSessionOptions redisSessionOptions) {
        services
            .AddDataProtection()
            .SetApplicationName(redisSessionOptions.ApplicationName)
            .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisSessionOptions.ConnectionString), redisSessionOptions.RedisKey);

        services.AddSession(options => {
            options = redisSessionOptions.SessionOptions;
        });

        return services;
    }
}
