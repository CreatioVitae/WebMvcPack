namespace Microsoft.AspNetCore.Builder;

public record RedisSessionOptions(string ApplicationName, string ConnectionString, string RedisKey, SessionOptions SessionOptions) {
}
