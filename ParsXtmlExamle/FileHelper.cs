using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HtmlAgilityPack;

namespace ParsXtmlExamle
{
    class FileHelper
    {
        private String stockName;
        private String letter;
        private String path;
        private String separator = "-";
        private StreamWriter file;
        //private int i = 0;


        public FileHelper(String stockName, String letter)
        {
            this.stockName = stockName;
            this.letter = letter;
            path = stockName + separator + letter;
            file = new StreamWriter(path);
        }

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
