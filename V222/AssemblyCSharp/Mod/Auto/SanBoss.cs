using System.IO;
using UnityEngine;

namespace UglyBoy;

internal class SanBoss
{
	private static SanBoss instance;

	private MyVector NBoss = new MyVector();

	private MyVector Boss = new MyVector();

	public bool fboss;

	public string s;

	public bool sb;

	private bool nrden = false;

	public static SanBoss gI()
	{
		if (instance == null)
		{
			return instance = new SanBoss();
		}
		return instance;
	}

	public void paint(mGraphics g)
	{
		for (int i = 0; i < Boss.size(); i++)
		{
			TimeTB timeTB = (TimeTB)Boss.elementAt(i);
			if (timeTB.tb.Contains(TileMap.mapName) && sb)
			{
				mFont.tahoma_7_red.drawStringBorder(g, timeTB.tb + $"-{timeTB.m}:{timeTB.s}", GameCanvas.w, 100 + 12 * i, mFont.RIGHT, mFont.tahoma_7_grey);
			}
			else
			{
				mFont.tahoma_7_yellow.drawStringBorder(g, timeTB.tb + $"-{timeTB.m}:{timeTB.s}", GameCanvas.w, 100 + 12 * i, mFont.RIGHT, mFont.tahoma_7_grey);
			}
		}
		int num = 0;
		nrden = false;
		for (int j = 0; j < GameScr.vCharInMap.size(); j++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(j);
			if (!@char.isPet && !@char.isMiniPet && !@char.cName.StartsWith("#") && !@char.cName.StartsWith("$") /*&& MyMenu.charinfo*/)
			{
				int y = 172 + 12 * num;
				string text = @char.cName + ": " + @char.cHP + "/" + @char.cHPFull;
				text = setText(@char, text);
				mFont mFont = mFont.tahoma_7_yellow;
				g.setColor(256252, 0.5f);
				g.fillRect(GameCanvas.w - 200, y, 200, mFont.tahoma_7_white.getHeight());
				if (checkBoss(@char.cName) && Char.myCharz().isMeCanAttackOtherPlayer(@char))
				{
					mFont = mFont.tahoma_7_red;
				}
				mFont.drawString(g, text, GameCanvas.w, y, mFont.RIGHT);
				num++;
			}
			if (@char.bag == 31 && TileMap.mapID >= 85 && TileMap.mapID <= 90)
			{
				mFont.tahoma_7_grey.drawString(g, "Đang Ôm NRSD: " + @char.cName + @char.cHP + "/" + @char.cHPFull, GameCanvas.w / 2, 90, mFont.CENTER);
				Char.myCharz().focusManualTo(@char);
				nrden = true;
				num++;
			}
		}
		if (!nrden && TileMap.mapID >= 85 && TileMap.mapID <= 90)
		{
			mFont.tahoma_7b_dark.drawString(g, "NRSD Đang Free", GameCanvas.w / 2, 90, mFont.CENTER);
		}
	}

	private string setText(Char c, string s)
	{
		if (c.isFlyAndCharge)
		{
			s += " QCKK";
		}
		if (c.isStandAndCharge)
		{
			s = ((c.cgender != 1) ? (s + " Bom") : (s + " Laze"));
		}
		return s;
	}

	public void drawLine(mGraphics g)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		Char @char = Char.myCharz();
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char char2 = (Char)GameScr.vCharInMap.elementAt(i);
			if (char2 == null)
			{
				continue;
			}
			if (checkBoss(char2.cName) && sb && !char2.isPet && !char2.isMiniPet)
			{
				g.setColor(Color.red);
				g.drawLine(@char.cx, @char.cy - @char.ch / 2, char2.cx, char2.cy - char2.ch / 2);
				if (fboss && Char.myCharz().charFocus != char2 && Char.myCharz().isMeCanAttackOtherPlayer(char2))
				{
					Char.myCharz().focusManualTo(char2);
				}
			}
			if (char2.charID == Char.myCharz().charID * -1)
			{
				g.setColor(Color.green);
				g.drawLine(@char.cx, @char.cy - @char.ch / 2, char2.cx, char2.cy - char2.ch / 2);
			}
			if (char2.bag == 31 && TileMap.mapID >= 85 && TileMap.mapID <= 90)
			{
				g.setColor(Color.black);
				g.drawLine(@char.cx, @char.cy - @char.ch / 2, char2.cx, char2.cy - char2.ch / 2);
			}
		}
		for (int j = 0; j < GameScr.vItemMap.size(); j++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(j);
			if (itemMap.template.id == 372)
			{
				g.setColor(Color.black);
				g.drawLine(@char.cx, @char.cy - @char.ch / 2, itemMap.x, itemMap.y);
			}
		}
	}

	public bool checkBoss(string str)
	{
		for (int i = 0; i < NBoss.size(); i++)
		{
			string value = (string)NBoss.elementAt(i);
			if (str.ToLower().IndexOf(value) != -1)
			{
				return true;
			}
		}
		return false;
	}

	public void ThongBao(string str)
	{
		if (!str.StartsWith("BOSS") || !checkBoss(str))
		{
			return;
		}
		string text = str.Substring(0, str.IndexOf(" vừa xuất hiện tại "));
		string text2 = str.Substring(str.IndexOf(" vừa xuất hiện tại ") + " vừa xuất hiện tại ".Length);
		if (Boss.size() > 6)
		{
			for (int i = 0; i < Boss.size(); i++)
			{
				Boss.set(i, Boss.elementAt(i + 1));
			}
			Boss.set(Boss.size() - 1, new TimeTB(text + "-" + text2));
		}
		else
		{
			Boss.addElement(new TimeTB(text + "-" + text2));
		}
	}

	public void LoadData()
	{
		if (File.Exists("Ugly\\boss.txt"))
		{
			NBoss.removeAllElements();
			s = "";
			using (StreamReader streamReader = new StreamReader("Ugly\\boss.txt"))
			{
				s = streamReader.ReadToEnd();
				streamReader.Close();
			}
			if (s.Length > 0)
			{
				string[] array = s.Split(';');
				for (int i = 0; i < array.Length; i++)
				{
					NBoss.addElement(array[i]);
				}
			}
		}
		else
		{
			File.Create("Ugly\\boss.txt");
		}
	}

	public void Update()
	{
		if (Boss.size() > 0)
		{
			for (int i = 0; i < Boss.size(); i++)
			{
				TimeTB timeTB = (TimeTB)Boss.elementAt(i);
				timeTB.Update2();
			}
		}
		int num = 0;
		for (int j = 0; j < GameScr.vCharInMap.size(); j++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(j);
			if (@char == null || @char.isPet || @char.isMiniPet || @char.cName.StartsWith("#") || @char.cName.StartsWith("$"))
			{
				continue;
			}
			int y = 172 + 12 * num;
			if (GameCanvas.isPointerHoldIn(GameCanvas.w - 200, y, 200, mFont.tahoma_7_white.getHeight()))
			{
				GameCanvas.isPointerJustDown = false;
				if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
				{
					Char.myCharz().focusManualTo(@char);
					SoundMn.gI().buttonClick();
					Char.myCharz().currentMovePoint = null;
					GameCanvas.clearAllPointerEvent();
				}
			}
			num++;
		}
	}
}
