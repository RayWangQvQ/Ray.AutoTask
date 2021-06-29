using System.Collections.Generic;
using System.Net.Http;
using Ray.AutoTask.Enums;

namespace Ray.AutoTask.Options
{
    public class TaskOptions
    {
        public Dictionary<string, string> DefaultHeaders { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> DefaultCookies { get; set; } = new Dictionary<string, string>();

        public List<TaskInfo> List { get; set; } = new List<TaskInfo>();

    }

    public class TaskInfo
    {
        public string Name { get; set; }

        public bool Open { get; set; }

        public string MapToClient { get; set; }

        public string Api { get; set; }

        public Headers Headers { get; set; }

        public Cookies Cookies { get; set; }

        public string Method { get; set; }
        public HttpMethod HttpMethod => new HttpMethod(Method);

        public Content Content { get; set; }
    }

    public class Headers
    {
        public Dictionary<string, string> Add { get; set; } = new Dictionary<string, string>();

        public Remove Remove { get; set; }
    }

    public class Cookies
    {
        public Dictionary<string, string> Add { get; set; } = new Dictionary<string, string>();

        public Remove Remove { get; set; }
    }

    public class Remove
    {
        public bool RemoveAll { get; set; }

        public List<string> List { get; set; } = new List<string>();
    }

    public class Content
    {
        public MyContentType Type { get; set; }

        public ContentString StringContent { get; set; }

        public ContentFormUrlEncoded FormUrlEncodedContent { get; set; }
    }

    public class ContentString
    {
        public string Content { get; set; }

        public string Encoding { get; set; }

        public string MediaType { get; set; }
    }

    public class ContentFormUrlEncoded
    {
        public Dictionary<string, string> Dic { get; set; } = new Dictionary<string, string>();
    }
}
