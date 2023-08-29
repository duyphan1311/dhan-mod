using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace UglyBoy;

public class Ugly
{
	private static Ugly instance;

	public static int bagimageid = 20;

	private int speed;

	private bool vutdo = false;

	private long timevutdo = 0L;

	public bool autoupde;

	public long timepickitem;

	public bool apick;

	public bool anitem;

	public bool annguoi;

	private long timetansat;

	public bool atansat;

	public bool atc;

	public string noidung;

	private long timechat;

	private Skill skill;

	private long timeuseskill;

	private bool autoskill;

	private bool lockk;

	private Char charlock;

	private bool autohs;

	public bool kmap;

	public static MyVector listMod = new MyVector();

	public bool xd;

	public bool cd;

	private long tgchodau;

	private long tgxindau;

	public bool cskb = false;

	private int typeTanSat = 0;

	public bool isTudanh = false;

	private long timetudanh = 0L;

	public bool xoamap = false;

	public bool isDetuPem = false;

	private bool isKvt = false;

	private MovePoint kvt = null;

	private int lastspeedrun = Char.myCharz().cspeed;

	private long timetudanh2 = 1000L;

	private bool ispetview = false;

	private bool ischarview = false;

	private bool isnesq = false;

	private long timensq = 0L;

	private bool findMob = true;

	private long timePetInfo = 0L;

	public static bool trading = false;

	public static Image bg = null;

	private List<int> itemBlock = new List<int>();

	private List<int> itemPickUp = new List<int> { 77, 861, 191, 76, 188, 189, 190, 192, 74 };

	public static bool autosell = false;

	private bool isSkin = false;

	public static int[] skins = new int[3];

	private bool startgame = false;

	private int lockCharId = -1;

	public bool tdlt = false;

	public bool xoabg = ((Rms.loadRMSString("xoabg") == "1") ? true : false);

	private int bagg = 0;

	public static Ugly gI()
	{
		if (instance == null)
		{
			return instance = new Ugly();
		}
		return instance;
	}

	public Ugly()
	{
		speed = 8;
		if (!Directory.Exists("Ugly"))
		{
			Directory.CreateDirectory("Ugly");
		}
		if (File.Exists("Ugly\\setting.txt"))
		{
			using StreamReader streamReader = new StreamReader("Ugly\\setting.txt");
			while (!streamReader.EndOfStream)
			{
				string text = streamReader.ReadLine();
				if (text.StartsWith("itemblock:") && text.Substring("itemblock:".Length) != "null")
				{
					string[] array = text.Substring("itemblock:".Length).Split(',', ';');
					for (int i = 0; i < array.Length; i++)
					{
						itemBlock.Add(int.Parse(array[i]));
					}
				}
				if (text.StartsWith("itempickup:") && text.Substring("itempickup:".Length) != "null")
				{
					string[] array2 = text.Substring("itempickup:".Length).Split(',', ';');
					for (int j = 0; j < array2.Length; j++)
					{
						itemPickUp.Add(int.Parse(array2[j]));
					}
				}
			}
			streamReader.Close();
		}
		else
		{
			File.Create("Ugly\\setting.txt");
		}
		if (!File.Exists("Ugly\\vutdo.txt"))
		{
			File.Create("Ugly\\vutdo.txt");
		}
		new Thread(AutoSell.xulygiaodich).Start();
		new Thread(autobikiep.update).Start();
		//new Thread(AutoXinDau2.update).Start();
		new Thread(AutoLogin.Update).Start();
		new Thread(NhanDua.vuahung).Start();
		//new Thread(Updateversion.update).Start();
		skins[0] = Char.myCharz().head;
		skins[1] = Char.myCharz().body;
		skins[2] = Char.myCharz().leg;
	}

