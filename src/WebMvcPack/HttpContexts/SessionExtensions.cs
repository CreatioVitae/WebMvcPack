using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Http;

public static class SessionExtensions {
    public static void Set<T>(this ISession session, string key, T target) =>
        session.Set(key, JsonSerializer.SerializeToUtf8Bytes(
            target,
            new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }
        ));

    public static T? Get<T>(this ISession session, string key) =>
        session.Get(key) is var bytes && bytes is null
            ? default
            : JsonSerializer.Deserialize<T>(
                new ReadOnlySpan<byte>(bytes),
                new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }
            );
}
