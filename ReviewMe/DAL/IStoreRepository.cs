using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using ReviewMe.Models;

namespace ReviewMe.DAL
{
    public interface IStoreRepository
    {
        Task<Store> GetStoreAsync(string storeName);

        void UpdateStore(Store store);
    }
}