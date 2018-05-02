using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReviewMe.DAL;
using ReviewMe.Models;

namespace ReviewMe.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task DashboardStatProcessor_GetVisitorsCountAsync()
        {
            string storeName = "player1";

            int initialHumanCount = 0;

            Store store = new Store()
            {
                Name = storeName,
                HumanCount = initialHumanCount
            };

            Mock<IStoreRepository> repository = new Mock<IStoreRepository>();

            repository.Setup(r => r.GetStoreAsync(store.Name))
                .Returns(() => {
                    TaskCompletionSource<Store> taskSource = new TaskCompletionSource<Store>();
                    taskSource.SetResult(store);
                    return taskSource.Task;
                });

            DashboardStatProcessor dashboardStatProcessor = new DashboardStatProcessor(repository.Object);

            int humanCount = await dashboardStatProcessor.GetVisitorsCountAsync(store.Name);

            Assert.AreEqual<int>(humanCount, initialHumanCount);
        }

        [TestMethod]
        public async Task DashboardStatProcessor_AddHumanVisitorsAsync()
        {
            string storeName = "player1";

            int initialHumanCount = 0;

            int humanCountForAdd = 4;

            Store store = new Store()
            {
                Name = storeName,
                HumanCount = initialHumanCount
            };

            Mock<IStoreRepository> repository = new Mock<IStoreRepository>();

            repository.Setup(r => r.GetStoreAsync(store.Name))
                .Returns(() => {
                    TaskCompletionSource<Store> taskSource = new TaskCompletionSource<Store>();
                    taskSource.SetResult(store);
                    return taskSource.Task;
                });

            DashboardStatProcessor dashboardStatProcessor = new DashboardStatProcessor(repository.Object);
                       
            int humanCount = await dashboardStatProcessor.AddHumanVisitorsAsync(store.Name, humanCountForAdd);

            Assert.AreEqual<int>(humanCount, initialHumanCount + humanCountForAdd);
        }

        [TestMethod]
        public async Task DashboardStatProcessor_DeleteVisitorsCountAsync()
        {
            string storeName = "player1";

            int initialHumanCount = 4;
            
            Store store = new Store()
            {
                Name = storeName,
                HumanCount = initialHumanCount
            };

            Mock<IStoreRepository> repository = new Mock<IStoreRepository>();

            repository.Setup(r => r.GetStoreAsync(store.Name))
                .Returns(() => {
                    TaskCompletionSource<Store> taskSource = new TaskCompletionSource<Store>();
                    taskSource.SetResult(store);
                    return taskSource.Task;
                });

            DashboardStatProcessor dashboardStatProcessor = new DashboardStatProcessor(repository.Object);

            await dashboardStatProcessor.DeleteVisitorsCountAsync("player1");
                        
            Assert.AreEqual<int>(store.HumanCount, 0);
        }
    }
}
