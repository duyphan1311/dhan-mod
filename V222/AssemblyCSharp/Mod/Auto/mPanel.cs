using AssemblyCSharp.Mod.PickMob;
using System;
using System.Threading;

namespace UglyBoy;

internal class mPanel : IActionListener, IChatable
{
	public static int type = 1;

	public static string putsoluong = "Nhập Số Lượng Để Bot Giao Dịch";

	public static string putgia = "Nhập Giá Để Bot Giao Dịch";

	public static string putchat = "Nhập Nội Dung Bán";

	public static string noitai;

	private static mPanel instance;

	public static mPanel gI()
	{
		return (instance != null) ? instance : (instance = new mPanel());
	}

	public static void setTypeSell()
	{
		int num = (GameCanvas.panel.type = 27);
		type = 1;
		GameCanvas.panel.tabName[num] = new string[2][]
		{
			mResources.inventory,
			new string[2] { "Item", "Bán" }
		};
		GameCanvas.panel.setType(0);
		if (GameCanvas.panel.currentTabIndex == 0)
		{
			GameCanvas.panel.setTabInventory();
		}
		if (GameCanvas.panel.currentTabIndex == 1)
		{
			setTabAutoSell();
		}
		GameCanvas.panel.show();
	}

	public void setTypeSetting()
	{
		int num = (GameCanvas.panel.type = 27);
		type = 3;
		//MyMenu.gI().loadSetting();
		GameCanvas.panel.tabName[num] = new string[1][] { new string[2] { "Setting", "" } };
		GameCanvas.panel.setType(0);
		if (GameCanvas.panel.currentTabIndex == 0)
		{
			setTabSetting();
		}
		GameCanvas.panel.show();
	}

	public static void setTypeSetDo()
	{
		int num = (GameCanvas.panel.type = 27);
		type = 2;
		GameCanvas.panel.tabName[num] = new string[3][]
		{
			mResources.inventory,
			new string[2] { "Set đồ", "1" },
			new string[2] { "Set đồ", "2" }
		};
		GameCanvas.panel.setType(0);
		if (GameCanvas.panel.currentTabIndex == 0)
		{
			GameCanvas.panel.setTabInventory();
		}
		if (GameCanvas.panel.currentTabIndex == 1)
		{
			setTabSetDo(1);
		}
		if (GameCanvas.panel.currentTabIndex == 1)
		{
			setTabSetDo(2);
		}
		GameCanvas.panel.show();
	}

	public static void setTab()
	{
		if (type == 1)
		{
			if (GameCanvas.panel.currentTabIndex == 0)
			{
				GameCanvas.panel.setTabInventory();
			}
			if (GameCanvas.panel.currentTabIndex == 1)
			{
				setTabAutoSell();
			}
		}
		if (type == 2)
		{
			if (GameCanvas.panel.currentTabIndex == 0)
			{
				GameCanvas.panel.setTabInventory();
			}
			if (GameCanvas.panel.currentTabIndex == 1)
			{
				setTabSetDo(1);
			}
			if (GameCanvas.panel.currentTabIndex == 2)
			{
				setTabSetDo(2);
			}
		}
		if (type == 3 && GameCanvas.panel.currentTabIndex == 0)
		{
			setTabSetting();
		}
	}

