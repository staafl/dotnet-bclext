using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Fairweather.Service
{
    public static class Filter
    {

        static bool enabled;
        public static bool Enabled {
            get { return enabled; }
            set {
                if (enabled == value)
                    return;

                enabled = value;

                if (value)
                    Application.AddMessageFilter(filter);
                else
                    Application.RemoveMessageFilter(filter);

            }
        }

        static readonly Filter_Inner filter = new Filter_Inner();


        public static void Register(int msg, Func<Message, bool> f) {

            dict.List_Modify(msg, f);
            Enabled = true;

        }

        readonly static Dictionary<int, List<Func<Message, bool>>>
        dict = new Dictionary<int, List<Func<Message, bool>>>();

        class Filter_Inner : IMessageFilter
        {

            public bool PreFilterMessage(ref Message m) {

                bool ret = false;

                do {
                    if (!Enabled)
                        break;

                    List<Func<Message, bool>> acts;

                    if (!dict.TryGetValue(m.Msg, out acts))
                        break;

                    foreach (var act in acts) {
                        if (act(m))
                            ret = true;
                    }

                } while (false);

                return ret;

            }
        }
    }
}
