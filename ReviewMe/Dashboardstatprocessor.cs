using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ReviewMe
{
    //[CR] TODO: Проанализировать необходимость использования модификатора public для класса и его членов.
    //При необходимости изменить. Public нужно использовать для классов, реализующих "внешний" API модуля.

    // [CR] TODO: Хорошая практика для классов с модификатором доступа public (и их членов) оставлять комментарий.

    public class DashboardStatProcessor 
    {
        // [CR] TODO: Не подходящий модификатор доступа (нарушение основополагающего
        //  принципа ООП - инкапсуляция) для поля. Заменить на private. 
        // private для полей вообще не использовать.

        public static Dictionary<string, int> _statisticData;

        //[CR] TODO: Для всех методов, привести в соответсвие названия параметров.

        //[CR] TODO: Для всех методов реализовать проверку входных параметров на корректность.

        //[CR] TODO: Реализовать кеширование статистики в переменной _statisticData.

        //[CR] TODO: В текущей реализации класс нарушает общепринятое правило о потокобезапасности.
        // В общем случае считаем, что экземплярные члены не потокобезопасными, статические - потокобезопасными.
        // Следовательно нужно, либо сделать методы экземплярными и возложить ответственность за синхронизацию на 
        // вызывающий код, либо оставить методы статическими и реализовать синхронизацию внутри класса DashboardStatProcessor.

        public static async Task<bool> AddHumanVisitors(string playerName, int value)
        {          
            using (var db = new ApplicationDbContext())
            {
                var player = await db.Stores.SingleAsync(x => x.Name == playerName);                
                player.HumanCount += value;
                db.SaveChanges();
            }                           
            return true;
        }

        //[CR] TODO: Методы GetVisitorsCount и DeleteVisitorsCount совершают обращение к БД
        // для получения/записи данных. В данном случае использование асиннхронных обращений
        // это хорошо. 

        public static int GetVisitorsCount(string playerName)
        {            
            using (var db = new ApplicationDbContext())
            {                
                var player = db.Stores.FirstOrDefault(x => x.Name == playerName);
                //[CR] TODO: В текущей реализации, оргпнизовать проверку player на null.
                return player.HumanCount;
            }
        }

        public static void DeleteVisitorsCount(string playerName)
        {           
            using (var db = new ApplicationDbContext())
            {                 
                var player = db.Stores.FirstOrDefault(x => x.Name == playerName);
                //[CR] TODO: В текущей реализации, оргпнизовать проверку player на null.
                player.HumanCount = 0;
                db.SaveChanges();
            }
        }
    }
}