	public static void setTabSetting()
	{
		//GameCanvas.panel.currentListLength = MyMenu.vItem.Count;
		GameCanvas.panel.ITEM_HEIGHT = 24;
		GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
		GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
		if (GameCanvas.panel.cmyLim < 0)
		{
			GameCanvas.panel.cmyLim = 0;
		}
		GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex]);
		if (GameCanvas.panel.cmy < 0)
		{
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
		}
		if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim)
		{
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim);
		}
	}

	public static void setTabAutoSell()
	{
		GameCanvas.panel.currentListLength = AutoSell.vItem.size() + 3;
		GameCanvas.panel.ITEM_HEIGHT = 24;
		GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
		GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
		if (GameCanvas.panel.cmyLim < 0)
		{
			GameCanvas.panel.cmyLim = 0;
		}
		GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex]);
		if (GameCanvas.panel.cmy < 0)
		{
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
		}
		if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim)
		{
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim);
		}
	}

	public static void setTabSetDo(int id)
	{
		SetDo.type = id;
		Item[] array = SetDo.set[id - 1];
		GameCanvas.panel.currentListLength = array.Length + 3;
		GameCanvas.panel.ITEM_HEIGHT = 24;
		GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
		GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
		if (GameCanvas.panel.cmyLim < 0)
		{
			GameCanvas.panel.cmyLim = 0;
		}
		GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex]);
		if (GameCanvas.panel.cmy < 0)
		{
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
		}
		if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim)
		{
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim);
		}
	}

	public static void updateKey()
	{
		if (GameCanvas.panel.currentTabIndex == 0)
		{
			GameCanvas.panel.updateKeyScrollView();
		}
		if (GameCanvas.panel.currentTabIndex == 1 || GameCanvas.panel.currentTabIndex == 2)
		{
			GameCanvas.panel.updateKeyScrollView();
		}
	}

	public static void doFire()
	{
		if (GameCanvas.panel.type == 27)
		{
			if (type == 2)
			{
				doFireSetDo();
			}
			if (type == 1)
			{
				doFireAutoSell();
			}
			if (type == 3)
			{
				doFireSetting();
			}
		}
	}

	public static void doFireSetDo()
	{
		if (GameCanvas.panel.currentTabIndex == 0)
		{
			if (GameCanvas.panel.selected == -1)
			{
				return;
			}
			GameCanvas.panel.currItem = null;
			MyVector myVector = new MyVector();
			Item[] arrItemBody = Char.myCharz().arrItemBody;
			if (GameCanvas.panel.selected >= arrItemBody.Length)
			{
				sbyte b = (sbyte)(GameCanvas.panel.selected - arrItemBody.Length);
				Item item = Char.myCharz().arrItemBag[b];
				if (item != null)
				{
					GameCanvas.panel.currItem = item;
					if (type == 2)
					{
						myVector.addElement(new Command("Thêm vào\nSet 1", gI(), 1111143, 1));
						myVector.addElement(new Command("Thêm vào\nSet 2", gI(), 1111143, 2));
					}
				}
			}
			else
			{
				Item item2 = Char.myCharz().arrItemBody[GameCanvas.panel.selected];
				if (item2 != null)
				{
					GameCanvas.panel.currItem = item2;
					myVector.addElement(new Command("Thêm vào\nSet 1", gI(), 1111144, 1));
					myVector.addElement(new Command("Thêm vào\nSet 2", gI(), 1111144, 2));
				}
			}
			GameCanvas.menu.startAt(myVector, GameCanvas.panel.X, (GameCanvas.panel.selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
			GameCanvas.panel.addItemDetail(GameCanvas.panel.currItem);
		}
		else
		{
			if (GameCanvas.panel.currentTabIndex != 1 && GameCanvas.panel.currentTabIndex != 2)
			{
				return;
			}
			if (GameCanvas.panel.selected == 6)
			{
				GameCanvas.panel.hide();
				return;
			}
			MyVector myVector2 = new MyVector();
			myVector2.addElement(new Command("Gỡ", gI(), 8000, GameCanvas.panel.selected));
			if (GameCanvas.panel.currItem != null)
			{
				GameCanvas.menu.startAt(myVector2, GameCanvas.panel.X, (GameCanvas.panel.selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
				GameCanvas.panel.addItemDetail(GameCanvas.panel.currItem);
			}
			else
			{
				GameCanvas.panel.cp = null;
			}
		}
	}

	public static void doFireAutoSell()
	{
		if (GameCanvas.panel.currentTabIndex == 0)
		{
			if (GameCanvas.panel.selected == -1)
			{
				return;
			}
			GameCanvas.panel.currItem = null;
			MyVector myVector = new MyVector();
			Item[] arrItemBody = Char.myCharz().arrItemBody;
			if (GameCanvas.panel.selected >= arrItemBody.Length)
			{
				sbyte b = (sbyte)(GameCanvas.panel.selected - arrItemBody.Length);
				Item item = Char.myCharz().arrItemBag[b];
				if (item != null)
				{
					GameCanvas.panel.currItem = item;
					myVector.addElement(new Command("Bán", gI(), 12345, item));
				}
			}
			GameCanvas.menu.startAt(myVector, GameCanvas.panel.X, (GameCanvas.panel.selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
			GameCanvas.panel.addItemDetail(GameCanvas.panel.currItem);
			return;
		}
		if (GameCanvas.panel.currentTabIndex == 0)
		{
			MyVector myVector2 = new MyVector();
			myVector2.addElement(new Command(mResources.CLOSE, GameCanvas.panel, 8000, GameCanvas.panel.currItem));
			if (GameCanvas.panel.currItem != null)
			{
				GameCanvas.menu.startAt(myVector2, GameCanvas.panel.X, (GameCanvas.panel.selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
				GameCanvas.panel.addItemDetail(GameCanvas.panel.currItem);
			}
			else
			{
				GameCanvas.panel.cp = null;
			}
		}
		if (GameCanvas.panel.currentTabIndex != 1)
		{
			return;
		}
		if (GameCanvas.panel.selected == GameCanvas.panel.currentListLength - 3)
		{
			startChat(1, putgia);
			return;
		}
		if (GameCanvas.panel.selected == GameCanvas.panel.currentListLength - 2)
		{
			startChat(0, putchat);
			return;
		}
		if (GameCanvas.panel.selected == GameCanvas.panel.currentListLength - 1)
		{
			GameCanvas.panel.hide();
			return;
		}
		GameCanvas.panel.currItem = (Item)AutoSell.vItem.elementAt(GameCanvas.panel.selected);
		MyVector myVector3 = new MyVector();
		myVector3.addElement(new Command("Xóa", gI(), 12346, GameCanvas.panel.currItem));
		if (GameCanvas.panel.currItem != null)
		{
			GameCanvas.menu.startAt(myVector3, GameCanvas.panel.X, (GameCanvas.panel.selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
			GameCanvas.panel.addItemDetail(GameCanvas.panel.currItem);
		}
		else
		{
			GameCanvas.panel.cp = null;
		}
	}

	public static void doFireSetting()
	{
		if (GameCanvas.panel.selected != -1)
		{
			//MyMenu.vItem[GameCanvas.panel.selected].perform();
		}
	}

	public static void startChat(int type, string caption)
	{
		ChatTextField chatTextField = null;
		if (GameCanvas.panel.chatTField == null)
		{
			chatTextField = (GameCanvas.panel.chatTField = new ChatTextField());
			GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
			GameCanvas.panel.chatTField.initChatTextField();
			GameCanvas.panel.chatTField.parentScreen = gI();
		}
		GameCanvas.panel.chatTField.parentScreen = gI();
		chatTextField = GameCanvas.panel.chatTField;
		chatTextField.strChat = caption;
		chatTextField.tfChat.name = mResources.input_quantity;
		chatTextField.to = string.Empty;
		chatTextField.isShow = true;
		chatTextField.tfChat.isFocus = true;
		chatTextField.tfChat.setIputType(type);
		if (GameCanvas.isTouch)
		{
			GameCanvas.panel.chatTField.tfChat.doChangeToTextBox();
		}
	}

	public void startChat2(int type, string caption)
    {
		ChatTextField.gI().strChat = caption;
		ChatTextField.gI().tfChat.name = mResources.input_quantity;
		ChatTextField.gI().to = string.Empty;
		ChatTextField.gI().isShow = true;
		ChatTextField.gI().tfChat.isFocus = true;
		ChatTextField.gI().tfChat.setIputType(type);
		//ChatTextField.gI().startChat(this, string.Empty);
	}

	public static void paint(mGraphics g)
	{
		if (type == 1)
		{
			if (GameCanvas.panel.currentTabIndex == 0)
			{
				GameCanvas.panel.paintInventory(g);
			}
			else
			{
				paintAutoSell(g);
			}
		}
		if (type == 2)
		{
			if (GameCanvas.panel.currentTabIndex == 0)
			{
				GameCanvas.panel.paintInventory(g);
			}
			else
			{
				paintSetDo(g, GameCanvas.panel.currentTabIndex);
			}
		}
		if (type == 3 && GameCanvas.panel.currentTabIndex == 0)
		{
			paintCaiDat(g);
		}
	}

	public static void paintAutoSell(mGraphics g)
	{
		int xScroll = GameCanvas.panel.xScroll;
		int yScroll = GameCanvas.panel.yScroll;
		int wScroll = GameCanvas.panel.wScroll;
		int hScroll = GameCanvas.panel.hScroll;
		int cmy = GameCanvas.panel.cmy;
		int iTEM_HEIGHT = GameCanvas.panel.ITEM_HEIGHT;
		int num = AutoSell.vItem.size() + 3;
		int selected = GameCanvas.panel.selected;
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < num; i++)
		{
			int num2 = xScroll + 36;
			int num3 = yScroll + i * iTEM_HEIGHT;
			int w = wScroll - 36;
			int num4 = iTEM_HEIGHT - 1;
			int num5 = xScroll;
			int num6 = yScroll + i * iTEM_HEIGHT;
			int num7 = 34;
			int num8 = iTEM_HEIGHT - 1;
			if (num3 - cmy > yScroll + hScroll || num3 - cmy < yScroll - iTEM_HEIGHT)
			{
				continue;
			}
			if (i == num - 1)
			{
				g.setColor(15196114);
				g.fillRect(num5, num3, wScroll, num4);
				g.drawImage((i != selected) ? GameScr.imgLbtn2 : GameScr.imgLbtnFocus2, xScroll + wScroll - 5, num3 + 2, StaticObj.TOP_RIGHT);
				((i != selected) ? mFont.tahoma_7b_dark : mFont.tahoma_7b_green2).drawString(g, mResources.done, xScroll + wScroll - 22, num3 + 7, 2);
				mFont.tahoma_7_grey.drawString(g, "Nhấn Xong Để Kết Thúc.", xScroll + 5, num3 + num4 / 2 - 4, mFont.LEFT);
				continue;
			}
			if (i == num - 2)
			{
				g.setColor((i != selected) ? 15196114 : 7300181);
				g.fillRect(num5, num3, wScroll, num4);
				mFont.tahoma_7_green.drawString(g, AutoSell.chat, num5 + 5, num3 + 11, 0);
				mFont.tahoma_7_green2.drawString(g, "Nhập Nội Dung Chat", num5 + 5, num3, 0);
				continue;
			}
			if (i == num - 3)
			{
				g.setColor((i != selected) ? 9993045 : 7300181);
				g.fillRect(num5, num6, num7, num8);
				g.drawImage(Panel.imgXu, num5 + num7 / 2, num6 + num8 / 2, 3);
				mFont.tahoma_7_green.drawString(g, NinjaUtil.getMoneys(AutoSell.gia) + mResources.XU, num2 + 5, num3 + 11, 0);
				mFont.tahoma_7_green2.drawString(g, "Nhập Giá Cho Các Món Đồ Ở Trên", num2 + 5, num3, 0);
				continue;
			}
			if (AutoSell.vItem.size() == 0)
			{
				return;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num2, num3, w, num4);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num5, num6, num7, num8);
			Item item = (Item)AutoSell.vItem.elementAt(i);
			if (item == null)
			{
				continue;
			}
			string text = string.Empty;
			if (item.itemOption != null)
			{
				for (int j = 0; j < item.itemOption.Length; j++)
				{
					if (item.itemOption[j].optionTemplate.id == 72)
					{
						text = " [+" + item.itemOption[j].param + "]";
					}
				}
			}
			mFont.tahoma_7_green2.drawString(g, item.template.name + text, num2 + 5, num3 + 1, 0);
			string text2 = string.Empty;
			if (item.itemOption != null)
			{
				if (item.itemOption.Length != 0 && item.itemOption[0] != null)
				{
					text2 += item.itemOption[0].getOptionString();
				}
				mFont mFont = mFont.tahoma_7_blue;
				if (item.compare < 0 && item.template.type != 5)
				{
					mFont = mFont.tahoma_7_red;
				}
				if (item.itemOption.Length > 1)
				{
					for (int k = 1; k < item.itemOption.Length; k++)
					{
						if (item.itemOption[k] != null && item.itemOption[k].optionTemplate.id != 102 && item.itemOption[k].optionTemplate.id != 107)
						{
							text2 = text2 + "," + item.itemOption[k].getOptionString();
						}
					}
				}
				mFont.drawString(g, text2, num2 + 5, num3 + 11, mFont.LEFT);
			}
			SmallImage.drawSmallImage(g, item.template.iconID, num5 + num7 / 2 - ((item.quantity > 1) ? 8 : 0), num6 + num8 / 2, 0, 3);
			if (item.quantity > 1)
			{
				mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num5 + num7 - 15, num6 + 6, 0);
			}
		}
		GameCanvas.panel.paintScrollArrow(g);
	}

	public static void paintSetDo(mGraphics g, int numset)
	{
		int xScroll = GameCanvas.panel.xScroll;
		int yScroll = GameCanvas.panel.yScroll;
		int wScroll = GameCanvas.panel.wScroll;
		int hScroll = GameCanvas.panel.hScroll;
		int cmy = GameCanvas.panel.cmy;
		int iTEM_HEIGHT = GameCanvas.panel.ITEM_HEIGHT;
		int num = 7;
		Item[] array = SetDo.set[numset - 1];
		int selected = GameCanvas.panel.selected;
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < num; i++)
		{
			int num2 = xScroll + 36;
			int num3 = yScroll + i * iTEM_HEIGHT;
			int w = wScroll - 36;
			int num4 = iTEM_HEIGHT - 1;
			int num5 = xScroll;
			int num6 = yScroll + i * iTEM_HEIGHT;
			int num7 = 34;
			int num8 = iTEM_HEIGHT - 1;
			if (num3 - cmy > yScroll + hScroll || num3 - cmy < yScroll - iTEM_HEIGHT)
			{
				continue;
			}
			if (i == num - 1)
			{
				g.setColor(15196114);
				g.fillRect(num5, num3, wScroll, num4);
				g.drawImage((i != selected) ? GameScr.imgLbtn2 : GameScr.imgLbtnFocus2, xScroll + wScroll - 5, num3 + 2, StaticObj.TOP_RIGHT);
				((i != selected) ? mFont.tahoma_7b_dark : mFont.tahoma_7b_green2).drawString(g, mResources.done, xScroll + wScroll - 22, num3 + 7, 2);
				mFont.tahoma_7_grey.drawString(g, "Nhấn Xong Để Kết Thúc.", xScroll + 5, num3 + num4 / 2 - 4, mFont.LEFT);
				continue;
			}
			if (array.Length == 0)
			{
				return;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num2, num3, w, num4);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num5, num6, num7, num8);
			Item item = array[i];
			if (item == null)
			{
				continue;
			}
			string text = string.Empty;
			if (item.itemOption != null)
			{
				for (int j = 0; j < item.itemOption.Length; j++)
				{
					if (item.itemOption[j].optionTemplate.id == 72)
					{
						text = " [+" + item.itemOption[j].param + "]";
					}
				}
			}
			mFont.tahoma_7_green2.drawString(g, item.template.name + text, num2 + 5, num3 + 1, 0);
			string text2 = string.Empty;
			if (item.itemOption != null)
			{
				if (item.itemOption.Length != 0 && item.itemOption[0] != null)
				{
					text2 += item.itemOption[0].getOptionString();
				}
				mFont mFont = mFont.tahoma_7_blue;
				if (item.compare < 0 && item.template.type != 5)
				{
					mFont = mFont.tahoma_7_red;
				}
				if (item.itemOption.Length > 1)
				{
					for (int k = 1; k < item.itemOption.Length; k++)
					{
						if (item.itemOption[k] != null && item.itemOption[k].optionTemplate.id != 102 && item.itemOption[k].optionTemplate.id != 107)
						{
							text2 = text2 + "," + item.itemOption[k].getOptionString();
						}
					}
				}
				mFont.drawString(g, text2, num2 + 5, num3 + 11, mFont.LEFT);
			}
			SmallImage.drawSmallImage(g, item.template.iconID, num5 + num7 / 2 - ((item.quantity > 1) ? 8 : 0), num6 + num8 / 2, 0, 3);
			if (item.quantity > 1)
			{
				mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num5 + num7 - 15, num6 + 6, 0);
			}
		}
		GameCanvas.panel.paintScrollArrow(g);
	}

	public static void paintCaiDat(mGraphics g)
	{
		int xScroll = GameCanvas.panel.xScroll;
		int yScroll = GameCanvas.panel.yScroll;
		int wScroll = GameCanvas.panel.wScroll;
		int hScroll = GameCanvas.panel.hScroll;
		int cmy = GameCanvas.panel.cmy;
		int iTEM_HEIGHT = GameCanvas.panel.ITEM_HEIGHT;
		int selected = GameCanvas.panel.selected;
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
		{
			int num = xScroll + 36;
			int num2 = yScroll + i * iTEM_HEIGHT;
			int num3 = wScroll - 36;
			int h = iTEM_HEIGHT - 1;
			int num4 = xScroll;
			int num5 = yScroll + i * iTEM_HEIGHT;
			int num6 = 34;
			int num7 = iTEM_HEIGHT - 1;
			if (num2 - cmy <= yScroll + hScroll && num2 - cmy >= yScroll - iTEM_HEIGHT)
			{
				//if (MyMenu.vItem[i] == null)
				//{
				//	g.setColor((i != selected) ? 15196114 : 7300181);
				//	g.fillRect(num4, num2, wScroll, h);
				//	continue;
				//}
				g.setColor((i != selected) ? 15196114 : 7300181);
				g.fillRect(num4, num2, wScroll, h);
				//mFont.tahoma_7_green2.drawString(g, MyMenu.vItem[i].info, num4 + 5, num2, 0);
				//mFont.tahoma_7_grey.drawString(g, MyMenu.vItem[i].value, num4 + 5, num2 + 11, 0);
			}
		}
		GameCanvas.panel.paintScrollArrow(g);
	}

	public static void paintTopInfo(mGraphics g)
	{
		if (type == 1)
		{
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), GameCanvas.panel.X + 25, 50, 0, 33);
			mFont.tahoma_7_yellow.drawString(g, mResources.select_item, 60, 4, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, "Nhập Giá", 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, "Nhập Nội Dung Chat", 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, mResources.press_done, 60, 38, mFont.LEFT, mFont.tahoma_7_grey);
		}
		if (type == 2)
		{
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), GameCanvas.panel.X + 25, 50, 0, 33);
			mFont.tahoma_7_yellow.drawString(g, mResources.select_item, 60, 4, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, "Chọn set đồ", 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, mResources.press_done, 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
		}
		if (type == 3)
		{
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), GameCanvas.panel.X + 25, 50, 0, 33);
		}
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1111143:
		{
			Item currItem = GameCanvas.panel.currItem;
			int num2 = (int)p;
			SetDo.set[num2 - 1][currItem.template.type] = currItem;
			break;
		}
		case 1111144:
		{
			Item currItem2 = GameCanvas.panel.currItem;
			int num3 = (int)p;
			SetDo.set[num3 - 1][currItem2.template.type] = currItem2;
			break;
		}
		case 8000:
		{
			int num = (int)p;
			SetDo.set[SetDo.type - 1][num] = null;
			break;
		}
		case 8001:
			setTypeSetDo();
			break;
		case 8002:
		{
			MyVector myVector = new MyVector();
			type = (int)p;
			ChatPopup.addChatPopup("Chọn đối tượng cần mặc đồ", 10000, null);
			myVector.addElement(new Command("Bản thân", gI(), 8003, 4));
			if (Char.myCharz().havePet)
			{
				myVector.addElement(new Command("Đệ tử", gI(), 8003, 6));
			}
			GameCanvas.menu.startAt(myVector, 3);
			break;
		}
		case 8003:
			SetDo.BAGTYPE = (int)p;
			new Thread(SetDo.macdo).Start();
			break;
		case 12345:
		{
			Item item = (Item)p;
			if (item.quantity > 1)
			{
				startChat(1, putsoluong);
				break;
			}
			Item item2 = new Item();
			item2.template = item.template;
			item2.quantity = item.quantity;
			item2.indexUI = item.indexUI;
			item2.itemOption = item.itemOption;
			AutoSell.vItem.addElement(item2);
			break;
		}
		case 12346:
			AutoSell.vItem.removeElement(p);
			break;
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (GameCanvas.panel.chatTField.strChat == putsoluong)
		{
			int num = 0;
			try
			{
				num = int.Parse(GameCanvas.panel.chatTField.tfChat.getText());
			}
			catch (Exception)
			{
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				GameCanvas.panel.chatTField.isShow = false;
				GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				return;
			}
			Item item = new Item();
			item.template = GameCanvas.panel.currItem.template;
			item.quantity = num;
			item.indexUI = GameCanvas.panel.currItem.indexUI;
			item.itemOption = GameCanvas.panel.currItem.itemOption;
			AutoSell.vItem.addElement(item);
			GameCanvas.panel.chatTField.isShow = false;
			GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		}
		else if (GameCanvas.panel.chatTField.strChat == putgia)
		{
			int num2 = 0;
			try
			{
				num2 = int.Parse(GameCanvas.panel.chatTField.tfChat.getText());
			}
			catch (Exception)
			{
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				GameCanvas.panel.chatTField.isShow = false;
				GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				return;
			}
			AutoSell.gia = num2;
			GameCanvas.panel.chatTField.isShow = false;
			GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		}
		else if (GameCanvas.panel.chatTField.strChat == putchat)
		{
			try
			{
			}
			catch (Exception)
			{
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				GameCanvas.panel.chatTField.isShow = false;
				GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				return;
			}
			AutoSell.chat = GameCanvas.panel.chatTField.tfChat.getText();
			GameCanvas.panel.chatTField.isShow = false;
			GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		}
		else if (GameCanvas.panel.chatTField.strChat == noitai)
		{
			int num3 = -1;
			try
			{
				num3 = int.Parse(GameCanvas.panel.chatTField.tfChat.getText());
			}
			catch (Exception)
			{
				GameCanvas.panel.chatTField.isShow = false;
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				return;
			}
			if (PickMob.isGHNT) AutoNoiTai.chiso[1] = num3;
			if (num3 != -1 && num3 >= AutoNoiTai.chiso[0] && num3 <= AutoNoiTai.chiso[1])
			{
				AutoNoiTai.max = num3;
				AutoNoiTai.openMax = true;
				AutoNoiTai.gI().isnoitai = true;
				GameCanvas.panel.hide();
				new Thread(AutoNoiTai.gI().open).Start();
				GameCanvas.panel.chatTField.isShow = false;
				GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
			}
			else
			{
				GameCanvas.startOKDlg(noitai);
			}
		}
		else if (GameCanvas.panel.chatTField.strChat == AutoNoiTai.increasePotentialTitle)
		{
			int num4 = -1;
			try
			{
				num4 = int.Parse(GameCanvas.panel.chatTField.tfChat.getText());
			}
			catch (Exception)
			{
				GameCanvas.panel.chatTField.isShow = false;
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				return;
			}
			if (num4 != -1 && num4 >= AutoNoiTai.chiSoStart && num4 <= AutoNoiTai.chiSoEnd)
			{
				AutoNoiTai.increasePotential = num4;
				AutoNoiTai.gI().isIncreasePotential = true;
				new Thread(AutoNoiTai.gI().AddPotential).Start();
				GameCanvas.panel.chatTField.isShow = false;
				GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
			}
			else
			{
				GameCanvas.startOKDlg(AutoNoiTai.increasePotentialTitle);
			}
		}
		ChatTextField.gI().parentScreen = GameCanvas.panel;
	}

	public void onCancelChat()
	{
		GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
	}
}
