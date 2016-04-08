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
    /// Interaction logic for CustomerExceptions.xaml
    /// </summary>
    public partial class CustomerExceptions : UserControl
    {
        public Window OWNER { get; set; }
        public CustomerExceptions()
        {
            InitializeComponent();
            this.DataContext = MainWindow.myUsers;
        }

        private void FOREWARD_BTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BACKWARD_BTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void INCLUDED_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox lbox = sender as ListBox;
            if (lbox != null)
            {
                var item = lbox.SelectedItem as Controlers.Objects.Customer;
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
            var item = e.Data.GetData("listboxitem") as Controlers.Objects.Customer;
            if (!INCLUDED.Items.Contains(item))
            {
                //EXCLUDED.Items.Remove(item);
                //INCLUDED.Items.Add(item);
                MainWindow.myUsers.FocusedEmplid.CUSTOMER_EXCEPTIONS.Remove(item);
                MainWindow.myUsers.FocusedEmplid.CUSTOMERS.Add(item);
                MainWindow.myUsers.FocusedEmplid.CUSTOMER_EXCEPTIONS.RemoveFromExceptions(int.Parse(((Controlers.Interfaces.ICustomer)item).ACCOUNTNUM));
            }
        }

        private void EXCLUDED_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox lbox = sender as ListBox;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (lbox != null)
                {
                    var item = lbox.SelectedItem as Controlers.Objects.Customer;
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
            var item = e.Data.GetData("listboxitem") as Controlers.Objects.Customer;
            if (!EXCLUDED.Items.Contains(item))
            {
                //INCLUDED.Items.Remove(item);
                //EXCLUDED.Items.Add(item);
                MainWindow.myUsers.FocusedEmplid.CUSTOMERS.Remove(item);
                MainWindow.myUsers.FocusedEmplid.CUSTOMER_EXCEPTIONS.Add(item);
                MainWindow.myUsers.FocusedEmplid.CUSTOMER_EXCEPTIONS.AddToExceptions(int.Parse(((Controlers.Interfaces.ICustomer)item).ACCOUNTNUM));
            }
        }

        private void CLOSE_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (OWNER != null) OWNER.Close();
        }

        
        protected void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.IsSelected = true;
        }

        private void ListBoxItem_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.IsSelected = true;
        }

    }
}
