using System;
using System.Windows.Forms;

namespace Fairweather.Service
{
    static partial class Magic
    {
        public static IDisposable
        bind_values(Control ctrl1, Control ctrl2) {
            Action r1 = null;
            Action r2 = null;

            var ret = new Bind_Values<string>(
                 ref r1,
                 ref r2,
                 () => ctrl1.Text,
                 () => ctrl2.Text,
                  _t => ctrl1.Text = _t,
                  _t => ctrl2.Text = _t
                  );

            ctrl1.TextChanged += (_1, _2) => r1();
            ctrl2.TextChanged += (_1, _2) => r2();

            return ret;
        }

    }
    /// <summary>
    /// Ensures two values are always the same.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Bind_Values<T> : IDisposable
    {
        bool disposed;

        readonly Func<T> get1;
        readonly Func<T> get2;
        readonly Action<T> set1;
        readonly Action<T> set2;


        public void Dispose() {
            disposed = true;

        }

        public Bind_Values(
            ref Action refresh1,
            ref Action refresh2,
            Func<T> get1,
            Func<T> get2,
            Action<T> set1,
            Action<T> set2) {

            this.set1 = set1;
            this.set2 = set2;
            this.get1 = get1;
            this.get2 = get2;

            refresh1 += Check1;
            refresh2 += Check2;

        }
        public void Check1() {
            Check(true);
        }

        public void Check2() {
            Check(false);
        }

        void Check(bool check_1) {
            if (disposed)
                return;

            var v1 = get1();
            var v2 = get2();
            if (v1.Safe_Equals(v2))
                return;

            (check_1 ? set2 : set1)(check_1 ? v1 : v2);

        }

    }
}
