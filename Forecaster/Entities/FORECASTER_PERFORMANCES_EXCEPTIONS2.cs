using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace daisybrand.forecaster.datastores
{
    [Serializable]
    public class FORECASTER_PERFORMANCES_EXCEPTIONS2 : datastores.FORECASTER_PERFORMANCES_EXCEPTIONS, System.Windows.IDataObject, ISerializable
    {
        public FORECASTER_PERFORMANCES_EXCEPTIONS2() { }
        public FORECASTER_PERFORMANCES_EXCEPTIONS2(SerializationInfo info, StreamingContext context)
        {
            GUID = Guid.Parse(info.GetString("GUID"));
            CUSTOMER_ID = info.GetInt32("CUSTOMER_ID");
            SKU = info.GetString("SKU");
            TYPE = info.GetString("TYPE");
            FROM = info.GetDateTime("FROM");
            TO = info.GetDateTime("TO");
            QUANTITY = info.GetInt32("QUANTITY");
            PRICE = info.GetInt32("PRICE");
            DESCRIPTION = info.GetString("DESCRIPTION");
            INCLUDE = info.GetBoolean("INCLUDE");
            IS_DEFAULT = info.GetBoolean("IS_DEFAULT");
        }

        #region IDataObject Implementation
        public object GetData(string format, bool autoConvert)
        {
            throw new NotImplementedException();
        }

        public object GetData(Type format)
        {
            throw new NotImplementedException();
        }

        public object GetData(string format)
        {
            throw new NotImplementedException();
        }

        public bool GetDataPresent(string format, bool autoConvert)
        {
            throw new NotImplementedException();
        }

        public bool GetDataPresent(Type format)
        {
            return format == typeof(FORECASTER_PERFORMANCES_EXCEPTIONS2);
        }

        public bool GetDataPresent(string format)
        {
            throw new NotImplementedException();
        }

        public string[] GetFormats(bool autoConvert)
        {
            //throw new NotImplementedException();
            return new string[] { typeof(datastores.FORECASTER_PERFORMANCES_EXCEPTIONS2).FullName };
        }

        public string[] GetFormats()
        {
            //throw new NotImplementedException();
            return new string[] { typeof(datastores.FORECASTER_PERFORMANCES_EXCEPTIONS2).FullName };
        }

        public static System.Windows.DataFormat GerFormat()
        {
            return System.Windows.DataFormats.GetDataFormat(typeof(datastores.FORECASTER_PERFORMANCES_EXCEPTIONS2).FullName);
        }

        public void SetData(string format, object data, bool autoConvert)
        {
            throw new NotImplementedException();
        }

        public void SetData(Type format, object data)
        {
            throw new NotImplementedException();
        }

        public void SetData(string format, object data)
        {
            throw new NotImplementedException();
        }

        public void SetData(object data)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new NotImplementedException();
            info.AddValue("GUID", GUID);
            info.AddValue("CUSTOMER_ID", CUSTOMER_ID);
            info.AddValue("SKU", SKU);
            info.AddValue("TYPE", TYPE);
            info.AddValue("FROM", FROM);
            info.AddValue("TO", TO);
            info.AddValue("QUANTITY", QUANTITY);
            info.AddValue("PRICE", PRICE);
            info.AddValue("DESCRIPTION", DESCRIPTION);
            info.AddValue("INCLUDE", INCLUDE);
            info.AddValue("IS_DEFAULT", IS_DEFAULT);
        }
        #endregion


    }
}
