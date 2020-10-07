using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI4AngularCosmosDB.Models
{
    public class PageResult<T>
    {
        public int Total { get; set; }

        public int Page { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}