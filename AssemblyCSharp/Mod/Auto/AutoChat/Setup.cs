using LitJson;
using Mod.CustomPanel;
using Mod.ModHelper.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Mod.Auto.AutoChat
{
    public class Setup : IChatable
    {
        public static string[] inputTextAutoChat = new string[] { "Nhập nội dung muốn autochat", "Nội dung" };

        public static string[] inputTextAutoChatChange = new string[] { "Nhập nội dung muốn thay đổi", "Nội dung" };

        public static string[] inputDelayAutoChat = new string[] { "Nhập thời gian delay", "Thời gian >5000(5 giây)" };

        public static string[] inputDelayAutoChatChange = new string[] { "Nhập thời gian delay muốn thay đổi", "Thời gian >5000(5 giây)" };

        public static int delayAutoChat = 5000;//default 5000ms = 5s
        public static Setup gI { get; } = new Setup();

        public static Chat currentChat = new();

        public static Chat newChat = new();

        public static bool isPutDelay = false;

        public static int delayPut = 0;

        public class Chat
        {
            public string content;
            public int delay;

            public Chat() { }

            public Chat(string content, int delay)
            {
                this.content = content;
                this.delay = delay;
            }
        }

        public static List<Chat> ChatList = new();

        /// <summary>
        /// Kích hoạt khi người chơi tắt chức năng hoặc tắt game sẽ xóa các dòng auto chat 
        /// <param name="pattern">Sử dụng biểu thức chính quy tìm các dòng autochat trong history.</param>
        /// </summary>
        public static void clearStringTrash()
        {
            if (!File.Exists(Utilities.PathChatHistory))
                return;
            // Đọc nội dung file text
            string content = File.ReadAllText(Utilities.PathChatHistory);

            string pattern = @",?\s*\[ATC\d{2}\]:\s*[^""]*""[^""]*""";

            //Regex.Replace() thay thế các chuỗi tìm được bằng chuỗi rỗng, loại bỏ chúng khỏi chuỗi đầu vào
            string output = Regex.Replace(content, pattern, "");

            // Ghi nội dung vào file output
            File.WriteAllText(Utilities.PathChatHistory, output);
        }

        public static void update()
        {
            delayPut++;
            if (delayPut == 1)
            {
                delayPut = 0;
                isPutDelay = false;
                ChatTextField.gI().strChat = string.Empty;
                ChatTextField.gI().tfChat.name = inputDelayAutoChat[1];
                ChatTextField.gI().startChat2(gI, inputDelayAutoChat[0]);
            }
            return;
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

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && string.IsNullOrEmpty(text))
                return;

            if (to == inputTextAutoChatChange[0])
            {
                try
                {
                    Chat oldChat = ChatList.OrderBy(chat => chat.content).ToList()[GameCanvas.panel.selected];
                    int index = ChatList.FindIndex(chat => chat == oldChat);
                    if (index != -1)
                        ChatList[index].content = text;
                    SaveData();
                    GameCanvas.panel.EmulateSetTypePanel(0);
                    setTabAutoChatPanel(GameCanvas.panel);
                    GameScr.info1.addInfo($"Thay đổi nội dung thành công!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else if (to == inputDelayAutoChatChange[0])
            {
                try
                {
                    if (!int.TryParse(text, out int delayAutoChat)) throw new Exception();
                    if (delayAutoChat < 0) throw new Exception();

                    if (delayAutoChat < 5000)
                        delayAutoChat = 5000;

                    Chat oldChat = ChatList.OrderBy(chat => chat.content).ToList()[GameCanvas.panel.selected];
                    int index = ChatList.FindIndex(chat => chat == oldChat);
                    if (index != -1)
                        ChatList[index].delay = delayAutoChat;

                    SaveData();
                    GameCanvas.panel.EmulateSetTypePanel(0);
                    setTabAutoChatPanel(GameCanvas.panel);
                    GameScr.info1.addInfo("Đã đổi delay thành " + ((float)delayAutoChat / 1000) + " giây", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Nhập sai dữ liệu vui lòng nhập lại!");
                }
            }
            else if (to == inputTextAutoChat[0])
            {
                try
                {
                    newChat.content = text;

                    isPutDelay = true;
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xảy ra");
                }
            }
            else if (to == inputDelayAutoChat[0])
            {
                try
                {
                    if (!int.TryParse(text, out int delayAutoChat)) throw new Exception();
                    if (delayAutoChat < 0) throw new Exception();

                    if (delayAutoChat < 5000)
                        delayAutoChat = 5000;

                    newChat.delay = delayAutoChat;

                    ChatList.Add(newChat);
                    SaveData();
                    newChat = new Chat();

                    GameScr.info1.addInfo("Đã đổi delay thành " + ((float)delayAutoChat / 1000) + " giây", 0);

                    AutoChat.showMenu();
                }
                catch
                {
                    newChat = new Chat();
                    GameCanvas.startOKDlg("Nhập sai dữ liệu");
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

        public static void showAutoChatPanel()
        {
            CustomPanelMenu.show(setTabAutoChatPanel, doFireAutoChatPanel, paintTabHeader, paintAutoChatPanel);
        }

        public static void setTabAutoChatPanel(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, ChatList);
        }

        public static void doFireAutoChatPanel(Panel panel)
        {
            if (panel.selected < 0) return;
            Chat chat = ChatList.OrderBy(chat => chat.content).ToList()[panel.selected];
            new MenuBuilder()
                .addItem(ifCondition: !AutoChat.gI.IsActing || (AutoChat.gI.IsActing && currentChat == chat),
                "Auto chat\n" + (AutoChat.gI.IsActing ? "[On]" : "[Off]"), new(() =>
                {
                    currentChat = chat;
                    delayAutoChat = currentChat.delay;
                    AutoChat.gI.toggle();
                    GameScr.info1.addInfo("Tự động chat " + (AutoChat.gI.IsActing ? "bắt đầu" : "kết thúc"), 0);
                    if (!AutoChat.gI.IsActing)
                        Setup.clearStringTrash();

                }))
                .addItem("Thay đổi\nnội dung", new(() =>
                {
                    panel.chatTField = new ChatTextField();
                    panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                    panel.chatTField.initChatTextField();
                    panel.chatTField.strChat = string.Empty;
                    panel.chatTField.tfChat.name = inputTextAutoChatChange[1];
                    panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    panel.chatTField.startChat2(new Setup(), inputTextAutoChatChange[0]);
                }))
                .addItem("Thay đổi\ndelay", new(() =>
                {
                    panel.chatTField = new ChatTextField();
                    panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                    panel.chatTField.initChatTextField();
                    panel.chatTField.strChat = string.Empty;
                    panel.chatTField.tfChat.name = inputDelayAutoChatChange[1];
                    panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    panel.chatTField.startChat2(new Setup(), inputDelayAutoChatChange[0]);
                }))
                .addItem(mResources.DELETE, new(() =>
                {
                    ChatList.Remove(chat);
                    SaveData();
                    GameCanvas.panel.EmulateSetTypePanel(0);
                    setTabAutoChatPanel(GameCanvas.panel);
                }))
                .setPos(panel.X, (panel.selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
                .start();

            panel.cp = new ChatPopup();
            panel.cp.isClip = false;
            panel.cp.sayWidth = 180;
            panel.cp.cx = 3 + panel.X - (panel.X != 0 ? Res.abs(panel.cp.sayWidth - panel.W) + 8 : 0);
            panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + chat.content + "\n|6|Delay: " + $"{chat.delay / 1000}s", panel.cp.sayWidth - 10);
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
            PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Danh sách nội dung auto chat");
        }

        public static void paintAutoChatPanel(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.paintCollectionCaptionAndDescriptionTemplate(panel, g, ChatList,
                chat => chat.content, chat => $"Delay: {chat.delay / 1000}s");
        }

        public static void SaveData()
        {
            try
            {
                Utilities.saveRMSString("listChat", JsonMapper.ToJson(ChatList));
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void loadData()
        {
            try
            {
                ChatList = string.IsNullOrEmpty(Utilities.loadRMSString("listChat")) ? new List<Chat>() : JsonMapper.ToObject<List<Chat>>(Utilities.loadRMSString("listChat"));
                if (!ChatList.Any()) ChatList.Add(new Chat("Mod by Duy Phan", 6500));
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }
    }
}
