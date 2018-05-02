using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using ReviewMe.Models;

namespace ReviewMe.DAL
{
    public class StoreRepository : IStoreRepository
    {
        private ApplicationDbContext _db = null;
        private ApplicationDbContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new ApplicationDbContext();
                }
                return _db;
            }
        }
        
        public async Task<Store> GetStoreAsync(string storeName)
        {
            if (string.IsNullOrEmpty(storeName))
                throw new ArgumentNullException("storeName");

            return await db.Stores.SingleAsync(x => x.Name == storeName);
        }

        public void UpdateStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            if (store.Id == 0)
            {
                db.Stores.Add(store);
            }
            else
            {
                db.Entry(store).State = EntityState.Modified;
            }

            db.SaveChanges();
        }
    }
}