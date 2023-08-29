using AssemblyCSharp.Mod.PickMob;
using AssemblyCSharp.Mod.Xmap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace UglyBoy;

internal class AutoUpGrade
{
	public static bool isupgrade;

	public static int sosao;

	public static int mapHome;

	public static int npcHome = 13;

	public static bool IsNhanVang = false;

	public static bool IsShowListUpgrade = false;

	public static ItemUpgrade currItemUpgrade = new ItemUpgrade();

	public static List<ItemUpgrade> listUpgrade = new List<ItemUpgrade>();

	public struct ItemUpgrade
	{
		public string info;
		public int type;
		public int id;
		public string name;

		public ItemUpgrade(int id, string info, int type, string name)
		{
			this.id = id;
			this.info = info;
			this.type = type;
			this.name = name;
		}
	}

	public static Item FindItemUpgrade(ItemUpgrade item)
	{
		Item[] arrItemBag = Char.myCharz().arrItemBag;
		for (int i = 0; i < arrItemBag.Length; i++)
		{
			if (arrItemBag[i] == null) continue;
			if (arrItemBag[i].template.type == item.type && i == item.id && item.name == arrItemBag[i].template.name)
			{
				return arrItemBag[i];
			}
		}
		return null;
	}

	public static bool fl = true;

	public static int idSel;

	public static bool isSaleGoldToUpgrade = false;

	public static bool isWait = false;

