using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsXtmlExamle
{
    public class ProgramFacade
    {
        private static List<string> alphabet = new List<string>();
        private static List<string> stocks = new List<string>();
        private static readonly String baseUrl  // * is placeholder to replace by stock and ^ for letter
            = "http://www.findata.co.nz/markets/*/symbols/^";

        List<string> alphabetT;
        private String connectionString;

        public ProgramFacade(String conString)
        {
            //initialization 
            for (char ch = 'A'; ch <= 'Z'; ch++){
                alphabet.Add(ch.ToString());
            }

            alphabetT = alphabet.GetRange(3, 4);


            stocks.Add("NYSE");
            stocks.Add("NASDAQ");
            stocks.Add("AMEX");
            connectionString = conString;
        }

        public List<String> getAvalibleStocks()
        {
            return stocks;
        }

        public void extractDataFromUrl(String stock)
        {
            Parallel.ForEach(alphabetT, s =>
            {
                Extractor ex = new Extractor(baseUrl, stock, s, ".htm");

                ex.Extract();

            });
        }

        public void loadAllDataToDb(String stock)
        {
            Console.WriteLine("load all Data to Db");
            loadAllCompaniesToDb(stock);
            loadAllRecordsToDb(stock);
        }

        public void loadAllCompaniesToDb(String stock)
        {
            Parallel.ForEach(alphabetT, s =>
            {
                try
                {
                    DatabaseHelper dh = new DatabaseHelper(stock, s);
                    dh.LoadCompaniesToDatabase();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace + ex.Data);
                }
                

            });
        
        }

        public void loadAllRecordsToDb(String stock)
        {
            Parallel.ForEach(alphabetT, s =>
            {
                DatabaseHelper dh = new DatabaseHelper(stock, s);

                dh.LoadRecordsToDatabase();

            });
        }

        public void testDb(String stock)
        {
            Parallel.ForEach(alphabetT, s =>
            {
                DatabaseHelper test = new DatabaseHelper(stock, s);

                test.testAddedRecord();

            });
        }

        public void extractAndPutToDbAllData()
        {
            foreach (String stock in stocks)
            {
                extractDataFromUrl(stock);
            }

            foreach (String stock in stocks)
            {
                loadAllCompaniesToDb(stock);
            }

            foreach (String stock in stocks)
            {
                loadAllRecordsToDb(stock);
            }
        }

    }
}
