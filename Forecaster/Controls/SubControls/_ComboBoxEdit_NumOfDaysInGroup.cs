using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors;
namespace daisybrand.forecaster.Controls.SubControls
{
    public class _ComboBoxEdit_NumOfDaysInGroup : _ComboBoxEdit
    {
        public _ComboBoxEdit_NumOfDaysInGroup()
            : base()
        {
            base.IsTextEditable = false;
        }
    }
}
