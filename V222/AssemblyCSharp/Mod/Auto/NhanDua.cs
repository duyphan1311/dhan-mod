using System;
using System.Threading;

namespace UglyBoy;

internal class NhanDua
{
	public static bool isauto;

	public static void vuahung()
	{
		while (true)
		{
			try
			{
				if (isauto && GameScr.findNPCInMap(52) != null)
				{
					Service.gI().openMenu(52);
					Thread.Sleep(500);
					Service.gI().confirmMenu(52, 0);
					Thread.Sleep(700);
				}
				else if (isauto && GameScr.findNPCInMap(52) == null)
				{
					GameScr.info1.addInfo("Không tìm thấy vua hùng", 0);
					isauto = false;
				}
				Thread.Sleep(100);
			}
			catch (Exception)
			{
			}
		}
	}
}
