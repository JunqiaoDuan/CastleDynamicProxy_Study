using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace InterfaceProxyWithoutTarget
{
    public class DelegateWrapper
    {
        public static T WrapAs<T>(Delegate impl) where T : class
        {
            var generator = new ProxyGenerator();
            var proxy = generator.CreateInterfaceProxyWithoutTarget( typeof( T ), new MethodInterceptor( impl ) );
            return (T) proxy;
        }

        public static TInterface WrapAs<TInterface>(Delegate d1, Delegate d2)
        {
            var generator = new ProxyGenerator();
            var options = new ProxyGenerationOptions { Selector = new DelegateSelector() };
            var proxy = generator.CreateInterfaceProxyWithoutTarget(
                typeof( TInterface ),
                new Type[0],
                options,
                new MethodInterceptor( d1 ),
                new MethodInterceptor( d2 ) );
            return (TInterface) proxy;
        }

    }


    internal class DelegateSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            foreach (var interceptor in interceptors)
            {
                var methodInterceptor = interceptor as MethodInterceptor;
                if (methodInterceptor == null)
                    continue;
                var d = methodInterceptor._impl;
                if (IsEquivalent(d, method))
                    return new[] { interceptor };
            }
            throw new ArgumentException();
        } 
 
        private static bool IsEquivalent(Delegate d, MethodInfo method)
        {
            var dm = d.Method;
            if (!method.ReturnType.IsAssignableFrom(dm.ReturnType))
                return false;
            var parameters = method.GetParameters();
            var dp = dm.GetParameters();
            if (parameters.Length != dp.Length)
                return false;
            for (int i = 0; i < parameters.Length; i++)
            {
                //BUG: does not take into account modifiers (like out, ref...)
                if (!parameters[i].ParameterType.IsAssignableFrom(dp[i].ParameterType))
                    return false;
            }
            return true;
        }
    } 

}
