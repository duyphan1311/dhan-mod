using Mod.Auto;
using Mod.Auto.AutoChat;
//using Mod.CSharpInteractive;
using Mod.Graphics;
using Mod.Info;
using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModMenu;
using Mod.OnScreenPaint;
using Mod.PickMob;
using Mod.Set;
using Mod.Xmap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Vietpad.InputMethod;

namespace Mod
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
        private static bool isZoomLevelChecked;
        private static string currentDirectory = Environment.CurrentDirectory;

        /// <summary>
        /// Kích hoạt khi người chơi chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns></returns>
        public static bool onSendChat(string text)
        {
            HistoryChat.gI.append(text);
            ExtensionManager.Invoke(text);
            //if (text.StartsWith("/") && (Pk9rXmap.Chat(text.Remove(0, 1)) || Pk9rPickMob.Chat(text.Remove(0, 1))))
            //    return true;
            bool result = ChatCommandHandler.handleChatText(text);
            return result;
        }

        /// <summary>
        /// Kích hoạt sau khi game khởi động.
        /// </summary>
        public static void onGameStarted()
        {
            ChatCommandHandler.loadDefault();
            HotkeyCommandHandler.loadDefalut();
            SocketClient.gI.initSender();
            ModMenuMain.LoadData();
            CustomBackground.LoadData();
            CustomLogo.LoadData();
            CustomCursor.LoadData();
            SetDo.LoadData();
            Messenger.Messenger.loadData();
            Setup.loadData();
            ListCharsInMap.LoadData();
            Boss.LoadData();
            //LogKillBoss.LoadData();
            ListBossInMap.LoadData();
            CharInfo.LoadData();
            PetInfo.LoadData();
            ShareInfo.LoadData();
            AutoUpgrade.LoadData();
            AutoGetItemOut.LoadData();
            AutoCrackBall.LoadData();
            FakeIPhoneClient.onGameStart();
            CustomGraphics.InitializeTileMap(true);
            VietKeyHandler.SmartMark = true;
            ExtensionManager.LoadExtensions();
            //System.Windows.Forms.Application.EnableVisualStyles();
            //System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            ExtensionManager.Invoke();
        }

        /// <summary>
        /// Kích hoạt khi game đóng
        /// </summary>
        /// <returns></returns>
        public static bool onGameClosing()
        {
            Setup.clearStringTrash();
            AutoFindBoss.onQuitGame();
            Setup.SaveData();
            SocketClient.gI.close();
            ModMenuMain.SaveData();
            TeleportMenu.TeleportMenu.SaveData();
            CustomBackground.SaveData();
            CustomLogo.SaveData();
            CustomCursor.SaveData();
            SetDo.SaveData();
            Messenger.Messenger.saveData();
            ListCharsInMap.SaveData();
            Boss.SaveData();
            //LogKillBoss.SaveData();
            ListBossInMap.SaveData();
            CharInfo.SaveData();
            PetInfo.SaveData();
            ShareInfo.SaveData();
            AutoUpgrade.SaveData();
            AutoGetItemOut.SaveData();
            AutoCrackBall.SaveData();
            //CSharpInteractiveForm.CloseForm();
            FakeIPhoneClient.onExitGame();
            ExtensionManager.Invoke();
            return false;
        }

        public static void onSaveRMSString(ref string filename, ref string data)
        {
            if (filename == "acc" || filename == "pass")
                data = "duyphan";
            ExtensionManager.Invoke(filename, data);
        }

        /// <summary>
        /// Kích hoạt sau khi load KeyMap.
        /// </summary>
        /// <param name="h"></param>
        public static void onKeyMapLoaded(Hashtable h)
        {
            ExtensionManager.Invoke(h);
        }

        /// <summary>
        /// Kích hoạt khi cài đăt kích thước màn hình.
        /// </summary>
        /// <returns></returns>
        public static bool onSetResolution()
        {
            ExtensionManager.Invoke();
            if (Utilities.sizeData != null)
            {
                int width = (int)Utilities.sizeData["width"];
                int height = (int)Utilities.sizeData["height"];
                bool fullScreen = false;
                if (Screen.width != width || Screen.height != height)
                    Screen.SetResolution(width, height, fullScreen);
                new Thread(delegate ()
                {
                    while (Screen.fullScreen != fullScreen)
                    {
                        Screen.fullScreen = fullScreen;
                        Thread.Sleep(100);
                    }
                }).Start();
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
            ExtensionManager.Invoke();
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr).
        /// </summary>
        public static void onGameScrPressHotkeys()
        {
            SetDo.UpdateKey();
            ExtensionManager.Invoke();
        }

        /// <summary>
        /// Kích hoạt sau khi vẽ khung chat.
        /// </summary>
        public static void onPaintChatTextField(ChatTextField instance, mGraphics g)
        {
            if (instance == ChatTextField.gI() && instance.strChat.Replace(" ", "") == "Chat" && instance.tfChat.name == "chat")
                HistoryChat.gI.paint(g);
            ExtensionManager.Invoke(instance, g);
        }

        /// <summary>
        /// Kích hoạt khi mở khung chat.
        /// </summary>
        public static bool onStartChatTextField(ChatTextField sender, IChatable parentScreen)
        {
            ChatTextField.gI().parentScreen = parentScreen;
            if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat") return false;
            if (sender == ChatTextField.gI())
            {
                HistoryChat.gI.show();
            }

            ExtensionManager.Invoke(sender);
            return false;
        }

        public static bool onLoadRMSInt(string file, out int result)
        {
            if (file == "lowGraphic" && Utilities.sizeData != null)
            {
                result = Utilities.sizeData["graphic"].ToString().ToLower() == "cao" ? 0 : 1;
                ExtensionManager.Invoke(file, result);
                if (result == 1)
                    Rms.saveRMSInt("lowGraphic", 1);
                else
                    Rms.saveRMSInt("lowGraphic", 0);
                return true;
            }

            result = -1;
            ExtensionManager.Invoke(file, result);
            return false;
        }

        internal static bool onGetRMSPath(out string result)
        {
            GameMidlet.IP = Utilities.host;
            GameMidlet.PORT = Utilities.port;

            if (Environment.CurrentDirectory != currentDirectory)
                Environment.CurrentDirectory = currentDirectory;
            //DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
            string path = $"{Path.GetTempPath()}\\DragonboyByDuyPhan\\{Utilities.version}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            result = $"{path}\\{GameMidlet.IP}_{GameMidlet.PORT}_x{mGraphics.zoomLevel}\\";
            //result = $"asset\\{GameMidlet.IP}_{GameMidlet.PORT}_x{mGraphics.zoomLevel}\\";
            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }
            ExtensionManager.Invoke(result);
            return true;
        }

        public static bool onTeleportUpdate(Teleport teleport)
        {
            if (teleport.isMe)
            {
                if (teleport.type == 0)
                    Controller.isStopReadMessage = false;
                else
                {
                    Char.myCharz().isTeleport = false;
                    Char.myCharz().cy = teleport.y2;
                }
            }
            else
            {
                var @char = GameScr.findCharInMap(teleport.id);
                if (@char != null)
                {
                    if (teleport.type == 0)
                        GameScr.vCharInMap.removeElement(@char);
                    else
                        @char.isTeleport = false;
                }
            }

            Teleport.vTeleport.removeElement(teleport);
            ExtensionManager.Invoke(teleport);
            return true;
        }

        /// <summary>
        /// Kích hoạt khi có ChatTextField update.
        /// </summary>
        public static void onUpdateChatTextField(ChatTextField sender)
        {
            if (!string.IsNullOrEmpty((string)typeof(TField).GetField("text", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sender.tfChat))) GameCanvas.keyPressed[14] = false;
            ExtensionManager.Invoke(sender);
        }

        public static bool onClearAllRMS()
        {
            FileInfo[] files = new DirectoryInfo(Rms.GetiPhoneDocumentsPath() + "/").GetFiles();
            foreach (FileInfo fileInfo in files)
                if (fileInfo.Name != "isPlaySound")
                    fileInfo.Delete();

            ExtensionManager.Invoke();
            return true;
        }

        /// <summary>
        /// Kích hoạt khi GameScr.gI() update.
        /// </summary>
        public static void onUpdateGameScr()
        {
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && AutoGa.isAutoGaEnabled) AutoGa.update();
            if (!AutoSS.isAutoSS && !AutoT77.isAutoT77) Pk9rPickMob.Update();
            if (AutoFindBoss.isFindBoss) AutoFindBoss.SanBoss();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && AutoUpgrade.isUpgrade) AutoUpgrade.update();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23)
                Service.gI().openUIZone();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && ShareInfo.isShareInfo) ShareInfo.sendInfo();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && Utilities.isAutoBuffPean) Utilities.AutoBuffPean();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && xskill) loadSkill();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && Utilities.IsRevive) Utilities.AutoRevive();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && AutoUpgrade.isShowListUpgrade) AutoUpgrade.UpdateListUpgrade();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && AutoBuy.isBuyItem) AutoBuy.update();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && AutoUseItem.isUseItem) AutoUseItem.update();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && AutoSellGold.isBanVang) AutoSellGold.update();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && AutoPlusPoint.isAutoPlusPoint()) AutoPlusPoint.update();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && GameScr.gI().isBagFull() && AutoGetItemOut.isGetItemOut)
            {
                AutoGetItemOut.isFullBag = true;
                GameScr.info1.addInfo("Auto vứt vật phẩm bắt đầu!", 0);
            }
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && AutoGetItemOut.isGetItemOut && AutoGetItemOut.isFullBag)
                AutoGetItemOut.update();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && Utilities.IsAutoFlag) Utilities.AutoFlag();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && Setup.isPutDelay) Setup.update();
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && Utilities.isBackToOldZone)
            {
                if (!Utilities.isLogin)
                {
                    Utilities.oldMap = TileMap.mapID;
                    Utilities.oldZone = TileMap.zoneID;
                }
                else
                    new Thread(new ThreadStart(Utilities.BackToOldZone)).Start();
            }
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0 && Utilities.isCheckLag)
            {
                Utilities.checkLag();
            }
            Char.myCharz().cspeed = Utilities.speedRun;
            Time.timeScale = Utilities.speedGame;
            CharEffect.Update();
            TeleportMenu.TeleportMenu.Update();
            if (GameCanvas.gameTick % (10 * Time.timeScale) == 0 && Char.myCharz().havePet) Service.gI().petInfo();
            if (Utilities.isPickSKH) Utilities.PickSKH();
            AutoGoback.update();
            AutoSS.update();
            AutoT77.update();
            AutoPet.update();
            SuicideRange.update();
            if (GameCanvas.gameTick % 5 == 0) Boss.Update();
            if (GameCanvas.gameTick % 5 == 0) ListCharsInMap.update();
            if (GameCanvas.gameTick % 5 == 0) ListBossInMap.update();
            //if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0) LogKillBoss.update();
            CharInfo.update();
            PetInfo.update();
            SetDo.Update();
            AutoPean.Update();
            AutoSkill.Update();
            if (!isALogin) isALogin = true;
            if (!Utilities.isLogin) Utilities.isLogin = false;
            //if (!Utilities.isChecking && Utilities.isCheckLag) Utilities.isChecking = true;
            //NOTE onUpdateChatTextField không thể bấm tab.
            if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat") return;
            HistoryChat.gI.update();
            ExtensionManager.Invoke();
        }


        [ChatCommand("8sk")]
        private static void toggleLoadSkill()
        {
            xskill = true;
        }

        private static bool xskill = true;

        private static void loadSkill()
        {
            for (int i = 0; i < global::Char.myCharz().nClass.skillTemplates.Length; i++)
            {
                SkillTemplate skillTemplate = global::Char.myCharz().nClass.skillTemplates[i];
                Skill skill = global::Char.myCharz().getSkill(skillTemplate);
                GameScr.keySkill[i] = skill;

            }
            xskill = false;
        }

        /// <summary>
        /// Kích hoạt khi gửi yêu cầu đăng nhập.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <param name="type"></param>
        public static void onLogin(ref string username, ref string pass, ref sbyte type)
        {
            username = Utilities.username == "" ? username : Utilities.username;
            if (username.StartsWith("User"))
            {
                pass = string.Empty;
                type = 1;
            }
            else pass = Utilities.password == "" ? pass : Utilities.password;
            //Không thêm hàm này
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
            Service.gI().setClientType();
            Service.gI().login(Utilities.username, Utilities.password, GameMidlet.VERSION, 0);
            LoginScr.serverName = ServerListScreen.nameServer[Utilities.server];
            TeleportMenu.TeleportMenu.LoadData();
            AutoPet.isFirstTimeCkeckPet = true;
            xskill = true;
            Utilities.isLogin = true;
            ExtensionManager.Invoke();
        }

        /// <summary>
        /// Kích hoạt khi Session kết nối đến server.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public static void onSessionConnecting(ref string host, ref int port)
        {
            host = Utilities.host;
            port = Utilities.port;
            ExtensionManager.Invoke(host, port);
        }

        public static void onSceenDownloadDataShow()
        {
            GameCanvas.serverScreen.perform(2, null);
            ExtensionManager.Invoke();
        }

        public static bool onCheckZoomLevel()
        {
            if (Utilities.sizeData != null)
            {
                mGraphics.zoomLevel = Utilities.sizeData["graphic"].ToString().ToLower() == "cao" ? 2 : 1;
                ExtensionManager.Invoke();
                return true;
            }
            ExtensionManager.Invoke();
            return false;
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
            ExtensionManager.Invoke(keyCode, isFromSync);
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
            ExtensionManager.Invoke(keyCode, isFromAsync);
            return false;
        }

        private static readonly List<string> strIgnore = new List<string>()
        {
            "không thể đổi khu vực trong map này",
            "không thể đổi khu vực này"
        };

        public static bool onAddInfo(string chat, int type)
        {
            if (strIgnore.Any(chat.ToLower().Contains))
            {
                return true;
            }
            AutoCrackBall.infoMe(chat);
            return false;
        }

        public static bool isALogin = true;

        public static bool onOKDlg(string chat)
        {
            if (chat.ToLower().Contains("danh sách") || chat.Contains("Nội dung tự động chat"))
            {
                return false;
            }
            if (chat.Contains("Thông tin tài khoản hoặc mật khẩu không chính xác") || chat.Contains("mật khẩu không chính xác"))
            {
                isALogin = false;
                return false;
            }
            if (GameCanvas.currentScreen != GameCanvas.loginScr && GameCanvas.currentScreen != GameCanvas.serverScreen
                && GameCanvas.currentScreen != GameCanvas.serverScr && GameCanvas.currentScreen != CreateCharScr.instance
                 && GameCanvas.currentScreen != GameCanvas.registerScr)
            {
                GameScr.info1.addInfo(chat, 0);
                return true;
            }
            return false;
        }

        public static bool onChatPopupMultiLine(string chat)
        {
            Pk9rXmap.Info(chat);
            string[] array = Res.split(chat, "\n", 0);
            for (int i = 0; i < array.Length; i++)
            {
                GameScr.info1.addInfo(array[i], 0);
            }
            ExtensionManager.Invoke(chat);
            return true;
        }

        public static bool onAddBigMessage(string chat, Npc npc)
        {
            string[] array = Res.split(chat, "\n", 0);
            if (npc.avatar == 1139 || AutoSS.isAutoSS || AutoT77.isAutoT77)
            {
                if (!chat.Contains("NGOCRONGONLINE.COM") && !chat.Contains("Hack, Mod"))
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        GameScr.info1.addInfo(array[i], 0);
                    }
                }
                ExtensionManager.Invoke(chat, npc);
                return true;
            }
            ExtensionManager.Invoke();
            return true;
        }

        public static void onInfoMapLoaded()
        {
            Utilities.updateWaypointChangeMap();
            ExtensionManager.Invoke();
        }

        public static void onPaintGameScr(mGraphics g)
        {
            CharEffect.Paint(g);
            SuicideRange.paint(g);
            Boss.Paint(g);
            ListCharsInMap.paint(g);
            ListBossInMap.paint(g);
            //LogKillBoss.paint(g);
            CharInfo.Paint(g);
            PetInfo.Paint(g);
            AutoUseItem.Paint(g);
            AutoUpgrade.PaintListUpgrade(g);

            ExtensionManager.Invoke(g);
        }

        public static bool onUseSkill(Skill skill)
        {
            CharEffect.AddEffectCreatedByMe(skill);
            ExtensionManager.Invoke(skill);
            return false;
        }

        public static void onFixedUpdateMain()
        {
            //Pk9rXmap.Update();
            CustomBackground.FixedUpdate();
            CustomLogo.update();
            ExtensionManager.Invoke();
        }

        public static void onUpdateMain()
        {
            CustomCursor.Update();
            ExtensionManager.Invoke();
        }

        public static void onAddInfoMe(string str)
        {
            Pk9rXmap.Info(str);
            if (str.StartsWith("Bạn vừa thu hoạch") && !AutoSS.isNeedMorePean) AutoSS.isHarvestingPean = false;
            if (str.ToLower().Contains("bạn vừa nhận thưởng bùa")) AutoSS.isNhanBua = true;
            ExtensionManager.Invoke(str);
        }

        public static void onUpdateTouchGameScr()
        {
            Boss.UpdateTouch();
            ListCharsInMap.updateTouch();
            ListBossInMap.updateTouch();
            //LogKillBoss.updateTouch();
            CharInfo.UpdateTouch();
            PetInfo.UpdateTouch();
            AutoUseItem.UpdateTouch();
            OnScreen.updateTouch();
            ExtensionManager.Invoke();
        }

        public static void onUpdateTouchPanel()
        {
            SetDo.UpdateTouch();
            Messenger.Messenger.UpdateTouch();
            ExtensionManager.Invoke();
        }

        public static void onSetPointItemMap(int xEnd, int yEnd)
        {
            if (xEnd == Char.myCharz().cx && yEnd == Char.myCharz().cy - 10)
            {
                if (AutoSS.isAutoSS) AutoSS.isPicking = false;
                if (ModMenuMain.modMenuItemInts[4].SelectedValue != 0) AutoPet.isPicking = false;
            }
            ExtensionManager.Invoke(xEnd, yEnd);
        }

        public static bool onMenuStartAt(MyVector menuItems)
        {
            if (AutoSS.isAutoSS && menuItems.size() == 2 && ((Command)menuItems.elementAt(0)).caption == "Nhận quà" && ((Command)menuItems.elementAt(1)).caption == "Từ chối")
            {
                GameCanvas.menu.menuSelectedItem = 0;
                ((Command)menuItems.elementAt(0)).performAction();
                AutoSS.isNhapCodeTanThu = true;
                ExtensionManager.Invoke(menuItems);
                return true;
            }
            ExtensionManager.Invoke(menuItems);
            return false;
        }

        public static void onAddInfoChar(string info, Char c)
        {
            if (info.Contains("Sao sư phụ không đánh đi") && ModMenuMain.modMenuItemInts[4].SelectedValue > 0 && c.charID == -Char.myCharz().charID) AutoPet.isSaoMayLuoiThe = true;
            ExtensionManager.Invoke(info, c);
        }

        public static void onLoadImageGameCanvas()
        {
            if (!isZoomLevelChecked)
            {
                isZoomLevelChecked = true;
                onCheckZoomLevel();
            }
            ExtensionManager.Invoke();
        }

        public static bool onPaintBgGameScr(mGraphics g)
        {
            //UnityEngine.Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BackgroundVideo.videoPlayer.texture);
            if (CustomBackground.isEnabled && CustomBackground.backgroundWallpapers.Count > 0
                && !ModMenuMain.modMenuItemBools[8].isDisabled && CustomBackground.bgIndex() != -1)
            {
                CustomBackground.paint(g);
                ExtensionManager.Invoke(g);
                return true;
            }
            ExtensionManager.Invoke(g);
            return false;
        }

        public static void onMobStartDie(Mob instance)
        {
            Pk9rPickMob.MobStartDie(instance);
            ExtensionManager.Invoke(instance);
        }

        public static void onUpdateMob(Mob instance)
        {
            Pk9rPickMob.UpdateCountDieMob(instance);
            ExtensionManager.Invoke(instance);
        }

        public static Image onCreateImage(string filename)
        {
            if (Environment.CurrentDirectory != currentDirectory)
                Environment.CurrentDirectory = currentDirectory;
            Image image = new Image();
            Texture2D texture2D = new Texture2D(1, 1);
            if (!Directory.Exists("Data\\CustomAssets"))
                Directory.CreateDirectory("Data\\CustomAssets");
            if (File.Exists("Data\\CustomAssets\\" + filename.Replace('/', '\\') + ".png"))
            {
                texture2D.LoadImage(File.ReadAllBytes("Data\\CustomAssets\\" + filename.Replace('/', '\\') + ".png"));
            }
            else texture2D = Resources.Load(filename) as Texture2D;
            image.texture = texture2D ?? throw new Exception("NULL POINTER EXCEPTION AT Image onCreateImage " + filename);
            image.w = image.texture.width;
            image.h = image.texture.height;
            image.texture.anisoLevel = 0;
            image.texture.filterMode = FilterMode.Point;
            image.texture.mipMapBias = 0f;
            image.texture.wrapMode = TextureWrapMode.Clamp;
            ExtensionManager.Invoke(filename);
            return image;
        }

        public static void onChatVip(string chatVip)
        {
            Boss.AddBoss(chatVip);
            //LogKillBoss.AddLog(chatVip);
            ExtensionManager.Invoke(chatVip);
        }

        public static void onUpdateScrollMousePanel(Panel instance, ref int pXYScrollMouse)
        {
            SetDo.UpdateScrollMouse(ref pXYScrollMouse);
            ExtensionManager.Invoke(instance, pXYScrollMouse);
        }

        public static void onPanelHide(Panel instance)
        {
            ExtensionManager.Invoke(instance);
        }

        public static void onUpdateKeyPanel(Panel instance)
        {
            ExtensionManager.Invoke(instance);
        }
    }
}