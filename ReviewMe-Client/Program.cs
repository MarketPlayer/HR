using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewMe_Client
{
    public interface IStoreApi
    {
        [Get("/visitors/count/{storeName}")]
        Task<int> FetchCountAsync([Path] string storeName);

        [Get("/visitors/add/{storeName}/{count}")]
        Task<int> AddHumanVisitorsAsync([Path] string storeName, [Path] int count);

        [Delete("/visitors/reset/{storeName}")]
        Task ResetVisitorsCountAsync([Path] string storeName);
    }

    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
            Console.WriteLine("Press any key for exit...");
            Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            IStoreApi api = RestClient.For<IStoreApi>("http://localhost:60404");

            const int count = 10;
            int expectedValue = Enumerable.Range(0, count).Sum();

            const string storeName = "player1"; 

            await api.ResetVisitorsCountAsync(storeName);

            var list = new List<Task>();
            for(int i = 0; i < count; i++)
            {
                int value = i;
                list.Add(Task.Run(async () => 
                {                    
                    await api.AddHumanVisitorsAsync(storeName, value);
                }));
            }
            
            Task.WaitAll(list.ToArray());

            var result = await api.FetchCountAsync(storeName);

            if (result == expectedValue)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed!"); 
            }
        }
    }
}
