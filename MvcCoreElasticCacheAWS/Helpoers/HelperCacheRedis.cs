using StackExchange.Redis;

namespace MvcCoreElasticCacheAWS.Helpoers
{
    public class HelperCacheRedis
    {
        private static Lazy<ConnectionMultiplexer>
            CreateConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                //AQUÍ VA LA CADENA DE CONEXIÓN 
                return ConnectionMultiplexer.Connect("");
            });

        public ConnectionMultiplexer Connection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
