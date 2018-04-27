using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ListaCompraRedisCache.Helpers
{
    public class AccesoRedisCache
    {
        private static String CadenaConexionRedis = 
            ConfigurationManager.AppSettings["azurerediscache"];
        
        private static Lazy<ConnectionMultiplexer> conectar =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(CadenaConexionRedis);
            });
        
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return conectar.Value;

            }
        }
    }
}