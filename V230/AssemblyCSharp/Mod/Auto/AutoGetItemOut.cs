using Mod.Auto.AutoChat;
using Mod.CustomPanel;
using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.ModMenu;
using Mod.TeleportMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
using static Mod.TeleportMenu.TeleportMenu;

namespace Mod.Auto
{
    internal class AutoGetItemOut : IChatable, IActionListener
    {
        public static bool isGetItemOut;
        private static bool isDataLoaded;
        public static bool isFullBag = false;
        private static string titleStartChat = "Nhập từ muốn thay đổi";
        public static List<string> listItemGetOut = new();
        public static long lastTimeVutDo = 0;
        public static sbyte idBag = 0;

        public static AutoGetItemOut gI { get; } = new AutoGetItemOut();

        public static void update()
        {
            if (mSystem.currentTimeMillis() - lastTimeVutDo > 500)
            {

                if (idBag >= global::Char.myCharz().arrItemBag.Length)
                {
                    idBag = 0;
                    isFullBag = false;
                    GameScr.info1.addInfo("Auto vứt vật phẩm kết thúc", 0);
                }
                if (Char.myCharz().arrItemBag[idBag] == null)
                {
                    idBag++;
                    return;
                }
                //for (int i = 0; i < listItemGetOut.Count; i++)
                //{
                string itemName = Char.myCharz().arrItemBag[idBag].template.name.ToLower();
                if (listItemGetOut.ConvertAll(str => str.Trim().ToLower()).Any(itemName.Contains))
                {
                    Service.gI().useItem(1, 1, idBag, -1);
                    if (GameCanvas.currentDialog != null)
                    {
                        ItemObject itemObject = new()
                        {
                            type = 1,
                            id = idBag,
                            where = 1
                        };
                        GameCanvas.panel.perform(2004, itemObject);
                    }
                    lastTimeVutDo = mSystem.currentTimeMillis();
                    GameCanvas.endDlg();
                    return;
                }
                //}
                idBag++;
            }
        }

        [ChatCommand("getout")]
        public static void ShowMenu()
        {
            new MenuBuilder()
                .addItem(ifCondition: listItemGetOut.Count > 0, "Auto vứt\nvật phẩm\n" + "[" + (isGetItemOut ? "On" : "Off") + "]", new(() =>
                {
                    isGetItemOut = !isGetItemOut;
                    if (isGetItemOut) lastTimeVutDo = mSystem.currentTimeMillis();
                    GameScr.info1.addInfo("Đã " + (isGetItemOut ? "bật" : "tắt") + " auto vứt vật phẩm", 0);
                }))
                .addItem("Nhập từ mới", new(() =>
                {
                    ChatTextField.gI().strChat = "Nhập từ mới";
                    ChatTextField.gI().tfChat.name = "Từ mới";
                    ChatTextField.gI().startChat2(gI, string.Empty);
                }))
                .addItem(ifCondition: listItemGetOut.Count > 0, "Danh sách\n từ đã lưu", new(() =>
                {
                    showListItemGetOutPanel();
                }))
                .addItem(ifCondition: listItemGetOut.Count > 0, "Xóa tất\ncả", new(() =>
                {
                    listItemGetOut.Clear();
                    SaveData();
                    GameScr.info1.addInfo("Đã xóa toàn bộ từ đã lưu!", 0);
                }))
                .start();
        }

        private static void showListItemGetOutPanel()
        {
            CustomPanelMenu.show(setTabItemGetOutPanel, doFireItemGetOutPanel, paintTabHeader, paintItemGetOutPanel);
        }

        public static void setTabItemGetOutPanel(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, listItemGetOut);
        }

