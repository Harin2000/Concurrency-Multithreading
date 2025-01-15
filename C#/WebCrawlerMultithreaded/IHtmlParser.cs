namespace WebCrawlerMultithreaded
{
    public interface IHtmlParser
    {
        List<string> GetUrls(string url);
    }

    // case 1:
    public class HtmlParser : IHtmlParser
    {
        public List<string> Urls = new List<string>()
        {
            "http://news.yahoo.com",
            "http://news.yahoo.com/news",
            "http://news.yahoo.com/news/topics/",
            "http://news.google.com",
            "http://news.yahoo.com/us"
        };

        public List<string> GetUrls(string url)
        {
            if (url == Urls[2])
            {
                return new List<string>() { Urls[0], Urls[1] };
            }
            else if (url == Urls[3])
            {
                return new List<string>() { Urls[2], Urls[1] };
            }
            else if(url == Urls[0])
            {
                return new List<string>() { Urls[4] };
            }
            else return new List<string>();
        }
    }
}