using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AssemblyCSharp.Mod.Auto
{
    internal class ALogin
    {
		public static string Account;
		public static string Password;
		public static int Server = -1;
		public static bool isLogin;
		public static void Login()
		{
			Other.GameEvents.onLogin(ref Account, ref Password, ref Server);
			while (!ServerListScreen.loadScreen)
			{
				Thread.Sleep(10);
			}

			if (Account != null)
			{
				isLogin = true;
				Rms.saveRMSString("acc", Account);
				Rms.saveRMSString("pass", Password);
				if (Rms.loadRMSInt("svselect") != Server)
				{
					Rms.saveRMSInt("svselect", Server);
					ServerListScreen.ipSelect = Server;
					GameCanvas.serverScreen.selectServer();
				}
				while (!Session_ME.gI().isConnected())
				{
					Thread.Sleep(100);
				}
				Thread.Sleep(500);
				if (GameCanvas.loginScr == null)
				{
					GameCanvas.loginScr = new LoginScr();
				}
				GameCanvas.loginScr.switchToMe();
				GameCanvas.loginScr.doLogin();

			}
		}
	}
}