	public bool Chat(string txt)
	{
		if (!txt.StartsWith("/"))
			return false;

		txt = txt.Substring(1);
		string[] array = txt.Trim().Split(';');
		for (int idx = 0; idx < array.Length; idx++)
		{
			string str = array[idx].Trim();
			if (str.Equals("sb"))
			{
				SanBoss.gI().sb = !SanBoss.gI().sb;
				GameScr.info1.addInfo("Săn Boss " + aa(SanBoss.gI().sb), 0);
				if (SanBoss.gI().sb)
				{
					SanBoss.gI().LoadData();
				}
				return true;
			}
			if (str.Equals("fboss"))
			{
				SanBoss.gI().fboss = !SanBoss.gI().fboss;
				GameScr.info1.addInfo("Focus Boss " + aa(SanBoss.gI().fboss), 0);
				return true;
			}
			if (str.Equals("adt"))
			{
				autoupde = !autoupde;
				GameScr.info1.addInfo("Auto up dt " + aa(autoupde), 0);
				return true;
			}
			if (str.Equals("anhat"))
			{
				apick = !apick;
				GameScr.info1.addInfo("Auto nhặt " + aa(apick), 0);
				return true;
			}
			if (str.Equals("anitem"))
			{
				anitem = !anitem;
				GameScr.info1.addInfo("Ẩn Item " + aa(anitem), 0);
				return true;
			}
			if (str.Equals("annguoi"))
			{
				annguoi = !annguoi;
				GameScr.info1.addInfo("Ẩn Người " + aa(annguoi), 0);
				return true;
			}
			if (str.Equals("ts"))
			{
				atansat = !atansat;
				typeTanSat = 0;
				GameScr.info1.addInfo("Tàn Sát Né Siêu Quái" + aa(atansat), 0);
				return true;
			}
			if (str.Equals("ts1"))
			{
				atansat = !atansat;
				typeTanSat = 1;
				GameScr.info1.addInfo("Tàn Sát " + aa(atansat), 0);
				return true;
			}
			if (str.Equals("ts2"))
			{
				atansat = !atansat;
				typeTanSat = 2;
				GameScr.info1.addInfo("Tàn Sát TDLT" + aa(atansat), 0);
				return true;
			}
			if (str.Equals("atc"))
			{
				atc = !atc;
				GameScr.info1.addInfo("Tự động chat: " + aa(atc), 0);
				if (atc)
				{
					loadAutochat();
				}
				return true;
			}
			if (str.Equals("lock"))
			{
				lockk = !lockk;
				lockCharId = Char.myCharz().charFocus.charID;
				GameScr.info1.addInfo("Khóa mục tiêu " + aa(lockk), 0);
				return true;
			}
			if (str.Equals("ahs"))
			{
				autohs = !autohs;
				GameScr.info1.addInfo("Auto dung skill 3 " + aa(autohs), 0);
				return true;
			}
			if (str.Equals("kmap"))
			{
				kmap = !kmap;
				GameScr.info1.addInfo("Khóa Map " + aa(kmap), 0);
				return true;
			}
			if (str.Equals("add"))
			{
				Mob mobFocus = Char.myCharz().mobFocus;
				if (!listMod.contains(mobFocus.templateId))
				{
					listMod.addElement(mobFocus.templateId);
					GameScr.info1.addInfo("Them " + mobFocus.getTemplate().name + " vao danh sach tan sat", 0);
				}
				return true;
			}
			if (str.Equals("acd"))
			{
				cd = !cd;
				GameScr.info1.addInfo("Tự động cho đậu " + aa(cd), 0);
				return true;
			}
			if (str.Equals("axd"))
			{
				xd = !xd;
				GameScr.info1.addInfo("Tự động xin đậu " + aa(xd), 0);
				return true;
			}
			//if (str.Equals("axd2"))
			//{
			//	AutoXinDau2.isAutoBuff = !AutoXinDau2.isAutoBuff;
			//	GameScr.info1.addInfo("Tự động xin đậu v2 " + aa(AutoXinDau2.isAutoBuff), 0);
			//	return true;
			//}
			if (str.Equals("tdlt"))
			{
				tdlt = !tdlt;
				GameScr.info1.addInfo("Tự động luyện tập " + aa(tdlt), 0);
				return true;
			}
			if (str.Equals("cskb"))
			{
				cskb = !cskb;
				GameScr.info1.addInfo("Chi Nhat Cskb " + aa(cskb), 0);
				return true;
			}
			if (str.Equals("autodt"))
			{
				new Thread(AutoDoanhTrai.update).Start();
				GameScr.info1.addInfo("Auto click doanh trai " + aa(AutoDoanhTrai.autodt), 0);
				return true;
			}
			if (str.Equals("ak"))
			{
				isTudanh = !isTudanh;
				GameScr.info1.addInfo("Tự đánh " + aa(isTudanh), 0);
				return true;
			}
			if (str.Equals("xoamap"))
			{
				xoamap = !xoamap;
				GameScr.info1.addInfo("Xóa map " + aa(xoamap), 0);
				return true;
			}
			if (str.Equals("kvt"))
			{
				isKvt = !isKvt;
				if (isKvt)
				{
					kvt = new MovePoint(Char.myCharz().cx, Char.myCharz().cy);
					lastspeedrun = speed;
					speed = 0;
				}
				else
				{
					speed = lastspeedrun;
				}
				GameScr.info1.addInfo("Khóa vị trí " + aa(isKvt), 0);
				return true;
			}
			if (str.Equals("petw"))
			{
				ispetview = !ispetview;
				GameScr.info1.addInfo("Hiển thị thông tin đệ tử " + aa(ispetview), 0);
				return true;
			}
			if (str.Equals("charw"))
			{
				ischarview = !ischarview;
				GameScr.info1.addInfo("Hiển thị thông tin sư phụ " + aa(ischarview), 0);
				return true;
			}
			if (str.Equals("nesq"))
			{
				isnesq = !isnesq;
				GameScr.info1.addInfo("Né siêu quái " + aa(isnesq), 0);
				return true;
			}
			if (str.Equals("clear"))
			{
				listMod.removeAllElements();
				return true;
			}
			if (str.Equals("skin"))
			{
				isSkin = !isSkin;
				if (!isSkin)
				{
					return true;
				}
				if (Char.myCharz().charFocus == null)
				{
					return true;
				}
				skins[0] = Char.myCharz().charFocus.head;
				skins[1] = Char.myCharz().charFocus.body;
				skins[2] = Char.myCharz().charFocus.leg;
				using (StreamWriter streamWriter = new StreamWriter("Ugly\\skin.txt", append: true))
				{
					streamWriter.Write(skins[0].ToString() + skins[1] + skins[2]);
					streamWriter.Close();
				}
				return true;
			}
			if (str.Equals("item"))
			{
				GameScr.info1.addInfo("Item cua " + Char.myCharz().itemFocus.template.id, 0);
				return true;
			}
			if (str.Equals("auto sell"))
			{
				if (AutoSell.vItem.size() <= 0)
				{
					GameScr.info1.addInfo("Vui lòng chọn item bán.", 0);
					return true;
				}
				autosell = !autosell;
				if (autosell)
				{
					int num = gI().findItemBag(((Item)AutoSell.vItem.elementAt(0)).template.id).quantity;
					for (int i = 0; i < AutoSell.vItem.size(); i++)
					{
						Item item = (Item)AutoSell.vItem.elementAt(i);
						int quantity = gI().findItemBag(item.template.id).quantity;
						if (quantity < num)
						{
							num = quantity;
						}
					}
					AutoSell.con = num;
				}
				GameScr.info1.addInfo("Tự động bán " + aa(autosell), 0);
				return true;
			}
			if (str.Equals("xmap"))
			{
				GameScr.info1.addInfo(Char.myCharz().myskill.dx + "x" + Char.myCharz().myskill.dy, 0);
				return true;
			}
			if (str.Equals("asell"))
			{
				mPanel.setTypeSell();
				return true;
			}
			if (str.Equals("cd"))
			{
				string text = "";
				mPanel.gI().setTypeSetting();
				return true;
			}
			if (str.Equals("setdo"))
			{
				SetDo.StartMenu();
				return true;
			}
			if (str.Equals("autobikiep"))
			{
				autobikiep.isautobikiep = !autobikiep.isautobikiep;
				autobikiep.khienIndex = findSkillIndex(19);
				autobikiep.ttnlIndex = findSkillIndex(8);
				return true;
			}
			//if (str.Equals("ee"))
			//{
			//	Updateversion.checkUpdate();
			//	return true;
			//}
			if (str.Equals("dt"))
			{
				isDetuPem = true;
				return true;
			}
			if (str.Equals("alogin"))
			{
				AutoLogin.autologin = !AutoLogin.autologin;
				GameScr.info1.addInfo("Tự động login " + aa(AutoLogin.autologin), 0);
				return true;
			}
			if (str.Equals("vutdo"))
			{
				new Thread(autovutdo).Start();
				return true;
			}
			if (str.Equals("vq"))
			{
				AutoCrackBall.startMenu();
				return true;
			}
			if (str.Equals("andua"))
			{
				NhanDua.isauto = !NhanDua.isauto;
				return true;
			}
			if (str.Equals("test"))
			{
				Service.gI().buyItem(1, 521, 0);
				return true;
			}
			//if (str.StartsWith("speed") && int.TryParse(str.Substring("speed".Length), out var result))
			//{
			//	Time.set_timeScale(float.Parse(str.Substring("speed".Length)));
			//	GameScr.info1.addInfo("Đặt Tốc Độ Game Thành " + Time.get_timeScale(), 0);
			//	return true;
			//}
			//if (str.StartsWith("s") && int.TryParse(str.Substring(1), out result))
			//{
			//	int num = int.Parse(str.Substring(1));
			//	if (int.TryParse(str.Substring(1), out result))
			//	{
			//		speed = num;
			//		GameScr.info1.addInfo("Đặt Tốc Chạy Thành " + num, 0);
			//		return true;
			//	}
			//}
			if (str.StartsWith("d") && int.TryParse(str.Substring(1), out var result))
			{
				int num2 = int.Parse(str.Substring(1));
				Char.myCharz().cy += num2;
				return true;
			}
			if (str.StartsWith("u") && int.TryParse(str.Substring(1), out result))
			{
				int num3 = int.Parse(str.Substring(1));
				Char.myCharz().cy -= num3;
				return true;
			}
			if (str.StartsWith("r") && int.TryParse(str.Substring(1), out result))
			{
				int num4 = int.Parse(str.Substring(1));
				Char.myCharz().cx += num4;
				return true;
			}
			if (str.StartsWith("l") && int.TryParse(str.Substring(1), out result))
			{
				int num5 = int.Parse(str.Substring(1));
				Char.myCharz().cx -= num5;
				return true;
			}
			if (str.StartsWith("k") && int.TryParse(str.Substring(1), out result))
			{
				int zoneId = int.Parse(str.Substring(1));
				Service.gI().requestChangeZone(zoneId, 0);
				return true;
			}
			if (str.StartsWith("npc") && int.TryParse(str.Substring(3), out result))
			{
				int npcId = int.Parse(str.Substring(3));
				Service.gI().openMenu(npcId);
				return true;
			}
			if (str.StartsWith("skill") && int.TryParse(str.Substring(5), out result))
			{
				autoskill = !autoskill;
				if (autoskill)
				{
					int num6 = int.Parse(str.Substring(5));
					skill = GameScr.keySkill[num6];
				}
				GameScr.info1.addInfo("Tự động dùng skill " + aa(autoskill), 0);
				return true;
			}
			if (str.StartsWith("tel") && int.TryParse(str.Substring(3), out result))
			{
				int id = int.Parse(str.Substring(3));
				Service.gI().gotoPlayer(id);
				return true;
			}
		}
		return false;
	}


