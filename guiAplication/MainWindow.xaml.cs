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
            foreach (List<Record> re in results)
            {
                foreach (Record r in re)
                {
                    constructed += r.CompanySymbol + separator + String.Format("{0:0.##}", r.High) + separator
                                + String.Format("{0:0.##}", r.Low) + separator + String.Format("{0:0.##}", r.Close)
                                + separator + r.Volume + separator
                                + String.Format("{0:0.##}", r.ChangeOne) + separator
                                + r.DateOfRecord.ToShortDateString() + newLineSeparator;

                }
            }

            dataResult.Text = constructed;

            

        }
    }
}
