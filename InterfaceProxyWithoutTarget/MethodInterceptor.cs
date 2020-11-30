using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceProxyWithoutTarget
{
    internal class MethodInterceptor : IInterceptor
    {
        public readonly Delegate _impl; 
 
        public MethodInterceptor( Delegate @delegate )
        {
            this._impl = @delegate;
        } 
 
        public void Intercept( IInvocation invocation )
        {
            // Notice also that we do not call invocation.Proceed(). Since there’s
            // no real implementation to proceed to, this would be illegal.
            var result = this._impl.DynamicInvoke( invocation.Arguments );
            invocation.ReturnValue = result;
        }
    }
}