	public static int goldSell = int.Parse(File.ReadAllText("Data\\goldSell.txt"));
	public static void update()
	{
		//while (isupgrade)
		//{
		//try
		//{
		if (GameCanvas.panel.vItemCombine.size() > 0 || listUpgrade.Count > 0)
		{
			if (Char.myCharz().xu <= goldSell || isWait)
			{
				isupgrade = false;
				IsShowListUpgrade = false;
				if (IsNhanVang)
				{
					if (OnScreen.IsNVV1)
					{
						switch (Char.myCharz().nClass.classId)
						{
							case 0:
								mapHome = 21;
								OnScreen.npcNV = 0;
								break;
							case 1:
								mapHome = 22;
								OnScreen.npcNV = 2;
								break;
							default:
								mapHome = 23;
								OnScreen.npcNV = 1;
								break;
						}
						if(TileMap.mapID != mapHome && !isWait)
                        {
							XmapController.StartRunToMapId(mapHome);
						}
					}
					if(!isWait)
                    {
						Npc npc = GameScr.findNPCInMap((short)OnScreen.npcNV);
						if (npc != null) XmapController.MoveMyChar(npc.cx, npc.cy);
						for (int i = 0; i < 4; i++)
						{
							Service.gI().openMenu(OnScreen.npcNV);
							Service.gI().confirmMenu((short)OnScreen.npcNV, (sbyte)OnScreen.npcNVCF);
							if (PickMob.isNVdd)
								Service.gI().confirmMenu((short)OnScreen.npcNV, (sbyte)OnScreen.npcNVCF);
						}
					}
					if (OnScreen.IsNVV1)
                    {
						if (TileMap.mapID != 5)
						{
							XmapController.StartRunToMapId(5);
							isWait = true;
							isupgrade = true;
							return;
						}
					}
                    Npc npc2 = GameScr.findNPCInMap(21);
					if (npc2 != null) XmapController.MoveMyChar(npc2.cx, npc2.cy);
					Service.gI().openMenu(21);
					Service.gI().confirmMenu(21, 1);
					Service.gI().confirmMenu(21, 1);
					isupgrade = true;
					IsShowListUpgrade = true;
					isWait = false;
				}
				else if (isSaleGoldToUpgrade)
				{
					PickMobController.solanSale = 4;
					PickMobController.timeSaleGold = 1000;
					PickMobController.lastTimeSaleGold = mSystem.currentTimeMillis();
					PickMobController.isBanVang = true;
					Npc npc2 = GameScr.findNPCInMap(21);
					if (npc2 != null) XmapController.MoveMyChar(npc2.cx, npc2.cy);
					Service.gI().openMenu(21);
					Service.gI().confirmMenu(21, 1);
					Service.gI().confirmMenu(21, 1);
					isupgrade = true;
					IsShowListUpgrade = true;
				}
				else
                {
					GameScr.info1.addInfo("Thiếu vàng", 0);
					return;
                }
			}
			Item item;
			if (PickMob.isNVdd)
				item = (Item)GameCanvas.panel.vItemCombine.elementAt(0);
			else
			{
				if (listUpgrade.Count > 0)
				{
					item = FindItemUpgrade(listUpgrade[0]);
					currItemUpgrade = listUpgrade[0];
				}
				else
				{
					item = (Item)GameCanvas.panel.vItemCombine.elementAt(0);
				}
			}
			if(item == null)
			{
				isupgrade = false;
				IsShowListUpgrade = false;
				GameScr.info1.addInfo("Chưa có item nâng cấp", 0);
				return;
			}
			int maxStart = getMaxStart(item);
			GameScr.info1.addInfo(maxStart.ToString(), 0);
			if (maxStart >= sosao)
			{
				listUpgrade.RemoveAt(0);
				GameCanvas.panel.vItemCombine.removeElementAt(0);
				if (GameCanvas.panel.vItemCombine.size() <= 0 && listUpgrade.Count <= 0)
				{
					isupgrade = false;
					IsShowListUpgrade = false;
					GameScr.info1.addInfo("Xong", 0);
					GameCanvas.panel.hide();
					//break;
					return;
				}
				if (PickMob.isNVdd)
					item = (Item)GameCanvas.panel.vItemCombine.elementAt(0);
				else
				{
					if (listUpgrade.Count > 0)
					{
						item = FindItemUpgrade(listUpgrade[0]);
						currItemUpgrade = listUpgrade[0];
					}
					else
					{
						item = (Item)GameCanvas.panel.vItemCombine.elementAt(0);
					}
				}
			}
            if (isMKTB)
            {
				if (currStar < maxStart)
				{
					Service.gI().openMenu(21);
					Service.gI().confirmMenu(21, 6);
					MyVector myVectorq = new MyVector();
					myVectorq.addElement(item);
					sbyte actionq = 1;
					Service.gI().combine(actionq, myVectorq);
					Service.gI().confirmMenu(21, 0);
					currStar = maxStart;

					Service.gI().openMenu(21);
					Service.gI().confirmMenu(21, 1);
					Service.gI().confirmMenu(21, 1);
				}
				if (currStar > maxStart)
				{
					currStar = maxStart - 1;
				}
			}
			MyVector myVector = new MyVector();
			myVector.addElement(item);
			sbyte action = 1;
			if (PickMob.isNVdd) action = 0;
			Service.gI().combine(action, myVector);
			//Thread.Sleep(500);
			Service.gI().confirmMenu(21, 0);
			//Thread.Sleep(1500);
			//continue;
		}
		else
		{
			isupgrade = false;
			IsShowListUpgrade = false;
			GameScr.info1.addInfo("Bạn cần chọn vật phẩm", 0);
		}
		//break;
		//return;
		//}
		//catch (Exception)
		//{
		//}
		//}
	}
	public static bool isKham = false;
	public static List<Item> listKham = new();
	public static void updateA()
	{
		if (listKham.Count > 0)
		{
			if (Char.myCharz().luong <= 10)
			{
				isKham = false;
				GameScr.info1.addInfo("Thiếu ngọc", 0);
				return;
			}
			Item item;
			Item item2;
			if (listKham[1].template.type == 5) 
			{
				item = listKham[1];
				item2 = listKham[0];
			}
            else
            {
				item = listKham[0];
				item2 = listKham[1];
			}
			if (item == null || item2 == null)
			{
				isKham = false;
				GameScr.info1.addInfo("Chưa có item khảm", 0);
				return;
			}
			if (getStartSlot(item2) == getMaxStart(item2))
			{
				listKham.RemoveAt(0);
				if (listKham.Count <= 1)
				{
					isKham = false;
					listKham.Clear();
					GameScr.info1.addInfo("Xong", 0);
					return;
				}
				if (listKham[1].template.type == 5)
				{
					item = listKham[1];
					item2 = listKham[0];
				}
				else
				{
					item = listKham[0];
					item2 = listKham[1];
				}
			}
			MyVector myVector = new MyVector();
			myVector.addElement(item);
			myVector.addElement(item2);
			sbyte action = 1;
			if (PickMob.isNVdd) action = 0;
			Service.gI().combine(action, myVector);
			Service.gI().confirmMenu(21, 0);
		}
		else
		{
			isKham = false;
			GameScr.info1.addInfo("Bạn cần chọn vật phẩm", 0);
		}
	}

