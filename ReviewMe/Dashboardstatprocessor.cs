using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ReviewMe.Models;

namespace ReviewMe
{
    /// <summary>
    /// Предоставляет API для работы со статистикой магазина. 
    /// </summary>
    internal class DashboardStatProcessor 
    {
        private static IAsyncLock _lock = new AsyncLock();

        private static Dictionary<string, int> _statisticData = new Dictionary<string, int>();
               
        internal static async Task<int> AddHumanVisitorsAsync(string storeName, int humanCount)
        {
            if (string.IsNullOrEmpty(storeName))
                throw new ArgumentNullException("storeName");
            if (humanCount < 0)
                throw new ArgumentOutOfRangeException("humanCount");

            using (await _lock.LockAsync())
            {
                using (var db = new ApplicationDbContext())
                {
                    var store = await db.Stores.SingleAsync(x => x.Name == storeName);

                    store.HumanCount += humanCount;

                    db.SaveChanges();

                    if (!_statisticData.ContainsKey(storeName))
                        _statisticData.Add(storeName, 0);

                    _statisticData[storeName] = store.HumanCount;

                    return store.HumanCount;                  
                }                
            }
        }
                
        internal static async Task<int> GetVisitorsCountAsync(string storeName)
        {
            if (string.IsNullOrEmpty(storeName))
                throw new ArgumentNullException("storeName");
                        
            if (_statisticData.ContainsKey(storeName))
                return _statisticData[storeName];

            using (await _lock.LockAsync())
            {
                using (var db = new ApplicationDbContext())
                {
                    var store = await db.Stores.SingleAsync(x => x.Name == storeName);

                    _statisticData.Add(storeName, store.HumanCount);

                    return store.HumanCount;
                }
            }
        }

        internal static async Task DeleteVisitorsCountAsync(string storeName)
        {
            if (string.IsNullOrEmpty(storeName))
                throw new ArgumentNullException("storeName");

            bool statisticCached = false;
            int humanCount = 0;

            if (_statisticData.ContainsKey(storeName))
            {
                statisticCached = true;
                humanCount = _statisticData[storeName];
            }

            if (statisticCached && humanCount == 0)
                return;

            using (await _lock.LockAsync())
            {
                using (var db = new ApplicationDbContext())
                {
                    var store = await db.Stores.SingleAsync(x => x.Name == storeName);

                    store.HumanCount = 0;

                    db.SaveChanges();

                    _statisticData[storeName] = 0;
                }
            }
        }
    }
}