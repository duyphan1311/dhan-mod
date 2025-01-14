namespace Mod.Auto
{
    internal class AutoSellGold : IActionListener, IChatable
    {
        public static bool isBanVang;
        public static int solanSale;
        public static int timeSellGold;
        public static long lastTimeSaleGold;
        public static string title = "Nhập số lượng thỏi vàng và thời gian delay (ms)";

        public static AutoSellGold gI { get; } = new AutoSellGold();

        public static void update()
        {
            if (solanSale <= 0)
            {
                isBanVang = false;
                GameScr.info1.addInfo("Auto bán thỏi vàng dừng", 0);
                return;
            }
            if (mSystem.currentTimeMillis() - lastTimeSaleGold > timeSellGold)
            {
                var index = Utilities.getIndexItemBag(457);
                if (index == -1)
                {
                    isBanVang = false;
                    GameScr.info1.addInfo("Không tìm thấy thỏi vàng", 0);
                    return;
                }
                Service.gI().saleItem(1, 1, index);
                if (GameCanvas.currentDialog != null)
                {
                    ItemObject itemObject = new()
                    {
                        type = 1,
                        id = index,
                        where = 1
                    };
                    GameCanvas.panel.perform(3002, itemObject);
                }
                GameCanvas.endDlg();
                solanSale--;
                lastTimeSaleGold = mSystem.currentTimeMillis();
            }
        }
        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    GameCanvas.panel.chatTField = new ChatTextField();
                    GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                    GameCanvas.panel.chatTField.initChatTextField();
                    GameCanvas.panel.chatTField.strChat = string.Empty;
                    GameCanvas.panel.chatTField.tfChat.name = "Mỗi giá trị cách nhau 1 khoảng trắng (VD: 100 1000)";
                    GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    GameCanvas.panel.chatTField.startChat2(new AutoSellGold(), title);
                    break;
                case 2:
                    isBanVang = false;
                    GameScr.info1.addInfo("Auto bán thỏi vàng dừng", 0);
                    break;
            }
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (to == title)
            {
                try
                {
                    string[] array = text.Trim().Split(' ');
                    if (array.Length > 2/* || array.Length < 2*/)
                    {
                        GameScr.info1.addInfo("Phải nhập nhiều nhất 2 giá trị", 0);
                        GameCanvas.panel.chatTField.isShow = false;
                        GameCanvas.panel.chatTField.ResetTF();
                        return;
                    }
                    int sl = 0, dl = 100;
                    if (array.Length == 2)
                    {
                        if (!int.TryParse(array[0], out int soluong) || !int.TryParse(array[1], out int timeDelay))
                        {
                            GameScr.info1.addInfo("Các giá trị phải là số tự nhiên", 0);
                            GameCanvas.panel.chatTField.isShow = false;
                            GameCanvas.panel.chatTField.ResetTF();
                            return;
                        }
                        sl = soluong;
                        dl = timeDelay;
                    }
                    else
                    {
                        if (!int.TryParse(array[0], out int soluong))
                        {
                            GameScr.info1.addInfo("Các giá trị phải là số tự nhiên", 0);
                            GameCanvas.panel.chatTField.isShow = false;
                            GameCanvas.panel.chatTField.ResetTF();
                            return;
                        }
                        sl = soluong;
                    }
                    if (sl < 0 || dl < 0)
                    {
                        GameScr.info1.addInfo("Các giá trị phải là số tự nhiên lớn hơn 0", 0);
                        GameCanvas.panel.chatTField.isShow = false;
                        GameCanvas.panel.chatTField.ResetTF();
                        return;
                    }
                    solanSale = sl;
                    timeSellGold = dl;
                    isBanVang = true;
                    lastTimeSaleGold = mSystem.currentTimeMillis() - timeSellGold;
                    GameScr.info1.addInfo($"Bắt đầu auto bán vàng {NinjaUtil.getMoneys(solanSale)} lần " +
                        $"và khoảng cách giữa mỗi lần bán là {NinjaUtil.getMoneys(timeSellGold)}ms", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else
                GameCanvas.panel.chatTField.isShow = false;
            GameCanvas.panel.chatTField.ResetTF();
        }

        public void onCancelChat()
        {
            GameCanvas.panel.chatTField.isShow = false;
            GameCanvas.panel.chatTField.ResetTF();
        }
    }
}
