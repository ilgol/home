using PoloniexWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoloniexWeb.Interfaces
{
    public interface IStatisticService
    {
        CurrencyModel GetData(string url);
        StatisticModel GetData(string startDate, string endDate);

    }
}
