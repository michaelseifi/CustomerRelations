using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daisybrand.forecaster.Controlers.Interfaces;
using datastores = CRDataStore;

namespace daisybrand.forecaster.Controlers.Objects
{
    public class Identity : IIdentity, System.Security.Principal.IIdentity
    {
        private Identity(string netWorkAlias)
        {
            //if (
            //    string.Compare(netWorkAlias, "mseifi", true) == 0
            //    ||
            //    string.Compare(netWorkAlias, "JSiracuse", true) == 0
            //    ||
            //    string.Compare(netWorkAlias, "PHill", true) == 0
            //    ||
            //    string.Compare(netWorkAlias, "BAppelwick", true) == 0
            //    ||
            //    string.Compare(netWorkAlias, "JSchneider", true) == 0) netWorkAlias = "APrebis";
            this.SessionId = Guid.NewGuid();
            this.IsReadOnly = false;
            this.IsAdmin = false;
            this.IsAuthenticated = false;
            this.AuthenticationType = "NetWork";
            this.NetWorkAlias = netWorkAlias;
            this.Name = string.Empty;
            if (_AuthenticateUser() && _GetUserInfo())
                this.IsAuthenticated = true;            
        }

        #region properties
        private string _AuthenticationType;
        private string _Name;
        private string _Damian;
        private string _NetWorkAlias;
        private bool _IsAuthenticated;
        private bool _IsAdmin;
        private bool _IsReadOnly;
        private Guid _SessionId;
        /// <summary>
        /// Full name of user
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            private set
            {
                _Name = value;
            }                          
                    
        }
        public string AuthenticationType
        {
            get
            {
                return _AuthenticationType;
            }
            private set
            {
                _AuthenticationType = value;
            }
        }
        public Guid SessionId
        {
            get
            {
                return _SessionId;
            }
            private set
            {
                _SessionId = value;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            private set
            {
                _IsReadOnly = value;
            }
        }
        public bool IsAdmin
        {
            get
            {
                return _IsAdmin;
            }
            private set
            {
                _IsAdmin = value;                
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return _IsAuthenticated;
            }
            private set
            {
                _IsAuthenticated = value;
            }
        }
        public string NetWorkAlias
        {
            get
            {
                return _NetWorkAlias;
            }
            private set
            {
                _NetWorkAlias = value;
            }
        }
        public string Damian
        {
            get
            {
                return _Damian;
            }
            private set
            {
                _Damian = value;
            }
        }
        #endregion

      
        #region privates
        /// <summary>
        /// GETS USER INFO FROM THE DATABASE
        /// SETS THE USER FULL NAME
        /// RETURNS TRUE IF SUCCESSFUL
        /// </summary>
        /// <returns></returns>
        bool _GetUserInfo()
        {
            using (var context = new datastores.DAX_PRODEntities(null))
            {
                this.Name = context.VMI_GetUserInfo(this.NetWorkAlias).Select(x => x.NAME).FirstOrDefault();
            }
            if (string.IsNullOrWhiteSpace(this.Name)) return false;
            return true;
        }

        bool _AuthenticateUser()
        {
            using (var context =
                   new datastores.ForecasterEntities(null))
            {
                var user = context.FORECASTER_USER_TABLE.Where(x => x.EMPLID.ToUpper().Trim() == this.NetWorkAlias.ToUpper()).AsEnumerable().FirstOrDefault();
                if (user != null)
                {
                    this.IsAdmin = user.STATUS.ToUpper() == "ADMIN";
                    return true;
                }
                return false;

            }
        }
        #endregion

        #region statics

        public static Identity GetIdnetity(string userName)
        {
            Identity i = new Identity(userName);
            return i;
        }
        #endregion
    }
}
