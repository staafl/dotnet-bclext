namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    /* MetaClass<TClass, TMetaObj> contains Metadata : TMetaObj
        * */
    public interface IHas_Meta<TMetaObj>
    {
        TMetaObj Metadata { get; }
    }
}
