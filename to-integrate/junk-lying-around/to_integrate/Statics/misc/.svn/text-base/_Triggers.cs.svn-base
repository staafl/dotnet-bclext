//#define TEST 
using System;
//using System.Windows.Forms;

namespace Fairweather.Service
{
    public static class Triggers
    {
#if TEST
        public static void Test() {
            NativeMethods.AllocConsole();

            GC.Collect(2);
            DateTime dt = DateTime.Now;

            double Accum = 0;
            for (int ii = 0; ii < 100000; ++ii) {
                TriggerMechanism t = new TriggerMechanism();

                t.CreateTrigger(ref t.evnt,
                          Always<MouseEventArgs>(),
                          (() => 1),
                          ((int i) => 1.0 + i),
                          ((double d) => { Accum += d; }));

                t.evnt(t, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));

            }

            Console.WriteLine(DateTime.Now - dt);
            Console.WriteLine(Accum);
            Console.ReadLine();

        }

        public event EventHandler<MouseEventArgs> evnt;
#endif

        public static Func<object, TEventArgs, bool> Always<TEventArgs>()
        where TEventArgs : EventArgs {
            return ((object sender, TEventArgs e) => true);
        }


        public static int CreateTrigger<TEventArgs, TParam>
            (ref EventHandler<TEventArgs> Event,
            Func<object, TEventArgs, bool> filter,
            Func<TEventArgs, TParam> extracter,
            Action<TEventArgs, TParam> result)
            where TEventArgs : EventArgs {

            EventHandler<TEventArgs> handler =
            (object sender, TEventArgs e) =>
            {
                ExecuteTrigger(filter, extracter, result, sender, e);
            };

            Event += handler;

            return 0;
        }

        static void ExecuteTrigger<TEventArgs, TParam>(
            Func<object, TEventArgs, bool> filter,
            Func<TEventArgs, TParam> extracter,
            Action<TEventArgs, TParam> result,
            object sender,
            TEventArgs e)
            where TEventArgs : EventArgs {

            if (filter(sender, e)) {
                TParam val = extracter(e);
                result(e, val);
            }

        }
    }
}

/*

        public int CreateTrigger<TEventArgs, TParam1, TParam2>
            (ref EventHandler<TEventArgs> Event,
            Func<object, TEventArgs, bool> filter,
            Func<TParam1> extracter,
            Func<TParam1, TParam2> converter,
            Action<TParam2> result)
            where TEventArgs : EventArgs {

            var handler = new EventHandler<TEventArgs>((object sender, TEventArgs e) =>
            {
                if (filter(sender, e)) {
                    TParam1 val1 = extracter();
                    TParam2 val2 = converter(val1);
                    result(val2);
                }
            });

            Event += handler;

            return 0;
        }
*/