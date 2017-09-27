using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Update.Interfaces;
using Update.Models;
using System.Net.NetworkInformation;
using Data;

namespace Update
{
    public class StatisticService : IStatisticService
    {
        public static bool PingHost(string nameOrAddress = "dgb-scrypt.theblocksfactory.com")
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return pingable;
        }
        public void GetStatistic(string url, string param)
        {
            WriteStatistic(HttpGet(url, param));
        }
        private string HttpGet(string url, string param)
        {
            String responseContent = "";
            HttpWebResponse httpWebResponse = null;
            StreamReader streamReader = null;
            try
            {
                if (param != null && !param.Equals(""))
                {
                    url = url + param;
                }
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                do
                {
                    do { } while (!PingHost());
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                while (httpWebResponse.StatusCode != HttpStatusCode.OK);
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                if (streamReader == null)
                {
                    return "";
                }
                responseContent = streamReader.ReadToEnd();
                if (responseContent == null || "".Equals(responseContent))
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
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
            return responseContent;
        }
        public void WriteStatistic(string data)
        {
            DigiBitData statistic = JsonConvert.DeserializeObject<DigiBitData>(data);
            using (var db = new homeEntities())
            {
                foreach (var worker in statistic.Workers)
                {
                    db.PartialHashRateHistories.Add(new PartialHashRateHistory()
                    {
                        Id = Guid.NewGuid(),
                        HashRate = worker.Value.hashrate,
                        Name = worker.Key,
                        Date = DateTime.Now
                    });
                }
                db.OverallHashRateHistories.Add(new OverallHashRateHistory()
                {
                    Id = Guid.NewGuid(),
                    HashRate = statistic.total_hashrate,
                    Date = DateTime.Now
                });
                db.PayoutHistories.Add(new PayoutHistory()
                {
                    Id = Guid.NewGuid(),
                    Payout = (statistic.payout_history + statistic.confirmed_rewards).ToString(),
                    Date = DateTime.Now
                });
                db.SaveChanges();
            }
        }
    }
}
