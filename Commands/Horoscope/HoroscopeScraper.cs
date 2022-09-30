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
    public async Task<string> GetHoroscopes(string url, string sign)
    {
        var response = await CallUrl("https://www.theonion.com/your-horoscopes-week-of-" + url);
        var horoscope = ParseHtml(response, sign);

        return horoscope;
    }

    private static async Task<string> CallUrl(string fullUrl)
    {
        var client = new HttpClient();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
        client.DefaultRequestHeaders.Accept.Clear();
        return await client.GetStringAsync(fullUrl);
    }

    private string ParseHtml(string html, string sign)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        var horoscopeCells = htmlDoc.DocumentNode
            .Descendants("section")
            .Where(node => !node
                .GetAttributeValue("class", "")
                .Contains("js_post-content"))
            .ToList();

        foreach (var horoscope in horoscopeCells)
        {
            if (horoscope == null || horoscope.FirstChild.Attributes.Count <= 0) continue;

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