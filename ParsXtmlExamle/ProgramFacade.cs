using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsXtmlExamle
{
    /// <summary>
    /// this class serve as external interface for GUI
    /// </summary>
    public class ProgramFacade
    {
        /// <summary>
        /// list of all letters in alphabet
        /// </summary>
        private static List<string> alphabet = new List<string>();
        /// <summary>
        /// list of all avalible stocks
        /// </summary>
        private static List<string> stocks = new List<string>();
        /// <summary>
        /// baseUrl for data to download
        /// </summary>
        private static readonly String baseUrl  // * is placeholder to replace by stock and ^ for letter
            = "http://www.findata.co.nz/markets/*/symbols/^";

        /// <summary>
        /// sublist of alphabet (A,B,C)
        /// </summary>
        List<string> alphabetT;
        /// <summary>
        /// connection String to database
        /// </summary>
        private String connectionString;

        /// <summary>
        /// constructor which fills alphabet, alphabeT - is alphabet from A to Z
        /// and stocks 
        /// </summary>
        /// <param name="conString">connectionString to dataBase</param>
        public ProgramFacade(String conString)
        {
            //initialization 
            for (char ch = 'A'; ch <= 'Z'; ch++){
                alphabet.Add(ch.ToString());
            }

            alphabetT = alphabet.GetRange(0, 3);


            stocks.Add("NYSE");
            stocks.Add("NASDAQ");
            stocks.Add("AMEX");
            connectionString = conString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of all avalible Stocks to search </returns>
        public List<String> getAvalibleStocks()
        {
            return stocks;
        }

        /// <summary>
        /// create instance of Extractor for each letter in 
        /// alphabet list and extract data in parallel
        /// </summary>
        /// <param name="stock">stocks for which download and extract data</param>
        public void extractDataFromUrl(String stock)
        {
            Parallel.ForEach(alphabetT, s =>
            {
                Extractor ex = new Extractor(baseUrl, stock, s, ".htm");

                ex.Extract();

            });
        }

        /// <summary>
        /// invoke loadAllCompaniesToDb, next invoke loadAllRecordsToDb
        /// 
        /// </summary>
        /// <param name="stock">stocks for which load data to database</param>
        public void loadAllDataToDb(String stock)
        {
            Console.WriteLine("load all Data to Db");
            loadAllCompaniesToDb(stock);
            loadAllRecordsToDb(stock);
        }

        /// <summary>
        /// load all copamies from earlier extracted data to database
        /// </summary>
        /// <param name="stock">stocks for which load data to database<</param>
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

        /// <summary>
        /// load all records for copamies
        /// from earlier extracted data to database
        /// </summary>
        /// <param name="stock">stocks for which load data to database</param>
        public void loadAllRecordsToDb(String stock)
        {
            Parallel.ForEach(alphabetT, s =>
            {
                DatabaseHelper dh = new DatabaseHelper(stock, s);

                dh.LoadRecordsToDatabase();

            });
        }


        
       /// <summary>
       /// extract and load data to database for all avalible stocks :
       /// NYSE, NASDAQ, AMEX
       /// </summary>

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


        public List<Record> getRecordsForSpecyficCompany(string nameOfStockToSearch)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.getRecordsForCompany(nameOfStockToSearch);
        }
    }
}
