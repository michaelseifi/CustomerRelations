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
using DevExpress.Xpf.Bars;

using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core.Native;
using System.IO;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for UtilitiesMenu.xaml
    /// </summary>
    public partial class UtilitiesMenu : UserControl
    {

        public UtilitiesMenu()
        {

            InitializeComponent();
            BAR_MANGER.LayoutUpdated += BAR_MANGER_LayoutUpdated;
            this.Loaded += (s, e) =>
            {
                Window.GetWindow(this).Closing += (s1, e1) =>
                {
                    var path = Caching.GetFilePath(Caching.Folder.xml, "utilmenubars");
                    BAR_MANGER.SaveLayoutToXml(path);
                };
            };
        }





        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.myTopMenuViewModel;
        }



        void BAR_MANGER_LayoutUpdated(object sender, EventArgs e)
        {
            //if (SEARCH_BY_CUSTOMER_BAR_EDIT_ITEM_LINK.Editor != null)
            //{
            //    SEARCH_BY_CUSTOMER_BAR_EDIT_ITEM_LINK.Editor.KeyDown += Editor_KeyDown;
            //    BAR_MANGER.LayoutUpdated -= BAR_MANGER_LayoutUpdated;
            //}
        }



        private void STORE_COUNT_EditValueChanged(object sender, RoutedEventArgs e)
        {
            //var bar = (e.Source) as BarEditItem;
            //if (MainWindow.myUsers != null && MainWindow.myUsers.FocusedEmplid != null && MainWindow.myUsers.FocusedEmplid.CUSTOMERS != null)
            //{
            //    var mainCustFocused = (ICustomer)MainWindow.myUsers.FocusedEmplid.CUSTOMERS.FocusedCustomer;
            //    if (mainCustFocused != null)
            //        MainWindow.myTopMenuViewModel.STORE_COUNT = bar.EditValue.ToString();
            //}
        }

        private void AVG_PRICEPOINT_EditValueChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private bool isInFocus;
        void Editor_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            ////Debug.WriteLine("Editor has lost keyboard focus.");
            //var box = (TextEdit)sender;

            //if (e.NewFocus != null && e.NewFocus.IsKeyboardFocused && LayoutHelper.IsChildElementEx((DependencyObject)sender, (DependencyObject)e.NewFocus))
            //{

            //}
            //else if (e.OldFocus != null && !e.OldFocus.IsKeyboardFocused)
            //{
            //    //change the property
            //    MainWindow.myTopMenuViewModel.AVG_PRICE_POINT_DOLLAR = ((e.OldFocus) as TextBox).Text;
            //}
        }

        private void AVG_PRICEPOINT_BAR_EDIT_ITEM_LINK_LinkControlLoaded(object sender, BarItemLinkControlLoadedEventArgs e)
        {
            ((BarEditItemLink)e.Link).Editor.LostKeyboardFocus -= new System.Windows.Input.KeyboardFocusChangedEventHandler(Editor_LostKeyboardFocus);
            ((BarEditItemLink)e.Link).Editor.LostKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(Editor_LostKeyboardFocus);
        }








        private void BAR_MANGER_Loaded(object sender, RoutedEventArgs e)
        {
            var path = Caching.GetFilePath(Caching.Folder.xml, "utilmenubars");
            if (System.IO.File.Exists(path))
                BAR_MANGER.RestoreLayoutFromXml(path);
        }



        private void AVG_PRICEPOINT_LostFocus(object sender, RoutedEventArgs e)
        {
            BindingExpression be = AVG_PRICE_POINT_TEXT_BOX.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        }





    }
}
