using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daisybrand.forecaster.Controls;
using DevExpress.Xpf.Grid;

namespace daisybrand.forecaster.Controls.SubControls
{
    public class _GridControl_Quantity:_GridControl
    {
        public bool BESTFITCOLUMNS { get; set; }
        public System.Windows.Style ColumnCellStyle { get; set; }
        public _GridControl_Quantity()
            : base()
        {
            
            base.AutoGenerateColumns = DevExpress.Xpf.Grid.AutoGenerateColumnsMode.None;
           
            this.Loaded += (o, s) =>
            {
                if (BESTFITCOLUMNS)
                {
                    ((TableView)this.View).BestFitArea = BestFitArea.All;
                    ((TableView)this.View).BestFitColumns();                    
                }
            };


            
        }

        protected override void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ColumnCellStyle != null)
                foreach (GridColumn c in this.Columns)
                {
                    c.CellStyle = ColumnCellStyle;
                }
        }
        public _GridControl_Quantity(DevExpress.Xpf.Grid.Native.IDataControlOriginationElement dataControlOriginationElement)
            : base(dataControlOriginationElement)
        {
            
        }

        
    }
}
