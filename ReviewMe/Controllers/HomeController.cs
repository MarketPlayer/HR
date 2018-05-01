using System.Threading.Tasks;
using System.Web.Http;

namespace ReviewMe.Controllers
{
    /// <summary>
    /// Предоставляет WEB API для работы со статистикой магазина.
    /// </summary>    
    public class HomeController : ApiController
    {      
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
        [HttpGet]
        [Route("visitors/add/{storeName}/{count}")]
        public async Task<int> AddHumanVisitors(string storeName, int count)
        {
            return await DashboardStatProcessor.AddHumanVisitorsAsync(storeName, count);            
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
            return await DashboardStatProcessor.GetVisitorsCountAsync(storeName);            
        }

        /// <summary>
        /// Обнулить статистику посетителей магазина.
        /// </summary>
        /// <param name="storeName">Название магазина.</param>
        [HttpDelete]
        [Route("visitors/reset/{storeName}")]
        public async Task DeleteVisitorsCount(string storeName)
        {           
            await DashboardStatProcessor.DeleteVisitorsCountAsync(storeName);            
        }
    }
}
