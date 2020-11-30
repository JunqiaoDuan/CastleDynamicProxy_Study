using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace FreezeExample
{
    public class FreezableProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {

        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            var method = memberInfo as MethodInfo; 
            if(method!=null) 
            { 
                this.ValidateNotSetter( method ); 
            } 
        }
        
        private void ValidateNotSetter(MethodInfo method) 
        { 
            if( method.IsSpecialName && IsSetterName( method.Name ) ) 
                throw new InvalidOperationException( 
                    string.Format( 
                        "Property {0} is not virtual. Can't freeze classes with non-virtual properties.", 
                        method.Name.Substring( "set_".Length ) 
                    ) 
                ); 
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo memberInfo)
        {
            return memberInfo.IsSpecialName &&
                   ( IsSetterName( memberInfo.Name ) ||
                     IsGetterName( memberInfo.Name ) );
        }
  
        private bool IsGetterName( string name )
        {
            return name.StartsWith( "get_", StringComparison.Ordinal );
        }
  
        private bool IsSetterName( string name )
        {
            return name.StartsWith("set_", StringComparison.Ordinal);
        }
    }
}
