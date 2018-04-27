using ListaCompraRedisCache.Helpers;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListaCompraRedisCache.Models
{
    public class ModeloRedisCache
    {
        //DEBEMOS INDICAR LA CLAVE DE REDIS PARA ALMACENAR EL OBJETO 
        //DICHA CLAVE DEBE SER UNICA ENTRE APLICACIONES SI DESEAMOS UTILIZAR 
        //CACHE REDIS PARA MAS FUNCIONALIDADES. 
        //CREAMOS UNA CLAVE REDIS CON EL NOMBRE DE NUESTRO PROYECTO 
        //Y EL NOMBRE DEL OBJETO QUE DESEAMOS ALMACENAR (LOS ALIMENTOS DE MODELS)
        public static String AppKeyRedis = "ListaComprarRedisCache@Alimentos@";
        
        public void AlmacenarAlimento(Alimentos alimentos)
        {
            IDatabase cacheredis = AccesoRedisCache.Connection.GetDatabase();
            String keyredis = AppKeyRedis + alimentos.IdAlimento;
            cacheredis.StringSet(keyredis, JsonConvert.SerializeObject(alimentos));
        }
        
        public Alimentos BuscarAlimentoRedis(String idalimento)
        {
            IDatabase cacheredis = AccesoRedisCache.Connection.GetDatabase();
            String keyredis = AppKeyRedis + idalimento;
            RedisValue objetoredis = cacheredis.StringGet(keyredis);
            if (objetoredis.HasValue == true)
            {
                String alimentocache = cacheredis.StringGet(keyredis);      
                Alimentos producto = 
                    JsonConvert.DeserializeObject<Alimentos>(alimentocache);
                return producto;
            }
            else
            {
                return null;
            }
        }
        
        public List<Alimentos> GetAlimentosCacheRedis()
        {
            List<Alimentos> listaalimentos = new List<Alimentos>();
            IDatabase cacheredis = AccesoRedisCache.Connection.GetDatabase();
            var endpoints = AccesoRedisCache.Connection.GetEndPoints();
            IServer server = AccesoRedisCache.Connection.GetServer(endpoints.First());
            IEnumerable<RedisKey> claves = server.Keys();
            if (claves.Count() != 0)
            {
                foreach (RedisKey key in claves)
                {
                    String alimentocache = cacheredis.StringGet(key);
                    Alimentos alimento = 
                        JsonConvert.DeserializeObject<Alimentos>(alimentocache);
                    listaalimentos.Add(alimento);
                }
                return listaalimentos;
            }
            else
            {
                return null;
            }
        }
        
        public List<RedisKey> GetKeysRedis()
        {
            var endpoints = AccesoRedisCache.Connection.GetEndPoints();
            IServer server = AccesoRedisCache.Connection.GetServer(endpoints.First());
            IEnumerable<RedisKey> claves = server.Keys();
            return claves.ToList();
        }
        
        public void LimpiarCacheRedis()
        {
            IDatabase cacheredis = AccesoRedisCache.Connection.GetDatabase();
            List<RedisKey> listaclaves = this.GetKeysRedis();
            foreach (var key in listaclaves)
            {
                cacheredis.KeyDelete(key);
            }
        }
    }
}