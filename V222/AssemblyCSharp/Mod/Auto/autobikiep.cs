using System;
using System.Threading;

namespace UglyBoy;

internal class autobikiep
{
	public static bool isautobikiep = false;

	public static long lastTimeNext = 0L;

	public static bool ispause = false;

	public static int ttnlIndex = -1;

	public static int khienIndex = -1;

	public static void update()
	{
		while (true)
		{
			try
			{
				if (isautobikiep)
				{
					if (khienIndex != -1 && Ugly.gI().canUseSkill(GameScr.keySkill[khienIndex]) && !Char.myCharz().isCharge)
					{
						GameScr.gI().doSelectSkill(GameScr.keySkill[khienIndex], isShortcut: true);
						GameScr.gI().doSelectSkill(GameScr.keySkill[khienIndex], isShortcut: true);
						if (GameScr.keySkill[2].coolDown > 1500)
						{
							GameScr.gI().auto = 10;
						}
						Thread.Sleep(500);
						GameScr.gI().doSelectSkill(GameScr.keySkill[0], isShortcut: true);
					}
					if (Char.myCharz().cgender == 2)
					{
						if (((double)Char.myCharz().cHP <= (double)Char.myCharz().cHPFull * 0.4 || (double)Char.myCharz().cMP <= (double)Char.myCharz().cMPFull * 0.3) && ttnlIndex != -1 && Ugly.gI().canUseSkill(GameScr.keySkill[ttnlIndex]))
						{
							GameScr.gI().doSelectSkill(GameScr.keySkill[ttnlIndex], isShortcut: true);
							GameScr.gI().doSelectSkill(GameScr.keySkill[ttnlIndex], isShortcut: true);
							if (GameScr.keySkill[2].coolDown > 1500)
							{
								GameScr.gI().auto = 10;
							}
							ispause = true;
						}
						Thread.Sleep(500);
					}
					if (!Char.myCharz().isMeCanAttackOtherPlayer(Char.myCharz().charFocus) || mSystem.currentTimeMillis() - lastTimeNext > 5000 || Char.myCharz().charFocus == null)
					{
						Char.myCharz().findNextFocusByKey();
						lastTimeNext = mSystem.currentTimeMillis();
					}
					if (!Char.myCharz().isCharge && ispause && Char.myCharz().myskill != GameScr.keySkill[0])
					{
						GameScr.gI().doSelectSkill(GameScr.keySkill[0], isShortcut: true);
						ispause = false;
					}
				}
				Thread.Sleep(100);
			}
			catch (Exception)
			{
			}
		}
	}
}
