using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WampSharp.V2;

namespace PoloniexWeb.Helpers
{
    public class StaticDataHelper
    {
        public static void GetPairs()
        {
            const string serverAddress = "wss://api.poloniex.com";

            DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

            IWampChannel channel =
                factory.CreateJsonChannel(serverAddress, "ticker");

            channel.Open();
        }
        public static IReadOnlyCollection<string> GetSitemapNodes(UrlHelper urlHelper)
        {
            List<string> nodes = new List<string>();

            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Index" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Poloniex" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "OKCoin" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "CEX" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Bittrex" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Account", action = "Index" }));

            return nodes;
        }

        public static string GetSitemapDocument(IEnumerable<string> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (string sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode)),
                    new XElement(xmlns + "changefreq", "monthly"),
                    new XElement(xmlns + "priority", 1));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }
    }
    public static class UrlHelperExtensions
    {
        public static string AbsoluteRouteUrl(this UrlHelper urlHelper,
            string routeName, object routeValues = null)
        {
            string scheme = urlHelper.RequestContext.HttpContext.Request.Url.Scheme;
            return urlHelper.RouteUrl(routeName, routeValues, scheme);
        }
    }
}