	public void Update()
	{
		if (!startgame && GameCanvas.gameTick % 50 == 0)
		{
			Chat("ee");
			startgame = true;
		}
		if (GameCanvas.currentScreen == GameCanvas.loginScr || GameCanvas.currentScreen == GameCanvas.serverScreen || GameScr.isChangeZone || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
		{
			return;
		}
		if (Input.GetKey((KeyCode)120))
		{
			if (AutoUpGrade.isupgrade)
			{
				AutoUpGrade.isupgrade = false;
				AutoUpGrade.IsShowListUpgrade = false;
				GameScr.info1.addInfo("Đã Dừng", 0);
			}
			if (AutoNoiTai.gI().isnoitai)
			{
				AutoNoiTai.gI().isnoitai = false;
				GameScr.info1.addInfo("Đã Dừng", 0);
			}
			if (AutoCrackBall.isauto)
			{
				AutoCrackBall.isauto = false;
				GameScr.info1.addInfo("Đã Dừng", 0);
			}
		}
		Char.myCharz().cspeed = speed;
		Char.myCharz().bag = 31;
		if (autosell && AutoSell.vItem.size() > 0)
		{
			for (int i = 0; i < AutoSell.vItem.size(); i++)
			{
				Item item = (Item)AutoSell.vItem.elementAt(i);
				if (findItemBag(item.template.id) == null)
				{
					autosell = false;
					break;
				}
			}
			if (mSystem.currentTimeMillis() - timechat > 2000 && AutoSell.chat != null)
			{
				Service.gI().chat(AutoSell.chat + " còn x" + AutoSell.con);
				timechat = mSystem.currentTimeMillis();
			}
		}
		if (apick && !atansat && !isTudanh)
		{
			autopickItem();
		}
		if (atansat && GameCanvas.gameTick % 5 == 0)
		{
			tansat();
		}
		if (mSystem.currentTimeMillis() - timechat > 2000 && atc && !autosell)
		{
			autochat();
			timechat = mSystem.currentTimeMillis();
		}
		Autoskill();
		if (lockk && Char.myCharz().charFocus.charID != lockCharId && lockCharId != -1)
		{
			Char @char = GameScr.findCharInMap(lockCharId);
			if (@char != null)
			{
				Char.myCharz().focusManualTo(@char);
			}
		}
		if (autohs && mSystem.currentTimeMillis() - GameScr.keySkill[2].coolDown > GameScr.keySkill[2].lastTimeUseThisSkill && Char.myCharz().nClass.classId == 1)
		{
			Char char2 = findMyPetChar();
			if (char2 != null && Char.myCharz().cMP >= GameScr.keySkill[2].manaUse && Char.myPetz().cHP <= 0)
			{
				Char.myCharz().focusManualTo(char2);
				GameScr.gI().doUseSkill(GameScr.keySkill[2], isShortcut: false);
				GameScr.gI().doUseSkill(GameScr.keySkill[2], isShortcut: false);
			}
		}
		autochodau();
		autoxindau();
		if (isTudanh && mSystem.currentTimeMillis() - Char.myCharz().myskill.lastTimeUseThisSkill > (long)Char.myCharz().myskill.coolDown + 1L && mSystem.currentTimeMillis() - timetudanh > timetudanh2)
		{
			tudanh();
			timetudanh = mSystem.currentTimeMillis();
		}
		if (mSystem.currentTimeMillis() - timePetInfo > 1500 && Char.myCharz().havePet && ispetview)
		{
			Service.gI().petInfo();
			timePetInfo = mSystem.currentTimeMillis();
		}
		autoUpdetu();
		khoavt();
		if (isnesq)
		{
			if (mSystem.currentTimeMillis() - timensq > 1500 && findMob)
			{
				if (Char.myCharz().mobFocus == null)
				{
					Mob mob = nesq();
					if (mob != null)
					{
						Char.myCharz().mobFocus = mob;
						if (distance(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y) > 30)
						{
							GameScr.gI().checkClickMoveTo(Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y);
						}
						findMob = false;
					}
				}
				timensq = mSystem.currentTimeMillis();
			}
			Mob mobFocus = Char.myCharz().mobFocus;
			if (mobFocus == null || mobFocus.status == 0 || mobFocus.status == 1 || mobFocus.hp < 1 || mobFocus.isMobMe || mobFocus.checkIsBoss())
			{
				findMob = true;
			}
		}
		SanBoss.gI().Update();
		if (!GameScr.canAutoPlay && tdlt && GameCanvas.gameTick % 20 == 0)
		{
			TDLT();
		}
	}

	private void khoavt()
	{
		if (isKvt && (Char.myCharz().cx != kvt.xEnd || Char.myCharz().cy != kvt.yEnd))
		{
			Char.myCharz().cx = kvt.xEnd;
			Char.myCharz().cy = kvt.yEnd;
			Service.gI().charMove();
		}
	}

	private Char findMyPetChar()
	{
		Char result = null;
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(i);
			if (@char.charID == Char.myCharz().charID * -1)
			{
				return @char;
			}
		}
		return result;
	}

