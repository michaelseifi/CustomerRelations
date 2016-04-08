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
using daisybrand.forecaster.Controlers.Interfaces;


namespace daisybrand.forecaster.Presentation.Views
{
    /// <summary>
    /// Interaction logic for CellComment.xaml
    /// </summary>
    public partial class CellComment : Window
    {
        ITab TAB { get; set; }
        public string COMMENT { get; set; }
        public Controlers.Objects.CellComment.Field FIELD { get; set; }
        public CellComment(ITab tab)
        {
            TAB = tab;
            if (TAB.myWeeklyData.FocusedDataCollection == null || TAB.myWeeklyData.FocusedDataCollection.Count() > 1)
            {
                MessageBox.Show(Application.Current.MainWindow, "Please select one row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            InitializeComponent();
            this.Loaded += (s, a) =>
            {
                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
                if (this.Top > screen.Bounds.Height - this.Height)
                    this.Top = screen.Bounds.Height - this.Height - 10;
                TITLE.Text = FIELD + " comment";
            };
            this.TEXTBOX.Loaded += (s, a) =>
            {
                TEXTBOX.Text = COMMENT;
            };            
        }

        
        private void CLOSE_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SAVE_Click(object sender, RoutedEventArgs e)
        {
            IDailyData data = TAB.myWeeklyData.Where(x => x.WEEK_ID == TAB.myWeeklyData.FocusedData.WEEK_ID).FirstOrDefault();
            int result = 0;
            switch (FIELD)
            {
                case Controlers.Objects.CellComment.Field.QS:
                case Controlers.Objects.CellComment.Field.QA:
                case Controlers.Objects.CellComment.Field.QO:
                case Controlers.Objects.CellComment.Field.QW:
                case Controlers.Objects.CellComment.Field.LY:
                case Controlers.Objects.CellComment.Field.FORECAST:
                case Controlers.Objects.CellComment.Field.BASE_LINE:
                case Controlers.Objects.CellComment.Field.ORDER:
                    result = daisybrand.forecaster.Controlers.Objects.CellComment.InsertOrUpdate(TAB.myWeeklyData.FocusedData, FIELD, TEXTBOX.Text);
                    break;
                case Controlers.Objects.CellComment.Field.QC:
                case Controlers.Objects.CellComment.Field.QD:
                    result = daisybrand.forecaster.Controlers.Objects.QcQd.InsertOrUpdateComment(TAB.myWeeklyData.FocusedData, TEXTBOX.Text, FIELD.ToString());
                    break;
                default:
                    break;
            }
            
            if (result > 0)
            {
                switch (FIELD)
                {
                    case Controlers.Objects.CellComment.Field.QS:
                        TAB.myWeeklyData.FocusedData.QS_COMMENT.VALUE = data.QS_COMMENT.VALUE = TEXTBOX.Text;
                        break;
                    case Controlers.Objects.CellComment.Field.QA:
                        TAB.myWeeklyData.FocusedData.QA_COMMENT.VALUE = data.QA_COMMENT.VALUE = TEXTBOX.Text;
                        break;
                    case Controlers.Objects.CellComment.Field.QO:
                        TAB.myWeeklyData.FocusedData.QO_COMMENT.VALUE = data.QO_COMMENT.VALUE = TEXTBOX.Text;
                        break;
                    case Controlers.Objects.CellComment.Field.QW:
                        TAB.myWeeklyData.FocusedData.QW_COMMENT.VALUE = data.QW_COMMENT.VALUE = TEXTBOX.Text;
                        break;
                    case Controlers.Objects.CellComment.Field.LY:
                        TAB.myWeeklyData.FocusedData.LY_COMMENT.VALUE = data.LY_COMMENT.VALUE = TEXTBOX.Text;
                        break;
                    case Controlers.Objects.CellComment.Field.ORDER:
                        TAB.myWeeklyData.FocusedData.ORDER_DELIVERY_COMMENT.VALUE = data.ORDER_DELIVERY_COMMENT.VALUE = TEXTBOX.Text;
                        break;
                    case Controlers.Objects.CellComment.Field.QC:
                        TAB.myWeeklyData.FocusedData.QC.COMMENT = data.QC.COMMENT = TEXTBOX.Text;
                        break;
                    case Controlers.Objects.CellComment.Field.QD:
                        TAB.myWeeklyData.FocusedData.QD.COMMENT = data.QD.COMMENT = TEXTBOX.Text;
                        break;
                    case Controlers.Objects.CellComment.Field.FORECAST:
                        data.FORECASTS.COMMENT.VALUE = TAB.myWeeklyData.FocusedData.FORECASTS.SetCommentValue(TEXTBOX.Text);
                        break;
                    case Controlers.Objects.CellComment.Field.BASE_LINE:
                        data.BASE_LINE.COMMENT.VALUE = TAB.myWeeklyData.FocusedData.BASE_LINE.COMMENT.VALUE = TEXTBOX.Text;
                        break;
                    default:
                        break;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "An error occurred while trying to save your comment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        } 
    }
}
