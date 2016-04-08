using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
namespace daisybrand.forecaster.Helpers
{
    public class DelayedAction
    {
        private BackgroundWorker worker { get; set; }

        public static DelayedAction Get(DelayedAction d)
        {
            if (d != null) { d.Kill(); d = null; }
            return new DelayedAction();
        }

        public void Do(int timeout, MethodInvoker callback)
        {
            worker = new BackgroundWorker() { WorkerSupportsCancellation = true };
            worker.DoWork += new DoWorkEventHandler(DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
            worker.RunWorkerAsync(new object[] { timeout, callback });
        }

        public void Kill()
        {
            if (worker != null)
                worker.CancelAsync();
        }
        // Notified when our thread terminates
        static void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MethodInvoker callback = (MethodInvoker)e.Result;
            callback.Invoke();
        }

        // Do nothing but sleep
        static void DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            Thread.Sleep((int)args[0]);
            e.Result = args[1]; // return callback
        }
    }
}
