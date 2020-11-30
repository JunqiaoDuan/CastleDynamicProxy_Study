using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreezeExample.Exceptions;

namespace FreezeExample
{
    public static class Freezable
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();
        private static readonly IInterceptorSelector _selector = new FreezableInterceptorSelector();

        /// <summary>
        /// The proxied class must own a parameterless constructor
        /// </summary>
        /// <typeparam name="TFreezable"></typeparam>
        /// <returns></returns>
        public static TFreezable MakeFreezable<TFreezable>() where TFreezable : class, new()
        {
            var freezableInterceptor = new FreezableInterceptor();
            var options = new ProxyGenerationOptions(new FreezableProxyGenerationHook()) { Selector = _selector };
            var proxy = Generator.CreateClassProxy(typeof(TFreezable), options, new CallLoggingInterceptor(), freezableInterceptor);
            return proxy as TFreezable;
        }

        /// <summary>
        /// The proxied class must own a constructor with ctorArguments.
        /// </summary>
        /// <typeparam name="TFreezable"></typeparam>
        /// <param name="ctorArguments"></param>
        /// <returns></returns>
        public static TFreezable MakeFreezable<TFreezable>(params object[] ctorArguments) where TFreezable : class
        {
            var freezableInterceptor = new FreezableInterceptor();
            var options = new ProxyGenerationOptions(new FreezableProxyGenerationHook()) { Selector = _selector };
            var proxy = Generator.CreateClassProxy(typeof(TFreezable), new Type[0], options, ctorArguments,
                new CallLoggingInterceptor(), freezableInterceptor);
            return proxy as TFreezable;
        }
        
        public static bool IsFreezable(object obj)
        {
            return AsFreezable(obj) != null;
        }

        public static void Freeze(object freezable)
        {
            var interceptor = AsFreezable(freezable);
            if(interceptor == null)
                throw new NotFreezableObjectException(freezable);
            interceptor.Freeze();
        }
  
        public static bool IsFrozen(object obj)
        {
            var freezable = AsFreezable(obj);
            return freezable != null && freezable.IsFrozen;
        }

        private static IFreezable AsFreezable(object target)
        {
            if (target == null)
                return null;

            var hack = target as IProxyTargetAccessor;
            if (hack == null)
                return null;

            return hack.GetInterceptors().FirstOrDefault(i => i is FreezableInterceptor) as IFreezable;
        }

    }
}
