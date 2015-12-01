using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Utilities
{
    public static class CacheManager
    {
        /// <summary>
        /// Property to set cache expiry
        /// </summary>     
        private static DateTime Expiration
        {
            get
            {
                int sec = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.CacheDuration]);
                return DateTime.Now.AddSeconds(sec);
            }
        }

        /// <summary>
        /// <para> English: add on Cache getting the duration from the web.config </para>
        /// <para>Español: agregar en cache con duración obtenida del web.config </para>
        /// </summary>
        /// <param name="key">string</param>
        /// <param name="obj">object</param>
        public static void Add(string key, object obj)
        {
            System.Web.HttpRuntime.Cache.Insert(key, obj, null, Expiration, TimeSpan.Zero);
            cacheAddCount++;
        }

        /// <summary>
        /// <para>English: add on cache with a specific priority </para>
        /// <para>Español: agrega al cache una prioridad especifica</para>
        /// </summary>
        /// <param name="key">string</param>
        /// <param name="obj">object</param>
        /// <param name="priority">ystem.Web.Caching.CacheItemPriority</param>
        public static void Add(string key, object obj, System.Web.Caching.CacheItemPriority priority)
        {
            System.Web.HttpRuntime.Cache.Add(key, obj, null, Expiration, TimeSpan.Zero, priority, null);
            cacheAddCount++;
        }

        /// <summary>
        /// <para>English: add on cache with a file that when it change the object and the asociated key is removed from the cache (an exaple could be an xml file)</para>
        /// <para>Español: agrega en el cache on un archivo que cuando este cambia, el objeto con su key asociada es removida del cache (un ejemplo puede ser un archivo xml)</para>
        /// </summary>
        /// <param name="key">string</param>
        /// <param name="obj">object</param>
        /// <param name="fileNameDependencyObj">string</param>
        public static void Add(string key, object obj, string fileNameDependencyObj)
        {
            System.Web.HttpRuntime.Cache.Insert(key, obj, new System.Web.Caching.CacheDependency(fileNameDependencyObj));
            cacheAddCount++;
        }

        /// <summary>
        /// <para>English: add on the cache with an specific properity and a file that when it change the object and the asociated key is removed from the cache (an exaple could be an xml file)</para>
        /// <para>Español: agrega en el cache con una prioridad especifica y archivo que cuando este cambia, el objeto con su key asociada es removida del cache (un ejemplo puede ser un archivo xml)</para>
        /// </summary>
        /// <param name="key">string</param>
        /// <param name="obj">object</param>
        /// <param name="priority">System.Web.Caching.CacheItemPriority</param>
        /// <param name="fileNameDependencyObj">string</param>
        public static void Add(string key, object obj, System.Web.Caching.CacheItemPriority priority, string fileNameDependencyObj)
        {
            System.Web.HttpRuntime.Cache.Insert(key, obj, new System.Web.Caching.CacheDependency(fileNameDependencyObj), Expiration, TimeSpan.Zero, priority, null);
            cacheAddCount++;
        }


        /// <summary>
        /// <para>English: remove an specific object from the cache</para>
        /// <para>Español: remueve un objeto especifico del cache</para>
        /// </summary>
        public static void Clear(string key)
        {
            System.Web.HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// <para>English: returns the object saved on cache</para>
        /// <para>Español: Retorna el objeto guardado en cache</para>
        /// </summary>
        /// <param name="key">string</param>
        /// <returns>Object</returns>
        public static Object Get(string key)
        {
            return System.Web.HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// <para>English: returns the count of objects saved on cache</para>
        /// <para>Español: retorna la cantidad de objetos cuardados en cache</para>
        /// </summary>
        /// <returns>int</returns>
        public static int Count()
        {
            return System.Web.HttpRuntime.Cache.Count;
        }

        /// <summary>
        /// Parameters for application statistics
        /// </summary>
        public static int cacheAddCount = 0;
        public static int totalSessionCount = 0;
        public static int activeSessionCount = 0;
        public static DateTime appStartTime;

        /// <summary>
        /// <para>Engish: returns true if the key exists on the cache memory, false if not</para>
        ///<para>Español: retorna true si la key existe en la memoria cache, en caso contrario retorna falso</para>
        /// </summary>
        /// <param name="key">string</param>
        /// <returns>bool</returns>
        public static bool ValidatExistence(string key)
        {
            return Get(key) != null;
        }

    }
}