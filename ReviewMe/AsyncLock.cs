using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ReviewMe
{
    public class AsyncLock : IAsyncLock
    {
        public Task<IDisposable> LockAsync()
        {
            throw new NotImplementedException();
        }
    }
}