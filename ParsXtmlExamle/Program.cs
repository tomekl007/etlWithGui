using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Threading;
using System.Threading.Tasks;

namespace ParsXtmlExamle
{
    class Program
    {

        static List<string> alphabet = new List<string>();
        static List<string> stocks = new List<string>();


        static void Main(string[] args)
        {
            //initialization 
            for (char ch = 'A'; ch <= 'Z'; ch++){
                alphabet.Add(ch.ToString());
            }

            List<string> alphabetT = alphabet.GetRange(3, 4);


            stocks.Add("NYSE");
            stocks.Add("NASDAQ");
            stocks.Add("AMEX");

            // * is placeholder to replace by stock and ^ for letter
            String baseUrl = "http://www.findata.co.nz/markets/*/symbols/^";

            int nrOfCores = Environment.ProcessorCount;

        
           /* Parallel.ForEach(alphabet, s =>
                {
                    Extractor ex = new Extractor(baseUrl, stocks[0], s, ".htm");
                  
                    ex.Extract();

                });



          */  
          Parallel.ForEach(alphabetT, s =>
             {
                    DatabaseHelper dh = new DatabaseHelper(stocks[0], s);

                    dh.LoadCompaniesToDatabase();

                });
            
            
           Parallel.ForEach(alphabetT, s =>
            {
                DatabaseHelper dh = new DatabaseHelper(stocks[0], s);

                dh.LoadRecordsToDatabase();

            });


            Parallel.ForEach(alphabetT, s =>
            {
                DatabaseHelper test = new DatabaseHelper(stocks[0], s);

                test.testAddedRecord();

            });

          

        }
    }
}
