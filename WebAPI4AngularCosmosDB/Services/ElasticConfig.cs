using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Nest;

namespace WebAPI4AngularCosmosDB.Services
{
    public class ElasticConfig
    {
        public static string IndexName
        {
            get { return ConfigurationManager.AppSettings["indexName"]; }
        }

        public static string ElastisearchUrl
        {
            get { return ConfigurationManager.AppSettings["elastisearchUrl"]; }
        }

        public static IElasticClient GetClient()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex(IndexName);
            return new ElasticClient(settings);
        }
    }
}