using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.Model
{
    public class SearchInformation
    {
        [JsonProperty("searchTime")]
        public double SearchTime { get; set; }

        [JsonProperty("formattedSearchTime")]
        public string FormattedSearchTime { get; set; }

        [JsonProperty("totalResults")]
        public string TotalResults { get; set; }

        [JsonProperty("formattedTotalResults")]
        public string FormattedTotalResults { get; set; }
    }
}
