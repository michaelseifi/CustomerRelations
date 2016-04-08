using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Exceptions;
using daisybrand.forecaster.Extensions;
using System.Threading;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class UserCollection:ObservableCollection<IUser>, INotifyPropertyChanged
    {
        public UserCollection()
        {
            
        }
        public UserCollection(List<IUser> list)
            : base(list)
        {
            
        }
        public UserCollection(IEnumerable<IUser> collection)
            : base(collection)
        {
            
        }


        private IUser _FocusedEmplid;
        public IUser FocusedEmplid
        {
            get
            {
                return _FocusedEmplid;
            }
            set
            {
                _FocusedEmplid = value;
                NotifyPropertyChanged();
            }
        }

        public static UserCollection Get(SkuCollection skus)
        {
            UserCollection list = new UserCollection();
            try
            {
                List<datastores.FORECASTER_USER_TABLE> users;
                using (var contextForecaster = new datastores.ForecasterEntities(null))
                {
                    users = contextForecaster.FORECASTER_USER_TABLE.ToList();
                }
                //ut.GetData().Select(x => new User(x.EMPLID, x.STATUS));
                //IF USER IS INCLUDED IN THE LIST PROCEED
                //ELSE SEND AN NONAUTHENTICATED ERROR
                if (users == null || !users.Any(x => x.EMPLID.ToUpper() == Environment.UserName.ToUpper()))
                    throw new NonAuthenticatedException("Unable to authenticate");
                List<datastores.VMI_Get_Users_Result> data;
                using (var contextDax = new datastores.DAX_PRODEntities(null))
                {
                    data = contextDax.VMI_Get_Users().Where(x => users.Select(u => u.EMPLID).Contains(x.EMPLID.ToUpper())).Where(x => x.CUSTGROUP != null && x.CUSTGROUP == "RETAIL").ToList();
                }
                var customerExceptions = CustomerCollection.GetListOfExceptions();
                var skuExceptions = SkuCollection.GetListOfExceptions();
                //var emplIds = data.Select(x => x.EMPLID.ToUpper()).Distinct();
                foreach (datastores.FORECASTER_USER_TABLE u in users)
                {
                    User user = new User(u.EMPLID.ToUpper(), u.STATUS);
                    var customers = data.Where(x => x.EMPLID.ToUpper() == u.EMPLID.ToUpper());
                    var Customers852 = CustomerCollection.GetListFrom852();
                    foreach (datastores.VMI_Get_Users_Result row in customers)
                    {
                        if (Customers852.Contains(row.ACCOUNTNUM.TrimLeadingZeros()))
                        {
                            Customer cust = new Customer();
                            if (row.ACCOUNTNUM == null) break;
                            ((ICustomer)cust).ACCOUNT_NAME = row.ACCOUNTNAME;
                            ((ICustomer)cust).ACCOUNTNUM = row.ACCOUNTNUM;
                            ((ICustomer)cust).ADDRESS = row.ADDRESS;
                            ((ICustomer)cust).CITY = row.CITY;
                            ((ICustomer)cust).CUSTGROUP = row.CUSTGROUP;
                            ((ICustomer)cust).DLVTERM = row.DLVTERM;
                            ((ICustomer)cust).INVENT_LOCATION = row.INVENTLOCATION;
                            ((ICustomer)cust).INVOICEACCOUNT = row.INVOICEACCOUNT;
                            ((ICustomer)cust).EMPLID = row.EMPLID;
                            ((ICustomer)cust).NAME = row.ACCOUNTNAME;
                            ((ICustomer)cust).PAYMDAY_ID = row.PAYMDAYID;
                            ((ICustomer)cust).PAYMTERM_ID = row.PAYMTERMID;
                            ((ICustomer)cust).ZIPCODE = row.ZIPCODE;
                            ((ICustomer)cust).CR_ASSOCIATE = row.CR_Associate;
                            ((ICustomer)cust).REGIONAL_MANGER = row.Regional_Manger;
                            ((ICustomer)cust).COMPANY_NAME = row.JSCOMPANYNAME;
                            ((ICustomer)cust).CLASSIFICATION_ID = row.CUSTCLASSIFICATIONID;
                            var s = skus.Where(x => x.SHIPTO == int.Parse(((ICustomer)cust).ACCOUNTNUM).ToString());
                            ((ICustomer)cust).SKUS.AddRange(s.Where(x => !skuExceptions.Any(d => d.KEY == int.Parse(((ICustomer)cust).ACCOUNTNUM.ToString()) && d.VALUE == x.SKUID)));
                            ((ICustomer)cust).SKUS_EXCEPTIONS.AddRange(s.Where(x => skuExceptions.Any(d => d.KEY == int.Parse(((ICustomer)cust).ACCOUNTNUM.ToString()) && d.VALUE == x.SKUID)));
                            cust.DISPLAY_VALUE = String.Format("{0} ({1})", row.ACCOUNTNAME, row.ACCOUNTNUM);
                            cust.SELECTED_VALUE = row.ACCOUNTNUM;
                            if (customerExceptions.Any(x => x == int.Parse(row.ACCOUNTNUM)))
                                user.CUSTOMER_EXCEPTIONS.Add(cust);
                            else
                                user.CUSTOMERS.Add(cust);
                        }
                    }
                    if (user.CUSTOMERS.Count > 0)
                    {
                        //var cs = from c in customers
                        //         select int.Parse(c.ACCOUNTNUM).ToString();
                        //user.SKUS.AddRange(skus.Where(x => cs.Contains(x.SHIPTO)));
                        list.Add(user);
                    }
                }


                if (list.Count > 0)
                    list.FocusedEmplid = list.Where(x => x.EMPLID.ToUpper() == MainWindow.myIdentity.NetWorkAlias.ToUpper()).FirstOrDefault();
            }
            catch (NonAuthenticatedException na)
            {
                LogManger.Insert1(na, "The user is not authenticated");
                return null;
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "General error in getting the user information");
                return null;
            }
            return list;
        }


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

        
    }
}
