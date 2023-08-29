using System;

namespace AssemblyCSharp.Mod.Other
{
    public class HotkeyCommandAttribute : BaseCommandAttribute
    {
        public char key;
        public string agrs = "";

        public HotkeyCommandAttribute(char key)
        {
            this.key = key;
        }
    }
}