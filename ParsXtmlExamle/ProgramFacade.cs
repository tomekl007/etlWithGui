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

        public ProgramFacade()
        {
            //initialization 
            for (char ch = 'A'; ch <= 'Z'; ch++){
                alphabet.Add(ch.ToString());
            }

            alphabetT = alphabet.GetRange(3, 4);


            stocks.Add("NYSE");
            stocks.Add("NASDAQ");
            stocks.Add("AMEX");
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

        public void loadAllCompaniesToDb(String stock)
        {
            Parallel.ForEach(alphabetT, s =>
            {
                DatabaseHelper dh = new DatabaseHelper(stock, s);

                dh.LoadCompaniesToDatabase();

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


    }
}
