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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Button _button = new Button { Content = "Go" };
      //  TextBlock _results = new TextBlock();
        ProgramFacade programFacade;
        String currentStock;
        readonly String connectionString;

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

        
        private void nameOfStockToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

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

        private void bw_DoWorkDownloadData(object sender, DoWorkEventArgs e)
        {
            programFacade.extractDataFromUrl(currentStock);
        }

        private void bw_RunWorkerDownloadDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("pobieranie zakonczone");
        }


       // private Task<int> extractAsynch()
       // {
       //     return 0;
        //}

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


        private void bw_DoWorkSaveToDb(object sender, DoWorkEventArgs e)
        {
            programFacade.loadAllDataToDb(currentStock);
        }

        private void bw_RunWorkerSaveToDbCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("zapisywanie do bazy danych zakonczone");
        }


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
        private void bw_DoWorkDownloadAndSaveAll(object sender, DoWorkEventArgs e)
        {
            programFacade.extractAndPutToDbAllData();
        }

        private void bw_RunWorkerDownloadAndSaveAllCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("pobieranie i zapisywanie do bazy zakonczone");
        }

        

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
                drawChart2(results);
            if(howManyResults==0)
                MessageBox.Show("NIe znaleziono danych dla podanej spolki");
            

        }

        private void drawChart()
        {
           
            // Prepare data in arrays
			const int N = 1000;
			double[] x = new double[N];
			double[] y = new double[N];
           


			for (int i = 0; i < N; i++)

			{
				x[i] = i * 0.1;
				y[i] = Math.Sin(x[i]);
			}

			// Create data sources:
			var xDataSource = x.AsXDataSource();
            //var xDataSource = dt.AsDataSource() ;
			var yDataSource = y.AsYDataSource();

			CompositeDataSource compositeDataSource = xDataSource.Join(yDataSource);
			// adding graph to plotter
			plotter.AddLineGraph(compositeDataSource,
				Colors.Goldenrod,
				3,
				"Sine");

			// Force evertyhing plotted to be visible
			plotter.FitToView();
        }

        public void drawChart2(List<List<Record>> results)
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

        private int counter = 0;
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

        private void clearChart_Click(object sender, RoutedEventArgs e)
        {

            plotter.Children.RemoveAll(typeof(LineGraph)); 
        }
    }
}
