using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using datastores = CRDataStore;
namespace daisybrand.forecaster
{
    public static class LogManger
    {
        public static event EventHandler OnRaiseError;

        public static void InsertEvent([CallerMemberName]string caller = "", [CallerFilePath]string path = "")
        {
            if (MainWindow.myTopMenuViewModel != null && MainWindow.myTopMenuViewModel.TRACK_EVENTS)
                Insert(Guid.NewGuid(), "FEVENT", String.Format("{0} ({1})", caller, path));
        }

        public static void InsertStep([CallerMemberName]string caller = "", [CallerFilePath]string path = "")
        {
            if (MainWindow.myTopMenuViewModel != null && MainWindow.myTopMenuViewModel.TRACK_STEPS)
                Insert(Guid.NewGuid(), "FSTEP", String.Format("{0} ({1})", caller, path));
        }

        public static void Insert(Guid guid, string message, string value, bool sendEmail = false)
        {
            using (var context = new datastores.ForecasterEntities(null))
            {

                context.FORECASTER_EVENTLOGS.Add(new datastores.FORECASTER_EVENTLOGS
                {
                    CREATED = DateTime.Now,
                    MESSAGE = message,
                    USER_ID = Environment.UserName,
                    VALUE = value,
                    GUID = guid
                });
                context.SaveChanges();
            }

            if (sendEmail)
            {
                SendEmailToMe(String.Format("Error from Ordering App: Message: {0} Value: {1}", message, value));
            }
        }

        public static void Insert1(Exception ex, string comment, bool sendEmail = false)
        {
            Insert(Guid.NewGuid(), String.Format("{0}: {1}", ex.Message, comment), ex.StackTrace, sendEmail);
        }

        public static void Insert(Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0 )
        {            
            var value = String.Format("Member name: {0} Source File Path: {1} Source Line Number: {2} Stack Trace: {3}", memberName, sourceFilePath, sourceLineNumber, ex.StackTrace);
            Insert(Guid.NewGuid(), ex.Message, value);
            if (ex.InnerException != null)
                Insert(ex.InnerException, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void Insert(string message, string value)
        {
            Insert(Guid.NewGuid(), message, value);
        }

        public static void RaiseErrorMessage(object sender)
        {
            if (OnRaiseError != null)
                OnRaiseError(sender, EventArgs.Empty);
        }

        [Conditional("Debug")]
        public static void RaiseErrorMessageInDebug(object sender)
        {
            if (OnRaiseError != null)
                OnRaiseError(sender, EventArgs.Empty);
        }

        [Conditional("Release")]
        public static void RaiseErrorMessageInRelease(object sender)
        {
            if (OnRaiseError != null)
                OnRaiseError(sender, EventArgs.Empty);
        }

        public static void SendEmail(string from, string to, string body)
        {
            try
            {
                using (var service = new Services.Email.EmailServiceSoapClient())
                {
                    service.SendMail(from, to, "", "", "Forecasting Software", body);
                }
            }
            catch { }
        }

        public static void SendEmailToMe(string body)
        {
            try
            {
                using (var service = new Services.Email.EmailServiceSoapClient())
                {
                    service.SendMail("", "mseifi@daisybrand.com", "", "", "Forecasting Software", body);
                }
            }
            catch
            {

            }
        }

        public static void SendEmailExceptionToMe(Exception ex, string message = "")
        {
            try
            {
                using (var service = new Services.Email.EmailServiceSoapClient())
                {
                    service.SendMail("", "mseifi@daisybrand.com", "", "", "Forecasting Software", String.Format("Message:{0}{1}{2}{1}Stack: {3}", ex.Message, Environment.NewLine, message, ex.StackTrace));
                }
            }
            catch { }
        }
    }
}
