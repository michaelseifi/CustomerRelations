using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
namespace daisybrand.forecaster
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : Window
    {
        public test()
        {
            InitializeComponent();

            content.Content = new Presentation.UserControls.Holidays();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //IDataObject data = Clipboard.GetDataObject();
            //if (data.GetDataPresent(DataFormats.CommaSeparatedValue))
            //{
            //    var s = data.GetData(DataFormats.CommaSeparatedValue).ToString();
            //    string[] lines = Regex.Split(s, "\r\n");
            //    foreach (var line in lines)
            //    {
            //        textbox1.Text += line + "-new line\r";
            //    }
            //}
        }
    }
}
