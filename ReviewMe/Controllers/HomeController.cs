using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReviewMe.Controllers
{
    //[CR] TODO: Комментарии к public классам и членам класса.
    public class HomeController : ApiController
    {
        //[CR] TODO: Реализовать кастомный примитив синхронизации потоков AsyncLock.

        //[CR] TODO: Проанализировать возможность (необходимость?) синхроизации потоков в контроллере. 
        // Синхронизация возможна (при использовании static примитива), но в данном случае избыточна,
        // т.к. каждое из действий вызывает static методы класса DashboardStatProcessor. По умолчанию
        // считаем static методы потокобезопасными.

        private static IAsyncLock _lock = new AsyncLock();

        //[CR] TODO: Привести в соответствие названия маршрутов 
        // для доступа к действиям AddHumanVisitors, DeleteVisitorsCount.

        //[CR] TODO: Реализовать обработку исключительных ситуаций.

        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Api started");
        }        

        [HttpGet]
        [Route("add")]
        public async Task<IHttpActionResult> AddHumanVisitors(string storeName, int count)
        {          
            {
                if (DashboardStatProcessor.AddHumanVisitors(storeName, count).Result)
                {
                    return Ok();
                }
            }
            return InternalServerError();
        }

        //[CR] TODO: GetVisitorsCount потенциально инициирует обращение к БД. Реализовать ассинхронность.

        [HttpGet]
        [Route("visitors/count")]
        public int GetVisitorsCount(string storeName)
        {
            return DashboardStatProcessor.GetVisitorsCount(storeName);            
        }

        [HttpDelete]
        [Route("visitors/count")]
        public async void DeleteVisitorsCount(string storeName)
        {
            using (await _lock.LockAsync())
            {
                DashboardStatProcessor.GetVisitorsCount(storeName);
            }
        }
    }
}
