using System.Collections.Generic;
using System.Linq;

namespace Nucleotic.Common.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static string ToJson(this Dictionary<string, string> dictionary)
        {
            var kvs = dictionary.Select(kvp => $"\"{kvp.Key}\":\"{string.Concat(",", kvp.Value)}\"");
            return string.Concat("{", string.Join(",", kvs), "}");
        }

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this string json)
        {
            var keyValueArray = json.Replace("{", string.Empty).Replace("}", string.Empty).Replace("\"", string.Empty).Split(',');
            return keyValueArray.ToDictionary(item => item.Split(':')[0], item => item.Split(':')[1]);
        }
    }
}
