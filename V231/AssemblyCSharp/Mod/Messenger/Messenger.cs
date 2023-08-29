using Assets.src.g;
using LitJson;
using Mod.Auto;
using Mod.Auto.AutoChat;
using Mod.CustomPanel;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.Menu;
using Mod.Set;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.EventSystems;
using static Mod.Auto.AutoChat.Setup;

namespace Mod.Messenger
{
    internal class Messenger : IChatable, IActionListener
    {
        public static Messenger gI { get; } = new Messenger();
        public static List<Conversation> conversationList = new List<Conversation>();
        public static Conversation conversation = new Conversation();
        public static Char newChar;
        public static long lastTimeChat;
        public static int typePanel = -1;
        public static int newID;
        public static string newName;

        #region messenger panel
        public static void showMessengerPanel() => CustomPanelMenu.show(setMessnegerPanel, doFireMessenger, paintMessengerPanelHeader, paintMessengerPanel);

        private static void setMessnegerPanel(Panel panel)
        {
            typePanel = 0;

            panel.ITEM_HEIGHT = 24;

            panel.currentListLength = conversationList.Count + 1;

            panel.selected = GameCanvas.isTouch ? (-1) : 0;

            panel.cmyLim = panel.currentListLength * panel.ITEM_HEIGHT - panel.hScroll;
            if (panel.cmyLim < 0) panel.cmyLim = 0;

            panel.cmy = panel.cmtoY = panel.cmyLim;
            if (panel.cmy < 0) panel.cmy = panel.cmtoY = 0;
            if (panel.cmy > panel.cmyLim) panel.cmy = panel.cmtoY = panel.cmyLim;
        }

