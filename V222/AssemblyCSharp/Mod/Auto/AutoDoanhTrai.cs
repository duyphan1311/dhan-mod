using System;
using System.Threading;

namespace UglyBoy;

internal class AutoDoanhTrai
{
	public static bool autodt;

	public static void update()
	{
		autodt = !autodt;
		try
		{
			while (autodt)
			{
				if (autodt && TileMap.mapName.ToLower().Equals("rừng bamboo"))
				{
					Thread.Sleep(700);
					Service.gI().openMenu(25);
					Thread.Sleep(1000);
					Service.gI().confirmMenu(25, 0);
				}
				Thread.Sleep(500);
			}
		}
		catch (Exception)
		{
		}
	}
}
