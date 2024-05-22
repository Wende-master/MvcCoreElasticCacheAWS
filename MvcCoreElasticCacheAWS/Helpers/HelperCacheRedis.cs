using StackExchange.Redis;

namespace MvcCoreElasticCacheAWS.Helpers
{
    public class HelperCacheRedis
    {
        private static Lazy<ConnectionMultiplexer>
            CreateConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                string connectionString =
                "cache-coches.enynhj.ng.0001.use1.cache.amazonaws.com:6379";
                //AQUÍ VA LA CADENA DE CONEXIÓN 
                return ConnectionMultiplexer.Connect(connectionString);
            });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
