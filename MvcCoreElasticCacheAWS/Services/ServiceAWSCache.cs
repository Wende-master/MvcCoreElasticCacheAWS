using MvcCoreElasticCacheAWS.Helpers;
using MvcCoreElasticCacheAWS.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElasticCacheAWS.Services
{
    public class ServiceAWSCache
    {
        private IDatabase cache;
        public ServiceAWSCache()
        {
            this.cache = HelperCacheRedis.Connection.GetDatabase();
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync()
        {
            //KEYS ÚNICAS DE CADA USER
            string jsonCoches =
                await this.cache.StringGetAsync("cochesfavoritos");

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
            else
            {
                //AÑADIMOS EL NUEVO COCHE A LA LISTA
                coches.Add(car);
                //SERIALIZAMOS A JSON LA COLECCIÓN
                string jsonCoches =
                    JsonConvert.SerializeObject(coches);
                //AÑADIREMOS 30 minutos de duración para los datos
                await this.cache.StringSetAsync("cochesfavoritos", jsonCoches, TimeSpan.FromMinutes(30));
            }
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
                    await this.cache.KeyDeleteAsync("cochesfavoritos");
                }
                else
                {
                    string jsonCoches =
                        JsonConvert.SerializeObject(cars);
                    //ACTUALIZAR EL CACHE
                    await this.cache.StringSetAsync("cochesfavoritos", jsonCoches,
                        TimeSpan.FromMinutes(30));
                }

            }

        }

    }
}
