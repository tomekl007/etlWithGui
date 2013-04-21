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

            programFacade.extractDataFromUrl(currentStock);
            MessageBox.Show("pobieranie zakonczone");

        }

        private void saveDataToDb_Click(object sender, RoutedEventArgs e)
        {

            currentStock = nameOfStockToSearch.Text;
            // MessageBoxResult result =
            if (!programFacade.getAvalibleStocks().Contains(currentStock))
            {
                MessageBox.Show("wpisales nazwe gieldy, która nie istnieje!");
                return;
            }

            programFacade.loadAllDataToDb(currentStock);
            MessageBox.Show("zapisywanie do bazy danych zakonczone");

        }

        private void downloadAndSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ta operacja moze trwac bardzo dlugo! Kontynuowac ? ",
            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                programFacade.extractAndPutToDbAllData();
            }
            else
            {
                return;
            } 
            

        }

        private void getRecordsForStock_Click(object sender, RoutedEventArgs e)
        {
            String nameOfStockToSearch = searchRecordsForStock.Text;
            Console.WriteLine(nameOfStockToSearch);
            //TO DO : get data from db and bind to grid view

        }
    }
}
