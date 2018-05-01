using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ReviewMe
{

    // Пример кастомного примитива синхронизации взят здесь:
    // https://blogs.msdn.microsoft.com/pfxteam/2012/02/12/building-async-coordination-primitives-part-6-asynclock/

    internal class AsyncLock : IAsyncLock
    {
        private readonly AsyncSemaphore _semaphore;

        private readonly Task<IDisposable> _releaser;
        
        internal AsyncLock()
        {
            _semaphore = new AsyncSemaphore(1);

            _releaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public Task<IDisposable> LockAsync()
        {
            Task wait = _semaphore.WaitAsync();

            return wait.IsCompleted ? _releaser: wait.ContinueWith((_, state) => 
                (IDisposable)new Releaser((AsyncLock)state), 
                this,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously, 
                TaskScheduler.Default);
        }

        internal class Releaser : IDisposable
        {
            private readonly AsyncLock _toRelease;

            internal Releaser(AsyncLock toRelease)
            {
                if (toRelease == null)
                    throw new ArgumentNullException("toRelease");

                _toRelease = toRelease;
            }

            public void Dispose()
            {
                if (_toRelease != null)
                {
                    _toRelease._semaphore.Release();
                }
            }            
        }        
    }
        
    internal class AsyncSemaphore
    {
        private readonly static Task _completed = Task.FromResult(true);

        private readonly Queue<TaskCompletionSource<bool>> _waiters = new Queue<TaskCompletionSource<bool>>();

        private int _currentCount;


        internal AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0)
                throw new ArgumentOutOfRangeException("initialCount");

            _currentCount = initialCount;
        }

        internal Task WaitAsync()
        {
            lock (_waiters)
            {
                if (_currentCount > 0)
                { 
                    _currentCount -= 1;

                    return _completed;
                }
                else
                {
                    TaskCompletionSource<bool> waiter = new TaskCompletionSource<bool>();

                    _waiters.Enqueue(waiter);

                    return waiter.Task;
                }
            }
        }

        internal void Release()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock (_waiters)
            {
                if (_waiters.Count > 0)
                {
                    toRelease = _waiters.Dequeue();
                }
                else
                {
                    _currentCount += 1;
                }
            }

            if (toRelease != null)
            {
                toRelease.SetResult(true);
            }
        }
    }
}