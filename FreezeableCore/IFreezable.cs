using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreezeExample
{
    internal interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
    }
}
