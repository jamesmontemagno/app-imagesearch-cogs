using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.Model
{
    public class SearchItem
    {

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("htmlTitle")]
        public string HtmlTitle { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("displayLink")]
        public string DisplayLink { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("htmlSnippet")]
        public string HtmlSnippet { get; set; }

        [JsonProperty("mime")]
        public string Mime { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("fileFormat")]
        public string FileFormat { get; set; }
    }
}