	public static bool isKham2 = false;
	public static int idSPL;

	public static void updateB()
	{
		if (listUpgrade.Count > 0)
		{
			if (Char.myCharz().luong <= 10)
			{
				isKham2 = false;
				GameScr.info1.addInfo("Thiếu ngọc", 0);
				return;
			}
			Item item = FindItemUpgrade(listUpgrade[0]);
			Item item2 = null;
			if (UseItem.findItem((short)idSPL) != -1)
				item2 = global::Char.myCharz().arrItemBag[(int)UseItem.findItem((short)idSPL)];
			if (item == null || item2 == null)
			{
				isKham2 = false;
				GameScr.info1.addInfo("Chưa có item khảm", 0);
				return;
			}
			if (getStartSlot(item) == getMaxStart(item))
			{
				listUpgrade.RemoveAt(0);
				if (listUpgrade.Count <= 0)
				{
					isKham2 = false;
					listUpgrade.Clear();
					GameScr.info1.addInfo("Xong", 0);
					GameCanvas.panel.hide();
					return;
				}
				item = FindItemUpgrade(listUpgrade[0]);
			}
			MyVector myVector = new MyVector();
			myVector.addElement(item);
			myVector.addElement(item2);
			Service.gI().combine(1, myVector);
			Service.gI().confirmMenu(21, 0);
		}
		else
		{
			isKham2 = false;
			GameScr.info1.addInfo("Bạn cần chọn vật phẩm", 0);
		}
	}

	public static int getStartSlot(Item item)
	{
		int result = 0;
		for (int i = 0; i < item.itemOption.Length; i++)
		{
			if (item.itemOption[i].optionTemplate.id == 102)
			{
				result = item.itemOption[i].param;
			}
		}
		return result;
	}

	public static int currStar = 0;
	public static bool isMKTB = false;
	public static int getMaxStart(Item item)
	{
		int result = 0;
		for (int i = 0; i < item.itemOption.Length; i++)
		{
			if (item.itemOption[i].optionTemplate.id == 107)
			{
				result = item.itemOption[i].param;
			}
		}
		return result;
	}
	public static List<ItemUpgrade> listShow = new();
	public static void ShowListUpgrade()
	{
		listShow.Clear();
		foreach (ItemUpgrade item in listUpgrade)
		{
			listShow.Add(item);
		}
	}

