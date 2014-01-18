namespace Fairweather.Service
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class _Delegates
    {
        static public T Cast<T>(Delegate source) where T : class {

            // idea comes from http://code.logos.com/blog/2008/07/casting_delegates.html
            
            return Cast_Inner(source, typeof(T)) as T;

        }

        static object Cast_Inner(Delegate source, Type type) {


            if (source == null)
                return null;

            var delegates = source.GetInvocationList();


            int count = delegates.Length;

            if (count == 0)
                return null;

            if (count == 1)
                return Delegate.CreateDelegate(type, source.Target, source.Method, true);

            var new_delegates = new Delegate[count];

            for (int ii = 0; ii < count; ++ii) {

                var temp = delegates[ii];
                new_delegates[ii] = Delegate.CreateDelegate(type, temp.Target, temp.Method, true);

            }

            var ret = Delegate.Combine(new_delegates);

            return ret;

        }

        // http://blogs.msdn.com/zelmalki/archive/2008/12/12/reflection-fast-object-creation.aspx
        public static Delegate 
        CreateDelegate(this ConstructorInfo ctor, Type del_type) {


            if (ctor == null) {
                throw new ArgumentNullException("constructor");
            }

            if (del_type == null) {
                throw new ArgumentNullException("delegateType");
            }

            var ctor_declaring = ctor.DeclaringType;

            // Validate the delegate return type
            MethodInfo del_method = del_type.GetMethod("Invoke");

            if (del_method.ReturnType != ctor_declaring) {
                throw new InvalidOperationException("The return type of the delegate must match the constructors declaring type");
            }

            // Validate the signatures
            ParameterInfo[] delParams = del_method.GetParameters();
            ParameterInfo[] constructorParam = ctor.GetParameters();

            if (delParams.Length != constructorParam.Length) {
                throw new InvalidOperationException("The delegate signature does not match that of the constructor");
            }

            for (int ii = 0; ii < delParams.Length; ++ii) {
                                                                        
                if (delParams[ii].ParameterType != constructorParam[ii].ParameterType ||  // Probably other things we should check ??
                    delParams[ii].IsOut) {

                    throw new InvalidOperationException("The delegate signature does not match that of the constructor");

                }

            }

            string guid = Guid.NewGuid().ToString().Replace("-", "");
            string name = "{0}__{1}".spf(ctor.DeclaringType.Name, guid);

            Type[] types = Array.ConvertAll<ParameterInfo, Type>(constructorParam, p => p.ParameterType);

            // Create the dynamic method
            DynamicMethod method = new DynamicMethod(name,
                                                     ctor_declaring,
                                                     types,
                                                     true);

            // Create the il
            ILGenerator gen = method.GetILGenerator();

            for (int ii = 0; ii < constructorParam.Length; ii++) {
                if (ii < 4) {
                    switch (ii) {
                    case 0:
                        gen.Emit(OpCodes.Ldarg_0);
                        break;
                    case 1:
                        gen.Emit(OpCodes.Ldarg_1);
                        break;
                    case 2:
                        gen.Emit(OpCodes.Ldarg_2);
                        break;
                    case 3:
                        gen.Emit(OpCodes.Ldarg_3);
                        break;
                    }
                }
                else {
                    gen.Emit(OpCodes.Ldarg_S, ii);
                }
            }

            gen.Emit(OpCodes.Newobj, ctor);
            gen.Emit(OpCodes.Ret);

            // Return the delegate
            return method.CreateDelegate(del_type);

        }

        const BindingFlags All =
        BindingFlags.NonPublic | BindingFlags.Public |
        BindingFlags.Instance | BindingFlags.Static |
        BindingFlags.IgnoreCase;

        const BindingFlags Normal =
        BindingFlags.Public | BindingFlags.Static;

        static MethodInfo create_generic_delegate1_info;
        static MethodInfo create_generic_delegate2_info;


        static public MethodInfo 
        Create_Generic_Delegate1_Info {

            get {
                if (create_generic_delegate1_info == null) {

                    create_generic_delegate1_info = typeof(_Delegates).GetMethod("Create_Generic_Delegate1", Normal);
                    create_generic_delegate1_info.tifn();

                }

                return create_generic_delegate1_info;
            }
        }

        static public MethodInfo 
        Create_Generic_Delegate2_Info {
            get {
                if (create_generic_delegate2_info == null) {

                    create_generic_delegate2_info = typeof(_Delegates).GetMethod("Create_Generic_Delegate2", Normal);
                    create_generic_delegate2_info.tifn();

                }

                return create_generic_delegate2_info;
            }
        }


        static public Func<object, TReturn>
        Create_Generic_Delegate1<TTarget, TReturn>(MethodInfo mi)

        where TTarget : class {

            var func = (Func<TTarget, TReturn>)
                       Delegate.CreateDelegate(typeof(Func<TTarget, TReturn>), null, mi);

            return (caller) => func((TTarget)caller);
        }

        //        static public
        //Func<object, Func<TReturn>>

        //Create_Generic_Delegate1<TTarget, TReturn>(MethodInfo mi)

        //where TTarget : class {

        //            var func = (Func<TTarget, TReturn>)
        //                       Delegate.CreateDelegate(typeof(Func<TTarget, TReturn>), null, mi);

        //            return ((caller) =>
        //            {
        //                TTarget cast = (TTarget)caller;
        //                return () => func(cast);
        //            });
        //        }



        static public Func<object, Func<TParam, TReturn>>
            CreateGenericDelegate2<TTarget, TParam, TReturn>(MethodInfo mi)
            where TTarget : class {

            var func = (Func<TTarget, TParam, TReturn>)
                       Delegate.CreateDelegate(typeof(Func<TTarget, TParam, TReturn>), null, mi);

            return ((caller) =>
            {
                TTarget cast = (TTarget)caller;
                return (TParam par) => func(cast, par);
            });
        }

    }
}
