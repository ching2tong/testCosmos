namespace WebAPI4AngularCosmosDB.Controllers
{
    using WebAPI4AngularCosmosDB.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/Hero")]
    public class HeroController : ApiController
    {

        [HttpGet]
        public async Task<IEnumerable<Hero>> GetAsync()
        {

            IEnumerable<Hero> value = await CosmosDBRepository<Hero>.GetItemsAsync();
            return value;
        }

        [HttpPost]
        public async Task<Hero> CreateAsync([FromBody] Hero hero)
        {
            Console.WriteLine("we are trying to input the hero");
            if (ModelState.IsValid)
            {
                await CosmosDBRepository<Hero>.CreateItemAsync(hero);
                return hero;
            }
            return null;
        }
        public async Task<string> Delete(string uid)
        {
            try
            {
                Hero item = await CosmosDBRepository<Hero>.GetItemAsync(uid);
                if (item == null)
                {
                    return "Failed";
                }
                await CosmosDBRepository<Hero>.DeleteItemAsync(item.Id);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<Hero> Put(string uid, [FromBody] Hero hero)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Hero item = await CosmosDBRepository<Hero>.GetItemAsync(uid);
                    if (item == null)
                    {
                        return null;
                    }
                    hero.Id = item.Id;
                    await CosmosDBRepository<Hero>.UpdateItemAsync(item.Id, hero);
                    return hero;
                }
                return null; ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}