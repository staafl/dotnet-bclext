using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Fairweather.Service;

namespace Sage_Int
{
    class Ticker
    {
        public Ticker(TimeSpan how_often, Action what) {
            this.how_often = how_often;
            this.what = what;
            this.when = DateTime.Now.Subtract(how_often);
        }

        public bool Tick() {
            return Tick(false);
        }
        public bool Tick(bool force) {

            if (!force &&
                DateTime.Now - when < how_often)
                return false;

            what();
            when = DateTime.Now;
            return true;

        }

        readonly TimeSpan how_often;
        readonly Action what;
        DateTime when;

    }
}
