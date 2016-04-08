using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
namespace daisybrand.forecaster.Helpers
{
    
    
    public static class Caching
    {
        public enum Folder { data, archive, xml, excel }
        public static string UserFolder { get { return string.Format(@"{0}\My Forecaster {1}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), GetBuild()).Trim(); } }
        public static string XmlFolder { get { return string.Format(@"{0}\xml", UserFolder); } }
        public static string DataFolder { get { return string.Format(@"{0}\data", UserFolder); } }
        public static string ArchiveFolder { get { return string.Format(@"{0}\archive", UserFolder); } }
        public static string ExcelFolder { get { return string.Format(@"{0}\excel", UserFolder); } }
        public static void SaveToXML<T>(T t, Folder location, string fileName)
        {
            using (TextWriter wr = new StreamWriter(GetFilePath(location, fileName)))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(wr, t);
            }
        }

        public static T GetFromXML<T>(Folder location, string fileName)
        {
            T result;
            var path = GetFilePath(location, fileName);
            
            using (TextReader Rect = new StreamReader(path))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                result = (T)xs.Deserialize(Rect);
            }
            return result;
        }

        public static string GetFilePath(Folder location, string fileName)
        {
            switch (location)
            {
                case Folder.data:
                    return GetDataFilePath(fileName);
                case Folder.archive:
                    return GetArchiveFilePath(fileName, "dat");
                case Folder.xml:
                    return GetXMLFilePath(fileName);                
                default:
                    return GetDataFilePath(fileName);
            }
        }

        public static string GetDataFilePath(string fileName)
        {
            if (!System.IO.Directory.Exists(DataFolder))
                System.IO.Directory.CreateDirectory(DataFolder);
            return string.Format(@"{0}\{1}.dat", DataFolder, fileName);
        }

        public static string GetXMLFilePath(string fileName)
        {
            if (!System.IO.Directory.Exists(XmlFolder))
                System.IO.Directory.CreateDirectory(XmlFolder);
            return string.Format(@"{0}\{1}.xml", XmlFolder, fileName);
        }

        public static string GetArchiveFilePath(string fileName, string extension)
        {
            if (!System.IO.Directory.Exists(ArchiveFolder))
                System.IO.Directory.CreateDirectory(ArchiveFolder);
            return string.Format(@"{0}\{1}.{2}", ArchiveFolder, fileName, extension);
        }

        public static string GetExcelFilePath(string fileName)
        {
            if (!System.IO.Directory.Exists(ExcelFolder))
                System.IO.Directory.CreateDirectory(ExcelFolder);
            return string.Format(@"{0}\{1}.xlsx", ExcelFolder, fileName);
        }

        /// <summary>
        /// <para>RETURNS EMPTY STRING IF BUILD IS RELEASE</para>
        /// <para>OTHERWISE IT RETURNS THE STRING OF BUILD</para>
        /// </summary>
        /// <returns></returns>
        static string  GetBuild()
        {
            return App.BUILD == daisybrand.forecaster.Controlers.ViewModels.MainWindow.Build.Release ? "" : App.BUILD.ToString();
        }

    }
}