	public void autoUpdetu()
	{
		if (!isDetuPem)
		{
			return;
		}
		Char @char = findMyPetChar();
		Skill skill = findSkillDown();
		if (skill == null)
		{
			return;
		}
		if (@char != null && Char.myCharz().isMeCanAttackOtherPlayer(@char) && skill.template.id != 22 && skill.template.id != 23)
		{
			Char.myCharz().focusManualTo(@char);
			Char.myCharz().myskill = skill;
			MyVector myVector = new MyVector();
			myVector.addElement(Char.myCharz().charFocus);
			if (Char.myCharz().myskill != null)
			{
				Service.gI().selectSkill(Char.myCharz().myskill.template.id);
			}
			Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
			isDetuPem = false;
			return;
		}
		Mob mob = searchMob();
		if (mob != null)
		{
			Char.myCharz().focusManualTo(mob);
			Char.myCharz().myskill = skill;
			MyVector myVector2 = new MyVector();
			myVector2.addElement(Char.myCharz().mobFocus);
			if (Char.myCharz().myskill != null)
			{
				Service.gI().selectSkill(Char.myCharz().myskill.template.id);
			}
			Service.gI().sendPlayerAttack(myVector2, new MyVector(), 1);
			isDetuPem = false;
		}
	}

	private Skill findSkillDown()
	{
		int num = GameScr.keySkill.Length - 1;
		Skill skill = null;
		List<int> list = new List<int> { 10, 11, 9 };
		while (skill == null && num >= 0)
		{
			if (GameScr.keySkill[num] != null && GameScr.keySkill[num].template.type == 1 && !list.Contains(GameScr.keySkill[num].template.id))
			{
				skill = GameScr.keySkill[num];
			}
			num--;
		}
		return skill;
	}

