using System;
using System.Threading;

namespace UglyBoy;

internal class SetDo
{
	public static Item[][] set = new Item[2][]
	{
		new Item[6],
		new Item[6]
	};

	public static int type = 1;

	public static int BAGTYPE = -1;

	public static void StartMenu()
	{
		MyVector myVector = new MyVector();
		ChatPopup.addChatPopup("Cài đặt và mặc set đồ", 10000, null);
		myVector.addElement(new Command("Setup", mPanel.gI(), 8001, null));
		myVector.addElement(new Command("Mặc set\n1", mPanel.gI(), 8002, 1));
		myVector.addElement(new Command("Mặc set\n2", mPanel.gI(), 8002, 2));
		GameCanvas.menu.startAt(myVector, GameCanvas.h - 3);
	}

	public static void macdo()
	{
		try
		{
			Item[] array = set[type - 1];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
					{
						if (array[i].template.type == Char.myCharz().arrItemBag[j].template.type && array[i].info == Char.myCharz().arrItemBag[j].info)
						{
							Service.gI().getItem((sbyte)BAGTYPE, (sbyte)j);
							break;
						}
					}
				}
				Thread.Sleep(200);
			}
			GameScr.info1.addInfo("Đã mặc set " + type, 0);
		}
		catch (Exception)
		{
		}
	}
}
