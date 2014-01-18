using System;
using System.Threading;
using Common.Dialogs;

using Fairweather.Service;
using System.Windows.Forms;
using System.Drawing;
namespace Common
{
    public class Progress_Reporter : IDisposable
    {
        /* 
        option one:
        public class Progress_Reporter

        public void Notify(int current)
        public void Notify_Total(int total)
		         
        option two:
		        
        public Progress_Reporter(
            ref Func<int> progress,
            ref Func<int?> total_work) {

            this.progress = progress;
            progress 
            this.total_work = total_work;

         * 
        }
		         
         In this case I'm inclined to take the non-functional
         approach, as it avoids the need to subscribe to delegates.
		         
         */

        public Progress_Reporter() {
        }

        public Progress_Reporter(int total) {
            this.total = total;
        }

        int disposed;

        public void Dispose() {

            if (Interlocked.Exchange(ref disposed, 1) != 0)
                return;
            if (form == null)
                return;
            if (Form.IsDisposed)
                return;

            Form.InvokeOrNot(true, false, () =>
            {
                Form.Dispose();
            });

        }


        // ****************************


        public Please_Wait Form {
            get {
                return form;
            }
        }

        public ProgressBar Bar {
            get {
                return form.Bar;
            }
        }

        public void Create_Form() {

            if (form == null) {
                form = new Please_Wait();
                form.Force_Handle();
            }

            if (total.HasValue)
                Prepare_Form(true);
        }

        void Unknown_Total() {

            total = null;

            Create_Form();

            Prepare_Form(false);

        }

        public void Notify(int current) {

            if (!total.HasValue)
                return;

            if (form == null || Form.IsDisposed)
                return;

            int value = total.Value;

            Form.InvokeOrNot(
                false, true, () =>
                {
                    Form.Invalidate();

                    int max = Form.Bar.Maximum;

                    if (value != 0)
                        Form.Bar.Value = (current * max) / value;

                    else
                        Form.Bar.Value = max;

                });

            if (current >= value) {

                Form.InvokeOrNot(false, true,
                () =>
                {
                    Form.Closeable = true;
                    Form.Try_Close();
                });

            }

        }

        public void Notify_Total(int total) {

            this.total = total;

            if (form != null)
                Prepare_Form(true);

        }



        /* Usage patterns for Progress_Reporter class:
                        
         * Create instance
         * Call either Unknown_Total or Notify_Total
         * Set the form's text and properties
         * Call .Form.Show()
         * Do work, occasionally calling .Form.Refresh()
           (take care to honor thread affinity rules)
         * Dispose of PR instance
         * 
         * Note that all of this can happen on a separate thread.
         * */

        // I should better look carefully at the other uses of this class
        // in order to codify any patterns that may emerge.



        public static IDisposable
        Separate_Thread_Unknown_Total(string text, int refresh_interval, out Action show, out Action close) {
            Func<Form> form_getter;
            return Separate_Thread_Unknown_Total(text, refresh_interval, null, out show, out close, out form_getter);
        }

        public static IDisposable Separate_Thread_Unknown_Total(string text, int refresh_interval, Rectangle? center_to, out Action show, out Action close, out Func<Form> form_getter) {
            Action<Form> loop = null;
            return Separate_Thread_Unknown_Total(text, refresh_interval, center_to, out show, out close, out form_getter, loop);
        }
        public static IDisposable
        Separate_Thread_Unknown_Total(
            string text,
            int refresh_interval,
            Rectangle? center_to,
            out Action show,
            out Action close,
            out Func<Form> form_getter,
            Action<Form> loop) {

            bool stop = false;

            IDisposable disp = null;

            var pr = new Progress_Reporter();
            form_getter = () => pr.Form;

            var thrd = new Thread(
            () =>
            {
                using (disp)
                using (pr) {

                    pr.Unknown_Total();

                    var form = pr.Form;

                    form.Force_Handle();
                    form.Text = text;

                    if (center_to != null) {
                        form.StartPosition = FormStartPosition.Manual;
                        form.Location = form.Bounds.Center_In_Rectangle(center_to.Value).Location;
                    }

                    form.Show();
                    form.Activate();
                    form.Refresh();

                    while (true) {

                        if (stop)
                            return;

                        Thread.Sleep(refresh_interval);

                        if (stop)
                            return;

                        if (loop != null)
                            loop(form);

                        form.Refresh();

                    }
                }
            });

            close = () =>
            {
                using (disp)
                    stop = true;
            };
            show = () =>
            {
                if (stop)
                    return;

                try {
                    thrd.Start();
                    disp = H.Change_Cursor(Cursors.WaitCursor);

                }
                catch {
                    using (disp) { }
                    throw;
                }
            };

            return new On_Dispose(close);
        }


        // ****************************



        void Prepare_Form(bool known) {

            if (known)
                total.tifn();

            if (H.Set(ref prepared, true))
                return;

            Form.InvokeOrNot(true, false,
            () =>
            {
                Form.Bar.Force_Handle();

                if (known) {
                    Form.Bar.Minimum = 0;

                    Form.Bar.Maximum = total.Value;
                }
                else {
                    /* You need to set the MarqueeAnimationSpeed before you change the ProgressBar 
                     * style to Marquee. Otherwise the speed setting has no effect.
                     * 
http://connect.microsoft.com/VisualStudio/feedback/details/115990/progress-style-marquee-marqueeanimationspeed-is-busted */

                    // Form.Bar.Maximum = 0; // setting maximum to 0 causes the 
                    // progress bar to remain inactive         

                    // In my experience the property has no effect whatsoever

                    Form.Bar.MarqueeAnimationSpeed = 100;

                    Form.Bar.Style = ProgressBarStyle.Marquee;


                }
            });

        }

        int? total;
        bool prepared;
        Please_Wait form;
    }
}
