using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI4AngularCosmosDB.Models
{
    public class SearchResult<T>
    {
        public int Total { get; set; }

        public int Page { get; set; }

        public IEnumerable<T> Results { get; set; }

        public int ElapsedMilliseconds { get; set; }

        public Dictionary<string, long> AggregationsByTags { get; set; }
    }
}