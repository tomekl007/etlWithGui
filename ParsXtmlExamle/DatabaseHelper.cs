using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Configuration;

namespace ParsXtmlExamle
{
    class DatabaseHelper
    {
        String stockName;
        String letter;
        String separator = "-";
        string connectionString// =@"metadata=res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl;provider=System.Data.SqlServerCe.3.5;provider connection string=&quot;Data Source=|DataDirectory|\StocExchange.sdf&quot;";
            //= ConfigurationManager.ConnectionStrings["StocExchangeEntities2"].ConnectionString;
          ;
        String filePath;
       

        public DatabaseHelper(String stockName, String letter)
        {
            Console.WriteLine("DatabaseHelper constructor with : " + stockName + " " + letter);
            this.stockName = stockName;
            this.letter = letter;
            filePath = getFileName();
           // Console.WriteLine("conf menager : ");
           // Console.WriteLine( ConfigurationManager.AppSettings);
            connectionString = ConfigurationManager.ConnectionStrings["StocExchangeEntities2"].ConnectionString;
           
        }


        public DatabaseHelper(String stockName, String letter, String conString)
        {
            Console.WriteLine("DatabaseHelper constructor with : " + stockName + " " + letter);
            this.stockName = stockName;
            this.letter = letter;
            filePath = getFileName();
            // Console.WriteLine("conf menager : ");
            // Console.WriteLine( ConfigurationManager.AppSettings);
            connectionString = conString;
        }

        private string getFileName()
        {
            return stockName + separator + letter;
        }

        public void LoadCompaniesToDatabase(){


            Console.WriteLine("loading to db for lettter : " + letter);
            List<String> companiesToAdd = readAllSymbolsAndNames();
            
            ObjectContext context = new ObjectContext(connectionString);
            
            ObjectSet<Company> company = context.CreateObjectSet<Company>();
            Console.WriteLine("nr of records before persist : "+ company.Count() );

            for (int i = 0; i < companiesToAdd.Count; i+=2 )
            {
                Company c = new Company();
                c.Symbol = companiesToAdd[i];
                c.Name = companiesToAdd[i + 1];
                company.AddObject(c);
            }

            //Console.WriteLine(company.Count());
            //Company c = new Company();
           // c.Symbol = "CYST";
           // c.Name = "WAR";
         //   company.AddObject(c);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e) 
            {
                Console.WriteLine("--catched : "  + e.Message);
            }



            ObjectSet<Company> company2 = context.CreateObjectSet<Company>();
            Console.WriteLine("after insering number of records is : " + company2.Count());
            context.Dispose();
            
        }

        //return all symbols and names for all companies
        private List<String> readAllSymbolsAndNames()
        {
            List<String> symbolsAndNames = new List<String>();

            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                if (counter == 0 || counter == 1)
                {
                  //  Console.WriteLine("adding : " + line);
                    symbolsAndNames.Add(line);
                    
                    
                }
                
                //Console.WriteLine(line);

                counter++;
                if (counter == 9)
                {
                    counter = 0;
                }

            }

            file.Close();

            Console.WriteLine("----------------------------");
          //  foreach (String s in symbolsAndNames)
          //      Console.WriteLine(s);

            return symbolsAndNames;
           
           
        }
      


        public void LoadRecordsToDatabase()
        {
            List<String> recordsToAdd = readAllRecordsAndSymbol();
            ObjectContext context = new ObjectContext(connectionString);
            ObjectSet<Company> companies = context.CreateObjectSet<Company>();
           // Console.WriteLine("nr of records before persist : " + company.Count());
            
            ObjectSet<Record> records = context.CreateObjectSet<Record>();


            Console.WriteLine("nr of records before persist : " + records.Count());

            for (int i = 0; i < recordsToAdd.Count; i += 7)
            {
                Record r = new Record();
                

                String companySymbol = recordsToAdd[i];
                Company companyForRecord = companies.Single(c => c.Symbol == companySymbol);
                

                r.CompanySymbol = companySymbol;
                r.High = float.Parse(recordsToAdd[i+1]);
                r.Low = float.Parse(recordsToAdd[i + 2]);
                r.Close = float.Parse(recordsToAdd[i + 3]);
               // r.Volume = int.Parse(recordsToAdd[i + 4]);
                String volume = recordsToAdd[i + 4].Replace(",", "");
                
                r.Volume = int.Parse(volume);
                r.ChangeOne = float.Parse(recordsToAdd[i + 5]);
                r.ChangeTwo = float.Parse(recordsToAdd[i + 6]);
                //DateTime today = new DateTime();
               // var sqlFormattedDate = today.Date.ToString("yyyy-MM-dd HH:mm:ss");
                r.DateOfRecord = DateTime.Today;
                //c.Symbol = companiesToAdd[i];
               // c.Name = companiesToAdd[i + 1];
               // company.AddObject(c);
                //r.Id = (System.Guid)1;
              //  r.Id = 3;

                companyForRecord.Records.Add(r);
                records.AddObject(r);
            }

            //Console.WriteLine(company.Count());
            //Company c = new Company();
            // c.Symbol = "CYST";
            // c.Name = "WAR";
            //   company.AddObject(c);

            try
            {
            
                context.SaveChanges();
            }
           catch (Exception e)
            {
                Console.WriteLine("--catched : " + e.Message);
            }



            ObjectSet<Record> record2 = context.CreateObjectSet<Record>();
            Console.WriteLine("after insering number of records is : " + record2.Count());
            context.Dispose();



        }

        private List<string> readAllRecordsAndSymbol()
        {
            List<String> symbolAndRecordsData = new List<String>();

            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                
                if (counter !=1 && counter !=7)
                {
                      Console.WriteLine("adding : " + line);
                    if(!String.IsNullOrEmpty(line))
                        symbolAndRecordsData.Add(line);


                }

                //Console.WriteLine(line);

                counter++;
                if (counter == 9)
                {
                    counter = 0;
                }

            }

            file.Close();

            Console.WriteLine("----------------------------");
          //  foreach (String s in symbolAndRecordsData)
            //      Console.WriteLine(s);

            return symbolAndRecordsData;
        }

        internal void testAddedRecord()
        {
            
            ObjectContext context = new ObjectContext(connectionString);
            ObjectSet<Company> companies = context.CreateObjectSet<Company>();
            // Console.WriteLine("nr of records before persist : " + company.Count());

            ObjectSet<Record> records = context.CreateObjectSet<Record>();
            foreach(Company c in companies)
            {
                Console.WriteLine("company : " +  c.Name + " records :  "  );
                var recordsQ = records.Where(r => r.CompanySymbol == c.Symbol);
                                        
              
                foreach (var rec in recordsQ)
                    Console.WriteLine(rec.DateOfRecord + " " + rec.Volume);

             //   foreach (Record r in c.Records)
              //  {
               //     Console.WriteLine(r.DateOfRecord + " " + r.Volume);
              //  }

            }

        

           


          /* var query = from c in context.CreateObjectSet<Company>()
                        select
                        from r in c.Records
                        select new { r.DateOfRecord, r.Volume };
            foreach (var company in query)
                foreach (var record in company)
                    Console.WriteLine(record.DateOfRecord + " " + record.Volume);*/

        /*    foreach (Record r in records)
            {
                Console.WriteLine(r.Volume+ " for comapny "+ r.CompanySymbol + r.Company.Name );
            }
            */

        }
    }
}
