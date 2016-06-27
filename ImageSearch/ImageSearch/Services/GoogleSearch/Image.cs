using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.Model.GoogleSearch
{
    public class Image
    {

        [JsonProperty("contextLink")]
        public string ContextLink { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("byteSize")]
        public int ByteSize { get; set; }

        [JsonProperty("thumbnailLink")]
        public string ThumbnailLink { get; set; }

        [JsonProperty("thumbnailHeight")]
        public int ThumbnailHeight { get; set; }

        [JsonProperty("thumbnailWidth")]
        public int ThumbnailWidth { get; set; }
    }
}
