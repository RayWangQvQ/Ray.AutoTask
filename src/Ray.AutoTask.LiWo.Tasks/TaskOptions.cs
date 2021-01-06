using System;
using System.Collections.Generic;
using System.Text;

namespace Ray.AutoTask.LiWo.Tasks
{
    public class TaskOptions
    {
        public Dictionary<string, string> DefaultHeaders { get; set; }

        public List<TaskInfo> List { get; set; }

    }

    public class TaskInfo
    {
        public string Name { get; set; }

        public string Api { get; set; }

        public Headers Headers { get; set; }

        public Content Content { get; set; }
    }

    public class Headers
    {
        public Dictionary<string, string> Add { get; set; }

        public Dictionary<string, string> Remove { get; set; }
    }

    public class Content
    {
        public string Type { get; set; }

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
        public Dictionary<string, string> Dic { get; set; }
    }
}
