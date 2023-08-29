using System;
using System.Threading;

namespace UglyBoy;

internal class AutoLogin
{
	public static bool autologin = false;

	private static bool wait = false;

	public static int waittime = 26000;

	private static AutoLogin instance;

	public string acc;

	public string pass;

	public int server = -1;

	public static AutoLogin gI()
	{
		return (instance != null) ? instance : (instance = new AutoLogin());
	}

	public static void Update()
	{
		while (true)
		{
			try
			{
				if (autologin && (GameCanvas.currentScreen == GameCanvas.loginScr || GameCanvas.currentScreen == GameCanvas.serverScreen))
				{
					Thread.Sleep(waittime);
					new Thread(gI().loadAccount).Start();
				}
				Thread.Sleep(500);
			}
			catch (Exception)
			{
			}
		}
	}

	public void loadAccount()
	{
		if (acc != string.Empty && pass != string.Empty && server != -1 && !ServerListScreen.isGetData)
		{
			Thread.Sleep(1500);
			if (GameCanvas.currentScreen == GameCanvas.loginScr || GameCanvas.currentScreen == GameCanvas.serverScreen)
			{
				if (GameCanvas.loginScr == null)
				{
					GameCanvas.loginScr = new LoginScr();
				}
				GameCanvas.loginScr.switchToMe();
				Thread.Sleep(500);
				if (server != ServerListScreen.ipSelect)
				{
					GameCanvas.serverScr.perform(100 + server, null);
					Thread.Sleep(500);
				}
				Thread.Sleep(700);
				Service.gI().login(acc, pass, GameMidlet.VERSION, 0);
				if (Session_ME.connected)
				{
					GameCanvas.startWaitDlg();
				}
				else
				{
					GameCanvas.startOKDlg(mResources.maychutathoacmatsong);
				}
			}
		}
		else if (GameCanvas.currentScreen == GameCanvas.loginScr || GameCanvas.currentScreen == GameCanvas.serverScreen)
		{
			GameCanvas.serverScreen.perform(3, null);
		}
	}
}
