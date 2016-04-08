using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Extensions;
using datastores = CRDataStore;

namespace daisybrand.forecaster.Controlers.Objects
{
    public class CellComment : ICellComment, INotifyPropertyChanged
    {
        #region properties
        public int CELL_COMMENT_ID { get; set; }
        public string WEEK_ID { get; set; }
        public Field FIELD { get; set; }
        //public string ITEM_UPC { get; set; }
        //public string SHIP_TO_TP { get; set; }
        private string _VALUE;
        public string VALUE
        {
            get
            {
                return _VALUE;
            }
            set
            {
                if (_VALUE == value) return;
                _VALUE = value;
                NotifyPropertyChanged();
            }
        }

        public string ENTERED_BY { get; set; }
        public DateTime DATE_ENTERED { get; set; }

        #endregion
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public enum Field
        {
            FORECAST,
            BASE_LINE,
            ORDER,
            QA,
            QC,
            QD,
            QO,
            QS,
            QW,
            LY,
        };



        public CellComment() { }
        public CellComment(string weekId, Field field, string value)
        {
            this.WEEK_ID = weekId;
            this.FIELD = field;
            this.VALUE = value;
        }
        public static ICellComment AddEmpty(List<ICellComment> list, string weekId, Field field)
        {
            list.Add(new CellComment(weekId, field, string.Empty));
            return list.Where(x => x.WEEK_ID == weekId && x.FIELD == field).FirstOrDefault();
        }

        public static List<ICellComment> GetCollection(IEnumerable<string> listOfSkusShipTo, int shipTo)
        {
            return _GetCellComments(listOfSkusShipTo, shipTo);
        }

        public static int InsertOrUpdate(IDailyData data, Field field, string value)
        {
            if (!Tools.IsInEditMode()) return 0;
            int result = 0;

            try
            {
                var f = field.ToString();
                int shipTo = int.Parse(data.SHIP_TO_TP);
                using (var context = new datastores.ForecasterEntities(null))
                //new datastores.FORECASTER_CELL_COMMENT_TABLETableAdapters.FORECASTER_CELL_COMMENT_TABLETableAdapter())
                {
                    var items = context.FORECASTER_CELL_COMMENT_TABLE.Where(i => i.WEEKID == data.WEEK_ID && i.FIELD == f);
                    if (items != null && items.Count() > 0)
                        foreach (var i in items)
                        {
                            i.VALUE = value;
                            i.ENTERED_BY = Environment.UserName;
                            i.DATE_ENTERED = DateTime.Now;
                        }
                    else
                    {
                        var item = new datastores.FORECASTER_CELL_COMMENT_TABLE
                        {
                            DATE_ENTERED = DateTime.Now,
                            ENTERED_BY = Environment.UserName,
                            FIELD = f,
                            ITEM_UPC = data.ITEM_UPC,
                            WEEKID = data.WEEK_ID,
                            SHIP_TO_TP = shipTo,
                            VALUE = value
                        };
                        context.FORECASTER_CELL_COMMENT_TABLE.Add(item);
                    }
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
            }
            return result;
        }

        public static ICellComment Get(ICellComment comment, string weekid, Field field)
        {
            if (comment == null) return new CellComment
            {
                WEEK_ID = weekid,
                FIELD = field,
                VALUE = null
            };
            return comment;
        }

        private static List<ICellComment> _GetCellComments(IEnumerable<string> listOfSkusShipTo, int shipTo)
        {
            List<ICellComment> cellCommentlist = new List<ICellComment>();
            try
            {
                List<datastores.FORECASTER_CELL_COMMENT_TABLE> list;
                using (var context = new datastores.ForecasterEntities(null))
                {
                    list = context.FORECASTER_CELL_COMMENT_TABLE.Where(i => i.SHIP_TO_TP == shipTo).ToList();
                }

                var query = list.Where(c => listOfSkusShipTo.Contains(c.ITEM_UPC + c.SHIP_TO_TP)).Select(x => new CellComment
                {
                    CELL_COMMENT_ID = x.CELLCOMMENTID,
                    DATE_ENTERED = x.DATE_ENTERED,
                    ENTERED_BY = x.ENTERED_BY,
                    FIELD = x.FIELD.ToField(),
                    VALUE = x.VALUE,
                    WEEK_ID = x.WEEKID
                });
                if (query.Count() > 0)
                    cellCommentlist.AddRange(query);

            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
            }
            return cellCommentlist;
        }
    }
}
