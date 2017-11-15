using PoloniexWeb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PoloniexWeb.Models;
using System.Globalization;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Data;

namespace PoloniexWeb.Services
{
    public class StatisticService : IStatisticService
    {
        public StatisticModel GetData(string startDate, string endDate)
        {
            using (homeEntities entities = new homeEntities())
            {
                DateTime start = Convert.ToDateTime(startDate);
                DateTime end = Convert.ToDateTime(endDate);
                var result = entities.PartialHashRateHistories
                    .Where(i => i.Date >= start && i.Date <= end)
                    .OrderBy(i => i.Date)
                    .ToList()
                    .GroupBy(i => i.Date.ToString("yyyy-MM-dd HH:mm"))
                    .ToDictionary(t => t.Key, t => t.OrderBy(j => j.Name)
                    .Select(ConvertToPartialHashRateModel)
                    .ToList());

                List<object> list = new List<object>();
                foreach (var pair in result)
                {
                    list.Add(new
                    {
                        Date = pair.Key,
                        SQ1 = Math.Round(pair.Value[0].HashRate),
                        SQ2 = Math.Round(pair.Value[1].HashRate),
                        SQ3 = Math.Round(pair.Value[2].HashRate),
                        SQ4 = Math.Round(pair.Value[3].HashRate),
                        SQ5 = Math.Round(pair.Value[4].HashRate)
                        //SQ6 = Math.Round(pair.Value[6].HashRate),
                        //SQ7 = Math.Round(pair.Value[7].HashRate),
                        //SQ8 = Math.Round(pair.Value[8].HashRate),
                        //SQ9 = Math.Round(pair.Value[9].HashRate),
                        //SQ10 = Math.Round(pair.Value[1].HashRate)
                    });
                }
                return new StatisticModel()
                {
                    OverallHashRate = entities.OverallHashRateHistories
                    .Where(i => i.Date >= start && i.Date <= end)
                    .Select(ConvertToOverallHashRateModel)
                    .OrderBy(i => i.Date)
                    .ToList(),
                    PartialHashRate = list,
                    Payout = entities.PayoutHistories
                    .Where(i => i.Date >= start && i.Date <= end)
                    .Select(ConvertToPayoutModel)
                    .OrderBy(i => i.Date)
                    .ToList()
                };
            }
        }

        public CurrencyModel GetData(string url)
        {
            String responseContent = "";
            HttpWebResponse httpWebResponse = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                if (streamReader == null)
                {
                    return null;
                }
                responseContent = streamReader.ReadToEnd();
                if (responseContent == null || "".Equals(responseContent))
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();

                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            return JsonConvert.DeserializeObject<CurrencyModel>(responseContent);
        }

        private PartialHashRateHistoryModel ConvertToPartialHashRateModel(PartialHashRateHistory arg)
        {
            return new PartialHashRateHistoryModel()
            {
                HashRate = Convert.ToDouble(Convert.ToDouble(arg.HashRate) / 1000)
            };
        }

        private PayoutHistoryModel ConvertToPayoutModel(PayoutHistory arg)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ",";
            return new PayoutHistoryModel()
            {
                Payout = Convert.ToDouble(arg.Payout, provider),
                Date = arg.Date.ToString("yyyy-MM-dd HH:mm")
            };
        }

        private OverallHashRateHistoryModel ConvertToOverallHashRateModel(OverallHashRateHistory arg)
        {
            return new OverallHashRateHistoryModel()
            {
                HashRate = Convert.ToDouble(Convert.ToInt32(arg.HashRate) / 1000),
                Date = arg.Date.ToString("yyyy-MM-dd HH:mm")
            };
        }
    }
}