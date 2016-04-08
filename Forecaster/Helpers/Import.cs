using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetGear;
using SpreadsheetGear.Data;
using System.Data;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Helpers;
namespace daisybrand.forecaster.Helpers
{
    public  class Import
    {
        public static  IEnumerable<ImportProperties> GetWorkbook(string fileName)
        {
            var w = SpreadsheetGear.Factory.GetWorkbook(Caching.GetExcelFilePath(fileName));
            var dataSet = w.GetDataSet(SpreadsheetGear.Data.GetDataFlags.FormattedText);
            var dataTable = dataSet.Tables[0];
            var list = new List<ImportProperties>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(new ImportProperties
                {
                    DATE = row["Date"].ToDate(),
                    COMMENT = row["COMMENT"].ToString(),                   
                    EVENT = row["EVENT"].ToString(),
                    FORECAST = int.Parse(row["FORECAST"].ToZeroIfEmpty()),
                    QC = int.Parse(row["QC"].ToZeroIfEmpty()),
                    QD = int.Parse(row["QD"].ToZeroIfEmpty())
                });
            }
            return list;
        }
    }

    public class ImportProperties
    {
        public DateTime? DATE { get; set; }
        public int FORECAST { get; set; }
        public int QC { get; set; }
        public int QD { get; set; }
        public string EVENT { get; set; }
        public string COMMENT { get; set; }
    }
}

