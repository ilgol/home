using Mailer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mailer
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Для выхода нажмите 'Enter'");
            Timer t = new Timer(TimerCallback, null, 0, 1800000);
            Console.ReadLine();
        }
        private static void TimerCallback(Object o)
        {
            ParserService.ParseSite();
            GC.Collect();
        }
    }
}
