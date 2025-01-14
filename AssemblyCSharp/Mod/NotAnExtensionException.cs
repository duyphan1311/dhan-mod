using System;

namespace Mod
{
    public class NotAnExtensionException : Exception
    {
        public NotAnExtensionException() : base() { }

        public NotAnExtensionException(string message) : base(message) { }
    }
}
