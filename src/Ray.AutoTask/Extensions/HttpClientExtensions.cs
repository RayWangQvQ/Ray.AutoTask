using System.Collections.Generic;
using System.Net.Http.Headers;
using Ray.Infrastructure.Extensions;

namespace Ray.AutoTask.Extensions
{
    public static class HttpClientExtensions
    {
        public static void AddHeaders(this HttpRequestHeaders headers, Dictionary<string, string> preAdd, Dictionary<string, string> add = null, List<string> remove = null)
        {
            preAdd.Merge(add, remove);

            foreach (var item in preAdd)
            {
                headers.Add(item.Key, item.Value);
            }
        }
    }
}
