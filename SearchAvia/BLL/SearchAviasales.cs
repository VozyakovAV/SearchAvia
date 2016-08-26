using ScrapySharp.Network;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Text;

namespace SearchAvia
{
    // https://search.aviasales.ru/MOW2608PAR19091

    public class SearchAviasales : SearchBase
    {
        public SearchAviasales(string cityFrom, string cityTo, DateTime date, DateTime dateBack) 
            : base(cityFrom, cityTo, date, dateBack)
        { }

        public override SearchResult Load()
        {
            var url = GetUrlLoad();
            var html = LoadContent(url);
            //File.WriteAllText(@"C:\temp\2\1.html", html);
            //var html = File.ReadAllText(@"C:\temp\2\1.html");
            var res = Parse(html);
            res.Url = url;
            return res;
        }

        private string GetUrlLoad()
        {
            //https://search.aviasales.ru/MOW2708PAR19091
            var sb = new StringBuilder("https://search.aviasales.ru/");
            sb.Append(CityFrom);
            sb.Append(Date.Day.ToString().PadLeft(2, '0'));
            sb.Append(Date.Month.ToString().PadLeft(2, '0'));
            sb.Append(CityTo);
            sb.Append(DateBack.Day.ToString().PadLeft(2, '0'));
            sb.Append(DateBack.Month.ToString().PadLeft(2, '0'));
            sb.Append("1");
            return sb.ToString();
        }

        private string LoadContent(string url)
        {
            var html = HelperAwesomium.Instance.Load(url, CheckLoading);
            HelperAwesomium.Instance.OnClick(".ticket-new__opener");
            Thread.Sleep(1000);
            html = HelperAwesomium.Instance.GetHtml();
            return html;
        }

        private bool CheckLoading(string html)
        {
            return html.Length > 0 && !html.Contains("countdown__time") && html.Contains("ticket-new");
        }

        private SearchResult Parse(string html)
        {
            var node = HtmlParsingHelper.ToHtmlNode(html);
            var ticketsNodes = node.CssSelect("#results_add_container .ticket-new__container");
            var ticketNode = ticketsNodes.FirstOrDefault();

            if (ticketNode != null)
            {
                var res = new SearchResult();

                res.Airline = ParseAirline(ticketNode);
                res.Price = ParsePrice(ticketNode);
                res.Date = ParseDate(ticketNode);
                res.DateBack = ParseDateBack(ticketNode);
                res.Flights = ParseFlights(ticketNode, 0);
                res.FlightsBack = ParseFlights(ticketNode, 1);
                return res;
            }
            return null;
        }

        private string ParseAirline(HtmlNode node)
        {
            var img = node.CssSelect(".ticket-new__airline-logo img").FirstOrDefault();
            if (img != null && img.Attributes.Contains("title"))
                return img.Attributes["title"].Value;
            return string.Empty;
        }

        private double ParsePrice(HtmlNode node)
        {
            var price = node.CssSelect(".ticket-new__buy-price-num").FirstOrDefault();
            if (price != null)
            {
                var st = price.InnerText.Replace(char.ConvertFromUtf32(8201), "");
                var mc = Regex.Match(st, "\\d+");
                if (mc.Success)
                    return int.Parse(mc.Value);
            }
            return 0;
        }

        private string ParseDate(HtmlNode node)
        {
            return ParseDate(node, ".fly-segment__origin", 0);
        }

        private string ParseDateBack(HtmlNode node)
        {
            return ParseDate(node, ".fly-segment__destination", 1);
        }

        private string ParseDate(HtmlNode node, string selector, int number)
        {
            var nodes = node.CssSelect(".fly-segment__common-info").ToList();
            if (nodes.Count() > number)
            {
                var nodeDate = nodes[number].CssSelect(selector + " .fly-segment__date").FirstOrDefault();
                var nodeTime = nodes[number].CssSelect(selector + " .fly-segment__time").FirstOrDefault();
                var dateSt = nodeDate == null ? "" : nodeDate.InnerText;
                var timeSt = nodeTime == null ? "" : nodeTime.InnerText;
                var date = dateSt + ", " + timeSt;
                return date;
            }
            return string.Empty;
        }

        private string[] ParseFlights(HtmlNode node, int number)
        {
            var list = new List<string>();
            var nodesTop = node.CssSelect(".ticket-new__segment").ToList();
            if (nodesTop.Count > number)
            {
                var nodes = nodesTop[number].CssSelect(".fly-segment__footer-info");
                foreach (var n in nodes)
                    list.Add(n.InnerText.Replace("Рейс:", "").Trim());
            }

            return list.ToArray();
        }
    }
}