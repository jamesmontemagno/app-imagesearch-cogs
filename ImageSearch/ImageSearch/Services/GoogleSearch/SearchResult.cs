using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.Model.GoogleSearch
{
   
    public class SearchResult
    {
        [JsonProperty("searchInformation")]
        public SearchInformation SearchInformation { get; set; }

        [JsonProperty("items")]
        public List<SearchItem> Items { get; set; }
    }
}
