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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for SkuExceptions.xaml
    /// </summary>
    public partial class SkuExceptions : UserControl
    {
        public Window OWNER { get; set; }
        public SkuExceptions()
        {
            InitializeComponent();
            this.DataContext = MainWindow.myUsers;
        }

        private void INCLUDED_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox lbox = sender as ListBox;
            if (lbox != null)
            {
                var item = lbox.SelectedItem as Controlers.Objects.Sku;
                if (item != null)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        DataObject dObj = new DataObject("listboxitem", item);
                        DragDrop.DoDragDrop(lbox, dObj, DragDropEffects.Move);
                    }
                }
            }
        }

        private void INCLUDED_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("listboxitem") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void INCLUDED_DragOver(object sender, DragEventArgs e)
        {

        }

        private void INCLUDED_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void INCLUDED_Drop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData("listboxitem") as Controlers.Objects.Sku;
            if (!INCLUDED.Items.Contains(item))
            {
                var cust = MainWindow.myUsers.FocusedEmplid.CUSTOMERS.Where(x => int.Parse(x.ACCOUNTNUM).ToString() == ((Controlers.Interfaces.ISku)item).SHIPTO).FirstOrDefault();
                var skusExcept = cust.SKUS_EXCEPTIONS;
                skusExcept.RemoveFromExceptions(item);
                skusExcept.Remove(item);
                var skus = cust.SKUS;
                skus.Add(item);
                skus = Controlers.Collections.SkuCollection.OrderByDisplayName(skus);
                //MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SKUS = Controlers.Collections.SkuCollection.OrderByDisplayName(MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SKUS);
            }
        }

        private void EXCLUDED_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox lbox = sender as ListBox;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (lbox != null)
                {
                    var item = lbox.SelectedItem as Controlers.Objects.Sku;
                    if (item != null)
                    {
                        DataObject dObj = new DataObject("listboxitem", item);
                        DragDrop.DoDragDrop(lbox, dObj, DragDropEffects.Move);
                    }
                }
            }
        }


        private void EXCLUDED_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("listboxitem") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void EXCLUDED_DragOver(object sender, DragEventArgs e)
        {

        }

        private void EXCLUDED_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void EXCLUDED_Drop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData("listboxitem") as Controlers.Objects.Sku;
            if (!EXCLUDED.Items.Contains(item))
            {
                var cust = MainWindow.myUsers.FocusedEmplid.CUSTOMERS.Where(x => int.Parse(x.ACCOUNTNUM).ToString() == ((Controlers.Interfaces.ISku)item).SHIPTO).FirstOrDefault();
                var skusExcept = cust.SKUS_EXCEPTIONS;
                var skus = cust.SKUS;
                skusExcept.AddToExceptions(item); 
                skus.Remove(item);                
                skusExcept.Add(item);                
            }
        }

        private void CUSTOMERS_COMBO_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null)
                CUSTOMERS_COMBO.SelectedItem = MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER;
        }

        private void CUSTOMERS_COMBO_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void CLOSE_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (OWNER != null) OWNER.Close();
        }

  


    }
}
