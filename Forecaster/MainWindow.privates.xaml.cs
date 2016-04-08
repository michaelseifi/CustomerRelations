using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Interfaces;
using Helpers = daisybrand.forecaster.Helpers;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
namespace daisybrand.forecaster
{
    public partial class MainWindow
    {
        bool normalizeIfMove = false;
        private void InitiateEventHandlers()
        {
            MAIN_MENU.ExitClick += TOP_MENU_ExitClick;
            MAIN_MENU.MaximizeScreenClick += TOP_MENU_MaximizeScreenClick;
            MAIN_MENU.NormalizeScreenClick += TOP_MENU_NormalizeScreenClick;
            MAIN_MENU.FullScreenClick += TOP_MENU_FullScreenClick;
            //MAIN_MENU.MouseDown += (s, e) =>
            //{
            //    _OnDragMove(e);
            //};

            TOP_MENU.ExitClick += TOP_MENU_ExitClick;
            TOP_MENU.MouseDoubleClick += TOP_MENU_MouseDoubleClick;
            TOP_MENU.MaximizeScreenClick += TOP_MENU_MaximizeScreenClick;
            TOP_MENU.NormalizeScreenClick += TOP_MENU_NormalizeScreenClick;
            TOP_MENU.FullScreenClick += TOP_MENU_FullScreenClick;
            TOP_MENU.DragMoveWindow += TOP_MENU_DragMoveWindow;
            //TOP_MENU.MouseDown += (s, e) =>
            //{
            //    _OnDragMove(e);
            //};

            //UTILITIES_MENU.MouseDown += (s, e) =>
            //{
            //    _OnDragMove(e);
            //};
            this.MouseLeftButtonUp += (s, e) =>
            {
                normalizeIfMove = false;
            };
            this.MouseMove += (s, e) =>
            {
                if (normalizeIfMove)
                    this._DefaultScreen();
            };
        }

        void TOP_MENU_DragMoveWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _OnDragMove(e);
        }


        private void _OnDragMove(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {

            }
            else
            {
                DragMove();

                if (myMainWindowViewModel.WINDOWS_STATE != Controlers.ViewModels.MainWindow.WindowsState.Normal)
                    normalizeIfMove = true;
            }
           
        }


        /// <summary>
        /// GET A LIST OF USER'S CUSTOMERS
        /// RETURNS TRUE IF SUCCESSFULL
        /// </summary>
        /// <returns></returns>
        //private static bool GetCustomers()
        //{
        //    myCustomers = CustomerCollection.Get(myIdentity.NetWorkAlias);
        //    if (myCustomers == null) return false;
        //    return true;
        //}

        private static bool _GetMyUsers(SkuCollection skus)
        {
            myUsers = UserCollection.Get(skus);
            if (myUsers == null) return false;
            return true;
        }

        /// <summary>
        /// GET A LIST OF ALL SKU IDS AVAILABLE
        /// RETURNS TRUE IF SUCCESSFULL
        /// </summary>
        /// <returns></returns>
        private static bool _GetMySkus()
        {
            mySkus = SkuCollection.Get();
            if (mySkus == null) return false;
            return true;
        }

        private void _DefaultScreen()
        {
            var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).WorkingArea;
            //this.Left = screen.Left + 40;
            //this.Top = screen.Top + 30;
            this.Width = screen.Width - 150;
            this.Height = screen.Height - 80;
            myMainWindowViewModel.WINDOWS_STATE = Controlers.ViewModels.MainWindow.WindowsState.Normal;
        }

        private void _MaximizeScreen()
        {
            if (myMainWindowViewModel.WINDOWS_STATE != Controlers.ViewModels.MainWindow.WindowsState.Maximized)
            {
                //this.Topmost = false;  
                var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).WorkingArea;
                this.Left = screen.Left;
                this.Top = screen.Top;
                this.Width = screen.Width;
                this.Height = screen.Height;
                myMainWindowViewModel.WINDOWS_STATE = Controlers.ViewModels.MainWindow.WindowsState.Maximized;
            }
            else
                _NormalizeScreen();
        }

        public  void NormalizeScreen()
        {
            _NormalizeScreen();
        }
        private  void _NormalizeScreen()
        {
            //if (WindowState == System.Windows.WindowState.Maximized)
                WindowState = System.Windows.WindowState.Normal;
            var workingArea = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).WorkingArea;
            
            
            this.Width = MainWindow.myMainWindowViewModel.ORIG_WIDTH -
                (System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).Bounds.Width - workingArea.Width);
            this.Left = ((workingArea.Width - this.Width) / 2) + workingArea.Left;
            this.Height = MainWindow.myMainWindowViewModel.ORIG_HEIGHT -
                 (System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).Bounds.Height - workingArea.Height);
            this.Top = ((workingArea.Height - this.Height) / 2);
            myMainWindowViewModel.WINDOWS_STATE = Controlers.ViewModels.MainWindow.WindowsState.Normal;
        }

        private void _FullScreen()
        {
            //this.Topmost = true;
            WindowState = System.Windows.WindowState.Maximized;
            myMainWindowViewModel.WINDOWS_STATE = Controlers.ViewModels.MainWindow.WindowsState.Maximized;
            this.Top = 0;
            this.Left = 0;
        }

        public  void MinimizeScreen()
        {
            //this.Topmost = false;
            WindowState = System.Windows.WindowState.Minimized;
            myMainWindowViewModel.WINDOWS_STATE = Controlers.ViewModels.MainWindow.WindowsState.Minimized;
        }
    }
}
