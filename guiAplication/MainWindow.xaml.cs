using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParsXtmlExamle;
using System.Configuration;

using System.Data.Objects;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;

using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;


namespace guiAplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml ( GUI )
    /// </summary>
    public partial class MainWindow : Window
    {
        //Button _button = new Button { Content = "Go" };
      //  TextBlock _results = new TextBlock();
       /// <summary>
       /// interface used to connect with logic project
       /// </summary>
        ProgramFacade programFacade;
        /// <summary>
        /// stock currenly typed by user in GUI
        /// </summary>
        String currentStock;
        /// <summary>
        /// connection string to database
        /// </summary>
        readonly String connectionString;

        /// <summary>
        /// default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
          //  var panel = new StackPanel();
           // panel.Children.Add(_button);
           // panel.Children.Add(_results);
           // Content = panel;
            connectionString = ConfigurationManager.ConnectionStrings["StocExchangeEntities2"].ConnectionString;
            programFacade = new ProgramFacade(connectionString);
        }

        /// <summary>
        /// listener for textChange in nameOfStockToSearch textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameOfStockToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        /// <summary>
        /// listener for downloadDataFromWeb button
        /// when clicked by user, first validate user input in nameOfStockToSearch textBox
        /// next starting backgroundWorker which will download and extract data from web
        /// for earlier given stock
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void downloadDataFromWeb_Click(object sender, RoutedEventArgs e)
        {
            currentStock = nameOfStockToSearch.Text;
           // MessageBoxResult result =
            if (!programFacade.getAvalibleStocks().Contains(currentStock))
            {   
                MessageBox.Show("wpisales nazwe gieldy, która nie istnieje!");
                return;
            }


            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = false;
            worker.DoWork += new DoWorkEventHandler(bw_DoWorkDownloadData);
            //worker.RunWorkerCompleted += WorkerEnded;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerDownloadDataCompleted);
            worker.RunWorkerAsync();
         
           
           
           

        }
        /// <summary>
        /// downlaoding data in background thread
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void bw_DoWorkDownloadData(object sender, DoWorkEventArgs e)
        {
            programFacade.extractDataFromUrl(currentStock);
        }

        /// <summary>
        /// invoking when data will be downloaded from web
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void bw_RunWorkerDownloadDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("pobieranie zakonczone");
        }


       // private Task<int> extractAsynch()
       // {
       //     return 0;
        //}

        /// <summary>
        /// listener for saveDataToDb button, when clicked by user
        /// it save earlier downloaded data to db in background thread
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void saveDataToDb_Click(object sender, RoutedEventArgs e)
        {

            currentStock = nameOfStockToSearch.Text;
            // MessageBoxResult result =
            if (!programFacade.getAvalibleStocks().Contains(currentStock))
            {
                MessageBox.Show("wpisales nazwe gieldy, która nie istnieje!");
                return;
            }



            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = false;
            worker.DoWork += new DoWorkEventHandler(bw_DoWorkSaveToDb);
            //worker.RunWorkerCompleted += WorkerEnded;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerSaveToDbCompleted);
            worker.RunWorkerAsync();
         

           
          

        }

        /// <summary>
        /// saving data to db in backgroud thread
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void bw_DoWorkSaveToDb(object sender, DoWorkEventArgs e)
        {
            programFacade.loadAllDataToDb(currentStock);
        }



        /// <summary>
        /// invoking when data will be saved to db
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void bw_RunWorkerSaveToDbCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("zapisywanie do bazy danych zakonczone");
        }



        /// <summary>
        /// listener for downloadAndSaveAll button, when clicked
        /// perform downloading, extracting, and loading data to db
        /// for all avalible stocks
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void downloadAndSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ta operacja moze trwac bardzo dlugo! Kontynuowac ? ",
            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.WorkerSupportsCancellation = false;
                    worker.DoWork += new DoWorkEventHandler(bw_DoWorkDownloadAndSaveAll);
                     //worker.RunWorkerCompleted += WorkerEnded;
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerDownloadAndSaveAllCompleted);
                    worker.RunWorkerAsync();


            }
            else
            {
                return;
            }
            

        }

        /// <summary>
        /// downlaoding, extracting, and saving data to db in backgroud thread
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void bw_DoWorkDownloadAndSaveAll(object sender, DoWorkEventArgs e)
        {
            programFacade.extractAndPutToDbAllData();
        }


        /// <summary>
        /// invoking when downlaoding, extracting, and saving data to db will be completed
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void bw_RunWorkerDownloadAndSaveAllCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("pobieranie i zapisywanie do bazy zakonczone");
        }


        /// <summary>
        /// listener for getRecordsForStock
        /// searching data in database for stock symbol typed by user in 
        /// searchRecordsForStock textBox. 
        /// then invoke drawChart
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void getRecordsForStock_Click(object sender, RoutedEventArgs e)
        {
            String nameOfStockToSearch = searchRecordsForStock.Text;
            char[] delimeter = {','};
            List<String> namesOfStocksToSearch = new List<string>();

            foreach (String s in nameOfStockToSearch.Split(delimeter))
                namesOfStocksToSearch.Add(s);


            //Console.WriteLine(nameOfStockToSearch);
           List<List<Record>> results=new List<List<Record>>();

            foreach(String st in namesOfStocksToSearch)
             results.Add( programFacade.getRecordsForSpecyficCompany(st) );
           // dataGrid1.ItemsSource = results;
            //dataGrid1.DataContext = results;
           
            List<String>symbols = new List<string>();
            foreach (List<Record> re in results){
                foreach(Record r in re)
                {
                Console.WriteLine(r.CompanySymbol + " " + r.Low);
                symbols.Add(r.CompanySymbol);
                }
            }

           /* DataGridTextColumn symbolRow = new DataGridTextColumn();
            symbolRow.Header = "symbol";
            dataGrid1.Columns.Add(symbolRow);

            DataGridTextColumn highRow = new DataGridTextColumn();
            highRow.Header = "high";
            dataGrid1.Columns.Add(highRow);
            **/

           // dataGrid1.ItemsSource = symbols;
            //dataGrid1.ItemsSource = results;
            

           // dataGrid1.Items.Add("asdsa");
           
            //dataGrid1.ItemsSource = results;
            String separator = "          " ;
            String newLineSeparator = " \n";
            String constructed = "";
            //String.Format("{0:0.##}", );      // "123.46"
            int howManyResults = 0;
            foreach (List<Record> re in results)
            {
                
                foreach (Record r in re)
                {
                    constructed += r.CompanySymbol + separator + String.Format("{0:0.##}", r.High) + separator
                                + String.Format("{0:0.##}", r.Low) + separator + String.Format("{0:0.##}", r.Close)
                                + separator + r.Volume + separator
                                + String.Format("{0:0.##}", r.ChangeOne) + separator
                                + r.DateOfRecord.ToShortDateString() + newLineSeparator;
                    howManyResults++;

                }
            }

            Console.WriteLine("res count : " + results.Count() + " " + howManyResults);

            dataResult.Text = constructed;


            if(howManyResults>1)
                drawChart(results);
            if(howManyResults==0)
                MessageBox.Show("NIe znaleziono danych dla podanej spolki");
            

        }
       
        
        /// <summary>
        /// drawing chart for list of lists of records
        /// </summary>
        /// <param name="results">list of list of records</param>
        public void drawChart(List<List<Record>> results)
        {
            
            
            List<DateTime> dates = new List<DateTime>();
           
            

            List<Double> nr = new List<Double>();
          
            String nameOfStock="";
           // int i = 0;
            foreach (List<Record> re in results)
            {
                foreach (Record r in re)
                {
                    dates.Add(r.DateOfRecord);
                    Double close = (Double)r.Close;
                    nr.Add(close);
                    nameOfStock = r.CompanySymbol;

                }
            }

            var datesDataSource = new EnumerableDataSource<DateTime>(dates);
            datesDataSource.SetXMapping(x => dateAxis.ConvertToDouble(x));

            var numberOpenDataSource = new EnumerableDataSource<Double>(nr);
            numberOpenDataSource.SetYMapping(y => y);
            

            CompositeDataSource compositeDataSource = new
        CompositeDataSource(datesDataSource, numberOpenDataSource);

            Console.WriteLine("nOfS : " + nameOfStock);
            
            if (!string.IsNullOrEmpty(nameOfStock))
            {

                plotter.AddLineGraph(compositeDataSource,
                    getRandomColor(),
                    3,
                    nameOfStock);
           

            // Force evertyhing plotted to be visible
            plotter.FitToView();
            }

            Console.WriteLine("plotterChild count : " + plotter.Children.Count());
            
            
        }


        /// <summary>
        /// counter used in random color generator
        /// </summary>
        private int counter = 0;
        /// <summary>
        /// getting random color used by drawChart
        /// </summary>
        /// <returns>random Color</returns>
        private Color getRandomColor()
        {
           
            Color[] colors = new Color[5];
            colors[0]=Colors.Brown;
            colors[1] = Colors.Blue ;
            colors[2]=Colors.Black;
            colors[3]=Colors.Yellow;
            colors[4]=Colors.Green;
            counter++;
            if (counter >= 5)
            {
                counter = 0;
            }
            
            return colors[counter];
            


        }

        /// <summary>
        /// listener for claerChart button
        ///clear chart for earlier drawn data
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event object</param>
        private void clearChart_Click(object sender, RoutedEventArgs e)
        {

            plotter.Children.RemoveAll(typeof(LineGraph)); 
        }
    }
}
