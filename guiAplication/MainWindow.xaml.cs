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

        public MainWindow()
        {
            InitializeComponent();
            programFacade = new ProgramFacade();
          //  var panel = new StackPanel();
           // panel.Children.Add(_button);
           // panel.Children.Add(_results);
           // Content = panel;
            
          
        }

        
        private void nameOfStockToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void downloadDataFromWeb_Click(object sender, RoutedEventArgs e)
        {
            String stock = nameOfStockToSearch.Text;
           // MessageBoxResult result =
            if (!programFacade.getAvalibleStocks().Contains(stock))
            {   
                MessageBox.Show("wpisales nazwe gieldy, która nie istnieje!");
                return;
            }

            programFacade.extractDataFromUrl(stock);

        }

        private void saveDataToDb_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
