using Microsoft.AspNetCore.WebUtilities; 
namespace RTCodingExercise.Monolithic.Utilities;

public static class QueryParameters
{
    public static Dictionary<string, string> ToDictionary(HttpRequest request)
    {
        var query = request.QueryString;
        var parsedQuery = QueryHelpers.ParseQuery(query.ToString());
        return parsedQuery.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
    }

    public static void AddOrReplace(this Dictionary<string, string> dictionary, string key, string value)
    {
        if (!dictionary.TryAdd(key, value))
        {
            dictionary[key] = value;
        }
    }
}