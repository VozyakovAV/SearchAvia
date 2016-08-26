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

namespace SearchAvia
{
    // https://search.aviasales.ru/MOW2608PAR19091

    public class SearchAviasales : SearchBase
    {
        public override bool IsSearching
        {
            get
            {
                throw new NotImplementedException();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override string GetResult()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            var html = LoadContent();
            //File.WriteAllText(@"C:\temp\2\1.html", html);
            //var html = File.ReadAllText(@"C:\temp\2\1.html");
            Parse(html);
        }

        private string LoadContent()
        {
            var url = "https://search.aviasales.ru/MOW2708PAR19091";
            var loader = new HelperAwesomium();
            loader.Load(url, CheckLoading);
            loader.Test2();
            //loader.JsFireEvent("document.getElementsByTagName('a')[0]", "click");
            //ticket-new__opener
            return loader.GetHtml();
        }

        private bool CheckLoading(string html)
        {
            if (html.Length > 0)
                File.WriteAllText(@"C:\temp\2\2.html", html);
            return html.Contains("ticket-new");
        }

        private void Parse(string html)
        {
            var node = HtmlParsingHelper.ToHtmlNode(html);
            var ticketsNodes = node.CssSelect("#results_add_container .ticket-new__container");
            var ticketNode = ticketsNodes.FirstOrDefault();

            if (ticketNode != null)
            {
                var res = new SearchResult();

                res.Airline = ParseAirline(ticketNode);

                DateTime date, dateBack;
                ParseDates(ticketNode, out date, out dateBack);
                res.Date = date;
                res.DateBack = dateBack;
                

            }

        }

        private string ParseAirline(HtmlNode node)
        {
            var img = node.CssSelect(".ticket-new__airline-logo img").FirstOrDefault();
            if (img != null && img.Attributes.Contains("title"))
                return img.Attributes["title"].Value;
            return string.Empty;
        }

        private void ParseDates(HtmlNode node, out DateTime date, out DateTime dateBack)
        {
            date = Date;
            dateBack = DateBack;

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