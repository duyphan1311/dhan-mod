using AssemblyCSharp.Mod;
using AssemblyCSharp.Mod.PickMob;
using AssemblyCSharp.Mod.Xmap;
using System.IO;
using System.Threading;
using static UglyBoy.AutoUpGrade;

namespace UglyBoy;

internal class AutoNoiTai : IActionListener
{
	public static string tennoitaicanmo;

	public static sbyte type = 1;

	public static bool openMax = false;

	public static int max = -1;

	public static int[] chiso = new int[2];

	private static AutoNoiTai instance;

	public bool isnoitai = false;

	public static AutoNoiTai gI()
	{
		return (instance != null) ? instance : (instance = new AutoNoiTai());
	}

	public void open()
	{
		if (Char.myCharz().cPower < 100000L)
		{
			GameScr.info1.addInfo("Cần 100k sức mạnh để mở.", 0);
			return;
		}
		if (openMax && max == -1)
		{
			isnoitai = false;
			openMax = false;
			return;
		}
		do
		{
			Service.gI().speacialSkill(0);
			if (Panel.specialInfo.Contains(tennoitaicanmo))
			{
				if (!openMax)
				{
					isnoitai = false;
					GameScr.info1.addInfo("Xong", 0);
					break;
				}
				int num = Panel.specialInfo.IndexOf("%");
				string text = Panel.specialInfo.Substring(0, num);
				int num2 = text.LastIndexOf(' ');
				string s = CutString(num2 + 1, num - 1, Panel.specialInfo);
				int num3 = int.Parse(s);
				if (num3 >= max)
				{
					isnoitai = false;
					openMax = false;
					GameScr.info1.addInfo("Xong", 0);
					break;
				}
			}
            Thread.Sleep(500);
            Service.gI().confirmMenu(5, type);
            Thread.Sleep(500);
            Service.gI().confirmMenu(5, 0);
            Thread.Sleep(1000);
        }
		while (isnoitai);
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
			case 1:
			{
				string text2 = (string)p;
				int length2 = text2.Substring(0, text2.IndexOf('%')).LastIndexOf(' ');
				tennoitaicanmo = text2.Substring(0, length2);
				isnoitai = true;
				type = (sbyte)idAction;
				GameCanvas.panel.hide();
				new Thread(open).Start();
				break;
			}
			case 2:
			{
				string text = (string)p;
				int length = text.Substring(0, text.IndexOf('%')).LastIndexOf(' ');
				tennoitaicanmo = text.Substring(0, length);
				isnoitai = true;
				type = (sbyte)idAction;
				GameCanvas.panel.hide();
				new Thread(open).Start();
				break;
			}
		}
		if (idAction == 3)
		{
			openMax = false;
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Mở Vip", gI(), 2, p));
			myVector.addElement(new Command("Mở Thường", gI(), 1, p));
			GameCanvas.menu.startAt(myVector, 3);
		}
		if (idAction == 4)
		{
			string text3 = (string)p;
			openMax = true;
			int num = text3.IndexOf("đến ");
			int length3 = text3.Substring(num + 4).IndexOf("%");
			max = int.Parse(text3.Substring(num + 4, length3));
			MyVector myVector2 = new MyVector();
			myVector2.addElement(new Command("Mở Vip", gI(), 2, p));
			myVector2.addElement(new Command("Mở Thường", gI(), 1, p));
			GameCanvas.menu.startAt(myVector2, 3);
		}
		if (idAction == 6)
		{
			PickMobController.IsDapDo = true;
		}
		if (idAction == 5)
		{
			MyVector myVector3 = new MyVector();
			for (int i = 1; i <= 16; i++)
			{
				myVector3.addElement(new Command(i + " sao", this, 7, i));
			}
			GameCanvas.menu.startAt(myVector3, 3);
		}
		if (idAction == 7)
		{
			Npc npc2 = GameScr.findNPCInMap(21);
			if (npc2 != null) XmapController.MoveMyChar(npc2.cx, npc2.cy);
			Service.gI().openMenu(21);
			Service.gI().confirmMenu(21, 1);
			if(PickMob.isNVdd)
            {
				GameCanvas.panel.vItemCombine.addElement(FindItemUpgrade(listUpgrade[0]));
			}
			AutoUpGrade.isupgrade = true;
			AutoUpGrade.sosao = (int)p;
			IsShowListUpgrade = true;
		}
		if (idAction == 8)
		{
			string text4 = (string)p;
			int length4 = text4.Substring(0, text4.IndexOf('%')).LastIndexOf(' ');
			tennoitaicanmo = text4.Substring(0, length4);
			int num2 = text4.IndexOf("%");
			int num3 = text4.IndexOf("đến ");
			int start = text4.Substring(0, num2).LastIndexOf(' ');
			int num4 = text4.LastIndexOf('%');
			chiso[0] = int.Parse(CutString(start, num2 - 1, text4));
			chiso[1] = int.Parse(CutString(num3 + 4, num4 - 1, text4));
			string text5 = CutString(start, num4, text4);
			mPanel.noitai = "Nhập chỉ số bạn muốn chọn trong khoảng " + text5;
			MyVector myVector4 = new MyVector();
			myVector4.addElement(new Command("Mở Vip", gI(), 9, 2));
			myVector4.addElement(new Command("Mở Thường", gI(), 9, 1));
			GameCanvas.menu.startAt(myVector4, 3);
		}
		if (idAction == 9)
		{
			int num5 = (int)p;
			type = (sbyte)num5;
			mPanel.startChat(1, mPanel.noitai);
		}
		if (idAction == 10)
		{
			Service.gI().combine(1, GameCanvas.panel.vItemCombine);
		}
		if (idAction == 11)
		{
			string str = (string)p;
			typePotential = str;
			increasePotentialTitle = "Nhập số lượng " + str + " bạn muốn tăng";
			mPanel.startChat(1, increasePotentialTitle);
		}
		if (idAction == 12)
		{
			for(int i = 0; i < GameCanvas.panel.vItemCombine.size(); i++)
            {
				listKham.Add((Item)GameCanvas.panel.vItemCombine.elementAt(i));
			}
			isKham = true;
			GameScr.info1.addInfo("Auto khảm bật", 0);
		}
		if(idAction == 13)
        {
			NgocRong = (int)p;
			SoNRBanDau = FindQuatity((short)NgocRong);
			epNRCaption = "Nhập số lượng " + (NgocRong - 13).ToString() + " sao bạn muốn ép";
			mPanel.gI().startChat2(1, epNRCaption);
		}
	}

	public static string epNRCaption;

	public static string potential;

	public static string typePotential;

	public static string increasePotentialTitle;

	public static int chiSoStart = 1;

	public static int chiSoEnd = 1000000000;

	public static int increasePotential;

	public bool isIncreasePotential = false;

	public void AddPotential()
    {
		int hpIncrs = Char.myCharz().cHPGoc + increasePotential;
		int mpInCrs = Char.myCharz().cMPGoc + increasePotential;
		int sdIncrs = Char.myCharz().cDamGoc + increasePotential;

		while (isIncreasePotential)
        {
			if (typePotential == "hp") {
				if (Char.myCharz().cHPGoc < hpIncrs)
				{
					if (Char.myCharz().cHPGoc <= hpIncrs - 2000)
					{
						Service.gI().upPotential(0, 100);
					}
					if (Char.myCharz().cHPGoc <= hpIncrs - 200)
					{
						Service.gI().upPotential(0, 10);
					}
					if (Char.myCharz().cHPGoc <= hpIncrs - 20)
					{
						Service.gI().upPotential(0, 1);
					}
				}
				else
				{
					isIncreasePotential = false;
					GameScr.info1.addInfo("Hoàn thành!", 0);
					return;
				}
			}
			if (typePotential == "mp")
			{
				if (Char.myCharz().cMPGoc < mpInCrs)
				{
					if (Char.myCharz().cMPGoc <= mpInCrs - 2000)
					{
						Service.gI().upPotential(1, 100);
					}
					if (Char.myCharz().cMPGoc <= mpInCrs - 200)
					{
						Service.gI().upPotential(1, 10);
					}
					if (Char.myCharz().cMPGoc <= mpInCrs - 20)
					{
						Service.gI().upPotential(1, 1);
					}
				}
				else
				{
					isIncreasePotential = false;
					GameScr.info1.addInfo("Hoàn thành!", 0);
					return;
				}
			}
			if (typePotential == "sd")
			{
				if (Char.myCharz().cDamGoc < sdIncrs)
				{
					if (Char.myCharz().cDamGoc <= sdIncrs - 100)
					{
						Service.gI().upPotential(2, 100);
					}
					if (Char.myCharz().cDamGoc <= sdIncrs - 10)
					{
						Service.gI().upPotential(2, 10);
					}
					if (Char.myCharz().cDamGoc <= sdIncrs - 1)
					{
						Service.gI().upPotential(2, 1);
					}
				}
				else
				{
					isIncreasePotential = false;
					GameScr.info1.addInfo("Hoàn thành!", 0);
					return;
				}
			}
			Thread.Sleep(1000);
		}
    }

	public static void addListUpgrade(Item item, int id)
	{
		foreach (ItemUpgrade it in listUpgrade)
		{
			if (it.type == item.template.type && it.info == item.info && item.template.name == it.name && it.id == id)
			{
				listUpgrade.Remove(it);
				GameScr.info1.addInfo("Đã xoá " + item.template.name + " khỏi danh sách nâng cấp", 0);
			}
		}
		listUpgrade.Add(new ItemUpgrade(id, item.info, item.template.type, item.template.name));
		GameScr.info1.addInfo("Đã thêm " + item.template.name + " vào danh sách nâng cấp", 0);
		
	}
	public string CutString(int start, int end, string s)
	{
		string text = "";
		for (int i = start; i <= end; i++)
		{
			text += s[i];
		}
		return text;
	}
}
