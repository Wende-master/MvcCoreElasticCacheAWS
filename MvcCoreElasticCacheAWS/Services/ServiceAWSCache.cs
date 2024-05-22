using Microsoft.Extensions.Caching.Distributed;
using MvcCoreElasticCacheAWS.Helpers;
using MvcCoreElasticCacheAWS.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElasticCacheAWS.Services
{
    public class ServiceAWSCache
    {
        //private IDatabase cache;
        private IDistributedCache cache;
        public ServiceAWSCache(IDistributedCache cache)
        {
            //this.cache = HelperCacheRedis.Connection.GetDatabase();
            this.cache = cache;
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync()
        {
            //KEYS ÚNICAS DE CADA USER
            string jsonCoches =
                await this.cache.GetStringAsync("cochesfavoritos");

            if (jsonCoches == null)
            {
                return null;
            }
            else
            {
                List<Coche> cars =
                    JsonConvert.DeserializeObject<List<Coche>>(jsonCoches);
                return cars;
            }
        }

        public async Task AddCocheFavoritoAsync(Coche car)
        {
            List<Coche> coches = await this.GetCochesFavoritosAsync();
            //SI NO EXISTEN TODAVÍA COCHES, CREAMOS LA COLECCIÓN
            if (coches == null)
            {
                coches = new List<Coche>();
            }
            //AÑADIMOS EL NUEVO COCHE A LA LISTA
            coches.Add(car);
            //SERIALIZAMOS A JSON LA COLECCIÓN
            string jsonCoches =
                JsonConvert.SerializeObject(coches);
            DistributedCacheEntryOptions options =
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                };
            //AÑADIREMOS 30 minutos de duración para los datos
            await this.cache.SetStringAsync("cochesfavoritos", jsonCoches, options);
        }

        public async Task DeleteCocheFavoritoAsync(int idcoche)
        {
            List<Coche> cars = await this.GetCochesFavoritosAsync();
            //SI NO EXISTEN TODAVÍA COCHES, CREAMOS LA COLECCIÓN
            if (cars != null)
            {
                Coche cocheEliminar =
                    cars.FirstOrDefault(x => x.IdCoche == idcoche);
                cars.Remove(cocheEliminar);

                if (cars.Count == 0)
                {
                    await this.cache.RemoveAsync("cochesfavoritos");
                }
                else
                {
                    string jsonCoches =
                        JsonConvert.SerializeObject(cars);
                    //ACTUALIZAR EL CACHE
                    DistributedCacheEntryOptions options =
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                };
                    //AÑADIREMOS 30 minutos de duración para los datos
                    await this.cache.SetStringAsync("cochesfavoritos", jsonCoches, options);
                }

            }

        }

    }
}