        public static void doFireItemGetOutPanel(Panel panel)
        {
            if (panel.selected < 0) return;
            string str = listItemGetOut[panel.selected].Trim();
            new MenuBuilder()
                .addItem("Thay đổi", new(() =>
                {
                    panel.chatTField = new ChatTextField();
                    panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                    panel.chatTField.initChatTextField();
                    panel.chatTField.strChat = string.Empty;
                    panel.chatTField.tfChat.name = "Từ mới";
                    panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    panel.chatTField.startChat2(new AutoGetItemOut(), titleStartChat);
                    //showListItemGetOutPanel();
                }))
                .addItem(mResources.DELETE, new(() =>
                {
                    listItemGetOut.Remove(str);
                    SaveData();
                    GameScr.info1.addInfo($"Đã xóa từ \"{str}\" ra khỏi danh sách vứt đồ!", 0);
                    //showListItemGetOutPanel();
                    GameCanvas.panel.EmulateSetTypePanel(0);
                    setTabItemGetOutPanel(GameCanvas.panel);
                }))
                .setPos(panel.X, (panel.selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
                .start();

            panel.cp = new ChatPopup();
            panel.cp.isClip = false;
            panel.cp.sayWidth = 180;
            panel.cp.cx = 3 + panel.X - (panel.X != 0 ? Res.abs(panel.cp.sayWidth - panel.W) + 8 : 0);
            panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + str + "\n|6|Vật phẩm có tên chứa từ: " + $"\"{str}\"", panel.cp.sayWidth - 10);
            panel.cp.delay = 10000000;
            panel.cp.c = null;
            panel.cp.sayRun = 7;
            panel.cp.ch = 15 - panel.cp.sayRun + panel.cp.says.Length * 12 + 10;
            if (panel.cp.ch > GameCanvas.h - 80)
            {
                panel.cp.ch = GameCanvas.h - 80;
                panel.cp.lim = panel.cp.says.Length * 12 - panel.cp.ch + 17;
                if (panel.cp.lim < 0)
                {
                    panel.cp.lim = 0;
                }
                ChatPopup.cmyText = 0;
                panel.cp.isClip = true;
            }
            panel.cp.cy = GameCanvas.menu.menuY - panel.cp.ch;
            while (panel.cp.cy < 10)
            {
                panel.cp.cy++;
                GameCanvas.menu.menuY++;
            }
            panel.cp.mH = 0;
            panel.cp.strY = 10;
        }

        private static void paintTabHeader(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Danh sách vứt đồ");
        }

        public static void paintItemGetOutPanel(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.paintCollectionCaptionAndDescriptionTemplate(panel, g, listItemGetOut,
                str => str, str => $"Vật phẩm có tên chứa từ \"{str}\"");
        }

        public static void SaveData()
        {
            string data = "";
            foreach (string str in listItemGetOut)
            {
                data += str.Trim() + ",";
            }
            Utilities.saveRMSString("listItemGetOut", data);
        }

        public static void LoadData()
        {
            try
            {
                if (!isDataLoaded) foreach (string str in Utilities.loadRMSString("listItemGetOut").Split(','))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(str.Trim()))
                            {
                                if (listItemGetOut.Contains(str.Trim())) continue;
                                listItemGetOut.Add(str.Trim());
                            }
                        }
                        catch (Exception) { }
                    }
                isDataLoaded = true;
            }
            catch (Exception) { }
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    break;
            }
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && string.IsNullOrEmpty(text))
                return;

            if (to == titleStartChat)
            {
                try
                {
                    string oldStr = listItemGetOut[GameCanvas.panel.selected];
                    int index = listItemGetOut.FindIndex(str => str == oldStr);
                    if (index != -1)
                        listItemGetOut[index] = text.Trim();
                    SaveData();
                    GameCanvas.panel.EmulateSetTypePanel(0);
                    setTabItemGetOutPanel(GameCanvas.panel);
                    GameScr.info1.addInfo($"Đã thay đổi từ \"{oldStr}\" thành từ \"{text}\" thành công!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else if (ChatTextField.gI().strChat.Contains("Nhập từ mới"))
            {
                try
                {
                    if (listItemGetOut.Contains(text.Trim()) && listItemGetOut.Count > 0)
                    {
                        GameScr.info1.addInfo($"Từ \"{text}\" đã tồn tại!", 0);
                        return;
                    }
                    listItemGetOut.Add(text.Trim());
                    SaveData();
                    GameScr.info1.addInfo($"Đã thêm từ \"{text}\" vào danh sách thành công!", 0);
                    ShowMenu();

                }
                catch (Exception)
                {
                    GameScr.info1.addInfo("Đã xảy ra lỗi!", 0);
                }
            }
            else if(ChatTextField.gI().isShow)
                ChatTextField.gI().isShow = false;
            else
                GameCanvas.panel.chatTField.isShow = false;

            if (ChatTextField.gI().isShow)
                ChatTextField.gI().ResetTF();
            else
                GameCanvas.panel.chatTField.ResetTF();
        }

        public void onCancelChat()
        {
            if (ChatTextField.gI().isShow)
            {
                ChatTextField.gI().isShow = false;
                ChatTextField.gI().ResetTF();
            }
            else
            {
                GameCanvas.panel.chatTField.isShow = false;
                GameCanvas.panel.chatTField.ResetTF();
            }
        }
    }
}
