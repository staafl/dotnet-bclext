
using Common;

using Fairweather.Service;

namespace Sage_Int
{
    public class D : Lazy_Dict<string, string>
    {
        public D()
            : base(_ => null) {
        }
    }
    public class DD : Lazy_Dict<string, D>
    {
        public DD()
            : base(_ => new D()) {
        }

        public D this[Company_Number company] {
            get {
                return this[company.As_String];
            }
            set {
                this[company.As_String] = value;
            }
        }
    }
}
