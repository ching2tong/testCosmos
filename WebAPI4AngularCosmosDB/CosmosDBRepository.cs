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

    /* 
     * Note: We have commented out code here from things we tried previously or a mixture of code from the DocumentDB
     * tutorial. 
     */
    public static class CosmosDBRepository<T> where T : class
    {
        //This is where we are creating the variables we need to use in order to access Cosmos DB
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string ContainerId = ConfigurationManager.AppSettings["container"];
        private static Database db;
        private static Container container;
        private static CosmosClient client;

        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                ItemResponse <T> temp = await container.ReadItemAsync<T>(id, new PartitionKey("1"));

                //***The code below this does not work and is a mixture of DocumentDB code and test code***
                //Document document = await client.ReadDocumentAsync
                //                    (UriFactory.CreateDocumentUri(DatabaseId, , id));
                /////////////////////
                
                // Read item from container
                /*ItemResponse<T> todoItemResponse = await client
                            .GetContainer(DatabaseId, ContainerId)
                            .ReadItemAsync<T>("ItemId", new PartitionKey("partitionKeyValue"));
                */
                /////////////////////
                return (T)(dynamic)temp;

            }
            catch (CosmosException e)
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
            /*IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .AsDocumentQuery();
                */
            /*
        List<T> results = new List<T>();
        while (query.HasMoreResults)
        {
            results.AddRange(await query.ExecuteNextAsync<T>());
        }
        */
            Console.WriteLine("get items async");
            List<T> allItems = new List<T>();
            using (FeedIterator<T> resultSet = container.GetItemQueryIterator<T>(
                queryDefinition: null,
                requestOptions: new QueryRequestOptions()
                {
                    PartitionKey = new PartitionKey("1")
                }))
            {
                while (resultSet.HasMoreResults)
                {
                    FeedResponse<T> response = await resultSet.ReadNextAsync();
                    T item = response.First();

                    allItems.AddRange(response);
                }
            }
            return allItems;
        }

        /*
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
        }*/
        /*
        public static async Task<T> GetSingleItemAsync(Expression<Func<T, bool>> predicate)
        {
            /*IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();
            
    
            
            List<T> results = new List<T>();
            results.AddRange(await query.ExecuteNextAsync<T>());
            return results.SingleOrDefault();
        }*/

        public static async Task<T> CreateItemAsync(T item)
        {
            return await container.CreateItemAsync(item, new PartitionKey("1"));
            /*
            return await client.CreateDocumentAsync
               (UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);*/
        }

        public static async Task<T> UpdateItemAsync(string id, T item)
        {
            /*return await client.ReplaceDocumentAsync
               (UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);*/
            return await container.ReplaceItemAsync(
             partitionKey: new PartitionKey("1"),
             id: id,
             item: item);
        }

        public static async Task DeleteItemAsync(string id)
        {
            ItemResponse<T> response = await container.DeleteItemAsync<T>(
                partitionKey: new PartitionKey("1"),
                id: id);
        }

        public static async void Initialize()
        {
            client = new CosmosClient(ConfigurationManager.AppSettings["endpoint"],
                    ConfigurationManager.AppSettings["authKey"]);
            db = await client.CreateDatabaseIfNotExistsAsync(DatabaseId);
            ContainerProperties containerProperties = new ContainerProperties()
            {
                Id = ContainerId,
                // PartionKeyPath see here: https://docs.microsoft.com/en-us/azure/cosmos-db/partitioning-overview#choose-partitionkey
                PartitionKeyPath = "/pk",
                IndexingPolicy = new IndexingPolicy()
                {
                    Automatic = false,
                    IndexingMode = IndexingMode.Lazy,
                }
            };
            container = await client.GetDatabase(DatabaseId).CreateContainerIfNotExistsAsync(containerProperties, throughput: 1000);
            //container = await db.CreateContainerAsync(ContainerId);
            //client.CreateDatabaseIfNotExistsAsync(DatabaseId);
            //client.CreateCollectionIfNotExistsAsync(CollectionId);
            
        }
    }
}