using System;
using System.Numerics;

namespace WebCrawlerMultithreaded
{
    internal class WebCrawler1
    {
        static void Main(string[] args)
        {
            var webCrawlerSoln = new Solution();

            // case 1
            var htmlParser = new HtmlParser();
            var urls = webCrawlerSoln.Crawl("http://news.yahoo.com/news/topics/", htmlParser);

            Console.WriteLine("\nCrawling successful!\n");
            foreach (var url in urls)
            {
                Console.WriteLine(url);
            }
        }
    }

    public class Solution()
    {
        public List<string> Crawl(string startUrl, IHtmlParser htmlParser)
        {
            HashSet<string> ans = new HashSet<string>();
            Queue<string> queue = new Queue<string>();
            string hostname = startUrl.Split('/')[2];
            ans.Add(startUrl);
            queue.Enqueue(startUrl);
            //int procCounts = Environment.ProcessorCount;
            //Console.WriteLine(procCounts);
            //return null;
            Parallel.ForEach(Enumerable.Range(0, 2*4), new ParallelOptions() { MaxDegreeOfParallelism = 4 }, _ =>
            {
                //Console.WriteLine($"New thread entered with id:{Thread.CurrentThread.ManagedThreadId}");
                while (true)
                {
                    string next = null;
                    lock (queue)
                    {
                        if (queue.Count == 0) {
                            //Console.WriteLine($"Thread exiting: {Thread.CurrentThread.ManagedThreadId}");
                            return;
                        } 
                        else next = queue.Dequeue();
                    }
                    //Console.WriteLine($"Thread in use: {Thread.CurrentThread.ManagedThreadId}");
                    var urls = htmlParser.GetUrls(next);
                    foreach (var url in urls)
                    {
                        string urlPrefix = url.Split('/')[2];
                        if(urlPrefix != hostname || ans.Contains(url)) continue;
                        lock (ans)
                        {
                            if(ans.Contains(url)) continue;
                            ans.Add(url);
                        }
                        lock (queue)
                        {
                            queue.Enqueue(url);
                        }
                    }
                }
            });
            return ans.ToList();
        }
    }
}
