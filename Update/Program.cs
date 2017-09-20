using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Update.Interfaces;

namespace Update
{
    class Program
    {
        public static IStatisticService StatisticService { get; set; }
        private static readonly string key = "d1f04914020cffe5e9e510c73bdbf717e9792bcee7e9b9fb073baed705fc0e3a";
        private static readonly string url = "https://dgb-scrypt.theblocksfactory.com/api.php?api_key=";

        static void Main(string[] args)
        {
            StatisticService = new StatisticService();
            Console.WriteLine("Для выхода нажмите 'Enter'");
            Timer t = new Timer(TimerCallback, null, 0, 3600000);
            Console.ReadLine();
        }
        private static void TimerCallback(Object o)
        {
            StatisticService.GetStatistic(url, key);
            GC.Collect();
        }
    }
}
