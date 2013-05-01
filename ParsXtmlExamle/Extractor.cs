using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Threading;

namespace ParsXtmlExamle
{
    /// <summary>
    /// download and extract data. Class is thread safe
    /// </summary>
    class Extractor
    {
        String baseUrl;
        String stock;
        String letter;
        static String separator = "_";
        String suffix;
        //static readonly int timeout = 150000;//ms

        /// <summary>
        /// only one constuctor
        /// </summary>
        /// <param name="baseUrl">baseUrl with strucutre as follows : www/*/sth/^</param>
        /// <param name="stock">name of stock</param>
        /// <param name="letter">first letter of companies</param>
        /// <param name="suffix">suffix will be used in constructing url</param>
        public Extractor(String baseUrl, String stock, String letter, String suffix )
        {
            this.baseUrl = baseUrl;
            this.stock = stock;
            this.letter = letter;
            this.suffix = suffix;
        }

        /// <summary>
        /// extracting and transforming data for earlier constructed url
        /// </summary>
        public void Extract()
        {

            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            WebClient webClient = new WebClient();
            
           // String filePath = @"\temp.html";

            String url = constructUrl();
            String filePath = constructTempFilePath();
            webClient.DownloadFile(url, filePath);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;

            // filePath is a path to a file containing the html
            htmlDoc.Load(filePath);
            //  Console.WriteLine("" + htmlDoc.DocumentNode.SelectSingleNode("//body").InnerText);

            // Use:  htmlDoc.LoadHtml(xmlString);  to load from a string (was htmlDoc.LoadXML(xmlString)

            // ParseErrors is an ArrayList containing any errors from the Load statement
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                Console.WriteLine("paresErrors != null");
                foreach (var s in htmlDoc.ParseErrors)
                {
                    Console.WriteLine(s.Reason);
                }


            }


            FileHelper fileHelper = new FileHelper(stock, letter);
            //tr
            Console.WriteLine("TR----------------");
            HtmlNodeCollection allTr = htmlDoc.DocumentNode.SelectNodes("//tr");
            //HtmlNodeCollection allTd = htmlDoc.DocumentNode.SelectNodes("//tr//td");
            //Console.WriteLine("after reg-------->");
            //foreach (HtmlNode n in allTd)
            //  Console.WriteLine(n.InnerText);


            // bool firstIteration = true;//first element not put to file
            int i = 0;
            foreach (HtmlNode node in allTr)
            {

                if (i >= 2)
                {
                    //Console.WriteLine(node.InnerText);


                    HtmlNodeCollection allTdForTr = node.ChildNodes;
                    fileHelper.saveLineToFile(allTdForTr);
                }
                if (i < 2)
                    i++;

            }

            fileHelper.closeFile();
            deleteTempFile();

        }

        /// <summary>
        /// deleting temporary file which was constructed when downloading data 
        /// </summary>
        private void deleteTempFile()
        {
            String path = constructTempFilePath();
            File.Delete(path);
        }

        /// <summary>
        /// constructing temporary file path
        /// </summary>
        /// <returns>temporary file path witch structure as follows : XXXX-X_temp</returns>
        private string constructTempFilePath()
        {
            return stock + separator + letter + separator + "temp";
        }

        /// <summary>
        /// constructing url from which data will be downloaded and extraced
        /// </summary>
        /// <returns>url consturcted from arguments given in constructor</returns>
        private string constructUrl()
        {
            String baseOne= baseUrl.Replace("*", stock);
            Console.WriteLine(baseOne);
            String finalUrl = baseOne.Replace("^", letter);
            Console.WriteLine(finalUrl);
            return finalUrl+suffix;
        }
    }
}
