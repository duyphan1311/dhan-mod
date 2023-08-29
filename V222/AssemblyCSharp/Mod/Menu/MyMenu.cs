using System.Collections.Generic;
using System.IO;
using System.Threading;
using AssemblyCSharp.Mod.Options;
using AssemblyCSharp.Mod.PickMob;
using AssemblyCSharp.Mod.Xmap;
using UglyBoy;

namespace AssemblyCSharp.Mod.Menu;

internal class MyMenu : IActionListener, IChatable
{
	private static MyMenu instance;

	private string sanboss = "Nhập boss bạn cần săn (,)";
	private string autochat = "Nhập nội dung auto chat";

	public static string status = "Trạng thái";
	public static string ShowNHideAll = string.Empty;

	public static bool charinfo = false;
	public static bool IsAuto = false;
	public static bool IsShowAll;

	public static MyMenu gI()
	{
		return (instance != null) ? instance : (instance = new MyMenu());
	}

	public void loadMenu()
	{
		if (PickMob.PickMob.IsTanSat) IsAuto = true;
		else IsAuto = false;
		MyVector myVector = new MyVector();
		if (IsAuto)
		{
			myVector.addElement(new Command("Tắt auto", this, 1, null));
		}
		myVector.addElement(new Command("Tàn sát", this, 19, null));
		myVector.addElement(new Command("Danh sách\nAuto", this, 2, null));
		myVector.addElement(new Command("Hiển thị\nThông tin", this, 3, null));
		myVector.addElement(new Command("NPC", this, 31, null));
		myVector.addElement(new Command("Săn boss", this, 5, null));
		myVector.addElement(new Command("Chức năng", this, 6, null));
		myVector.addElement(new Command("Thiết lập", this, 7, null));
		myVector.addElement(new Command("Rương đồ", this, 28, null));
		GameCanvas.menu.startAt(myVector, 8);
	}
	public void loadAutoList()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Nâng cấp", this, 8, null));
		myVector.addElement(new Command("Auto vứt đồ\n[" + (PickMobController.isVutDo ? "Bật" : "Tắt") + "]", this, 9, null));
		myVector.addElement(new Command("Auto 3", this, 10, null));
		myVector.addElement(new Command("Auto 4", this, 11, null));
		myVector.addElement(new Command("Auto 5", this, 12, null));
		GameCanvas.menu.startAt(myVector, 5);
	}
	public void loadListShow()
	{
		IsShowAll = !OnScreen.IshideNShowCSSP || !OnScreen.IshideNShowCSDT || !OnScreen.viewBoss || !OnScreen.IsThongTinCN;
		if (IsShowAll)
        {
			ShowNHideAll = "Hiển thị\nTất cả";
		}
		else
        {
			ShowNHideAll = "Ẩn\nTất cả";
		}
		MyVector myVector = new MyVector();
		myVector.addElement(new Command(ShowNHideAll, this, 13, IsShowAll));
		myVector.addElement(new Command("Thông tin\nSư phụ\n[" + (OnScreen.IshideNShowCSSP ? "Bật" : "Tắt") + "]", this, 14, null));
		myVector.addElement(new Command("Thông tin\nĐệ tử\n[" + (OnScreen.IshideNShowCSDT ? "Bật" : "Tắt") + "]", this, 15, null));
		myVector.addElement(new Command("Thông báo\nBoss\n[" + (OnScreen.viewBoss ? "Bật" : "Tắt") + "]", this, 16, null));
		myVector.addElement(new Command("Danh sách\nNhân vật\n[" + (OnScreen.nhanVat ? "Bật" : "Tắt") + "]", this, 17, null));
		myVector.addElement(new Command("Thông tin\nChức năng\n[" + (OnScreen.IsThongTinCN ? "Bật" : "Tắt") + "]", this, 18, null));
		GameCanvas.menu.startAt(myVector, 6);
	}
	public void loadTanSat()
	{
		MyVector myVector = new MyVector(); 
		if(PickMob.PickMob.IsTanSat)
			myVector.addElement(new Command("Tắt\ntàn sát", this, 26, null));
		myVector.addElement(new Command("Dịch chuyển\nĐến quái", this, 19, null));
		myVector.addElement(new Command("Chạy đến quái", this, 20, null));
		GameCanvas.menu.startAt(myVector, 2);
	}
	public void loadMob()
	{
		MyVector myVector = new MyVector();
		if (PickMob.PickMob.IsTanSat)
			myVector.addElement(new Command("Tắt\ntàn sát", this, 26, null));
		if (GameScr.vMob.size() > 0)
		{
			myVector.addElement(new Command("Tất cả", this, 21, null));
		}
		List<sbyte> idMob = new();
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (!idMob.Contains(mob.getTemplate().mobTemplateId))
			{
				myVector.addElement(new Command(mob.getTemplate().name + "\n" + mob.getTemplate().hp, this, 22, mob));
			}
			idMob.Add(mob.getTemplate().mobTemplateId);
		}
		GameCanvas.menu.startAt(myVector, GameScr.vMob.size() + 1);
	}

	public void loadNPC()
	{
		MyVector myVector = new MyVector();
		int idNpc = -1;
		for (int i = 0; i < GameScr.vNpc.size(); i++)
		{
			Npc npc = (Npc)GameScr.vNpc.elementAt(i);
			if (npc.template.npcTemplateId != idNpc)
			{
				myVector.addElement(new Command(npc.template.name, this, 32, npc));
			}
			idNpc = npc.template.npcTemplateId;
		}
		GameCanvas.menu.startAt(myVector, GameScr.vNpc.size() + 1);
	}

	public void loadKhamSPL()
    {
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Sức đánh", this, 30, 16));
		myVector.addElement(new Command("HP", this, 30, 20));
		myVector.addElement(new Command("KI", this, 30, 19));
		myVector.addElement(new Command("TNSM", this, 30, 447));
		myVector.addElement(new Command("Giáp", this, 30, 15));
		myVector.addElement(new Command("Né đòn", this, 30, 14));
		myVector.addElement(new Command("PST", this, 30, 443));
		myVector.addElement(new Command("Vàng", this, 30, 446));
		myVector.addElement(new Command("Hút HP", this, 30, 441));
		myVector.addElement(new Command("Hút KI", this, 30, 442));
		GameCanvas.menu.startAt(myVector, 10);
	}

	public void loadTrangBi()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Áo", this, 36, 223));
		myVector.addElement(new Command("Quần", this, 36, 222));
		myVector.addElement(new Command("Găng", this, 36, 224));
		myVector.addElement(new Command("Giày", this, 36, 221));
		myVector.addElement(new Command("Rada", this, 36, 220));
		GameCanvas.menu.startAt(myVector, 5);
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 1)
		{
			IsAuto = false;
			PickMob.PickMob.IsTanSat = false;
			GameScr.info1.addInfo("Đã dừng auto!", 0);
		}
		if (idAction == 2)
		{
			loadAutoList();
		}
		if (idAction == 3)
		{
			loadListShow();
		}
		if (idAction == 4)
		{
			loadTanSat();
		}
		if (idAction == 5)
		{
			MyVector myVector1 = new MyVector();
			myVector1.addElement(new Command("Đến map boss\n[" + (PickMobController.isGoToMapBoss ? "Bật" : "Tắt") + "]", this, 33, null));
			GameCanvas.menu.startAt(myVector1, 3);
		}
		if (idAction == 6)
		{
			GameScr.info1.addInfo(idAction.ToString(), 0);
		}
		if (idAction == 7)
		{
			PickMob.PickMob.Chat("/menu");
		}
		if (idAction == 8)
		{
			MyVector myVector1 = new MyVector();
			myVector1.addElement(new Command("Pha lê\nhoá", AutoNoiTai.gI(), 5, null));
			myVector1.addElement(new Command("Khảm SPL", this, 29, null));
			myVector1.addElement(new Command("Ép ngọc\nrồng", this, 34, null));
			myVector1.addElement(new Command("Nâng cấp\ntrang bị", this, 35, null));
			myVector1.addElement(new Command("Auto\nnhận vàng\n[" + (AutoUpGrade.IsNhanVang ? "Bật" : "Tắt") + "]", this, 23, null));
			myVector1.addElement(new Command("Auto\nnhận vàng\ntại nhà\n[" + (OnScreen.IsNVV1 ? "Bật" : "Tắt") + "]", this, 24, null));
			myVector1.addElement(new Command("Auto\nbán vàng\n[" + (AutoUpGrade.isSaleGoldToUpgrade ? "Bật" : "Tắt") + "]", this, 27, null));
			myVector1.addElement(new Command("Xoá\ndanh sách\nnâng cấp", this, 25, null));
			GameCanvas.menu.startAt(myVector1, 8);
		}
		if (idAction == 9)
		{
			PickMobController.isVutDo = !PickMobController.isVutDo;
			if (PickMobController.isVutDo)
			{
				PickMobController.lastTimeVutDo = mSystem.currentTimeMillis();
			}
			GameScr.info1.addInfo("Auto vứt đồ khi hành trang đầy: " +  (PickMobController.isVutDo ? "Bật" : "Tắt"), 0);
		}
		if (idAction == 10)
		{
			GameScr.info1.addInfo(idAction.ToString(), 0);
		}
		if (idAction == 11)
		{
			GameScr.info1.addInfo(idAction.ToString(), 0);
		}
		if (idAction == 12)
		{
			GameScr.info1.addInfo(idAction.ToString(), 0);
		}
		if (idAction == 13)
		{
            if (bool.Parse(p.ToString()))
			{
				OnScreen.IshideNShowCSSP = true;
				OnScreen.IshideNShowCSDT = true;
				OnScreen.viewBoss = true;
				OnScreen.nhanVat = true;
				OnScreen.IsThongTinCN = true;
			}
            else
            {
				OnScreen.IshideNShowCSSP = false;
				OnScreen.IshideNShowCSDT = false;
				OnScreen.viewBoss =	false;
				OnScreen.nhanVat = false;
				OnScreen.IsThongTinCN = false;
			}

			Option.SaveOptInt("cssp", OnScreen.IshideNShowCSSP ? 1 : 0);
			Option.SaveOptInt("csdt", OnScreen.IshideNShowCSDT ? 1 : 0);
			Option.SaveOptInt("vb", OnScreen.viewBoss ? 1 : 0);
			Option.SaveOptInt("nv", OnScreen.nhanVat ? 1 : 0);
			Option.SaveOptInt("tt", OnScreen.IsThongTinCN ? 1 : 0);
		}
		if (idAction == 14)
		{
			OnScreen.IshideNShowCSSP = !OnScreen.IshideNShowCSSP;
            Option.SaveOptInt("cssp", OnScreen.IshideNShowCSSP ? 1 : 0);
        }
		if (idAction == 15)
		{
			OnScreen.IshideNShowCSDT = !OnScreen.IshideNShowCSDT;
			Option.SaveOptInt("csdt", OnScreen.IshideNShowCSDT ? 1 : 0);
		}
		if (idAction == 16)
		{
			OnScreen.viewBoss = !OnScreen.viewBoss;
			Option.SaveOptInt("vb", OnScreen.viewBoss ? 1 : 0);
		}
		if (idAction == 17)
		{
			OnScreen.nhanVat = !OnScreen.nhanVat;
			Option.SaveOptInt("nv", OnScreen.nhanVat ? 1 : 0);
		}
		if (idAction == 18)
		{
			OnScreen.IsThongTinCN = !OnScreen.IsThongTinCN;
			Option.SaveOptInt("tt", OnScreen.IsThongTinCN ? 1 : 0);
		}
		if (idAction == 19)
		{
			loadMob();
		}
		if (idAction == 20)
		{
			loadMob();
		}
		if (idAction == 21)
		{
			PickMob.PickMob.IsTanSat = !PickMob.PickMob.IsTanSat;
			if (PickMob.PickMob.TypeMobsTanSat.Count > 0) PickMob.PickMob.TypeMobsTanSat.Clear();
			GameScr.info1.addInfo("Tự động đánh quái: " + (PickMob.PickMob.IsTanSat ? "Bật" : "Tắt"), 0);
		}
		if (idAction == 22)
		{
			Mob mob = (Mob)p;
			if (PickMob.PickMob.TypeMobsTanSat.Contains(mob.getTemplate().mobTemplateId))
			{
				PickMob.PickMob.TypeMobsTanSat.Remove(mob.getTemplate().mobTemplateId);
				GameScr.info1.addInfo($"Đã xoá loại mob: {Mob.arrMobTemplate[mob.getTemplate().mobTemplateId].name}[{mob.getTemplate().mobTemplateId}]", 0);
			}
			else
			{
				PickMob.PickMob.TypeMobsTanSat.Add(mob.getTemplate().mobTemplateId);
				GameScr.info1.addInfo($"Đã thêm loại mob: {Mob.arrMobTemplate[mob.getTemplate().mobTemplateId].name}[{mob.getTemplate().mobTemplateId}]", 0);
			}
			PickMob.PickMob.IsTanSat = true;
			GameScr.info1.addInfo("Tự động đánh quái: Bật" , 0);
		}
		if (idAction == 23)
		{
			AutoUpGrade.IsNhanVang = !AutoUpGrade.IsNhanVang;
			GameScr.info1.addInfo("Auto nhận vàng " + (UglyBoy.AutoUpGrade.IsNhanVang ? "Bật" : "Tắt"), 0);
		}
		if(idAction == 24)
        {
			OnScreen.IsNVV1 = !OnScreen.IsNVV1;
			GameScr.info1.addInfo("Auto nhận vàng tại nhà " + (OnScreen.IsNVV1 ? "Bật" : "Tắt"), 0);
		}
		if (idAction == 25)
		{
			AutoUpGrade.listUpgrade.Clear();
			AutoUpGrade.listShow.Clear();
			GameScr.info1.addInfo("Danh sách nâng cấp đã xoá", 0);
		}
		if (idAction == 26)
		{
			PickMob.PickMob.IsTanSat = !PickMob.PickMob.IsTanSat;
			if (PickMob.PickMob.TypeMobsTanSat.Count > 0) PickMob.PickMob.TypeMobsTanSat.Clear();
			GameScr.info1.addInfo("Tự động đánh quái: " + (PickMob.PickMob.IsTanSat ? "Bật" : "Tắt"), 0);
		}
		if (idAction == 27)
		{
			AutoUpGrade.isSaleGoldToUpgrade = !AutoUpGrade.isSaleGoldToUpgrade;
			GameScr.info1.addInfo("Auto bán vàng khi nâng cấp:  " + (AutoUpGrade.isSaleGoldToUpgrade ? "Bật" : "Tắt"), 0);
		}
		if(idAction == 28)
        {
			Service.gI().openMenu(3);
		}
		if (idAction == 29)
		{
			loadKhamSPL();
		}
		if (idAction == 30)
		{
			Npc npc2 = GameScr.findNPCInMap(21);
			if (npc2 != null) XmapController.MoveMyChar(npc2.cx, npc2.cy);
			Service.gI().openMenu(21);
			Service.gI().confirmMenu(21, 0);
			AutoUpGrade.idSPL = (int)p;
			AutoUpGrade.isKham2 = true;
		}
		if(idAction == 31)
        {
			loadNPC();
        }
		if (idAction == 32)
		{
			Npc npc = (Npc)p;
			if (npc != null) XmapController.MoveMyChar(npc.cx, npc.cy);
			Service.gI().openMenu(npc.template.npcTemplateId);
		}
		if (idAction == 33)
		{
			PickMobController.isGoToMapBoss = !PickMobController.isGoToMapBoss;
			GameScr.info1.addInfo("Auto đến map boss: " + (PickMobController.isGoToMapBoss ? "Bật" : "Tắt"), 0);
		}
		if(idAction == 34)
        {
			MyVector myVector3 = new MyVector();
			for (int i = 1; i <= 6; i++)
			{
				myVector3.addElement(new Command(i + " sao", AutoNoiTai.gI(), 13, i + 13));
			}
			GameCanvas.menu.startAt(myVector3, 3);
		}
		if (idAction == 35)
		{
			loadTrangBi();
		}
		if(idAction == 36)
        {
			AutoUpGrade.idDaNangCap = (int)p;
			Npc npc2 = GameScr.findNPCInMap(21);
			if (npc2 != null) XmapController.MoveMyChar(npc2.cx, npc2.cy);
			Service.gI().openMenu(21);
			Service.gI().confirmMenu(21, 1);
			AutoUpGrade.isNCTB = true;
		}
	}

	public void startChat(int type, string caption, string noidung)
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
		chatTextField.tfChat.setText(noidung);
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

	public void onChatFromMe(string text, string to)
	{
		if (GameCanvas.panel.chatTField.strChat == sanboss)
		{
			string text2 = GameCanvas.panel.chatTField.tfChat.getText();
			File.AppendAllText("Data\\bossName.txt", "," + text2);
			GameCanvas.panel.chatTField.isShow = false;
			GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		}
		if (GameCanvas.panel.chatTField.strChat == autochat)
		{
			string text3 = GameCanvas.panel.chatTField.tfChat.getText();
			using (StreamWriter streamWriter2 = new StreamWriter("Ugly\\chat.txt"))
			{
				streamWriter2.Write(text3);
				streamWriter2.Close();
			}
			GameCanvas.panel.chatTField.isShow = false;
			GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		}
		GameCanvas.panel.chatTField.parentScreen = GameCanvas.panel;
	}

	public void onCancelChat()
	{
		GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
	}
}
