using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Update.Interfaces
{
    public interface IStatisticService
    {
        void GetStatistic(string url, string param);
        void WriteStatistic(string data);
    }
}