	public static void PaintListUpgrade(mGraphics g)
	{
		int x = GameCanvas.w / 2;
		int y;
		y = isSaleGoldToUpgrade ? 65 : 55;
		if(isSaleGoldToUpgrade)
        {
			mFont.tahoma_7b_yellow.drawString(g, "Danh sách item nâng cấp (" + sosao + " sao)", x, y - 40, mFont.CENTER, mFont.tahoma_7_greySmall);
			mFont.tahoma_7b_yellow.drawString(g, "Ngọc xanh: " + Char.myCharz().luongStr + " - Ngọc hồng: " + Char.myCharz().luongKhoaStr, x, y - 30, mFont.CENTER, mFont.tahoma_7_greySmall);
			mFont.tahoma_7b_yellow.drawString(g, "Vàng: " + Char.myCharz().xuStr + " - Thỏi vàng: " + NinjaUtil.getMoneys(FindQuatity(457)), x, y - 20, mFont.CENTER, mFont.tahoma_7_greySmall);
			mFont.tahoma_7b_yellow.drawString(g, "Bán thỏi vàng khi <" + NinjaUtil.getMoneys(goldSell), x, y - 10, mFont.CENTER, mFont.tahoma_7_greySmall);
		}
		else
        {
			mFont.tahoma_7b_yellow.drawString(g, "Danh sách item nâng cấp (" + sosao + " sao)", x, y - 30, mFont.CENTER, mFont.tahoma_7_greySmall);
			mFont.tahoma_7b_yellow.drawString(g, "Ngọc xanh: " + Char.myCharz().luongStr + " - Ngọc hồng: " + Char.myCharz().luongKhoaStr, x, y - 20, mFont.CENTER, mFont.tahoma_7_greySmall);
			mFont.tahoma_7b_yellow.drawString(g, "Vàng: " + Char.myCharz().xuStr + " - Thỏi vàng: " + NinjaUtil.getMoneys(FindQuatity(457)), x, y - 10, mFont.CENTER, mFont.tahoma_7_greySmall);
		}
		foreach (ItemUpgrade item in listShow)
		{
			Item it = FindItemUpgrade(item);
            if(it != null)
            {
				if (item.type == currItemUpgrade.type && currItemUpgrade.id == item.id && item.name == currItemUpgrade.name)
					mFont.tahoma_7b_red.drawString(g, it.template.name + ": " + getMaxStart(findItemBagWithIndexUI(it.indexUI)).ToString() + " sao", x, y, mFont.CENTER, mFont.tahoma_7_greySmall);
				else
					mFont.tahoma_7b_white.drawString(g, it.template.name + ": " + getMaxStart(findItemBagWithIndexUI(it.indexUI)).ToString() + " sao", x, y, mFont.CENTER, mFont.tahoma_7_greySmall);
				y += 10;
			}
		}
	}

	public static string nameSKH;
	public static string nameTypeSKH;
	public static bool CheckSKH(Item item)
	{
		for (int i = 0; i < item.itemOption.Length; i++)
		{
			if (item.itemOption[i].optionTemplate.name.StartsWith("$"))
			{
                nameTypeSKH = item.template.type switch
                {
                    1 => "Quần",
                    2 => "Găng",
                    3 => "Giày",
                    4 => "Rada",
                    _ => "Áo",
                };
				nameSKH = nameTypeSKH + " " + NinjaUtil.replace(item.itemOption[i - 1].optionTemplate.name, "Set ", string.Empty);
				return true;
			}
		}
		return false;
	}

	public static int NgocRong;
	public static int SoLuongEp;
	public static int SoNRBanDau = 0;
	public static int SoNRHienTai = 0;
	public static bool isEpNR = false;
	public static int idBag = 0;

