using System.Text.Json;

namespace Microsoft.AspNetCore.Http {
    public static class SessionExtensions {
        public static void Set<T>(this ISession session, string key, T target) =>
            session.Set(key, JsonSerializer.SerializeToUtf8Bytes(target, new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, IgnoreNullValues = true }));

        public static T? Get<T>(this ISession session, string key) =>
            session.Get(key) is var bytes && bytes is null
                ? default
                : JsonSerializer.Deserialize<T>(bytes);
    }
}