	private int findSkillIndex(int id)
	{
		int num = 0;
		Skill[] keySkill = GameScr.keySkill;
		foreach (Skill skill in keySkill)
		{
			if (skill.template.id == id)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	public void paint(mGraphics g)
	{
		mFont.tahoma_7b_white.drawStringBorder(g, "MANHHDC.ONLINE", GameCanvas.w / 2, 0, mFont.CENTER, mFont.tahoma_7b_dark);
		int num = 70;
		int num2 = Char.vItemTime.size() * 12 + 90;
		mFont.tahoma_7_green.drawStringBorder(g, TileMap.mapID + "-" + TileMap.mapName + "-k" + TileMap.zoneID, num, num2, 0, mFont.tahoma_7_grey);
		if (ispetview)
		{
			infoView(g, Char.myPetz(), num, num2);
			num += 100;
		}
		if (ischarview)
		{
			infoView(g, Char.myCharz(), num, num2);
			num += 100;
		}
		if (autosell)
		{
			paintAutoSell(g, num, num2);
			num += 100;
		}
		if (AutoNoiTai.gI().isnoitai)
		{
			mFont.tahoma_7b_white.drawString(g, "Cần Mở: " + AutoNoiTai.tennoitaicanmo, num, num2 + 72, 0);
			mFont.tahoma_7b_white.drawString(g, "Nội tại hiện tại: " + Panel.specialInfo, num, num2 + 84, 0);
			if (AutoNoiTai.openMax)
			{
				mFont.tahoma_7b_white.drawString(g, "Max: " + AutoNoiTai.max, num, num2 + 96, 0);
			}
		}
		SanBoss.gI().paint(g);
		if (GameCanvas.currentDialog != null || ChatPopup.currChatPopup != null || GameCanvas.menu.showMenu || GameScr.gI().isPaintPopup() || GameCanvas.panel.isShow || Char.myCharz().taskMaint.taskId == 0 || ChatTextField.gI().isShow || GameCanvas.currentScreen == MoneyCharge.instance)
		{
			return;
		}
		for (int i = 0; i < GameScr.keySkill.Length; i++)
		{
			if (GameScr.keySkill[i] != null)
			{
				long num3 = GameScr.keySkill[i].coolDown - mSystem.currentTimeMillis() + GameScr.keySkill[i].lastTimeUseThisSkill;
				mFont.tahoma_7b_white.drawStringBorder(g, (num3 > 0) ? (num3 / 1000 + "s") : string.Empty, GameScr.xSkill + GameScr.xS[i] + 14, GameScr.yS[i] + 7, mFont.CENTER, mFont.tahoma_7b_dark);
			}
		}
	}

	private void paintAutoSell(mGraphics g, int x, int y)
	{
		mFont.tahoma_7_yellow.drawString(g, "Đang Bán", x, y, 0);
		mFont.tahoma_7_yellow.drawString(g, "Giá: " + AutoSell.gia, x, y + 10, 0);
	}

	private void infoView(mGraphics g, Char c, int xStart, int yStart)
	{
		mFont.tahoma_7b_white.drawStringBorder(g, "Tên: " + c.cName, xStart, yStart + 12, 0, mFont.tahoma_7b_dark);
		mFont.tahoma_7b_white.drawStringBorder(g, "HP: " + c.cHP + "/" + c.cHPFull, xStart, yStart + 24, 0, mFont.tahoma_7b_dark);
		mFont.tahoma_7b_white.drawStringBorder(g, "MP: " + c.cMP + "/" + c.cMPFull, xStart, yStart + 36, 0, mFont.tahoma_7b_dark);
		mFont.tahoma_7b_white.drawStringBorder(g, "SĐ: " + c.cDamFull, xStart, yStart + 48, 0, mFont.tahoma_7b_dark);
		mFont.tahoma_7b_white.drawStringBorder(g, "SM: " + NinjaUtil.getMoneys(c.cPower), xStart, yStart + 60, 0, mFont.tahoma_7b_dark);
		mFont.tahoma_7b_white.drawStringBorder(g, "TN: " + NinjaUtil.getMoneys(c.cTiemNang), xStart, yStart + 72, 0, mFont.tahoma_7b_dark);
	}

	public Item findItemBag(int id)
	{
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			Item item = Char.myCharz().arrItemBag[i];
			if (item != null && item.template.id == id)
			{
				return item;
			}
		}
		return null;
	}

	public Item findItemBagByType(int type)
	{
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			Item item = Char.myCharz().arrItemBag[i];
			if (item != null && item.template.type == type)
			{
				return item;
			}
		}
		return null;
	}

	private Item findItemBox(int id)
	{
		for (int i = 0; i < Char.myCharz().arrItemBox.Length; i++)
		{
			Item item = Char.myCharz().arrItemBox[i];
			if (item != null && item.template.id == id)
			{
				return item;
			}
		}
		return null;
	}

	private Item findItemBoxByType(int type)
	{
		for (int i = 0; i < Char.myCharz().arrItemBox.Length; i++)
		{
			Item item = Char.myCharz().arrItemBox[i];
			if (item != null && item.template.type == type)
			{
				return item;
			}
		}
		return null;
	}

	public string aa(bool o)
	{
		if (!o)
		{
			return "tắt";
		}
		return "bật";
	}

	public void autopickItem()
	{
		if (mSystem.currentTimeMillis() - timepickitem > 1000)
		{
			ItemMap itemMap = searchItem();
			if (itemMap != null && havePickupItem(itemMap) && distance(Char.myCharz().cx, Char.myCharz().cy, itemMap.x, itemMap.y) < 100)
			{
				Service.gI().pickItem(itemMap.itemMapID);
			}
			timepickitem = mSystem.currentTimeMillis();
		}
	}

