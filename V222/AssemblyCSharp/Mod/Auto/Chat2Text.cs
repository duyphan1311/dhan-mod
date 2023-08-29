using AssemblyCSharp.Mod.Auto;

namespace AssemblyCSharp.Mod;
public class Chat2Text : mScreen, IActionListener
{
	public static Chat2Text instance;

	public TField tfSerial;

	public TField tfCode;

	private int x;

	private int y;

	private int w;

	private int h;

	private string[] strPaint;

	private int focus;

	private int yt;

	private int freeAreaHeight;

	private int yy = GameCanvas.hh - mScreen.ITEM_HEIGHT - 5;

	private int yP;

	public static string strTitle = string.Empty;
	public static string strTFCode = string.Empty;

	public Chat2Text()
	{
		w = GameCanvas.w - 20;
		if (w > 320)
		{
			w = 320;
		}
		strPaint = mFont.tahoma_7b_green2.splitFontArray(strTitle, w - 20);
		x = (GameCanvas.w - w) / 2;
		y = GameCanvas.h - 150 - (strPaint.Length - 1) * 20;
		h = 110 + (strPaint.Length - 1) * 20;
		yP = y;
		tfSerial = new TField();
		tfSerial.name = "Số lượng";
		tfSerial.x = x + 10;
		tfSerial.y = y + 35 + (strPaint.Length - 1) * 20;
		yt = tfSerial.y;
		tfSerial.width = w - 20;
		tfSerial.height = mScreen.ITEM_HEIGHT + 2;
		if (GameCanvas.isTouch)
		{
			tfSerial.isFocus = false;
		}
		else
		{
			tfSerial.isFocus = true;
		}
		tfSerial.setIputType(TField.INPUT_TYPE_ANY);
		if (Main.isWindowsPhone)
		{
			tfSerial.showSubTextField = false;
		}
		if (Main.isIPhone)
		{
			tfSerial.isPaintMouse = false;
		}
		if (!GameCanvas.isTouch)
		{
			right = tfSerial.cmdClear;
		}
		tfCode = new TField();
		tfCode.name = strTFCode;
		tfCode.x = x + 10;
		tfCode.y = tfSerial.y + 35;
		tfCode.width = w - 20;
		tfCode.height = mScreen.ITEM_HEIGHT + 2;
		tfCode.isFocus = false;
		tfCode.setIputType(TField.INPUT_TYPE_ANY);
		if (Main.isWindowsPhone)
		{
			tfCode.showSubTextField = false;
		}
		if (Main.isIPhone)
		{
			tfCode.isPaintMouse = false;
		}
		left = new Command(mResources.CLOSE, this, 1, null);
		center = new Command("Đồng ý", this, 2, null);
		if (GameCanvas.isTouch)
		{
			center.x = GameCanvas.w / 2 + 18;
			left.x = GameCanvas.w / 2 - 85;
			center.y = (left.y = y + h + 5);
		}
		freeAreaHeight = tfSerial.y - (4 * tfSerial.height - 10);
		yP = tfSerial.y;
	}

	public static Chat2Text gI()
	{
		if (instance == null)
		{
			instance = new Chat2Text();
		}
		return instance;
	}

	public override void switchToMe()
	{
		focus = 0;
		tfSerial.isFocus = true;
		base.switchToMe();
	}

	public void updateTfWhenOpenKb()
	{
	}

	public override void paint(mGraphics g)
	{
		GameScr.gI().paint(g);
		PopUp.paintPopUp(g, x, y, w, h, -1, isButton: true);
		for (int i = 0; i < strPaint.Length; i++)
		{
			mFont.tahoma_7b_green2.drawString(g, strPaint[i], GameCanvas.w / 2, y + 15 + i * 20, mFont.CENTER);
		}
		tfSerial.paint(g);
		tfCode.paint(g);
		base.paint(g);
	}

