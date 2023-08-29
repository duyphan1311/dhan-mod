using AssemblyCSharp.Mod.Other;

namespace AssemblyCSharp.Mod.Options
{
    internal class ServerUse
    {
		public static string hostDefault;

		public static int port;

		static ServerUse()
		{
			hostDefault = Utilities.host;
			port = Utilities.port;
		}
	}
}
