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
            //var html = LoadContent(url);
            //File.WriteAllText(@"C:\temp\2\1.html", html);
            var html = File.ReadAllText(@"C:\temp\2\1.html");
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
            if (DateBack.HasValue)
            {
                sb.Append(DateBack.Value.Day.ToString().PadLeft(2, '0'));
                sb.Append(DateBack.Value.Month.ToString().PadLeft(2, '0'));
            }
            sb.Append("1");
            return sb.ToString();
        }

        private string LoadContent(string url)
        {
            var loader = new HelperAwesomium();
            loader.Load(url, CheckLoading);
            return loader.GetHtml();
        }

        private bool CheckLoading(string html)
        {
            return html.Contains("ticket-new");
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

                DateTime date, dateBack;
                ParseDates(ticketNode, out date, out dateBack);
                res.Date = date;
                res.DateBack = dateBack;

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

        private void ParseDates(HtmlNode node, out DateTime date, out DateTime dateBack)
        {
            date = Date;
            dateBack = DateBack.GetValueOrDefault();

            var nodes = node.CssSelect(".fly-segment__common-info").ToList();
            if (nodes.Count() > 0)
            {
                var nodeTime = nodes[0].CssSelect(".fly-segment__time").FirstOrDefault();
                date = ParseDate(nodeTime, date);
            }
            if (nodes.Count() > 1)
            {
                var nodeTime = nodes[1].CssSelect(".fly-segment__time").FirstOrDefault();
                dateBack = ParseDate(nodeTime, dateBack);
            }
        }

        private DateTime ParseDate(HtmlNode node, DateTime currentDate)
        {
            var mc = Regex.Match(node.InnerText, "(?<h>\\d{1,2}):(?<m>\\d{1,2})");
            if (mc.Success)
            {
                var h = int.Parse(mc.Groups["h"].Value);
                var m = int.Parse(mc.Groups["m"].Value);
                return new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, h, m, 0);
            }
            return currentDate;
        }
    }
}