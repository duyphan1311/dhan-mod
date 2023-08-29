using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp.Mod.Other
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class BaseCommandAttribute: Attribute
    {
        public char delimiter = ' ';
    }
}
