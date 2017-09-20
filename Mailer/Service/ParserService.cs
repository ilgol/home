using Data;
using HtmlAgilityPack;
using Mailer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.Service
{
    public class ParserService
    {
        private static readonly string _url = "https://shop.bitmain.com/main.htm";
        public static void ParseSite()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HttpGet(_url));
            HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes("//button[@class='add']");
            var itemData = new List<ItemModel>();
            foreach (var node in nodes)
            {
                var li = node.ParentNode.ParentNode.ParentNode;
                var name = li.ChildNodes.ToList()[3].InnerText.Replace("\t", "").Trim().Split('\n')[0];
                var price = li.ChildNodes.ToList()[5].ChildNodes.ToList()[3].InnerText.Replace("&nbsp;", " ");
                var link = "https://shop.bitmain.com/" + li.ChildNodes.ToList()[7].ChildNodes.ToList()[3].Attributes["href"].Value;
                itemData.Add(new ItemModel()
                {
                    Name = name,
                    Price = price,
                    Link = link
                });
            }
            using (var db = new homeEntities())
            {
                do { } while (!PingHost());

                var data = db.Notifies.Select(i => new { Name = i.Name, Id = i.Id }).ToList();
                var newItems = new List<ItemModel>();
                var notDeleted = new List<Guid>();

                if (data != null && data.Any())
                {
                    foreach (var item in itemData)
                    {
                        if (data.Any(i => i.Name == item.Name))
                            notDeleted.Add(data.Where(i => i.Name == item.Name).Select(i => i.Id).First());
                        else
                            newItems.Add(item);
                    }
                    do { } while (!PingHost());
                    db.Notifies.RemoveRange(db.Notifies.Where(i => !notDeleted.Contains(i.Id)));
                    if (newItems.Any() && newItems != null)
                    {
                        do { } while (!PingHost());
                        db.Notifies.AddRange(newItems.Select(i => new Notify() { Id = Guid.NewGuid(), Name = i.Name }));
                        newItems.ForEach(EmailService.Notify);
                    }
                }
                else
                {
                    do { } while (!PingHost());
                    db.Notifies.AddRange(itemData.Select(i => new Notify() { Id = Guid.NewGuid(), Name = i.Name }));
                    itemData.ForEach(EmailService.Notify);
                }
                do { } while (!PingHost());
                db.SaveChanges();
            }
        }
        private static bool PingHost(string nameOrAddress = "shop.bitmain.com")
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

        private static string HttpGet(string url)
        {
            String responseContent = "";
            HttpWebResponse httpWebResponse = null;
            StreamReader streamReader = null;
            try
            {
                do { } while (!PingHost());

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
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
    }
}
