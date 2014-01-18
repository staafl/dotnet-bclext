using System;

namespace Fairweather.Service
{
    /*
         * for $i (5..10) { 
      print "public delegate void Action<"; 
      print "T$_," for 1..$i; 
      print ">("; 
      print "T$_ t$_," for 1..$i; 
      print ");\n"; }
        */

    /*
* for $i (4..10) { acws
print "public delegate TResult Func<"; 
print "T$_," for 1..$i; 
print "TResult";
print ">("; 
print "T$_ t$_," for 1..$i; 
print ");\n"; }
*/


    public delegate bool GetItemDelegate<TK, TV>(TK key, out TV value, out bool stop);
}
