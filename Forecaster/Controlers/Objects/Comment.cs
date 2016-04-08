using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Exceptions;
using daisybrand.forecaster.Helpers;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Comment : IComment, INotifyPropertyChanged
    {
        public Comment(string weekId)
        {
            WEEKID = weekId;
            ENTERED_BY = Environment.UserName;
            DATE_ENTERED = DateTime.Now;
            VALUE = string.Empty;
        }
        public Comment()
        {
            ENTERED_BY = Environment.UserName;
            DATE_ENTERED = DateTime.Now;
            VALUE = string.Empty;
        }

        #region properties
        private string _ITEM_UPC;
        private string _SHIP_TO_TP;
        private string _WEEKID;
        private string _VALUE;
        private string _ENTERED_BY;
        private DateTime _DATE_ENTERED;
        private int _COMMENTID;
        public int COMMENTID
        {
            get
            {
                return _COMMENTID;
            }
            set
            {
                if (_COMMENTID == value) return;
                _COMMENTID = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime DATE_ENTERED
        {
            get
            {
                return _DATE_ENTERED;
            }
            set
            {
                if (_DATE_ENTERED == value) return;
                _DATE_ENTERED = value;
                NotifyPropertyChanged();
            }
        }
        public string ENTERED_BY
        {
            get
            {
                return _ENTERED_BY;
            }
            set
            {
                if (_ENTERED_BY == value) return;
                _ENTERED_BY = value;
                NotifyPropertyChanged();
            }
        }
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
        public string WEEKID
        {
            get
            {
                return _WEEKID;
            }
            set
            {
                if (_WEEKID == value) return;
                _WEEKID = value;
                NotifyPropertyChanged();
            }
        }

        public string SHIP_TO_TP
        {
            get
            {
                return _SHIP_TO_TP;
            }
            set
            {
                _SHIP_TO_TP = value;
                NotifyPropertyChanged();
            }
        }

        public string ITEM_UPC
        {
            get
            {
                return _ITEM_UPC;
            }
            set
            {
                _ITEM_UPC = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
       
        public static List<IComment> GetCollection(IEnumerable<string> listOfSkusShipTo, int shipTo)
        {
            return _GetComments(listOfSkusShipTo, shipTo);
        }

        public static void Delete(IDailyData data)
        {
            if (Tools.IsInEditMode())
            {
                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                           //new datastores.FORECASTER_COMMENT_TABLETableAdapters.FORECASTER_COMMENT_TABLETableAdapter())
                    {
                        var shipTp = int.Parse(data.SHIP_TO_TP);
                        var items = context.FORECASTER_COMMENT_TABLE.Where(i => i.SHIP_TO_TP == shipTp && i.ITEM_UPC == data.ITEM_UPC && i.WEEKID == data.WEEK_ID);
                        foreach (var i in items)
                            context.FORECASTER_COMMENT_TABLE.Remove(i);
                        context.SaveChanges();                        
                    }
                }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                    throw new DeleteException("Unable to delete comment field.");
                }
            }
           
        }
        public static void UpdateOrInsert(Interfaces.IDailyData data)
        {
            if (Tools.IsInEditMode())
            {
                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                    {
                        var shipTo = int.Parse(data.SHIP_TO_TP);
                        var items = context.FORECASTER_COMMENT_TABLE.Where(i => i.WEEKID == data.WEEK_ID);
                        if (items != null && items.Count() > 0)
                            if (data.COMMENT.VALUE != null && data.COMMENT.VALUE.Trim().Length > 0)
                                foreach (var i in items)
                                {
                                    i.VALUE = data.COMMENT.VALUE;
                                    i.ENTERED_BY = Environment.UserName;
                                    i.DATE_ENTERED = DateTime.Now;
                                }
                            else
                                foreach (var i in items)
                                    context.FORECASTER_COMMENT_TABLE.Remove(i);
                        else
                        {
                            var item = new datastores.FORECASTER_COMMENT_TABLE
                            {
                                DATE_ENTERED = DateTime.Now,
                                ENTERED_BY = Environment.UserName,
                                ITEM_UPC = data.ITEM_UPC,
                                SHIP_TO_TP = shipTo,
                                VALUE = data.COMMENT.VALUE,
                                WEEKID = data.WEEK_ID
                            };
                            context.FORECASTER_COMMENT_TABLE.Add(item);
                        }
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                    throw new InsertException("Unable to update or insert comment value.");
                }
            }
        }

        public static List<IComment> Create(IEnumerable<string> weekids, List<IComment> cs)
        {
            List<IComment> list = new List<IComment>();

            foreach (string weekId in weekids)
            {
                if (cs.Any(x => x.WEEKID == weekId))
                    list.Add(cs.Where(x => x.WEEKID == weekId).Select(x => x).FirstOrDefault());
                else
                    list.Add(new Comment(weekId));
            }
            return list;
        }

        public static IComment AddEmpty(List<IComment> list, string weekId)
        {
            list.Add(new Comment(weekId));
            return list.Where(x=>x.WEEKID == weekId).FirstOrDefault();
        }

        //public static IComment Get(string weekId)
        //{
        //    Comment comment = null;
        //    using (datastores.FORECASTER_COMMENT_TABLETableAdapters.FORECASTER_COMMENT_TABLETableAdapter dt = new datastores.FORECASTER_COMMENT_TABLETableAdapters.FORECASTER_COMMENT_TABLETableAdapter())
        //    {
        //        comment = dt.GetData().Where(x => x.WEEKID == weekId).Select(x => new Comment(x.WEEKID)
        //        {
        //            COMMENTID = x.COMMENTID,
        //            DATE_ENTERED = x.DATE_ENTERED,
        //            ENTERED_BY = x.ENTERED_BY,
        //            VALUE = x.VALUE,
        //            WEEKID = x.WEEKID
        //        }).FirstOrDefault();
                
        //    }
        //    return comment ?? new Comment(weekId);

        //}

        private static List<IComment> _GetComments(IEnumerable<string> listOfSkusShipTo, int shipTo)
        {
            List<IComment> c = new List<IComment>();
            try
            {
                List<datastores.FORECASTER_COMMENT_TABLE> list;
                using (var context = new datastores.ForecasterEntities(null))
                    //new datastores.FORECASTER_COMMENT_TABLETableAdapters.FORECASTER_COMMENT_TABLETableAdapter())
                {
                    list = context.FORECASTER_COMMENT_TABLE.Where(i => i.SHIP_TO_TP == shipTo).ToList();
                }
                if (list != null && list.Count() > 0)
                {
                    var comment = list.Where(x => listOfSkusShipTo.Contains(x.ITEM_UPC + x.SHIP_TO_TP)).Select(x => new Comment(x.WEEKID)
                    {
                        DATE_ENTERED = x.DATE_ENTERED,
                        ENTERED_BY = x.ENTERED_BY,
                        VALUE = x.VALUE,
                        SHIP_TO_TP = x.SHIP_TO_TP.ToString(),
                        ITEM_UPC = x.ITEM_UPC,
                        WEEKID = x.WEEKID,
                        COMMENTID = x.COMMENTID
                    });
                    if (comment.Count() > 0)
                        c.AddRange(comment);
                }
                
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to get a list of comments");
                throw new ApplicationException("Unable to get a list of comments");
            }
            return c;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
