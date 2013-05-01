using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HtmlAgilityPack;

namespace ParsXtmlExamle
{
    /// <summary>
    /// class which help with saving earlier exracted and transformed data to file.
    /// Class is thread Safe
    /// </summary>
    class FileHelper
    {
        /// <summary>
        /// name of stock
        /// </summary>
        private String stockName;
        /// <summary>
        /// first letter of companies 
        /// </summary>
        private String letter;
        /// <summary>
        /// path pointing where file will be save
        /// </summary>
        private String path;
        /// <summary>
        /// separator used in consturcting url to file
        /// </summary>
        private String separator = "-";
        /// <summary>
        /// file object used to operations on files
        /// </summary>
        private StreamWriter file;
        //private int i = 0;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockName">name of stock</param>
        /// <param name="letter">first letter of companies on stock</param>
        public FileHelper(String stockName, String letter)
        {
            this.stockName = stockName;
            this.letter = letter;
            path = stockName + separator + letter;
            file = new StreamWriter(path);
        }

        /// <summary>
        /// saving given collection of /td> to file 
        /// </summary>
        /// <param name="allTdForTr">collections of td inside tr </param>
        public void saveLineToFile(HtmlNodeCollection allTdForTr){
          //  if (checkCorectness(allTdForTr))
          //  {
                foreach (HtmlNode innerNode in allTdForTr)
                {
                    try
                    {
                        // Console.WriteLine("->saving : " + i + " "  + );
                        String toSave = innerNode.InnerText;
                        Console.WriteLine("saving : " + toSave);
                        if(filter(toSave))
                        {
                            file.WriteLine(toSave);
                        }
                    }
                    catch (IOException e)
                    {
                        Console.Write(e);
                    }
                 
                    
                      
                }
               // i++;
          //  }
        }

        /// <summary>
        /// filter used to prevent form saving not wanted characters to file
        /// </summary>
        /// <param name="toFilter">string against which filter will be invoke</param>
        /// <returns>true if given string could be save to file, false otherwise</returns>
        private Boolean filter(String toFilter)
        {
            if (toFilter.Contains("nbsp"))
                return false;

            return true;
        }

      /*  private Boolean checkCorectness(HtmlNodeCollection tdForTr)
        {
            String shouldBeParsedToDouble = tdForTr[3].InnerText;
            foreach (HtmlNode s in tdForTr)
           {
                Console.WriteLine(s.InnerText);
            }

            Console.WriteLine("second td element : ");
            Console.WriteLine(shouldBeParsedToDouble);
          //  foreach (HtmlNode node in tdForTr)
          //  {
          //      String first =node.FirstChild.InnerText;
          //      String second= node.
          //  }


            return true;
        }
        */

        public void closeFile()
        {
            file.Close();
        }
    }
}
