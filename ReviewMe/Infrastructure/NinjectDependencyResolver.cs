using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using System.Web.Configuration;
using Ninject;
using ReviewMe;
using ReviewMe.DAL;

namespace ReviewMe.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            this.kernel = new StandardKernel();
            this.kernel.Bind<IStoreRepository>().To<StoreRepository>();
            this.kernel.Bind<IDashboardStatProcessor>().To<DashboardStatProcessor>();           
        }

        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyResolver();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.kernel.Dispose();
        }
    }
}