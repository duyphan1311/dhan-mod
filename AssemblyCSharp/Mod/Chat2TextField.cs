using Mod.Auto;

namespace Mod;
public class Chat2TextField : mScreen, IActionListener
{
    public static Chat2TextField instance;

    public TField tfQuatity;

    public TField tfTimeDelay;

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

    public static string strQuatity = "Số lượng";

    public static string strTimeDelay = string.Empty;

    public Chat2TextField()
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
        tfQuatity = new TField();
        tfQuatity.name = strQuatity;
        tfQuatity.x = x + 10;
        tfQuatity.y = y + 35 + (strPaint.Length - 1) * 20;
        yt = tfQuatity.y;
        tfQuatity.width = w - 20;
        tfQuatity.height = mScreen.ITEM_HEIGHT + 2;
        if (GameCanvas.isTouch)
        {
            tfQuatity.isFocus = false;
        }
        else
        {
            tfQuatity.isFocus = true;
        }
        tfQuatity.setIputType(TField.INPUT_TYPE_ANY);
        if (Main.isWindowsPhone)
        {
            tfQuatity.showSubTextField = false;
        }
        if (Main.isIPhone)
        {
            tfQuatity.isPaintMouse = false;
        }
        if (!GameCanvas.isTouch)
        {
            right = tfQuatity.cmdClear;
        }
        tfTimeDelay = new TField();
        tfTimeDelay.name = strTimeDelay;
        tfTimeDelay.x = x + 10;
        tfTimeDelay.y = tfQuatity.y + 35;
        tfTimeDelay.width = w - 20;
        tfTimeDelay.height = mScreen.ITEM_HEIGHT + 2;
        tfTimeDelay.isFocus = false;
        tfTimeDelay.setIputType(TField.INPUT_TYPE_ANY);
        if (Main.isWindowsPhone)
        {
            tfTimeDelay.showSubTextField = false;
        }
        if (Main.isIPhone)
        {
            tfTimeDelay.isPaintMouse = false;
        }
        left = new Command(mResources.OK, this, 1, null);
        center = new Command(mResources.CLOSE, this, 2, null);
        if (GameCanvas.isTouch)
        {
            center.x = GameCanvas.w / 2 + 18;
            left.x = GameCanvas.w / 2 - 85;
            center.y = (left.y = y + h + 5);
        }
        freeAreaHeight = tfQuatity.y - (4 * tfQuatity.height - 10);
        yP = tfQuatity.y;
    }

    public static Chat2TextField gI()
    {
        if (instance == null)
        {
            instance = new Chat2TextField();
        }
        return instance;
    }

    public override void switchToMe()
    {
        focus = 0;
        tfQuatity.isFocus = true;
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
        tfQuatity.paint(g);
        tfTimeDelay.paint(g);
        base.paint(g);
    }

    public override void update()
    {
        GameScr.gI().update();
        tfQuatity.update();
        tfTimeDelay.update();
        if (Main.isWindowsPhone)
        {
            updateTfWhenOpenKb();
        }
    }

    public override void keyPress(int keyCode)
    {
        if (tfQuatity.isFocus)
        {
            tfQuatity.keyPressed(keyCode);
        }
        else if (tfTimeDelay.isFocus)
        {
            tfTimeDelay.keyPressed(keyCode);
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
        if (GameCanvas.keyPressed[15])
        {
            if (left != null && tfQuatity.getText() != string.Empty && tfTimeDelay.getText() != string.Empty)
            {
                left.performAction();
            }
            GameCanvas.keyPressed[15] = false;
            GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
        }
        if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] || GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
        {
            GameCanvas.clearKeyPressed();
            if (focus == 1)
            {
                tfQuatity.isFocus = false;
                tfTimeDelay.isFocus = true;
                if (!GameCanvas.isTouch)
                {
                    right = tfTimeDelay.cmdClear;
                }
            }
            else if (focus == 0)
            {
                tfQuatity.isFocus = true;
                tfTimeDelay.isFocus = false;
                if (!GameCanvas.isTouch)
                {
                    right = tfQuatity.cmdClear;
                }
            }
            else
            {
                tfQuatity.isFocus = false;
                tfTimeDelay.isFocus = false;
            }
        }
        if (GameCanvas.isPointerJustRelease)
        {
            if (GameCanvas.isPointerHoldIn(tfQuatity.x, tfQuatity.y, tfQuatity.width, tfQuatity.height))
            {
                focus = 0;
            }
            else if (GameCanvas.isPointerHoldIn(tfTimeDelay.x, tfTimeDelay.y, tfTimeDelay.width, tfTimeDelay.height))
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
            if (tfQuatity.getText() == null || tfQuatity.getText().Equals(string.Empty))
            {
                GameCanvas.startOKDlg("Số lượng không được rỗng");
                return;
            }
            if (tfTimeDelay.getText() == null || tfTimeDelay.getText().Equals(string.Empty))
            {
                GameCanvas.startOKDlg("Thời gian delay không được rỗng");
                return;
            }
            if (strTitle == AutoUseItem.title)
            {
                AutoUseItem.ItemUse item = new()
                {
                    quatity = int.Parse(tfQuatity.getText()) - 1,
                    timeUse = int.Parse(tfTimeDelay.getText()) * 1000,
                    lastTimeUse = mSystem.currentTimeMillis(),
                    item = AutoUseItem.currentItem
                };
                AutoUseItem.listItemUse.Add(item);
                AutoUseItem.isUseItem = true;
                AutoUseItem.useItem(item);
                GameScr.info1.addInfo($"Auto sử dụng {item.item.template.name} {item.quatity + 1} lần", 0);
            }
            if (strTitle == AutoBuy.title)
            {
                AutoBuy.ItemBuy item2 = new()
                {
                    soLanMua = int.Parse(tfQuatity.getText()) - 1,
                    timeBuy = int.Parse(tfTimeDelay.getText()),
                    lastTimeMua = mSystem.currentTimeMillis(),
                    item = AutoBuy.currentItem,
                    typeBuy = AutoBuy.typeBuy
                };
                AutoBuy.listItemBuy.Add(item2);
                AutoBuy.isBuyItem = true;
                GameScr.info1.addInfo($"Auto mua {item2.item.template.name} {item2.soLanMua + 1} lần", 0);
                Service.gI().buyItem(item2.typeBuy, item2.item.template.id, 0);
            }
            if (strTitle == AutoSellGold.title)
            {
                AutoSellGold.solanSale = int.Parse(tfQuatity.getText());
                AutoSellGold.timeSellGold = int.Parse(tfTimeDelay.getText());
                AutoSellGold.isBanVang = true;
                AutoSellGold.lastTimeSaleGold = mSystem.currentTimeMillis() - AutoSellGold.timeSellGold;
                GameScr.info1.addInfo($"Bắt đầu auto bán vàng {AutoSellGold.solanSale} lần và và khoảng cách giữa mỗi lần bán là {AutoSellGold.timeSellGold}ms", 0);
            }
            GameScr.instance.switchToMe();
            clearScreen();
            if (strTitle == AutoUseItem.title)
            {
                GameCanvas.panel.setTypeMain();
                GameCanvas.panel.show();
            }
        }
        if (idAction == 2)
        {
            GameScr.instance.switchToMe();
            clearScreen();
        }
    }
}