	public static void EpNR()
	{
		if (SoNRBanDau + SoLuongEp <= FindQuatity((short)NgocRong))
		{
			isEpNR = false;
			idBag = 0;
			GameScr.info1.addInfo("Xong", 0);
			return;
		}
		if (idBag >= global::Char.myCharz().arrItemBag.Length)
		{
			isEpNR = false;
			idBag = 0;
			GameScr.info1.addInfo("Không tìm thấy ngọc rồng " + (NgocRong - 12), 0);
			return;
		}
		if (global::Char.myCharz().arrItemBag[idBag] == null)
		{
			idBag++;
			return;
		}
		if (global::Char.myCharz().arrItemBag[idBag].template.id == (short)(NgocRong + 1)
			&& global::Char.myCharz().arrItemBag[idBag].quantity < 7
		)
		{
			idBag++;
			return;
		}
		if (global::Char.myCharz().arrItemBag[idBag].template.id == (short)(NgocRong + 1))
		{
			GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag]);
			MyVector myVector = new MyVector();
			myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
			Service.gI().combine(1, myVector);
			Service.gI().confirmMenu(21, 0);
			GameCanvas.panel.vItemCombine.removeAllElements();
			return;
		}
		idBag++;
	}

	public static int idDaNangCap;
	public static int idDaBaoVe = 987;
	public static int idBag1 = 0;
	public static int idBag2 = 0;
	public static bool isNCTB = false;
	public static bool isUseDBV = false;

	public static void NCTB()
    {
		if (idBag2 >= global::Char.myCharz().arrItemBag.Length)
		{
			isNCTB = false;
			idBag2 = 0;
			GameScr.info1.addInfo("Không tìm thấy đá nâng cấp", 0);
			return;
		}
		if (global::Char.myCharz().arrItemBag[idBag2] == null)
		{
			idBag2++;
			return;
		}
		if (global::Char.myCharz().arrItemBag[idBag2].template.id == (short)idDaNangCap
			&& global::Char.myCharz().arrItemBag[idBag2].quantity < 25
		)
		{
			idBag2++;
			return;
		}
        if (isUseDBV)
        {
			if (idBag1 >= global::Char.myCharz().arrItemBag.Length)
			{
				isNCTB = false;
				idBag1 = 0;
				GameScr.info1.addInfo("Không tìm thấy đá bảo vệ", 0);
				return;
			}
			if (global::Char.myCharz().arrItemBag[idBag1] == null)
			{
				idBag1++;
				return;
			}
		}
		if (global::Char.myCharz().arrItemBag[idBag2].template.id == (short)idDaNangCap)
		{
			Item item = FindItemUpgrade(listUpgrade[0]);
			if (isUseDBV)
			{
				if (global::Char.myCharz().arrItemBag[idBag1].template.id != (short)idDaBaoVe) {
					idBag1++;
					return;
				}
				GameCanvas.panel.vItemCombine.addElement(item);
				GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag2]);
				GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag1]);
				MyVector myVector = new MyVector();
				myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
				myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
				myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(2));
				Service.gI().combine(1, GameCanvas.panel.vItemCombine);
				Service.gI().confirmMenu(21, 0);
				GameCanvas.panel.vItemCombine.removeAllElements();
				return;
			}
			GameCanvas.panel.vItemCombine.addElement(item);
			GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag2]);
			MyVector myVector1 = new MyVector();
			myVector1.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
			myVector1.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
			Service.gI().combine(1, myVector1);
			Service.gI().confirmMenu(21, 0);
			GameCanvas.panel.vItemCombine.removeAllElements();
			return;
		}
		idBag2++;
	}

	public static void GotoNPC21()
	{
		Thread.Sleep(500);
		GameCanvas.menu.doCloseMenu();
		ChatTextField.gI().isShow = false;
		ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
		ChatTextField.gI().strChat = "Chat ";
		ChatTextField.gI().tfChat.name = "chat";
		Npc npc2 = GameScr.findNPCInMap(21);
		if (npc2 != null) XmapController.MoveMyChar(npc2.cx, npc2.cy);
		Service.gI().openMenu(21);
		Thread.Sleep(500);
		sbyte id = 0;
		while (id < GameCanvas.menu.menuItems.size())
		{
			Command command = (Command)GameCanvas.menu.menuItems.elementAt((int)id);
			if (command != null && command.caption.ToLower().Contains("ngọc rồng"))
			{
				Service.gI().confirmMenu(21, id);
				GameCanvas.menu.doCloseMenu();
				AutoUpGrade.isEpNR = true;
				return;
			}
			id++;
		}
	}

	public static int FindQuatity(short id)
	{
		sbyte itemID = 0;
		int sl = 0;
		while (itemID < global::Char.myCharz().arrItemBag.Length)
		{
			if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
			{
				itemID++;
				continue;
			}
			if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == id)
			{
				sl += global::Char.myCharz().arrItemBag[(int)itemID].quantity;
			}
			itemID++;
		}
		return sl;
	}

	public static void AddChatPopup(string[] says)
	{
		if (!isupgrade)
		{
			return;
		}
		for (int i = 0; i < says.Length; i++)
		{
			if (says[i].ToLower().Contains("cần"))
			{
				string[] array = says[i].Split('|');
				int num = int.Parse(array[1]);
				if (num == 3 || num == 7)
				{
					GameScr.info1.addInfo("Thiếu vàng rồi dmm", 0);
					isupgrade = false;
					break;
				}
			}
		}
	}

	public static Item findItemBagWithIndexUI(int index)
	{
		Item[] arrItemBag = Char.myCharz().arrItemBag;
		foreach (Item item in arrItemBag)
		{
			if (item != null && item.indexUI == index)
			{
				return item;
			}
		}
		return null;
	}
}
