using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UglyBoy;

internal class AutoSell
{
	public static MyVector vItem = new MyVector();

	public static List<CharSell> vChar = new List<CharSell>();

	public static int charId;

	public static int soluong;

	public static int gia;

	public static string chat;

	public static int con;

	private static string[][] boxGD = new string[3][]
	{
		mResources.inventory,
		mResources.item_give,
		mResources.item_receive
	};

	public static void xulygiaodich()
	{
		while (true)
		{
			try
			{
				if (Ugly.autosell && Ugly.trading && vItem.size() > 0)
				{
					_ = gia;
					if (true && GameCanvas.panel2 != null && GameCanvas.panel2.isFriendLock)
					{
						if (GameCanvas.panel2.friendMoneyGD >= gia)
						{
							if (GameCanvas.panel.isLock)
							{
								break;
							}
							int num = Mathf.FloorToInt((float)GameCanvas.panel2.friendMoneyGD / (float)gia);
							for (int i = 0; i < vItem.size(); i++)
							{
								Item item = (Item)vItem.elementAt(i);
								int num2 = item.quantity * num;
								Item item2 = Ugly.gI().findItemBag(item.template.id);
								if (num2 > item2.quantity)
								{
									num2 = item2.quantity;
								}
								if (item2 != null)
								{
									Item item3 = new Item();
									item3.template = item2.template;
									item3.itemOption = item2.itemOption;
									item3.indexUI = item2.indexUI;
									GameCanvas.panel.vMyGD.addElement(item3);
									Service.gI().giaodich(2, -1, (sbyte)item3.indexUI, num2);
									Thread.Sleep(200);
								}
							}
							Thread.Sleep(500);
							Service.gI().giaodich(5, -1, -1, -1);
							Thread.Sleep(1000);
							Service.gI().giaodich(7, -1, -1, -1);
							Ugly.trading = false;
						}
						else
						{
							Service.gI().giaodich(3, -1, -1, -1);
							Ugly.trading = false;
							CharSell charSell = getCharSell(charId);
							if (charSell != null)
							{
								charSell.cout++;
							}
							else
							{
								charSell = new CharSell(charId, 1);
								vChar.Add(charSell);
							}
						}
					}
				}
				if (vChar.Count > 0)
				{
					for (int j = 0; j < vChar.Count; j++)
					{
						CharSell charSell2 = vChar[j];
						if (charSell2.cout >= 5 && mSystem.currentTimeMillis() - charSell2.timeStart > 300000)
						{
							vChar.RemoveAt(j);
						}
					}
				}
				Thread.Sleep(100);
			}
			catch (Exception)
			{
			}
		}
	}

	public static void ChatRieng(InfoItem infoC)
	{
		if (infoC.isChatServer && !Ugly.autosell)
		{
		}
	}

	public static void addInfo(string c)
	{
		if (c.ToLower().IndexOf("chờ đối phương đồng ý") != -1)
		{
			Ugly.trading = false;
		}
		if (c.ToLower().IndexOf("giao dịch thành công") != -1)
		{
			Ugly.trading = false;
			con = Ugly.gI().findItemBag(((Item)vItem.elementAt(0)).template.id).quantity;
			if (vItem.size() > 1)
			{
				for (int i = 0; i < vItem.size(); i++)
				{
					Item item = (Item)vItem.elementAt(i);
					int quantity = Ugly.gI().findItemBag(item.template.id).quantity;
					if (quantity < con)
					{
						con = quantity;
					}
				}
			}
		}
		if (c.ToLower().IndexOf("giao dịch bị hủy bỏ") == -1)
		{
			return;
		}
		Ugly.trading = false;
		CharSell charSell = getCharSell(charId);
		if (charSell != null)
		{
			charSell.cout++;
			if (charSell.cout >= 5)
			{
				charSell.timeStart = mSystem.currentTimeMillis();
			}
		}
		else
		{
			charSell = new CharSell(charId, 1);
			vChar.Add(charSell);
		}
	}

	public static void StartTrading(int c)
	{
		CharSell charSell = getCharSell(c);
		if (charSell == null || charSell.cout < 5)
		{
			Char p = GameScr.findCharInMap(c);
			GameScr.gI().actionPerform(11114, p);
			charId = c;
			Ugly.trading = true;
		}
		else if (charSell.cout >= 5)
		{
			Service.gI().chatPlayer("bạn bị chặn 5 phút.", c);
		}
	}

	private static CharSell getCharSell(int c)
	{
		for (int i = 0; i < vChar.Count; i++)
		{
			if (vChar[i].charId == c)
			{
				return vChar[i];
			}
		}
		return null;
	}

	public static bool checkChar(int c)
	{
		for (int i = 0; i < vChar.Count; i++)
		{
			if (vChar[i].charId == c)
			{
				return true;
			}
		}
		return false;
	}
}
