using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceProxyWithoutTarget
{
    public interface IDeepThought
    {
        void SetAnsweringEngine(IAnsweringEngine answeringEngine);
    } 
}