	public override void update()
	{
		GameScr.gI().update();
		tfSerial.update();
		tfCode.update();
		if (Main.isWindowsPhone)
		{
			updateTfWhenOpenKb();
		}
	}

	public override void keyPress(int keyCode)
	{
		if (tfSerial.isFocus)
		{
			tfSerial.keyPressed(keyCode);
		}
		else if (tfCode.isFocus)
		{
			tfCode.keyPressed(keyCode);
		}
		base.keyPress(keyCode);
	}

	public override void updateKey()
	{
		if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21])
		{
			focus--;
			if (focus < 0)
			{
				focus = 1;
			}
		}
		else if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
		{
			focus++;
			if (focus > 1)
			{
				focus = 1;
			}
		}
		if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] || GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
		{
			GameCanvas.clearKeyPressed();
			if (focus == 1)
			{
				tfSerial.isFocus = false;
				tfCode.isFocus = true;
				if (!GameCanvas.isTouch)
				{
					right = tfCode.cmdClear;
				}
			}
			else if (focus == 0)
			{
				tfSerial.isFocus = true;
				tfCode.isFocus = false;
				if (!GameCanvas.isTouch)
				{
					right = tfSerial.cmdClear;
				}
			}
			else
			{
				tfSerial.isFocus = false;
				tfCode.isFocus = false;
			}
		}
		if (GameCanvas.isPointerJustRelease)
		{
			if (GameCanvas.isPointerHoldIn(tfSerial.x, tfSerial.y, tfSerial.width, tfSerial.height))
			{
				focus = 0;
			}
			else if (GameCanvas.isPointerHoldIn(tfCode.x, tfCode.y, tfCode.width, tfCode.height))
			{
				focus = 1;
			}
		}
		base.updateKey();
		GameCanvas.clearKeyPressed();
	}

	public void clearScreen()
	{
		instance = null;
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 1)
		{
			GameScr.instance.switchToMe();
			clearScreen();
		}
		if (idAction == 2)
		{
			if (tfSerial.getText() == null || tfSerial.getText().Equals(string.Empty))
			{
				GameCanvas.startOKDlg("Số lượng không được rỗng");
				return;
			}
			if (tfCode.getText() == null || tfCode.getText().Equals(string.Empty))
			{
				GameCanvas.startOKDlg("Thời gian delay không được rỗng");
				return;
			}
			if(strTitle == "Nhập số lượng cần dùng và thời gian delay mỗi lần")
            {
				UseItemMultiple.ItemUseMulti item = new();
				item.quatity = int.Parse(tfSerial.getText()) - 1;
				item.timeUse = int.Parse(tfCode.getText()) * 1000;
				item.lastTimeUse = mSystem.currentTimeMillis();
				item.item = UseItemMultiple.curr;
				UseItemMultiple.itemList.Add(item);
				UseItemMultiple.isUseItemMulti = true;
				UseItemMultiple.useItem(item.item);
				GameScr.info1.addInfo($"Auto mua item {item.item.template.name} {item.quatity + 1} lần", 0);
			}
			if(strTitle == "Nhập số lượng cần mua và thời gian delay")
            {
				UseItemMultiple.ItemBuyMulti item2 = new();
				item2.soLanMua = int.Parse(tfSerial.getText()) - 1;
				item2.timeBuy = int.Parse(tfCode.getText());
				item2.lastTimeMua = mSystem.currentTimeMillis();
				item2.item = UseItemMultiple.curr;
				item2.typeBuy = UseItemMultiple.typeBuy;
				UseItemMultiple.listItemBuy.Add(item2);
				UseItemMultiple.isAutoMuaDo = true;
				GameScr.info1.addInfo($"Auto mua item {item2.item.template.name} {item2.soLanMua + 1} lần", 0);
				Service.gI().buyItem(item2.typeBuy, item2.item.template.id, 0);
			}
			GameScr.instance.switchToMe();
			clearScreen();
		}
	}
}
