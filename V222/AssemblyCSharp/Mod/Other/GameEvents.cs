using AssemblyCSharp.Mod.ModHelper;
using AssemblyCSharp.Mod.Options;
using Mod;
using Mod.ModHelper;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace AssemblyCSharp.Mod.Other
{
    /// <summary>
    /// Định nghĩa các sự kiện của game.
    /// </summary>
    /// <remarks>
    /// - Các hàm bool trả về true thì sự kiện game sẽ không được thực hiện, 
    /// trả về false thì sự kiện sẽ được kích hoạt như bình thường.<br/>
    /// - Các hàm void hỗ trợ thực hiện các lệnh cùng với sự kiện.
    /// </remarks>
    public static class GameEvents
    {
        /// <summary>
        /// Kích hoạt khi người chơi chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns></returns>
        public static bool onSendChat(string text)
        {
            HistoryChat.gI.append(text);
            bool result = ChatCommandHandler.handleChatText(text);

            return result;
        }

        /// <summary>
        /// Kích hoạt sau khi game khởi động.
        /// </summary>
        public static void onGameStarted()
        {
            try
            {
                //ChatCommandHandler.loadDefalut();
                //HotkeyCommandHandler.loadDefalut();
                SocketClient.gI.initSender();
            }
            catch (Exception e)
            {
                File.AppendAllText("a.txt", e.ToString());
            }
        }

        /// <summary>
        /// Kích hoạt khi game đóng
        /// </summary>
        /// <returns></returns>
        public static bool onGameClosing()
        {
            SocketClient.gI.close();
            return false;
        }

        /// <summary>
        /// Kích hoạt sau khi load KeyMap.
        /// </summary>
        /// <param name="h"></param>
        public static void onKeyMapLoaded(Hashtable h)
        {
        }

        /// <summary>
        /// Kích hoạt khi cài đăt kích thước màn hình.
        /// </summary>
        /// <returns></returns>
        public static bool onSetResolution()
        {
            if (Utilities.sizeData != null)
            {
                int width = int.Parse(Utilities.sizeData["width"].ToString());
                int height = int.Parse(Utilities.sizeData["height"].ToString());
                Screen.SetResolution(width, height, fullscreen: false);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr) chưa được xử lý.
        /// </summary>
        public static void onGameScrPressHotkeysUnassigned()
        {
            HotkeyCommandHandler.handleHotkey(GameCanvas.keyAsciiPress);
        }

        /// <summary>
        /// Kích hoạt sau khi vẽ khung chat.
        /// </summary>
        /// <param name="g"></param>
        public static void onPaintChatTextField(mGraphics g)
        {
            HistoryChat.gI.paint(g);
        }

        /// <summary>
        /// Kích hoạt khi mở khung chat.
        /// </summary>
        public static bool onStartChatTextField(ChatTextField sender)
        {
            if (sender == ChatTextField.gI())
            {
                HistoryChat.gI.show();
            }

            return false;
        }

        public static void onLoadGraphic()
        {
            if (Utilities.sizeData != null)
            {
                int result = Utilities.sizeData["graphic"].ToString().ToLower() == "cao" ? 0 : 1;
                if (result == 1)
                {
                    mGraphics.zoomLevel = 1;
                    Rms.saveRMSInt("lowGraphic", 1);
                    PickMob.OnScreen.IsXoaMap = true;
                }
                else
                {
                    mGraphics.zoomLevel = 2;
                    Rms.saveRMSInt("lowGraphic", 0);
                }
            }
        }
        internal static bool onGetRMSPath(out string result)
        {
            GameMidlet.IP = ServerUse.hostDefault;
            GameMidlet.PORT = ServerUse.port;
            DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
            string path = Path.GetTempPath() + info.Name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            result = $"{path}\\{GameMidlet.IP}_{GameMidlet.PORT}_x{mGraphics.zoomLevel}\\";
            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }
            return true;
        }

        /// <summary>
        /// Kích hoạt khi có ChatTextField update.
        /// </summary>
        public static void onUpdateChatTextField(ChatTextField sender)
        {
        }

        /// <summary>
        /// Kích hoạt khi GameScr.gI() update.
        /// </summary>
        public static void onUpdateGameScr()
        {
            Char.myCharz().cspeed = Utilities.speedRun;

            //NOTE onUpdateChatTextField không thể bấm tab.
            HistoryChat.gI.update(); 
        }

        /// <summary>
        /// Kích hoạt khi gửi yêu cầu đăng nhập.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        public static void onLogin(ref string username, ref string pass, ref int server)
        {
            username = Utilities.username == "" ? username : Utilities.username;
            pass = Utilities.password == "" ? pass : Utilities.password;
            server = Utilities.server == -1 ? server : Utilities.server;
        }

        /// <summary>
        /// Kích hoạt sau khi màn hình chọn server được load.
        /// </summary>
        public static void onServerListScreenLoaded()
        {
            if (GameCanvas.loginScr == null)
            {
                GameCanvas.loginScr = new LoginScr();
            }

            GameCanvas.loginScr.switchToMe();
            Service.gI().login("", "", GameMidlet.VERSION, 0);
            GameCanvas.startWaitDlg();
        }

        /// <summary>
        /// Kích hoạt khi Session kết nối đến server.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public static void onSessionConnecting(ref string host, ref int port)
        {
            if (Utilities.server != -1)
            {
                host = ServerUse.hostDefault;
                port = ServerUse.port;
            }
        }

        public static void onSceenDownloadDataShow()
        {
            GameCanvas.serverScreen.perform(2, null);
        }

        public static bool onKeyPressedz(int keyCode, bool isFromSync)
        {
            if (Utilities.channelSyncKey != -1 && !isFromSync)
            {
                SocketClient.gI.sendMessage(new 
                {
                    action = "syncKeyPressed",
                    keyCode,
                    Utilities.channelSyncKey
                });
            }
            return false;
        }

        public static bool onKeyReleasedz(int keyCode, bool isFromAsync)
        {
            if (Utilities.channelSyncKey != -1 && !isFromAsync)
            {
                SocketClient.gI.sendMessage(new
                {
                    action = "syncKeyReleased",
                    keyCode,
                    Utilities.channelSyncKey
                });
            }
            return false;
        }

        public static bool onUseSkill(Skill skill)
        {
            CharEffect.AddEffectCreatedByMe(skill);
            return false;
        }
    }
}