        private static void doFireMessenger(Panel panel)
        {
            if (panel.selected < 0) return;
            if (panel.selected == 0)
                panel.setTypeMessage();
            else
            {
                new MenuBuilder()
                .addItem("Xem", new(() =>
                {
                    conversation = conversationList[panel.selected - 1];
                    showConversation();
                }))
                .addItem("Xoá", new(() =>
                {
                    conversationList.RemoveAt(panel.selected - 1);
                    setMessnegerPanel(panel);
                    Utilities.EmulateSetTypePanel(panel, 0);
                }))
                .setPos(panel.X, (panel.selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
                .start();
            }
        }

        private static void paintMessengerPanel(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll - 1;
                int h = panel.ITEM_HEIGHT - 1;
                if (num2 - panel.cmy > panel.yScroll + panel.hScroll || num2 - panel.cmy < panel.yScroll - panel.ITEM_HEIGHT)
                {
                    continue;
                }
                g.setColor((i != panel.selected) ? 15196114 : 16383818);
                g.fillRect(num, num2, num3, h);
                if(i == 0)
                    mFont.tahoma_7b_dark.drawString(g, "Kênh thế giới", panel.xScroll + panel.wScroll / 2, num2 + 6, mFont.CENTER);
                else
                {
                    mFont.tahoma_7b_dark.drawString(g, conversationList[i - 1].name, panel.xScroll + panel.wScroll / 2, num2 + 6, mFont.CENTER);
                    if (conversationList[i - 1].isNewMessage && GameCanvas.gameTick % 20 > 10)
                        g.drawImage(Panel.imgNew, num3 - 15, num2 + 10, 3);
                }

            }
            panel.paintScrollArrow(g);
        }

        private static void paintMessengerPanelHeader(Panel panel, mGraphics g) => PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Tin nhắn");
        #endregion

        #region conversation
        public static void showConversation() => CustomPanelMenu.show(setConversation, null, paintConversationHeader, paintConversation);

        private static int getLenghtCurrentList(List<Message> messages, int w)
        {
            int lenght = 0;
            for (int i = 0; i < messages.Count; i++)
            {
                if (i == 0)
                    lenght += 2;
                else if (messages[i].date.Day != messages[i - 1].date.Day)
                    lenght += 2;
                lenght += mFont.tahoma_7.splitFontArray(messages[i].message, w).Length;
            }
            return lenght;
        }

        public static void setConversation(Panel panel)
        {
            typePanel = 1;

            conversation.isNewMessage = false;

            panel.ITEM_HEIGHT = 12;

            panel.currentListLength = getLenghtCurrentList(conversation.messages, 100);

            panel.selected = GameCanvas.isTouch ? (-1) : 0;

            panel.cmyLim = panel.currentListLength * panel.ITEM_HEIGHT - panel.hScroll;
            if (panel.cmyLim < 0) panel.cmyLim = 0;

            panel.cmy = panel.cmtoY = panel.cmyLim;
            if (panel.cmy < 0) panel.cmy = panel.cmtoY = 0;
            if (panel.cmy > panel.cmyLim) panel.cmy = panel.cmtoY = panel.cmyLim;
        }

        private static void paintConversation(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 7 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperLeft,
                richText = true,
            };
            style.normal.textColor = Color.black;
            int lastLength = 0;
            int lastH = 0;
            for (int i = conversation.messages.Count - 1; i >= 0; i--)
            {
                int x = 5;
                Message message = conversation.messages[i];
                string[] content = mFont.tahoma_7.splitFontArray(message.message, 100);
                int h = content.Length * (Utilities.getHeight(style, content[0]) - 3);
                lastH += h + 4;
                int y = panel.cmyLim + 280 - lastH - Utilities.getHeight(style, content[0]) * lastLength
                    - (conversation.messages.Count - 1 - i) * 3;
                int w = 0;
                for (int k = 0; k < content.Length; k++)
                    w = Math.max(w, Utilities.getWidth(style, content[k]));
                int m_x = x + w + 8;
                if (!conversation.messages[i].isRecieve)
                {
                    x = panel.wScroll - w - 5;
                    m_x = x - Utilities.getWidth(style, message.getTime()) - 2;
                }
                paintReactMessage(g, x, y, w + 6, h + 4);
                g.drawString(message.getTime(), m_x, y + h / 2 - Utilities.getHeight(style, message.getTime()) / 3, style);
                for (int j = 0; j < content.Length; j++)
                    g.drawString(content[j], x + 3, y + j * (Utilities.getHeight(style, content[j]) - 3), style);

                string dateStr = $" {message.getDate()} ";
                int y0 = y - Utilities.getHeight(style, dateStr) * 7 / 4 + 1;
                int x0 = panel.xScroll + panel.wScroll / 2 - Utilities.getWidth(style, dateStr) / 2;
                g.setColor(5921370);
                if (i == 0)
                {
                    g.fillRect(panel.X - 2, y0 + Utilities.getHeight(style, dateStr) / 2, x0 - panel.X, 1);
                    g.drawString(dateStr, x0, y0, style);
                    g.fillRect(x0 + Utilities.getWidth(style, dateStr) + 1, y0 + Utilities.getHeight(style, dateStr) / 2, x0 - panel.X - 2, 1);
                    lastLength += 2;
                }
                else if (message.date.Day != conversation.messages[i - 1].date.Day)
                {
                    g.fillRect(panel.X - 2, y0 + Utilities.getHeight(style, dateStr) / 2, x0 - panel.X, 1);
                    g.drawString(dateStr, x0, y0, style);
                    g.fillRect(x0 + Utilities.getWidth(style, dateStr) + 1, y0 + Utilities.getHeight(style, dateStr) / 2, x0 - panel.X - 2, 1);
                    lastLength += 2;
                }
            }
            panel.paintScrollArrow(g);
        }

        private static void paintConversationHeader(Panel panel, mGraphics g) => PaintPanelTemplates.paintTabHeaderTemplate(panel, g, $"{conversation.name} (Click để chat)");

        private static void paintReactMessage(mGraphics g, int x, int y, int w, int h)
        {
            Image goc = GameCanvas.loadImage("/mainImage/myTexture2dbd3.png");
            g.setColor(0);
            g.fillRect(x + 6, y, w - 14 + 1, h);
            g.fillRect(x, y + 6, w, h - 12 + 1);
            g.setColor(16777215);
            g.fillRect(x + 6, y + 1, w - 12, h - 2);
            g.fillRect(x + 1, y + 6, w - 2, h - 12);
            g.drawRegion(goc, 0, 0, 7, 6, 0, x, y, 0);
            g.drawRegion(goc, 0, 0, 7, 6, 2, x + w - 7, y, 0);
            g.drawRegion(goc, 0, 0, 7, 6, 1, x, y + h - 6, 0);
            g.drawRegion(goc, 0, 0, 7, 6, 3, x + w - 7, y + h - 6, 0);
        }
        #endregion

