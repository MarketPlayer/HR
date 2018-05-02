using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReviewMe.Controllers
{
    /// <summary>
    /// Предоставляет WEB API для работы со статистикой магазина.
    /// </summary>    
    public class HomeController : ApiController
    {
        private static IAsyncLock _lock = new AsyncLock();

        private IDashboardStatProcessor _dashboardStatProcessor;

        public HomeController(IDashboardStatProcessor dashboardStatProcessor)
        {            
            if (dashboardStatProcessor == null)
                throw new ArgumentNullException("dashboardStatProcessor");

            _dashboardStatProcessor = dashboardStatProcessor;
        }

        /// <summary>
        /// Тестовый метод.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Api started");
        }

        /// <summary>
        /// Добавить посетителей.
        /// </summary>
        /// <param name="storeName">Название магазина.</param>
        /// <param name="count">Количество посетителей.</param>
        /// <returns>Tекущее количество посетителей магазина</returns>
        [HttpPost]
        [Route("visitors/add/{storeName}/{count}")]
        public async Task<int> AddHumanVisitors(string storeName, int count)
        {            
            using (await _lock.LockAsync())
            {
                return await _dashboardStatProcessor.AddHumanVisitorsAsync(storeName, count);
            }           
        }

        /// <summary>
        /// Получить текущее количество посетителей магазина.
        /// </summary>
        /// <param name="storeName">Название магазина.</param>
        /// <returns>Tекущее количество посетителей магазина</returns>
        [HttpGet]
        [Route("visitors/count/{storeName}")]
        public async Task<int> GetVisitorsCount(string storeName)
        {
            using (await _lock.LockAsync())
            {
                return await _dashboardStatProcessor.GetVisitorsCountAsync(storeName);
            }        
        }

        /// <summary>
        /// Обнулить статистику посетителей магазина.
        /// </summary>
        /// <param name="storeName">Название магазина.</param>
        [HttpDelete]
        [Route("visitors/reset/{storeName}")]
        public async Task DeleteVisitorsCount(string storeName)
        {
            using (await _lock.LockAsync())
            {
                await _dashboardStatProcessor.DeleteVisitorsCountAsync(storeName);
            }      
        }
    }
}
