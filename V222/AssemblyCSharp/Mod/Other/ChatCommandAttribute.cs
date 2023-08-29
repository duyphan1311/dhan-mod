using System;

namespace AssemblyCSharp.Mod.Other
{
    public class ChatCommandAttribute : BaseCommandAttribute
    {
        public string command;

        public ChatCommandAttribute(string command)
        {
            this.command = command;
        }
    }
}