        public static bool hasNewMsg()
        {
            foreach (var conv in conversationList)
                if (conv != null && conv.isNewMessage) return true;
            return false;
        }

        public static void updateGameCanvas()
        {
            if (GameCanvas.currentScreen != GameScr.instance)
            {
                foreach (var item in conversationList)
                {
                    item.isNewMessage = false;
                }
            }
        }

        public static void updatePanel()
        {
            if (typePanel == 1 && GameCanvas.panel.type == CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU && conversation != null) 
                conversation.isNewMessage = false;
        }

        public static void UpdateTouch()
        {
            Panel panel = GameCanvas.panel;
            if(typePanel == 1)
            {
                if (GameCanvas.isPointer(panel.X, 52, panel.W, 25))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        panel.chatTField = new ChatTextField();
                        panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                        panel.chatTField.initChatTextField();
                        panel.chatTField.strChat = string.Empty;
                        panel.chatTField.tfChat.name = "Nội dung";
                        panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                        panel.chatTField.startChat2(gI, $"Chat với {conversation.name}");
                    }
                    GameCanvas.clearAllPointerEvent();
                }
            }
        }

        public static string getNameWithoutClanTag(string cName) => cName.Remove(0, cName.IndexOf(']') + 1).TrimStart(' ', '#', '$');

        public static void addMessage(int charID, string cName, bool isRecieve, string msg)
        {
            if (msg == null || charID == Char.myCharz().charID) return;
            Message newMsg = new(isRecieve, msg);
            var conv = conversationList.Find(x => x.id == charID);
            if (conv == null)
            {
                if(conversationList.Count > 10) conversationList.RemoveAt(0);
                conv = new Conversation()
                {
                    id = charID,
                    name = getNameWithoutClanTag(cName),
                    messages = new List<Message> { newMsg }
                };
                conversationList.Add(conv);
            }
            else
            {
                if(conv.messages.Count > 100) conv.messages.RemoveAt(0);
                conv.messages.Add(newMsg);
            }
            conv.isNewMessage = isRecieve;
        }

        public static void saveData()
        {
            try
            {
                Utilities.saveRMSString("messages", JsonMapper.ToJson(conversationList));
            }
            catch (Exception e)
            {
                WriteLog.write("log_saveData.txt", e.ToString(), "Messenger");
            }
        }

        public static void loadData()
        {
            try
            {
                conversationList = Utilities.loadRMSString("messages") == "" ? new List<Conversation>() : JsonMapper.ToObject<List<Conversation>>(Utilities.loadRMSString("messages"));
            }
            catch (Exception e)
            {
                WriteLog.write("log_loadData.txt", e.ToString(), "Messenger");
            }
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && string.IsNullOrEmpty(text))
                return;

            if (to == $"Chat với {conversation.name}" && typePanel == 1)
            {
                try
                {
                    Service.gI().chatPlayer(text, conversation.id, conversation.name);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else if(to == $"Chat với {newName}")
            {
                try
                {
                    Service.gI().chatPlayer(text, newID, newName);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else if (ChatTextField.gI().isShow)
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

        public void perform(int idAction, object p)
        {
            if (idAction == 1)
            {
                string[] arr = ((string)p).Split('|');
                newID = int.Parse(arr[0]);
                newName = arr[1];
                GameCanvas.panel.chatTField = new ChatTextField();
                GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                GameCanvas.panel.chatTField.initChatTextField();
                GameCanvas.panel.chatTField.strChat = string.Empty;
                GameCanvas.panel.chatTField.tfChat.name = "Nội dung";
                GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                GameCanvas.panel.chatTField.startChat2(gI, $"Chat với {newName}");
            }
        }
    }
}
