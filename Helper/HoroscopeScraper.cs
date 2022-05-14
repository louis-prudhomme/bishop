using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net;
using System.Linq;

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
        HttpClient client = new HttpClient();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
        client.DefaultRequestHeaders.Accept.Clear();
        var response = client.GetStringAsync(fullUrl);
        return await response;
    }

    private string ParseHtml(string html, string sign)
    {
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        var horoscopeCells = htmlDoc.DocumentNode.Descendants("section")
                .Where(node => !node.GetAttributeValue("class", "").Contains("js_post-content")).ToList();

        List<string> horoscopes = new List<string>();

        foreach (var horoscope in horoscopeCells)
        {
            if (horoscope.FirstChild.Attributes.Count > 0) {
                //horoscopes.add(horoscope.firstchild.attributes[0].value
                HtmlNode node = horoscope.Elements("div").ToList()[1];
                string parsedSign = node.FirstChild.InnerText;
                if (parsedSign.Contains(sign)) {
                    string parsedHoroscope = node.LastChild.InnerText;
                    return parsedHoroscope;
                }
            }
        }

        return "Horoscope Parser failed, yo";
    }
}
