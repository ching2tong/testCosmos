namespace WebAPI4AngularCosmosDB
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Linq;
    public static class CosmosDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string ContainerId = ConfigurationManager.AppSettings["container"];
        private static Database db;
        private static Container container;
        //private static DocumentClient client;
        private static CosmosClient client;
        /*
        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync
                                    (UriFactory.CreateDocumentUri(DatabaseId, , id));
                /////////////////////
                
                // Read item from container
                ItemResponse<T> todoItemResponse = await client
                            .GetContainer(DatabaseId, ContainerId)
                            .ReadItemAsync<T>("ItemId", new PartitionKey("partitionKeyValue"));
                
                /////////////////////
                return (T)(dynamic)document;

            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<IEnumerable<T>> GetItemsAsync()
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<T> GetSingleItemAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();
            List<T> results = new List<T>();
            results.AddRange(await query.ExecuteNextAsync<T>());
            return results.SingleOrDefault();
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            return await client.CreateDocumentAsync
               (UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public static async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync
               (UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public static async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync
               (UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }*/

        public static async void Initialize()
        {
            client = new CosmosClient(ConfigurationManager.AppSettings["endpoint"],
                     ConfigurationManager.AppSettings["authKey"]);
            db = await client.CreateDatabaseAsync(DatabaseId);
            ContainerProperties containerProperties = new ContainerProperties()
            {
                Id = ContainerId,
                PartitionKeyPath = "/pk",
                IndexingPolicy = new IndexingPolicy()
                {
                    Automatic = false,
                    IndexingMode = IndexingMode.Lazy,
                }
            };
        container = await client.GetDatabase(DatabaseId).CreateContainerIfNotExistsAsync(containerProperties);
            //container = await db.CreateContainerAsync(ContainerId);
            //client.CreateDatabaseIfNotExistsAsync(DatabaseId);
            //client.CreateCollectionIfNotExistsAsync(CollectionId);
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            //try
            //{
                await client.CreateDatabaseAsync(DatabaseId);
            //}
            /*catch (Exception e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(DatabaseId);
                }
                else
                {
                    throw;
                }
            }*/
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            //try
            //{
            //    await client.ReadDocumentCollectionAsync
            //      (UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            /*}
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }*/
        }
    }
}