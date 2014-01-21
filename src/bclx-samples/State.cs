using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Scratch
{
    static class Program
    {
        static void Main(string[] args)
        {
            var x = 1;
            var y = "asdf";
            try
            {
                using (State.Push(() => new { x, y }))
                {
                    Foo();
                }
            }
            catch (Exception ex)
            {
                State.Log(ex);
            }
        }

        static void Foo()
        {
            var a = 12.0;
            var b = "asdfasdf";
            using (State.Push(() => new { a, b }))
            {
                Bar();
            }
        }

        static void Bar()
        {
            var q = 12341234;
            object w = null;
            using (State.Push(() => new { q, w }))
            {
                throw new Exception();
            }
        }



    }
}
