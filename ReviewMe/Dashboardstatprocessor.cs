using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ReviewMe.DAL;
using ReviewMe.Models;

namespace ReviewMe
{
    public interface IDashboardStatProcessor
    {
        Task<int> AddHumanVisitorsAsync(string storeName, int humanCount);

        Task<int> GetVisitorsCountAsync(string storeName);

        Task DeleteVisitorsCountAsync(string storeName);
    }

    /// <summary>
    /// Предоставляет API для работы со статистикой магазина. 
    /// </summary>
    public class DashboardStatProcessor : IDashboardStatProcessor
    {
        private static object _lockObj = new object();

        private static Dictionary<string, int> _statisticData = new Dictionary<string, int>();

        private IStoreRepository _storeRepository;

        public DashboardStatProcessor(IStoreRepository storeRepository)
        {
            if (storeRepository == null)
                throw new ArgumentNullException("storeRepository");

            _storeRepository = storeRepository;
        }

        public async Task<int> AddHumanVisitorsAsync(string storeName, int humanCount)
        {
            if (string.IsNullOrEmpty(storeName))
                throw new ArgumentNullException("storeName");
            if (humanCount < 0)
                throw new ArgumentOutOfRangeException("humanCount");
                                    
            Store store = await _storeRepository.GetStoreAsync(storeName);
            
            store.HumanCount += humanCount;

            _storeRepository.UpdateStore(store);

            this.CacheStatisticData(storeName, store.HumanCount);

            return store.HumanCount;                  
        }

        public async Task<int> GetVisitorsCountAsync(string storeName)
        {
            if (string.IsNullOrEmpty(storeName))
                throw new ArgumentNullException("storeName");
                        
            if (_statisticData.ContainsKey(storeName))
                return _statisticData[storeName];

            Store store = await _storeRepository.GetStoreAsync(storeName);

            this.CacheStatisticData(storeName, store.HumanCount);

            return store.HumanCount;            
        }

        public async Task DeleteVisitorsCountAsync(string storeName)
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

            Store store = await _storeRepository.GetStoreAsync(storeName);

            store.HumanCount = 0;

            _storeRepository.UpdateStore(store);

            this.CacheStatisticData(storeName, store.HumanCount);

            return;                        
        }

        private void CacheStatisticData(string storeName, int humanCount)
        {
            lock (_lockObj)
            {
                if (!_statisticData.ContainsKey(storeName))
                {
                    _statisticData.Add(storeName, humanCount);
                }
                else
                {
                    _statisticData[storeName] = humanCount;
                }
            }
        }
    }
}