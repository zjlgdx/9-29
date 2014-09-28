using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBooksDownload
{
    class Program
    {
        /// <summary>
        /// Holds the cookies used to keep the session.
        /// </summary>
        private CookieContainer cookies = new CookieContainer();

        /// <summary>
        /// Download the specified text page.
        /// </summary>
        /// <param name="response">The HttpWebResponse to download from.</param>
        /// <param name="filename">The local file to save to.</param>
        public void DownloadBinaryFile(HttpWebResponse response, String filename)
        {
            byte[] buffer = new byte[4096];
            FileStream os = new FileStream(filename, FileMode.Create);
            Stream stream = response.GetResponseStream();

            int count = 0;
            do
            {
                count = stream.Read(buffer, 0, buffer.Length);
                if (count > 0)
                    os.Write(buffer, 0, count);
            } while (count > 0);

            response.Close();
            stream.Close();
            os.Close();
        }

        /// <summary>
        /// Download the specified text page.
        /// </summary>
        /// <param name="response">The HttpWebResponse to download from.</param>
        /// <param name="filename">The local file to save to.</param>
        public void DownloadTextFile(HttpWebResponse response, String filename)
        {
            byte[] buffer = new byte[4096];
            FileStream os = new FileStream(filename, FileMode.Create);
            StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.ASCII);
            StreamWriter writer = new StreamWriter(os, System.Text.Encoding.ASCII);

            String line;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                    writer.WriteLine(line);

            } while (line != null);

            reader.Close();
            writer.Close();
            os.Close();
        }

        /// <summary>
        /// Download either a text or binary file from a URL.
        /// The URL's headers will be scanned to determine the
        /// type of tile.
        /// </summary>
        /// <param name="remoteURL">The URL to download from.</param>
        /// <param name="localFile">The local file to save to.</param>
        public void Download(Uri remoteURL, String localFile)
        {
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(remoteURL);
            http.CookieContainer = cookies;
            //http.Proxy = new WebProxy("127.0.0.1:8580");
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();

            String type = response.Headers["Content-Type"].ToLower().Trim();
            if (type.StartsWith("text"))
                DownloadTextFile(response, localFile);
            else
                DownloadBinaryFile(response, localFile);

        }

        public string DownloadJson(Uri remoteURL)
        {
            try
            {

            
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(remoteURL);
            http.CookieContainer = cookies;
            http.Accept = "*/*";

            //Get the headers associated with the request.
            //WebHeaderCollection myWebHeaderCollection = http.Headers;

            //Console.WriteLine("Configuring Webrequest to accept Danish and English language using 'Add' method");

            //Add the Accept-Language header (for Danish) in the request.
            //myWebHeaderCollection.Add("Accept-Encoding: gzip,deflate,sdch");
            //myWebHeaderCollection.Add("Accept-Language: zh-CN,zh;q=0.8,en;q=0.6");

           // http.Proxy = new WebProxy("127.0.0.1:8580");
            //http.Referer = "http://books.google.com.hk/";
              //  http.UserAgent
            http.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.124 Safari/537.36";// "User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)"; //"User-Agent: Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.18) Gecko/20110614 Firefox/3.6.18";// 
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();


            Stream r_stream = response.GetResponseStream();

            //convert it
            StreamReader response_stream = new StreamReader(r_stream, System.Text.Encoding.GetEncoding("utf-8"));

            string jSon = response_stream.ReadToEnd();

            //clean up your stream
            response_stream.Close();
            r_stream.Close();
            return jSon;
            }
            catch (Exception)
            {
                return string.Empty;
        
            }

            
        }

        public int PageWidth = 1920;

        public List<string> DownloadedPages { get; set; }

        public List<string> FailedPages { get; set; }

        public static string BaseFolder = "E:\\googleBooks";

        public static string BookFolderName = "Professional ASP.NET MVC 5";

        public List<string> ExistLocalBookPages { get; set; }

        public void GetExistLocalBookPages()
        {
            var bookFolder = Path.Combine(BaseFolder, BookFolderName);

            if (!Directory.Exists(bookFolder))
            {
                Directory.CreateDirectory(bookFolder);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(bookFolder);
          var files=  dirInfo.GetFiles();

           
            
          ExistLocalBookPages = files.Select(f => f.Name.Replace(f.Extension,"")).ToList();

          //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\downloadFiles.txt"))
          //{
          //    var filename = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\downloadFiles.txt");
          //    ExistLocalBookPages.AddRange(filename);
          //}

          //if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\downloadFiles.txt"))
          //{
          //    foreach (var item in ExistLocalBookPages)
          //    {
          //        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\downloadFiles.txt", item + System.Environment.NewLine);
          //    }
          //}
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < 50; i++)
            {
                try
                {
                    // begin


                    
            //var list = new List<string> { 
         //   "books.google.nl","books.google.se","books.google.ru","www.google.de","www.google.co.uk","www.google.co.jp","www.google.com.tw","www.google.com.hk","www.google.es","www.google.fr","www.google.be","books.google.co.kr","books.google.co.nz","books.google.dk","books.google.gr","www.google.hu","www.google.co.in","books.google.com.mx","www.google.co.za","books.google.at","www.google.com.bd","books.google.az","books.google.bg","books.google.com.by","books.google.ch","www.google.com.co","books.google.com.cu","www.google.fi","www.google.kz","books.google.mn","www.google.com.my","www.google.com.sg","books.google.sk"};

            //foreach (var burl in list)
            //{
            //    try
            //    {
            //        Console.WriteLine("host:{0}",burl);
                
            //var basePath = "E:\\googleBooks";
            //Console.Write("Please enter the book folder path:");
            //Program.BaseFolder = Console.ReadLine();

            //var driver = BaseFolder.Split('\\')[0];

            //if (!Directory.Exists(driver))
            //{
            //    BaseFolder = BaseFolder.Split('\\')[1];
            //}



            if (!Directory.Exists(BaseFolder))
            {
                Directory.CreateDirectory(BaseFolder);
            }
            //Joe Celko's Trees and Hierarchies in SQL for Smarties - Joe Celko - Google 图书
            var proWPF45Folder = Path.Combine(BaseFolder, BookFolderName);
            if (!Directory.Exists(proWPF45Folder))
            {
                Directory.CreateDirectory(proWPF45Folder);
            }

            // Joe Celko Trees and Hierarchies in SQL for Smarties
            //http://books.google.com.hk/books?id=8NdQY8TuEDUC

            //wpf
            string url = "http://books.google.ru/books?id=znAVMHNSen0C&pg={0}&jscmd=click3";

            // Joe Celko Trees and Hierarchies in SQL for Smarties
            //url = "http://www.google.az/books?id=8NdQY8TuEDUC&pg={0}&jscmd=click3";

            

            

            
           

            //To-Do List: From Buying Milk to Finding a Soul Mate, What Our Lists Reveal
            //url = "http://books.google.com.hk/books?id=2JdMqzYe85oC&pg={0}&jscmd=click3";

            

            //阅读与经典
            url = "http://books.google.de/books?id=Aa_9NqXTOq0C&pg={0}&jscmd=click3";

            //唐诗三百首(最新儿童普及版)
            //url = "http://books.google.fr/books?id=rObM2SJuIGAC&pg={0}&jscmd=click3";

            //ppk on JavaScript
            url = "http://books.google.de/books?id=oUpYRNKCjrgC&pg={0}&jscmd=click3";

            //Professional ASP.NET MVC 5
        //http://books.google.com.hk/books?id=8KMLBAAAQBAJ&pg=PA1&lpg=PA1&dq=Professional+ASP.NET+MVC+5&source=bl&ots=JWADwJQA5T&sig=bAhy7UhAqwCzfXHf2LDbUeJmcq4&hl=zh-CN&sa=X&ei=hz8mVLeEMpK78gX41YKgBw&ved=0CFUQ6AEwBjgU#v=onepage&q=Professional%20ASP.NET%20MVC%205&f=false
            url = "http://books.google.ru/books?id=8KMLBAAAQBAJ&pg={0}&jscmd=click3";
            var program = new Program();

            var pageId = "1";

            program.DownloadedPages = new List<string>();
            program.FailedPages = new List<string>();

            var jsonData = program.DownloadJson(new Uri(string.Format(url, pageId)));


            var rootObject = !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<RootObject>(jsonData) : new RootObject();

            var pids = rootObject.page.Select(p => p.pid).Distinct().ToList();

            Console.WriteLine("Total Page count:{0}", pids.Count);

            program.GetExistLocalBookPages();

            DownloadBookPage(proWPF45Folder, program, rootObject);

            var maxPageid = GetLastestDownloadPageId(program, pids);

            

            pids = pids.Except(program.ExistLocalBookPages).ToList();



            //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\downloadFiles.txt"))
            //{
            //    var filename = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\downloadFiles.txt");
            //    pids = filename.ToList();
            //}

            //return;
            //var existPids = .

            Console.WriteLine("Remain to download Pages count:{0}", pids.Count);

            if (pids.Count > maxPageid + 1)
            {
                pageId = pids[maxPageid + 1];
            }
            else
            {
                Console.WriteLine("Download complete!");
                Console.ReadKey();
                return;
            }
            //return;

          //  pageId = "PA490";

            var count = 0;

            do
            {
                count++;

                jsonData = program.DownloadJson(new Uri(string.Format(url, pageId)));


                rootObject = !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<RootObject>(jsonData) : new RootObject();

                DownloadBookPage(proWPF45Folder, program, rootObject);


                maxPageid = GetLastestDownloadPageId(program, pids);

                if (pids.Count > maxPageid + 1)
                {
                    if (pageId == pids[maxPageid + 1])
                    {
                        if (pids.Count > maxPageid + 2)
                        {
                            pageId = pids[maxPageid + 2];
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        var maxcurrentPageid = pids.LastIndexOf(pageId);

                        if (maxcurrentPageid >= maxPageid + 1)
                        {
                            if (pids.Count > maxcurrentPageid + 1)
                            {
                                pageId = pids[maxcurrentPageid + 1];
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            pageId = pids[maxPageid + 1];
                        }
                    }
                }
                else
                {
                    break;
                }


            } while (true);

            Console.WriteLine("Download complete!");
          //  Console.ReadKey();

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);

            //    }
            //}
                    //end

                }
                catch (Exception)
                {

                    // throw;
                }
            }

            Console.ReadKey();
        }

        private static int GetLastestDownloadPageId(Program program, List<string> pids)
        {
            var lastDownloadPage = program.DownloadedPages.LastOrDefault();
            var lastFailedPage = program.FailedPages.LastOrDefault();

            var lastDownloadPageIndexOf = pids.IndexOf(lastDownloadPage);
            var lastFailedPageIndexOf = pids.IndexOf(lastFailedPage);

            var maxPageid = new int[] { lastDownloadPageIndexOf, lastFailedPageIndexOf }.Max();

            return maxPageid;
        }

        private static void DownloadBookPage(string proWPF45Folder, Program program, RootObject rootObject)
        {
            try
            {

            
            var downloadingPages = rootObject.page.Where(p => p.src != null).ToList();

            foreach (var downloadingPage in downloadingPages)
            {
                if (program.DownloadedPages.Contains(downloadingPage.pid))
                {
                    continue;
                }

                if (program.ExistLocalBookPages.Contains(downloadingPage.pid))
                {
                    continue;
                }

                try
                {
                    if (!File.Exists(Path.Combine(proWPF45Folder, downloadingPage.pid + ".png")))
                    {
                        Console.WriteLine("Downloading book page: {0}-{1}", downloadingPage.pid, downloadingPage.src);
                        program.Download(new Uri(downloadingPage.src + "&w=" + program.PageWidth), Path.Combine(proWPF45Folder, downloadingPage.pid + ".png"));
                    }

                    program.DownloadedPages.Add(downloadingPage.pid);
                }
                catch (Exception ex)
                {
                    if (!program.FailedPages.Contains(downloadingPage.pid))
                    {
                        program.FailedPages.Add(downloadingPage.pid);
                    }

                    Console.WriteLine("Downloading book page: {0}-{1} failed", downloadingPage.pid, ex.Message);
                }
            }
            }
            catch (Exception)
            {

                //throw;
            }
        }
    }
}
