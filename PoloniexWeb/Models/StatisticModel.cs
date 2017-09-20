using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoloniexWeb.Models
{
    public class StatisticModel
    {
        public StatisticModel()
        {
            OverallHashRate = new List<OverallHashRateHistoryModel>();
            PartialHashRate = new List<object>();
            Payout = new List<PayoutHistoryModel>();
        }
        public List<OverallHashRateHistoryModel> OverallHashRate { get; set; }
        public List<object> PartialHashRate { get; set; }
        public List<PayoutHistoryModel> Payout { get; set; }
    }

    public class OverallHashRateHistoryModel
    {
        public double HashRate { get; set; }
        public string Date { get; set; }
    }
    public class PartialHashRateHistoryModel
    {
        public double HashRate { get; set; }
    }
    public class PayoutHistoryModel
    {
        public double Payout { get; set; }
        public string Date { get; set; }
    }
}