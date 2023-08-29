using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Mod.Auto.AutoUseItem;

namespace Mod.Auto
{
    internal class AutoBuy : IActionListener, IChatable
    {
        public static bool isBuyItem;

        public static string title = "Nhập số lượng cần mua và thời gian delay (ms)";

        public static List<ItemBuy> listItemBuy = new();

        public static Item currentItem;

        public static sbyte typeBuy;

        public static AutoBuy gI { get; } = new AutoBuy();

        public struct ItemBuy
        {
            public int soLanMua;
            public int timeBuy;
            public long lastTimeMua;
            public sbyte typeBuy;
            public Item item;

            public ItemBuy(int soLanMua, int timeBuy, long lastTimeMua, Item item, sbyte typeBuy)
            {
                this.soLanMua = soLanMua;
                this.timeBuy = timeBuy;
                this.lastTimeMua = lastTimeMua;
                this.item = item;
                this.typeBuy = typeBuy;
            }
        }

        public static void update()
        {
            if (listItemBuy.Count <= 0)
            {
                isBuyItem = false;
                listItemBuy.Clear();
                return;
            }
            foreach (ItemBuy item in listItemBuy)
            {
                if (item.soLanMua == 0)
                {
                    listItemBuy.Remove(item);
                    GameScr.info1.addInfo($"Auto mua {item.item.template.name} đã dừng", 0);
                    continue;
                }
                if (item.soLanMua > 0 && mSystem.currentTimeMillis() - item.lastTimeMua > item.timeBuy)
                {
                    buyItem(item);
                    ItemBuy i = new()
                    {
                        typeBuy = item.typeBuy,
                        soLanMua = item.soLanMua - 1,
                        timeBuy = item.timeBuy,
                        lastTimeMua = mSystem.currentTimeMillis(),
                        item = item.item
                    };
                    RefreshListItem(i);
                }
            }
        }

        public static void buyItem(ItemBuy i)
        {
            try
            {
                Service.gI().buyItem(i.typeBuy, i.item.template.id, 0);
            }
            catch (Exception ex)
            {
                isBuyItem = false;
                listItemBuy.Clear();
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }

        public static List<ItemBuy> RefreshListItem(ItemBuy item)
        {
            int index = listItemBuy.FindIndex(i => i.item.template.id == item.item.template.id);
            if (index != -1)
                listItemBuy[index] = item;
            return listItemBuy;
        }

        public static void startChat()
        {
            GameCanvas.panel.chatTField = new ChatTextField();
            GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
            GameCanvas.panel.chatTField.initChatTextField();
            GameCanvas.panel.chatTField.strChat = string.Empty;
            GameCanvas.panel.chatTField.tfChat.name = "Mỗi giá trị cách nhau 1 khoảng trắng (VD: 100 1000)";
            GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
            GameCanvas.panel.chatTField.startChat2(new AutoBuy(), title);
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    currentItem = (Item)p;
                    typeBuy = 0;
                    startChat();
                    break;
                case 2:
                    currentItem = (Item)p;
                    typeBuy = 1;
                    startChat();
                    break;
                case 3:
                    currentItem = (Item)p;
                    typeBuy = 3;
                    startChat();
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
                    if (array.Length > 2 || array.Length < 2)
                    {
                        GameScr.info1.addInfo("Phải nhập đúng 2 giá trị", 0);
                        GameCanvas.panel.chatTField.isShow = false;
                        GameCanvas.panel.chatTField.ResetTF();
                        return;
                    }
                    if (!int.TryParse(array[0], out int soluong) || !int.TryParse(array[1], out int timeDelay))
                    {
                        GameScr.info1.addInfo("Các giá trị phải là số tự nhiên", 0);
                        GameCanvas.panel.chatTField.isShow = false;
                        GameCanvas.panel.chatTField.ResetTF();
                        return;
                    }
                    if (soluong < 0 || timeDelay < 0)
                    {
                        GameScr.info1.addInfo("Các giá trị phải là số tự nhiên lớn hơn 0", 0);
                        GameCanvas.panel.chatTField.isShow = false;
                        GameCanvas.panel.chatTField.ResetTF();
                        return;
                    }
                    ItemBuy item = new()
                    {
                        soLanMua = soluong - 1,
                        timeBuy = timeDelay,
                        lastTimeMua = mSystem.currentTimeMillis(),
                        item = currentItem,
                        typeBuy = typeBuy
                    };
                    listItemBuy.Add(item);
                    isBuyItem = true;
                    GameScr.info1.addInfo($"Auto mua {item.item.template.name} {item.soLanMua + 1} lần", 0);
                    Service.gI().buyItem(typeBuy, item.item.template.id, 0);
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
