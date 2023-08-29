using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod.Auto
{
    internal class AutoUseItem : IActionListener, IChatable
    {
        public static bool isUseItem;

        public static string title = "Nhập số lượng cần dùng và thời gian delay (s)";

        public static List<ItemUse> listItemUse = new();

        public static Item currentItem;

        public static bool isEnabled;

        public static int distanceBetweenLines = 8;

        public static int x = 15 - 9;

        public static int y = 90;

        public static bool isCollapsed;

        static readonly string listTitle = "Danh sách vật phẩm tự động";

        static int titleWidth;

        static int maxLength = 0;

        public static AutoUseItem gI { get; } = new AutoUseItem();

        public struct ItemUse
        {
            public int quatity;
            public long timeUse;
            public long lastTimeUse;
            public Item item;

            public ItemUse(int quatity, int timeUse, long lastTimeUse, Item item)
            {
                this.quatity = quatity;
                this.timeUse = timeUse;
                this.lastTimeUse = lastTimeUse;
                this.item = item;
            }
        }

        public static void update()
        {
            if (listItemUse.Count <= 0)
            {
                isUseItem = false;
                listItemUse.Clear();
                return;
            }
            foreach (ItemUse item in listItemUse)
            {
                if (item.quatity == 0)
                {
                    listItemUse.Remove(item);
                    GameScr.info1.addInfo($"Auto dùng item {item.item.template.name} đã dừng", 0);
                    continue;
                }
                if (item.quatity > 0 && mSystem.currentTimeMillis() - item.lastTimeUse > item.timeUse)
                {
                    useItem(item);
                    ItemUse i = new()
                    {
                        quatity = item.quatity - 1,
                        timeUse = item.timeUse,
                        lastTimeUse = mSystem.currentTimeMillis(),
                        item = item.item
                    };
                    RefreshListItem(i);
                }
            }
        }

        public static void useItem(ItemUse item)
        {
            try
            {
                var index = Utilities.getIndexItemBag(item.item.template.id);
                if (index == -1) throw new Exception();
                Service.gI().useItem(0, 1, index, -1);
                //GameScr.info1.addInfo($"Đã sử dụng {item.template.name}", 0);
            }
            catch (Exception ex)
            {
                listItemUse.Remove(item);
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }

        public static List<ItemUse> RefreshListItem(ItemUse item)
        {
            int index = listItemUse.FindIndex(i => i.item.template.id == item.item.template.id);
            if (index != -1)
                listItemUse[index] = item;
            return listItemUse;
        }

        public static void Paint(mGraphics g)
        {
            if(!isUseItem) return;
            if (!isEnabled) return;
            if (listItemUse.Count <= 0) return;

            if (!isCollapsed)
                paintAutoUseItem(g);
            PaintRect(g);
        }

        public static string CustomString(ItemUse itemUse)
        {
            string itemName = "yellow";
            string quatity = "cyan";
            string timeDelay = "lime";
            long timeUseSecond = itemUse.timeUse / 1000;
            int timeUseHour = (int)(timeUseSecond / 3600);
            int timeUseMinute = (int)((timeUseSecond - timeUseHour * 3600) / 60);
            string strTimeUse = string.Empty;
            if (timeUseHour > 0)
                strTimeUse += $"<color={timeDelay}>{timeUseHour}</color>h";
            if (timeUseMinute > 0)
                strTimeUse += $"<color={timeDelay}>{timeUseMinute}</color>m";
            strTimeUse += $"<color={timeDelay}>{timeUseSecond - (timeUseHour * 3600) - (timeUseMinute * 60)}</color>s";
            string result = $"<color={itemName}>{getNameItemWithoutIDTag(itemUse.item)}</color> - SL: <color={quatity}>{itemUse.quatity}</color> " +
                $"- Delay: {strTimeUse} - ";
            long timeSpan = itemUse.timeUse - (mSystem.currentTimeMillis() - itemUse.lastTimeUse);
            int second = (int)(timeSpan / 1000);
            int hour = (int)(second / 3600);
            int minute = (int)((second - hour * 3600) / 60);
            if (hour > 0)
                result += $"<color=orange>{hour}</color>h";
            if (minute > 0)
                result += $"<color=orange>{minute}</color>m";
            result += $"<color=orange>{second - (hour * 3600) - (minute * 60)}</color>s";
            return result;
        }

        public static string getNameItemWithoutIDTag(Item item)
        {
            return item.template.name.Remove(0, item.template.name.IndexOf(']') + 1).Trim();
        }

        static void paintAutoUseItem(mGraphics g)
        {
            GUIStyle[] styles = new GUIStyle[listItemUse.Count];
            for (int i = 0; i < listItemUse.Count; i++)
            {
                styles[i] = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperLeft,
                    fontSize = 6 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Bold,
                    richText = true,
                };
                int length = Utilities.getWidth(styles[i], $" {i + 1}. {CustomString(listItemUse[i])}");
                maxLength = Math.max(length, maxLength);
            }
            FillBackground(g);
            int xDraw = x;
            for (int i = 0; i < listItemUse.Count; i++)
            {
                int yDraw = y + distanceBetweenLines * i;
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                g.fillRect(xDraw, yDraw + 1, maxLength, 7);
                g.drawString($" {i + 1}. {CustomString(listItemUse[i])}", x, mGraphics.zoomLevel - 3 + yDraw, styles[i]);
            }
        }

        static void PaintRect(mGraphics g)
        {
            int w = maxLength + 5;
            int h = distanceBetweenLines * listItemUse.Count + 7;
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 7 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperLeft,
                richText = true
            };
            style.normal.textColor = Color.white;
            titleWidth = Utilities.getWidth(style, $" {listTitle} ");
            g.setColor(new Color(.2f, .2f, .2f, .7f));
            g.fillRect(x, y - distanceBetweenLines, titleWidth, 8);
            if (GameCanvas.isMouseFocus(x, y - distanceBetweenLines, titleWidth, 8))
            {
                g.setColor(style.normal.textColor);
                g.fillRect(x, y - 1, titleWidth - 1, 1);
            }
            g.drawString($" <color=yellow>{listTitle}</color> ", x, y - distanceBetweenLines - 2, style);
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            g.drawRegion(Mob.imgHP, 0, 18, 9, 6, (isCollapsed ? 4 : 5), collapseButtonX, collapseButtonY, 0);
            if (isCollapsed)
                return;
            g.setColor(Color.yellow);
            g.fillRect(x + titleWidth + 4, y - 5, w - titleWidth - 7, 1);
            g.fillRect(x - 3, y - 5, 3, 1);
            g.fillRect(x - 3, y - 5, 1, h);
            g.fillRect(x + maxLength + 2, y - 5, 1, h + 1);
            g.fillRect(x - 3, y - 5 + h, w, 1);
        }

        private static void FillBackground(mGraphics g)
        {
            if (!isCollapsed)
            {
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
                int w = maxLength + 5;
                int h = distanceBetweenLines * listItemUse.Count + 7;
                g.fillRect(x - 3, y - 5, w, h);
            }
        }

        public static void UpdateTouch()
        {
            if (!isEnabled)
                return;
            if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                return;
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            if (GameCanvas.isPointerHoldIn(collapseButtonX, collapseButtonY, 6, 9)
                || GameCanvas.isMouseFocus(x, y - distanceBetweenLines, titleWidth, 8))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    isCollapsed = !isCollapsed;
                }
                Char.myCharz().currentMovePoint = null;
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }

        static void getCollapseButton(out int collapseButtonX, out int collapseButtonY)
        {
            collapseButtonX = x + titleWidth - 2;
            collapseButtonY = y - distanceBetweenLines + 1;
        }

        public static void setState(bool value) => isEnabled = value;

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    currentItem = (Item)p;
                    GameCanvas.panel.chatTField = new ChatTextField();
                    GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                    GameCanvas.panel.chatTField.initChatTextField();
                    GameCanvas.panel.chatTField.strChat = string.Empty;
                    GameCanvas.panel.chatTField.tfChat.name = "Mỗi giá trị cách nhau 1 khoảng trắng (VD: 100 1)";
                    GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    GameCanvas.panel.chatTField.startChat2(new AutoUseItem(), title);
                    break;
                case 2:
                    Item item = (Item)p;
                    foreach (ItemUse i in listItemUse)
                    {
                        if (i.item.template.id == item.template.id && i.item.GetFullInfo() == item.GetFullInfo())
                        {
                            listItemUse.Remove(i);
                            GameScr.info1.addInfo($"Auto dùng item {i.item.template.name} đã dừng", 0);
                            break;
                        }

                    }
                    if (listItemUse.Count <= 0)
                    {
                        isUseItem = false;
                        listItemUse.Clear();
                        GameScr.info1.addInfo("Auto dùng item dừng", 0);
                    }
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
                    if(array.Length > 2 || array.Length < 2)
                    {
                        GameScr.info1.addInfo("Phải nhập đúng 2 giá trị", 0);
                        GameCanvas.panel.chatTField.isShow = false;
                        GameCanvas.panel.chatTField.ResetTF();
                        return;
                    }
                    if (!int.TryParse(array[0], out int soluong) || !float.TryParse(array[1], out float timeDelay))
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
                    ItemUse item = new()
                    {
                        quatity = soluong - 1,
                        timeUse = (long)(timeDelay * 1000),
                        lastTimeUse = mSystem.currentTimeMillis(),
                        item = currentItem
                    };
                    listItemUse.Add(item);
                    isUseItem = true;
                    useItem(item);
                    GameScr.info1.addInfo($"Auto sử dụng {item.item.template.name} {item.quatity + 1} lần", 0);
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
