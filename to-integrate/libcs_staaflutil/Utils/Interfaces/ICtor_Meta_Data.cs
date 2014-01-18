namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    /* MetaClass<TClass, TMetaObj>.Metadata contains a constructor
         * where TMetaObj : ICtor_Meta_Data<TClass, TArg>
         * */
    public interface ICtor_Meta_Data<TClass, TArg>
    {
        TClass Constructor(TArg arg);
    }
}
