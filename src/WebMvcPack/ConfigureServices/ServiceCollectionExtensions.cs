using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddRedisSession(this IServiceCollection services, RedisSessionOptions redisSessionOptions) {
        services
            .AddDataProtection()
            .SetApplicationName(redisSessionOptions.ApplicationName)
            .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisSessionOptions.ConnectionString), redisSessionOptions.RedisKey);

        var sessionOptions = redisSessionOptions.SessionOptions;

        _ = services.AddSession(options => {
            options.IdleTimeout = sessionOptions.IdleTimeout;
            options.Cookie.HttpOnly = sessionOptions.Cookie.HttpOnly;
            options.Cookie.IsEssential = sessionOptions.Cookie.IsEssential;
        });

        return services;
    }
}
