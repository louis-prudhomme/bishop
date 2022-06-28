using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bishop.Commands.Dump;
using HtmlAgilityPack;

namespace Bishop.Helper;

/// <summary>
///     See <see cref="Horoscope" />.
///     Scrapes TheOnion's website
/// </summary>
public class HoroscopeScraper
{
    public string GetHoroscopes(string url, string sign)
    {
        var response = CallUrl("https://www.theonion.com/your-horoscopes-week-of-" + url).Result;
        var horoscope = ParseHtml(response, sign);

        return horoscope;
    }

    private static async Task<string> CallUrl(string fullUrl)
    {
        var client = new HttpClient();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
        client.DefaultRequestHeaders.Accept.Clear();
        var response = client.GetStringAsync(fullUrl);
        return await response;
    }

    private string ParseHtml(string html, string sign)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        var horoscopeCells = htmlDoc.DocumentNode.Descendants("section")
            .Where(node => !node.GetAttributeValue("class", "").Contains("js_post-content")).ToList();

        var horoscopes = new List<string>();

        foreach (var horoscope in horoscopeCells)
        {
            if (horoscope.FirstChild.Attributes.Count <= 0) continue;

            //horoscopes.add(horoscope.firstchild.attributes[0].value
            var node = horoscope.Elements("div").ToList()[1];
            var parsedSign = node.FirstChild.InnerText;

            if (!parsedSign.Contains(sign)) continue;

            var parsedHoroscope = node.LastChild.InnerText;
            return parsedHoroscope;
        }

        return "Horoscope Parser failed, yo";
    }
}