	public ItemMap searchItem()
	{
		ItemMap result = null;
		int num = -1;
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			int num2 = Math.abs(Char.myCharz().cx - itemMap.x);
			int num3 = Math.abs(Char.myCharz().cy - itemMap.y);
			int num4 = ((num2 > num3) ? num3 : num2);
			bool flag = itemMap.playerId == -1 || itemMap.playerId == Char.myCharz().charID;
			if (itemBlock.Count <= 0 || !itemBlock.Contains(itemMap.template.id))
			{
				if (num == -1 && flag && itemMap.template.type != 33)
				{
					num = num4;
					result = itemMap;
				}
				else if (num4 < num && num4 <= 48 && flag && itemMap.template.type != 33)
				{
					num = num4;
					result = itemMap;
				}
			}
		}
		return result;
	}

	public bool havePickupItem(ItemMap item)
	{
		if (itemPickUp.Contains(item.template.id))
		{
			return true;
		}
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			Item item2 = Char.myCharz().arrItemBag[i];
			if (Char.myCharz().arrItemBag[i] == null)
			{
				return true;
			}
			if (item2.template.id == item.template.id && item2.quantity < 99)
			{
				return true;
			}
		}
		return false;
	}

	private void tansat()
	{
		ItemMap itemMap = searchItem();
		if (itemMap != null && havePickupItem(itemMap))
		{
			if (mSystem.currentTimeMillis() - timepickitem > 1000)
			{
				if (distance(Char.myCharz().cx, Char.myCharz().cy, itemMap.x, itemMap.y) > 50)
				{
					GameScr.gI().checkClickMoveTo(itemMap.x, itemMap.y);
				}
				else
				{
					Service.gI().pickItem(itemMap.itemMapID);
				}
				timepickitem = mSystem.currentTimeMillis();
			}
		}
		else if (!isCanAttackMob())
		{
			Mob mob = ((typeTanSat == 0) ? searchMob() : findMobBoss());
			if (mob != null)
			{
				Char.myCharz().focusManualTo(mob);
			}
		}
		else if (Char.myCharz().mobFocus != null && (Char.myCharz().mobFocus.hp <= 0 || Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0 || Char.myCharz().mobFocus.isMobMe))
		{
			Char.myCharz().mobFocus = null;
		}
		else
		{
			if (Char.myCharz().mobFocus == null)
			{
				return;
			}
			if (typeTanSat == 0)
			{
				if (Char.myCharz().mobFocus.checkIsBoss())
				{
					Char.myCharz().mobFocus = null;
					return;
				}
			}
			else if (typeTanSat == 1 && !Char.myCharz().mobFocus.checkIsBoss())
			{
				Char.myCharz().mobFocus = null;
				return;
			}
			if (!Char.myCharz().myskill.paintCanNotUseSkill)
			{
				if (distance(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y) <= Char.myCharz().myskill.dx)
				{
					doUseSkill(Char.myCharz().myskill);
				}
				else if (Char.myCharz().currentMovePoint == null)
				{
					GameScr.gI().checkClickMoveTo(Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y);
				}
			}
		}
	}

	private bool isCanAttackMob()
	{
		return Char.myCharz().mobFocus != null && GameScr.vMob.size() > 0 && !Char.myCharz().mobFocus.isMobMe && canMobAttackInMap();
	}

	private bool canMobAttackInMap()
	{
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob.status != 0 && mob.status != 1 && mob.hp > 0 && !mob.isMobMe)
			{
				return true;
			}
		}
		return false;
	}

	private Mob firstMob()
	{
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if ((listMod.size() <= 0 || listMod.contains(mob.templateId)) && mob.status != 0 && mob.status != 1 && mob.hp > 0 && !mob.isMobMe)
			{
				if (typeTanSat == 0 && !mob.checkIsBoss())
				{
					return mob;
				}
				if (typeTanSat == 1 && mob.checkIsBoss())
				{
					return mob;
				}
			}
		}
		return null;
	}

	public Mob searchMob()
	{
		Mob mob = firstMob();
		int num = distance(Char.myCharz().cx, Char.myCharz().cy, mob.x, mob.y);
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob2 = (Mob)GameScr.vMob.elementAt(i);
			int num2 = distance(Char.myCharz().cx, Char.myCharz().cy, mob2.x, mob2.y);
			if ((listMod.size() <= 0 || listMod.contains(mob2.templateId)) && num2 < num && mob2.status != 0 && mob2.status != 1 && mob2.hp > 0 && !mob2.isMobMe && !mob2.checkIsBoss())
			{
				mob = mob2;
				num = num2;
			}
		}
		return mob;
	}

	public Mob findMobBoss()
	{
		Mob mob = firstMob();
		int num = distance(Char.myCharz().cx, Char.myCharz().cy, mob.x, mob.y);
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob2 = (Mob)GameScr.vMob.elementAt(i);
			int num2 = distance(Char.myCharz().cx, Char.myCharz().cy, mob2.x, mob2.y);
			if ((listMod.size() <= 0 || listMod.contains(mob2.templateId)) && num2 < num && mob2.status != 0 && mob2.status != 1 && mob2.hp > 0 && !mob2.isMobMe && mob2.checkIsBoss())
			{
				mob = mob2;
				num = num2;
			}
		}
		return mob;
	}

	private void autochat()
	{
		Service.gI().chat(noidung);
	}

	public void loadAutochat()
	{
		if (File.Exists("Ugly\\chat.txt"))
		{
			using (StreamReader streamReader = new StreamReader("Ugly\\chat.txt"))
			{
				noidung = streamReader.ReadToEnd();
				streamReader.Close();
				return;
			}
		}
		File.Create("Ugly\\chat.txt");
	}

	private void doUseSkill(Skill skill)
	{
		if (mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill > skill.coolDown)
		{
			GameScr.gI().doSelectSkill(skill, isShortcut: true);
			if (skill.coolDown > 1500)
			{
				GameScr.gI().auto = 10;
			}
		}
	}

	private void Autoskill()
	{
		if (autoskill && skill != null && mSystem.currentTimeMillis() - timeuseskill > 500)
		{
			doUseSkill(skill);
			timeuseskill = mSystem.currentTimeMillis();
		}
	}

	public Char FindChar()
	{
		Char result = null;
		int num = -1;
		if (GameScr.vCharInMap.size() <= 0)
		{
			return null;
		}
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(i);
			int num2 = distance(Char.myCharz().cx, Char.myCharz().cy, @char.cx, @char.cy);
			if (num == -1)
			{
				result = @char;
				num = num2;
			}
			if (num != -1 && num < num2)
			{
				result = @char;
				num = num2;
			}
		}
		return result;
	}

	public void autochodau()
	{
		if (!cd || Char.myCharz().clan == null || mSystem.currentTimeMillis() - tgchodau <= 500)
		{
			return;
		}
		if (findItemBoxByType(6) == null)
		{
			Item item = null;
			if ((item = findItemBagByType(6)) != null)
			{
				Service.gI().getItem(1, (sbyte)item.indexUI);
			}
			else if (GameScr.gI().magicTree.currPeas > 0 && TileMap.mapID == 21 + Char.myCharz().cgender)
			{
				Service.gI().magicTree(1);
				GameCanvas.gI().perform(888392, null);
			}
			return;
		}
		for (int i = 0; i < ClanMessage.vMessage.size(); i++)
		{
			ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(i);
			if (clanMessage.type == 1 && clanMessage.playerId != Char.myCharz().charID && clanMessage.recieve < clanMessage.maxCap)
			{
				if (findItemBoxByType(6) != null)
				{
					Service.gI().clanDonate(clanMessage.id);
				}
				break;
			}
		}
		tgchodau = mSystem.currentTimeMillis();
	}

	public void autoxindau()
	{
		if (xd && Char.myCharz().clan != null && mSystem.currentTimeMillis() - tgxindau > 301000)
		{
			Service.gI().clanMessage(1, null, -1);
			tgxindau = mSystem.currentTimeMillis();
		}
	}

	public void tudanh()
	{
		if (apick && searchItem() != null)
		{
			autopickItem();
		}
		else if (Char.myCharz().myskill.template.type == 1)
		{
			if (Char.myCharz().charFocus != null)
			{
				MyVector myVector = new MyVector();
				myVector.addElement(Char.myCharz().charFocus);
				Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
			}
			else if (Char.myCharz().mobFocus != null)
			{
				MyVector myVector2 = new MyVector();
				myVector2.addElement(Char.myCharz().mobFocus);
				Service.gI().sendPlayerAttack(myVector2, new MyVector(), 1);
			}
		}
	}

	public Mob nesq()
	{
		Mob mob = searchMob();
		if (mob != null)
		{
			int num = distance(Char.myCharz().cx, Char.myCharz().cy, mob.x, mob.y);
			for (int i = 0; i < GameScr.vMob.size(); i++)
			{
				Mob mob2 = (Mob)GameScr.vMob.elementAt(i);
				int num2 = distance(Char.myCharz().cx, Char.myCharz().cy, mob2.x, mob2.y);
				if (listMod.size() > 0)
				{
					if (listMod.contains((int)mob2.getTemplate().mobTemplateId) && mob2.status != 0 && mob2.status != 1 && mob2.hp > 0 && !mob2.isMobMe && !mob2.checkIsBoss() && num2 < num)
					{
						mob = mob2;
						num = num2;
					}
				}
				else if (mob2.status != 0 && mob2.status != 1 && mob2.hp > 0 && !mob2.isMobMe && !mob2.checkIsBoss() && num2 < num)
				{
					mob = mob2;
					num = num2;
				}
			}
		}
		return mob;
	}

	public void saveSkill()
	{
		sbyte[] array = new sbyte[GameScr.keySkill.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (GameScr.keySkill[i] != null)
			{
				array[i] = GameScr.keySkill[i].template.id;
			}
			else
			{
				array[i] = -1;
			}
		}
		Rms.saveRMS("saveSkill" + Char.myCharz().charID, array);
	}

	public void loadSkill()
	{
		sbyte[] array = Rms.loadRMS("saveSkill" + Char.myCharz().charID);
		if (array == null || array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < Char.myCharz().nClass.skillTemplates.Length; j++)
			{
				if (array[i] == Char.myCharz().nClass.skillTemplates[j].id)
				{
					GameScr.keySkill[i] = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[j]);
					break;
				}
			}
		}
	}

	public bool canUseSkill(Skill skill)
	{
		if (skill == null)
		{
			return false;
		}
		int num = ((skill.template.manaUseType == 2) ? 1 : ((skill.template.manaUseType == 1) ? (skill.manaUse * Char.myCharz().cMPFull / 100) : skill.manaUse));
		return mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill > skill.coolDown && Char.myCharz().cMP >= num;
	}

	public void addInfo(string text, Char o)
	{
		if (text.ToLower().Contains("sao sư phụ không đánh đi?") && gI().autoupde && o.charID == Char.myCharz().charID * -1)
		{
			gI().isDetuPem = true;
		}
	}

	public void infoMe(string s)
	{
		if (s.ToLower().Contains("bạn không đủ vàng"))
		{
			AutoNoiTai.gI().isnoitai = false;
			AutoNoiTai.openMax = false;
		}
		AutoSell.addInfo(s);
		AutoCrackBall.infoMe(s);
	}

	private int distance(int x1, int y1, int x2, int y2)
	{
		int num = Math.abs(x1 - x2);
		int num2 = Math.abs(y1 - y2);
		return (num > num2) ? num2 : num;
	}

	private void TDLT()
	{
		if (GameScr.isChangeZone || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5 || Char.myCharz().isCharge || Char.myCharz().isFlyAndCharge || Char.myCharz().isUseChargeSkill())
		{
			return;
		}
		ItemMap itemMap = searchItem();
		if (itemMap != null && havePickupItem(itemMap))
		{
			if (mSystem.currentTimeMillis() - timepickitem > 1000)
			{
				if (distance(Char.myCharz().cx, Char.myCharz().cy, itemMap.x, itemMap.y) > 50)
				{
					GameScr.gI().checkClickMoveTo(itemMap.x, itemMap.y);
				}
				else
				{
					Service.gI().pickItem(itemMap.itemMapID);
				}
				timepickitem = mSystem.currentTimeMillis();
			}
			return;
		}
		bool flag = false;
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob.status != 0 && mob.status != 1)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		bool flag2 = false;
		for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
		{
			Item item = Char.myCharz().arrItemBag[j];
			if (item != null && item.template.type == 6)
			{
				flag2 = true;
				break;
			}
		}
		if (!flag2 && GameCanvas.gameTick % 150 == 0)
		{
			Service.gI().requestPean();
		}
		if (Char.myCharz().cHP <= Char.myCharz().cHPFull * 20 / 100 || Char.myCharz().cMP <= Char.myCharz().cMPFull * 20 / 100)
		{
			GameScr.gI().doUseHP();
		}
		if (Char.myCharz().mobFocus == null || (Char.myCharz().mobFocus != null && Char.myCharz().mobFocus.isMobMe))
		{
			for (int k = 0; k < GameScr.vMob.size(); k++)
			{
				Mob mob2 = (Mob)GameScr.vMob.elementAt(k);
				if (mob2.status != 0 && mob2.status != 1 && mob2.hp > 0 && !mob2.isMobMe)
				{
					Char.myCharz().cx = mob2.x;
					Char.myCharz().cy = mob2.y;
					Char.myCharz().mobFocus = mob2;
					Service.gI().charMove();
					Char.myCharz().cx = mob2.x;
					Char.myCharz().cy = mob2.y + 1;
					Service.gI().charMove();
					Char.myCharz().cx = mob2.x;
					Char.myCharz().cy = mob2.y;
					Service.gI().charMove();
					Res.outz("focus 1 con bossssssssssssssssssssssssssssssssssssssssssssssssss");
					break;
				}
			}
		}
		else if (Char.myCharz().mobFocus.hp <= 0 || Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0)
		{
			Char.myCharz().mobFocus = null;
		}
		if (Char.myCharz().mobFocus == null || (Char.myCharz().skillInfoPaint() != null && Char.myCharz().indexSkill < Char.myCharz().skillInfoPaint().Length && Char.myCharz().dart != null && Char.myCharz().arr != null))
		{
			return;
		}
		Skill skill = null;
		for (int l = 0; l < GameScr.keySkill.Length; l++)
		{
			if (GameScr.keySkill[l] == null || GameScr.keySkill[l].paintCanNotUseSkill || GameScr.keySkill[l].template.id == 10 || GameScr.keySkill[l].template.id == 11 || GameScr.keySkill[l].template.id == 14 || GameScr.keySkill[l].template.id == 23 || GameScr.keySkill[l].template.id == 7 || Char.myCharz().skillInfoPaint() != null)
			{
				continue;
			}
			int num = 0;
			num = ((GameScr.keySkill[l].template.manaUseType == 2) ? 1 : ((GameScr.keySkill[l].template.manaUseType == 1) ? (GameScr.keySkill[l].manaUse * Char.myCharz().cMPFull / 100) : GameScr.keySkill[l].manaUse));
			if (Char.myCharz().cMP >= num)
			{
				if (skill == null)
				{
					skill = GameScr.keySkill[l];
				}
				else if (skill.coolDown < GameScr.keySkill[l].coolDown)
				{
					skill = GameScr.keySkill[l];
				}
			}
		}
		if (skill != null)
		{
			GameScr.gI().doSelectSkill(skill, isShortcut: true);
			GameScr.gI().doDoubleClickToObj(Char.myCharz().mobFocus);
		}
	}

	private void autovutdo()
	{
		if (!File.Exists("Ugly\\vutdo.txt"))
		{
			return;
		}
		string[] array = File.ReadAllText("Ugly\\vutdo.txt").Split(',');
		GameScr.info1.addInfo(array[0], 0);
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			if (Char.myCharz().arrItemBag[i] == null && !Char.myCharz().arrItemBag[i].isLock)
			{
				continue;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != string.Empty && Char.myCharz().arrItemBag[i].template.name.ToLower().Contains(array[j].Trim().ToLower()))
				{
					Service.gI().useItem(1, 1, (sbyte)i, -1);
					Thread.Sleep(500);
					if (GameCanvas.currentDialog != null)
					{
						ItemObject itemObject = new ItemObject();
						itemObject.type = 1;
						itemObject.id = i;
						itemObject.where = 1;
						GameCanvas.panel.perform(2004, itemObject);
					}
					break;
				}
			}
			Thread.Sleep(500);
		}
		GameScr.info1.addInfo("Xong.", 0);
	}
}
