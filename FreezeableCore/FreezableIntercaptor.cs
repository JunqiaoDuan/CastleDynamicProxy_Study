using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using FreezeExample.Exceptions;

namespace FreezeExample
{
    internal class FreezableInterceptor : IInterceptor, IFreezable
    {
        public void Freeze()
        {
            IsFrozen = true;
        }
 
        public bool IsFrozen { get; private set; }
 
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Freezable interceptor. ");
            if (IsFrozen && IsSetter(invocation.Method))
            {
                throw new ObjectFrozenException();
            }
 
            invocation.Proceed();
        }
 
        private static bool IsSetter(MethodInfo method)
        {
            return method.IsSpecialName && method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase);
        }
    }
}
