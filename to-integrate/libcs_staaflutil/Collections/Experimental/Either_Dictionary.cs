using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fairweather.Service.Collections
{
    /*       TBI        */
    /*       Check Two_Way_Dictionary for autogeneration ideas        */

    class Either_Dictionary<TIndex1, TIndex2, TValue>
    {
        readonly Dictionary<TIndex1, TValue> rd_dict1;
        readonly Dictionary<TIndex2, TValue> rd_dict2;

        public Either_Dictionary() {

            rd_dict1 = new Dictionary<TIndex1, TValue>();
            rd_dict2 = new Dictionary<TIndex2, TValue>();

        }

        public void Add(TIndex1 index1, TIndex2 index2, TValue value){

            rd_dict1.Add(index1, value);
            rd_dict2.Add(index2, value);

        }
    }
}
