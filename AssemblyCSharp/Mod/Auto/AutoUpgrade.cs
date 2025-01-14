using Mod.CustomPanel;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.Menu;
using Mod.ModMenu;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Mod.Auto
{
    internal class AutoUpgrade : IActionListener, IChatable
    {
        public static bool isUpgrade;

        public static int starUpgrade;
        public static int currStar = 0;
        public static sbyte npcHome;
        public static short idThoiVang;
        public static int goldSell;
        public static int maxStarUpgrade;

        public static int idNgocRong;
        public static int SoLuongEp;
        public static string epNRCaption;
        public static int SoNRBanDau = 0;
        public static int SoNRHienTai = 0;
        public static int idBag = 0;
        public static sbyte idBaHatMit = 21;
        public static int menuPLH = 0;

        public static int solanSale;
        public static int soThoiVangBan = 10;
        public static int timeSaleGold = 500;
        public static int menu2ID = 0;
        public static long lastTimeSaleGold;
        public static bool isBanVang;

        public static sbyte npcNVCF;
        public static sbyte npcNNCF;
        public static sbyte npccsbt;
        public static sbyte npcNV;
        public static short idSPL;
        public static short idDaNangCap;

        public static int idDaBaoVe = 987;
        public static int idBag1 = 0;
        public static int idBag2 = 0;
        public static bool isUseDBV = false;

        public static int optionPorataID = -1;
        public static int paramPorataRequest = -1;
        public static string csbtCaption;

        public static bool isPLH;
        public static bool isEpNR;
        public static bool isKham;
        public static bool isNCTB;
        public static bool isMoCSBT;
        public static bool isNhanVang = false;
        public static bool isNhanNgoc = true;
        public static bool isShowListUpgrade = false;
        public static bool isWait = false;
        public static bool isSaleGoldToUpgrade = false;
        public static bool isGoHomeGetGold = false;
        public static bool isHaveMenu2 = false;

        public static List<Item> listKham = new();
        public static ItemUpgrade currItemUpgrade = new();
        public static List<ItemUpgrade> listUpgrade = new();
        public static List<ItemUpgrade> listShow = new();

        public static string nameSKH;
        public static string nameTypeSKH;

        public static bool isCM;

        public static AutoUpgrade gI { get; } = new AutoUpgrade();

        public static void toggle(bool flag) => isUpgrade = flag;

        public class ItemUpgrade
        {
            public int id;
            public string name;
            public int type;

            public ItemUpgrade() { }

            public ItemUpgrade(Item item, int id)
            {
                if (item == null)
                    return;
                this.id = id;
                this.name = item.template.name;
                this.type = item.template.type;
            }
        }

        public static void update()
        {
            if (XmapController.gI.IsActing)
                return;
            if (Char.isLoadingMap || Char.ischangingMap)
                return;
            if (isPLH) upgrade();
            if (isKham) KhamSPL();
            if (isEpNR) EpNR();
            if (isNCTB) NCTB();
            if (isMoCSBT) MoCSBT();
            if (isNCGTS) NCGTS();
            if (isHHTB) HacHacTB();
            if (isTayHHTB) TayHacHacTB();
            if (isNCSKH) NangCapSKH();
            if (isBanVang) sellGold();
            if (isCM) chanMenh();
        }

        [ChatCommand("acm")]
        public static void startCM()
        {
            isCM = !isCM;
            isUpgrade = !isUpgrade;
            GameScr.info1.addInfo("Auto chân mệnh: " + (isCM ? "On" : "Off"), 0);
        }

        private static void chanMenh()
        {
            if (GameScr.gI().isBagFull())
            {
                isCM = false;
                isUpgrade = false;
                GameScr.info1.addInfo("Hành trang đã đầy, auto chân mệnh dừng!", 0);
                return;
            }
            Utilities.openMenu(56);
            Service.gI().confirmMenu(56, 1);
            Service.gI().confirmMenu(56, 0);
            Service.gI().confirmMenu(56, 1);
        }

        [ChatCommand("nc")]
        public static void showMenu()
        {
            new MenuBuilder()
                .addItem("Pha lê\nhoá", new(ShowMenuPhaLeHoa))
                .addItem("Khảm SPL", new(ShowMenuKhamSPL))
                .addItem("Ép\nNgọc rồng", new(ShowMenuEpNR))
                .addItem("Nâng cấp\ntrang bị", new(ShowMenuUpgradeItem))
                .addItem("Mở chỉ số\nbông tai", new(ShowMenuMoCSBT))
                .addItem(ifCondition: listUpgrade.Count > 0, "Xoá List Item", new(() =>
                {
                    listUpgrade.Clear();
                    listShow.Clear();
                    GameScr.info1.addInfo("Danh sách nâng cấp đã xoá", 0);
                }))
                .addItem("Cài đặt", new(() =>
                {
                    showTabMenuModPanel();
                }))
                .start();
        }

        [ChatCommand("ncgts")]
        public static void NCGang()
        {
            new Thread(delegate ()
            {
                while (TileMap.mapID != 5)
                    if (!XmapController.gI.IsActing)
                        XmapController.start(5);

                while (Char.isLoadingMap || Char.ischangingMap)
                    Thread.Sleep(500);

                Thread.Sleep(500);
                Utilities.openMenu(21);
                Service.gI().confirmMenu(21, 2);
                GameCanvas.menu.doCloseMenu();
                ChatPopup.currChatPopup = null;
                isNCGTS = true;
                toggle(true);
            })
            {
                IsBackground = true,
                Name = "GotoDaoKame"
            }.Start();
        }

        private static sbyte menuHacHoa = 5;
        private static sbyte menuTayHacHoa = 6;

        [ChatCommand("mhh")]
        public static void setMenuHacHoa(sbyte menu) => menuHacHoa = menu;

        [ChatCommand("mthh")]
        public static void setMenuTayHacHoa(sbyte menu) => menuTayHacHoa = menu;

        [ChatCommand("hhtb")]
        public static void HacHoa()
        {
            new Thread(delegate ()
            {
                while (TileMap.mapID != 5)
                    if (!XmapController.gI.IsActing)
                        XmapController.start(5);

                while (Char.isLoadingMap || Char.ischangingMap)
                    Thread.Sleep(500);

                Thread.Sleep(500);
                Utilities.openMenu(21);
                Service.gI().confirmMenu(21, menuHacHoa);
                GameCanvas.menu.doCloseMenu();
                ChatPopup.currChatPopup = null;
                isHHTB = true;
                toggle(true);
            })
            {
                IsBackground = true,
                Name = "GotoDaoKame"
            }.Start();
        }

        [ChatCommand("tayhh")]
        public static void TayHacHoa()
        {
            new Thread(delegate ()
            {
                while (TileMap.mapID != 5)
                    if (!XmapController.gI.IsActing)
                        XmapController.start(5);

                while (Char.isLoadingMap || Char.ischangingMap)
                    Thread.Sleep(500);

                Thread.Sleep(500);
                Utilities.openMenu(21);
                Service.gI().confirmMenu(21, menuTayHacHoa);
                GameCanvas.menu.doCloseMenu();
                ChatPopup.currChatPopup = null;
                isTayHHTB = true;
                toggle(true);
            })
            {
                IsBackground = true,
                Name = "GotoDaoKame"
            }.Start();
        }

        private static short[] listMainItem;
        private static short[] listSubItem;

        [ChatCommand("ncskh")]
        public static void NCSKH()
        {
            Dictionary<string, List<short[]>> options = new Dictionary<string, List<short[]>>()
            {
                { "Áo",
                    new List<short[]> {
                        new short[] { 650, 652, 654 },
                        new short[] { 659, 651, 657, 653, 661, 655, 656, 658, 660, 662 }
                    }
                },
                { "Quần",
                    new List<short[]> {
                        new short[] { 651, 653, 655 },
                        new short[] { 650, 657, 652, 659, 654, 661, 656, 658, 660, 662 }
                    }
                },
                { "Găng",
                    new List<short[]> {
                        new short[] { 657, 659, 661 },
                        new short[] { 650, 651, 652, 653, 654, 655, 656, 658, 660, 662 }
                    }
                },
                { "Giầy",
                    new List<short[]> {
                        new short[] { 658, 660, 662 },
                        new short[] { 650, 651, 652, 653, 654, 655, 656, 657, 659, 661 }
                    }
                },
                { "Rada",
                    new List<short[]> {
                        new short[] { 656 },
                        new short[] { 650, 651, 652, 653, 654, 655, 657, 658, 659, 660, 661, 662 }
                    }
                },
            };

            new MenuBuilder()
                .map(options.Keys, (key) =>
                {
                    return new($"{key}", new(() =>
                    {
                        listMainItem = options[key][0];
                        listSubItem = options[key][1];

                        new Thread(delegate ()
                        {
                            while (TileMap.mapID != 5)
                                if (!XmapController.gI.IsActing)
                                    XmapController.start(5);

                            while (Char.isLoadingMap || Char.ischangingMap)
                                Thread.Sleep(500);

                            Thread.Sleep(500);
                            Utilities.openMenu(21);
                            Service.gI().confirmMenu(21, 2);
                            Service.gI().confirmMenu(21, 1);
                            GameCanvas.menu.doCloseMenu();
                            ChatPopup.currChatPopup = null;
                            isNCSKH = true;
                            toggle(true);
                        })
                        {
                            IsBackground = true,
                            Name = "GotoDaoKame"
                        }.Start();
                    }));
                }).start();
        }

        private static void ShowMenuPhaLeHoa()
        {
            new MenuBuilder()
                .map(Enumerable.Range(1, maxStarUpgrade), index =>
                {
                    return new($"{index} sao", new(() =>
                    {
                        new Thread(delegate ()
                        {
                            while (TileMap.mapID != 5)
                                if (!XmapController.gI.IsActing)
                                    XmapController.start(5);

                            while (Char.isLoadingMap || Char.ischangingMap)
                                Thread.Sleep(500);

                            Thread.Sleep(500);
                            Utilities.openMenu(21);
                            Service.gI().confirmMenu(21, 1);
                            if (isHaveMenu2)
                                Service.gI().confirmMenu(idBaHatMit, (sbyte)menu2ID);

                            GameCanvas.menu.doCloseMenu();
                            ChatPopup.currChatPopup = null;
                            starUpgrade = index;
                            isPLH = true;
                            isShowListUpgrade = true;
                            toggle(true);
                        })
                        {
                            IsBackground = true,
                            Name = "GotoDaoKame"
                        }.Start();

                    }));
                })
                .start();
        }

        private static void ShowMenuKhamSPL()
        {
            Dictionary<string, int> options = new Dictionary<string, int>()
            {
                { "Sức đánh", 16 },
                { "HP", 20 },
                { "KI", 19 },
                { "TNSM", 447 },
                { "Giáp", 15 },
                { "Né đòn", 14 },
                { "PST", 443 },
                { "Vàng", 446 },
                { "Hút HP", 441 },
                { "Hút KI", 442 }
            };

            new MenuBuilder()
                .map(options.Keys, (key) =>
                {
                    return new($"{key}", new(() =>
                    {
                        idSPL = (short)options[key];

                        new Thread(delegate ()
                        {
                            while (TileMap.mapID != 5)
                                if (!XmapController.gI.IsActing)
                                    XmapController.start(5);

                            while (Char.isLoadingMap || Char.ischangingMap)
                                Thread.Sleep(500);

                            Thread.Sleep(500);
                            Utilities.openMenu(21);
                            Service.gI().confirmMenu(21, 0);
                            GameCanvas.menu.doCloseMenu();
                            ChatPopup.currChatPopup = null;
                            isKham = true;
                            toggle(true);
                        })
                        {
                            IsBackground = true,
                            Name = "GotoDaoKame"
                        }.Start();
                    }));
                })
                .addItem("Khác", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = string.Empty;
                    ChatTextField.gI().startChat2(gI, "Nhập id vật phẩm dùng để khảm");
                }))
                .start();
        }

        private static void ShowMenuEpNR()
        {
            new MenuBuilder()
                .map(Enumerable.Range(1, 6), index =>
                {
                    return new($"{index} sao", new(() =>
                    {
                        idNgocRong = index + 13;
                        SoNRBanDau = FindQuatity((short)idNgocRong);
                        epNRCaption = $"Nhập số lượng {index} sao bạn muốn ép";
                        ChatTextField.gI().strChat = string.Empty;
                        ChatTextField.gI().tfChat.name = string.Empty;
                        ChatTextField.gI().startChat2(gI, epNRCaption);
                    }));
                })
                .start();
        }

        private static void ShowMenuMoCSBT()
        {
            Dictionary<string, int> options = new Dictionary<string, int>()
            {
                { "Sức đánh", 50 },
                { "HP", 77 },
                { "KI", 103 }
            };

            new MenuBuilder()
                .map(options.Keys, (key) =>
                {
                    return new($"{key}", new(() =>
                    {
                        optionPorataID = (short)options[key];
                        csbtCaption = $"Nhập chỉ số phần trăm {key} bạn muốn mở";
                        ChatTextField.gI().strChat = string.Empty;
                        ChatTextField.gI().tfChat.name = string.Empty;
                        ChatTextField.gI().startChat2(gI, csbtCaption);
                    }));
                })
                .start();
        }

        private static void ShowMenuUpgradeItem()
        {
            Dictionary<string, int> options = new Dictionary<string, int>()
            {
                { "Áo", 223 },
                { "Quần", 222 },
                { "Găng", 224 },
                { "Giầy", 221 },
                { "Rada", 220 }
            };

            new MenuBuilder()
                .map(options.Keys, (key) =>
                {
                    return new($"{key}", new(() =>
                    {
                        idDaNangCap = (short)options[key];

                        new Thread(() =>
                        {
                            while (TileMap.mapID != Char.myCharz().cgender + 42)
                                if (!XmapController.gI.IsActing)
                                    XmapController.start(Char.myCharz().cgender + 42);

                            while (Char.isLoadingMap || Char.ischangingMap)
                                Thread.Sleep(500);

                            Thread.Sleep(500);
                            Utilities.openMenu(21);
                            Service.gI().confirmMenu(21, 1);
                            isNCTB = true;
                            toggle(true);
                        })
                        {
                            IsBackground = true,
                            Name = "GotoVachNui"
                        }.Start();
                    }));
                })
                .start();
        }

        #region tabpanel

        public static ModMenuItem[] tabItem = new ModMenuItem[]
        {
            new ModMenuItemBoolean("Auto nhận vàng", "Bật/tắt auto nhận vàng khi nâng cấp", setStateGetGold, false, "isNhanVang", false, "Bạn cần tắt chức năng \"Auto bán vàng\"!"),
            new ModMenuItemBoolean("Về nhà nhận vàng", "Bật/tắt về nhà nhận vàng khi nâng cấp", setStateGoHomeGetGold, false, "isGoHomeGetGold", true, "Bạn chưa bật chức năng \"Auto nhận vàng\"!"),
            new ModMenuItemBoolean("Auto bán vàng", "Bật/tắt auto bán vàng khi nâng cấp", setStateSellGold, false, "isSaleGoldToUpgrade", true, "Bạn cần tắt chức năng \"Auto nhận vàng\"!"),
            new ModMenuItemBoolean("Auto ngọc", "Bật/tắt auto nhận ngọc khi nâng cấp", setStateGetDiamond, true, "isNhanNgoc"),
            new ModMenuItemBoolean("Sử dụng đá bảo vệ khi nâng cấp", "Bật/tắt Sử dụng đá bảo vệ khi nâng cấp", setStateUseDBV, false, "", false),
            new ModMenuItemBoolean("Menu pha lê hoá cấp 2", "Có/Không menu pha lê hoá cấp 2", setStateMenuPLH2, false, "havemenu2"),

            new ModMenuItemInt("Số sao tối đa pha lê hoá", null, "Điều chỉnh số sao pha lê tối đa", 8, setMaxStarUpgrade, "maxStarUpgrade", false),
            new ModMenuItemInt("ID item bán vàng", null, "Điều chỉnh id item thỏi vàng", 457, setIDItemGold, "idThoiVang", false),
            new ModMenuItemInt("Số vàng bắt đầu bán", null, "Điều chỉnh số vàng bắt đầu bán khi nâng cấp", 250000000, setGoldToSell, "goldSell", false),
            new ModMenuItemInt("ID NPC nhận vàng tại đảo kame", null, "Điều chỉnh id npc nhận vàng tại đảo kame", 13, setIDNPCGetGold, "npcNV", false),
            new ModMenuItemInt("Menu confirm nhận vàng", null, "Điều chỉnh menu confirm của npc khi nhận vàng", 2, setMenuConfirm, "npcNVCF", false),
            new ModMenuItemInt("Menu confirm nhận ngọc", null, "Điều chỉnh menu confirm của npc khi nhận ngọc", 1, setMenuConfirmGD, "npcNNCF", false),
            new ModMenuItemInt("Menu confirm mở chỉ số bông tai", null, "Điều chỉnh menu confirm của npc khi mở chỉ số bông tai", 5, setMenuConfirmCSBT, "npccsbtcf", false),
            new ModMenuItemInt("Số lượng thỏi vàng bán", null, "Điều chỉnh số lượng thỏi vàng sẽ bán khi thiếu vàng", 10, setSoLuongThoiVang, "soThoiVangBanPLH", false),
            new ModMenuItemInt("Menu pha lê hoá câp 2", null, "Điều chỉnh menu pha lê hoá cấp 2", 0, setMenu2, "menuplh2", false),
            new ModMenuItemInt("Menu confirm pha lê hoá", null, "Điều chỉnh menu confirm pha lê hoá", 0, setMenuConfirmPLH, "menuplh", false),
        };

        public static void setStateGetGold(bool value) => isNhanVang = value;
        public static void setStateGetDiamond(bool value) => isNhanNgoc = value;
        public static void setStateGoHomeGetGold(bool value) => isGoHomeGetGold = value;
        public static void setStateSellGold(bool value) => isSaleGoldToUpgrade = value;
        public static void setStateUseDBV(bool value) => isUseDBV = value;
        public static void setStateMenuPLH2(bool value) => isHaveMenu2 = value;

        public static void setMaxStarUpgrade(int value) => maxStarUpgrade = value;
        public static void setIDItemGold(int value) => idThoiVang = (short)value;
        public static void setGoldToSell(int value) => goldSell = value;
        public static void setIDNPCGetGold(int value) => npcNV = (sbyte)value;
        public static void setMenuConfirm(int value) => npcNVCF = (sbyte)value;
        public static void setMenuConfirmGD(int value) => npcNNCF = (sbyte)value;
        public static void setMenuConfirmCSBT(int value) => npccsbt = (sbyte)value;
        public static void setSoLuongThoiVang(int value) => soThoiVangBan = value;
        public static void setMenu2(int value) => menu2ID = value;
        public static void setMenuConfirmPLH(int value) => menuPLH = value;

        private static void showTabMenuModPanel()
        {
            CustomPanelMenu.show(setTabMenuMod, doFireTabMenuMod, paintTabHeader, paintTabMenuMod);
        }

        private static void notifySelectDisabledItem(Panel panel)
        {
            int selected = panel.selected;
            if (!tabItem[selected].isDisabled) return;
            GameScr.info1.addInfo(tabItem[selected].DisabledReason, 0);
        }

        public static void setTabMenuMod(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, tabItem);
        }

        private static void paintTabHeader(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Cài đặt nâng cấp trang bị");
        }

        public static void doFireTabMenuMod(Panel panel)
        {
            if (panel.selected < 0) return;
            int selected = panel.selected;
            if (panel.selected < 6)
            {
                if (!tabItem[selected].isDisabled)
                {
                    ModMenuItemBoolean menuItem = (ModMenuItemBoolean)tabItem[selected];
                    menuItem.setValue(!menuItem.Value);
                    GameScr.info1.addInfo("Đã " + (menuItem.Value ? "bật" : "tắt") + " " + menuItem.Title + "!", 0);
                }
            }
            else
            {
                if (!tabItem[selected].isDisabled)
                {
                    panel.chatTField = new ChatTextField();
                    panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                    panel.chatTField.initChatTextField();
                    panel.chatTField.strChat = string.Empty;
                    panel.chatTField.tfChat.name = inputModMenuItemInts[selected][1];
                    panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    panel.chatTField.startChat2(new AutoUpgrade(), inputModMenuItemInts[selected][0]);
                }

            }
            notifySelectDisabledItem(panel);
        }

        public static Dictionary<int, string[]> inputModMenuItemInts = new Dictionary<int, string[]>()
        {
            { 6, new string[]{ "Nhập số sao pha lê hoá tối đa", "Số sao" } },
            { 7, new string[]{ "Nhập id thỏi vàng", "ID thỏi vàng" } },
            { 8, new string[]{ "Nhập số vàng bắt đầu bán", "Số vàng" } },
            { 9, new string[]{ "Nhập id npc nhận vàng tại đảo kame", "ID NPC" } },
            { 10, new string[]{ "Nhập menu confirm nhận vàng", "Menu confirm" } },
            { 11, new string[]{ "Nhập menu confirm nhận ngọc", "Menu confirm" } },
            { 12, new string[]{ "Nhập menu confirm mở chỉ số bông tai", "Menu confirm" } },
            { 13, new string[]{ "Nhập số lượng thỏi vàng", "Value" } },
            { 14, new string[]{ "Nhập menu pha lê hoá câp 2", "Menu confirm" } },
            { 15, new string[]{ "Nhập menu confirm pha lê hoá", "Menu confirm" } },
        };

        public static void paintTabMenuMod(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (tabItem == null || tabItem.Length != panel.currentListLength) return;
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0;
            string str = (mResources.status + ": ") == "Trạng thái: " ? "Đang " : (mResources.status + ": ");
            for (int i = 0; i < 6; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                ModMenuItemBoolean modMenuItem = (ModMenuItemBoolean)tabItem[i];
                if (!modMenuItem.isDisabled) g.setColor((i != panel.selected) ? 15196114 : 16383818);
                else g.setColor((i != panel.selected) ? new Color(0.54f, 0.51f, 0.46f) : new Color(0.61f, 0.63f, 0.18f));
                g.fillRect(num, num2, num3, num4);
                if (modMenuItem != null)
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                    string description = string.Empty;
                    if (mFont.tahoma_7_blue.getWidth(modMenuItem.Description) > 145 - mFont.tahoma_7b_red.getWidth(str))
                    {
                        string str2 = modMenuItem.Description;
                        while (mFont.tahoma_7_blue.getWidth(str2 + "...") > 145 - mFont.tahoma_7b_red.getWidth(str)) str2 = str2.Remove(str2.Length - 1, 1);
                        description = str2 + "...";
                    }
                    else description = modMenuItem.Description;
                    //modMenuItem.Description.Length > 28 ? (modMenuItem.Description.Substring(0, 27) + "...") : modMenuItem.Description;
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(modMenuItem.Description) > 145 - mFont.tahoma_7b_red.getWidth(str) && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = modMenuItem.Description;
                        x = num + 5;
                        y = num2 + 11;
                    }
                    else mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                    mFont mf = mFont.tahoma_7_grey;
                    if (modMenuItem.Value) mf = mFont.tahoma_7b_red;
                    mf.drawString(g, str + (modMenuItem.Value ? mResources.ON.ToLower() : mResources.OFF.ToLower()), num + num3 - 2, num2 + panel.ITEM_HEIGHT - 14, mFont.RIGHT);
                }
            }
            if (panel.selected < 6)
            {
                if (isReset) TextInfo.reset();
                else
                {
                    TextInfo.paint(g, descriptionTextInfo, x, y, 145 - mFont.tahoma_7b_red.getWidth(str), 15, mFont.tahoma_7_blue);
                    g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                    g.translate(0, -panel.cmy);
                }
            }
            int currSelectedValue = 0;
            for (int i = 6; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                ModMenuItemInt modMenuItem = (ModMenuItemInt)tabItem[i];
                if (!modMenuItem.isDisabled) g.setColor((i != panel.selected) ? 15196114 : 16383818);
                else g.setColor((i != panel.selected) ? new Color(0.54f, 0.51f, 0.46f) : new Color(0.61f, 0.63f, 0.18f));
                g.fillRect(num, num2, num3, num4);
                if (modMenuItem != null)
                {
                    string description;
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                    if (modMenuItem.Values != null)
                    {
                        str = modMenuItem.getSelectedValue();
                        if (mFont.tahoma_7_blue.getWidth(str) > 160)
                        {
                            string str2 = str;
                            while (mFont.tahoma_7_blue.getWidth(str2 + "...") > 160) str2 = str2.Remove(str2.Length - 1, 1);
                            description = str2 + "...";
                        }
                        else description = str;
                        //description = str.Length > 28 ? (str.Substring(0, 27) + "...") : str;
                    }
                    else
                    {
                        str = modMenuItem.Description;
                        //description = str.Length > 35 ? (str.Substring(0, 34) + "...") : str;
                        if (mFont.tahoma_7b_red.getWidth(str) > 160 - mFont.tahoma_7_blue.getWidth(NinjaUtil.getMoneys(modMenuItem.SelectedValue)))
                        {
                            string str2 = str;
                            while (mFont.tahoma_7_blue.getWidth(str2 + "...") > 160 - mFont.tahoma_7_blue.getWidth(NinjaUtil.getMoneys(modMenuItem.SelectedValue))) str2 = str2.Remove(str2.Length - 1, 1);
                            description = str2 + "...";
                        }
                        else description = str;
                        mFont.tahoma_7b_red.drawString(g, NinjaUtil.getMoneys(modMenuItem.SelectedValue), num + num3 - 2, num2 + panel.ITEM_HEIGHT - 14, mFont.RIGHT);
                    }
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(str) > 160 - mFont.tahoma_7_blue.getWidth(NinjaUtil.getMoneys(modMenuItem.SelectedValue)) && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = modMenuItem.Description;
                        currSelectedValue = modMenuItem.SelectedValue;
                        x = num + 5;
                        y = num2 + 11;
                    }
                    else mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                }
            }
            if (panel.selected >= 6)
            {
                if (isReset) TextInfo.reset();
                else
                {
                    TextInfo.paint(g, descriptionTextInfo, x, y, 160 - mFont.tahoma_7_blue.getWidth(currSelectedValue.ToString()), 15, mFont.tahoma_7_blue);
                    g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                    g.translate(0, -panel.cmy);
                }
            }
            panel.paintScrollArrow(g);
        }

        public static void onTabMenuModValueChanged()
        {
            ModMenuItemBoolean autoGetGold = (ModMenuItemBoolean)tabItem[0];
            ModMenuItemBoolean autoGoHomeGetGold = (ModMenuItemBoolean)tabItem[1];
            ModMenuItemBoolean autoSellGold = (ModMenuItemBoolean)tabItem[2];

            tabItem[0].isDisabled = autoSellGold.Value;
            tabItem[1].isDisabled = !autoGetGold.Value;
            tabItem[2].isDisabled = autoGetGold.Value;
        }

        public static void SaveData()
        {
            for (int i = 0; i < 6; i++)
            {
                ModMenuItemBoolean modMenuItem = (ModMenuItemBoolean)tabItem[i];
                if (!string.IsNullOrEmpty(modMenuItem.RMSName))
                    Utilities.saveRMSBool(modMenuItem.RMSName, modMenuItem.Value);
            }
            for (int i = 6; i < tabItem.Length; i++)
            {
                ModMenuItemInt modMenuItem = (ModMenuItemInt)tabItem[i];
                if (!string.IsNullOrEmpty(modMenuItem.RMSName))
                    Utilities.saveRMSInt(modMenuItem.RMSName, modMenuItem.SelectedValue);
            }
        }

        public static void LoadData()
        {
            for (int i = 0; i < 6; i++)
            {
                ModMenuItemBoolean modMenuItem = (ModMenuItemBoolean)tabItem[i];
                try
                {
                    if (!string.IsNullOrEmpty(modMenuItem.RMSName) && !Utilities.isFirstTimeLoad) modMenuItem.setValue(Utilities.loadRMSBool(modMenuItem.RMSName));
                }
                catch { }
            }
            for (int i = 6; i < tabItem.Length; i++)
            {
                ModMenuItemInt modMenuItem = (ModMenuItemInt)tabItem[i];
                try
                {
                    int data = Utilities.loadRMSInt(modMenuItem.RMSName);
                    if (data != -1) modMenuItem.setValue(data);

                }
                catch { }
            }
        }

        #endregion

        public static void upgrade()
        {
            if (listUpgrade.Count <= 0)
            {
                isPLH = false;
                isShowListUpgrade = false;
                GameScr.info1.addInfo("Bạn cần chọn vật phẩm", 0);
                toggle(false);
                return;
            }

            if ((Char.myCharz().luong <= 10000 || isWait))
            {
                isPLH = false;
                isShowListUpgrade = false;

                if (isNhanNgoc)
                {
                    if (!isWait)
                    {
                        npcHome = Char.myCharz().cgender switch
                        {
                            1 => 2,
                            2 => 1,
                            _ => 0
                        };

                        if (TileMap.mapID != Char.myCharz().cgender + 21)
                        {
                            if (!XmapController.gI.IsActing)
                                XmapController.start(Char.myCharz().cgender + 21);
                            isPLH = true;
                            return;
                        }

                        Utilities.openMenu(npcHome);
                        Service.gI().confirmMenu(npcHome, npcNNCF);
                    }

                    if (TileMap.mapID != 5)
                    {
                        if (!XmapController.gI.IsActing)
                            XmapController.start(5);
                        isWait = true;
                        isPLH = true;
                        return;

                    }

                    Utilities.openMenu(idBaHatMit);
                    Service.gI().confirmMenu(idBaHatMit, 1);
                    if (isHaveMenu2)
                        Service.gI().confirmMenu(idBaHatMit, (sbyte)menu2ID);

                    GameCanvas.menu.doCloseMenu();
                    ChatPopup.currChatPopup = null;
                    isPLH = true;
                    isShowListUpgrade = true;
                    isWait = false;
                }
                else
                {
                    toggle(false);
                    isPLH = false;
                    GameScr.info1.addInfo("Thiếu ngọc", 0);
                    return;
                }
            }

            if (Char.myCharz().xu <= goldSell || isWait)
            {
                isPLH = false;
                isShowListUpgrade = false;

                if (isNhanVang)
                {
                    if (isGoHomeGetGold && !isWait)
                    {
                        npcHome = Char.myCharz().cgender switch
                        {
                            1 => 2,
                            2 => 1,
                            _ => 0
                        };

                        if (TileMap.mapID != Char.myCharz().cgender + 21)
                        {
                            if (!XmapController.gI.IsActing)
                                XmapController.start(Char.myCharz().cgender + 21);
                            isPLH = true;
                            return;
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            Utilities.openMenu(npcHome);
                            Service.gI().confirmMenu(npcHome, npcNVCF);
                        }
                    }
                    if (!isWait && !isGoHomeGetGold)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Utilities.openMenu(npcNV);
                            Service.gI().confirmMenu(npcNV, npcNVCF);
                        }
                    }

                    if (isGoHomeGetGold)
                    {
                        if (TileMap.mapID != 5)
                        {
                            if (!XmapController.gI.IsActing)
                                XmapController.start(5);

                            isWait = true;
                            isPLH = true;
                            return;
                        }
                    }

                    Utilities.openMenu(idBaHatMit);
                    Service.gI().confirmMenu(idBaHatMit, 1);
                    if (isHaveMenu2)
                        Service.gI().confirmMenu(idBaHatMit, (sbyte)menu2ID);

                    GameCanvas.menu.doCloseMenu();
                    ChatPopup.currChatPopup = null;
                    isPLH = true;
                    isShowListUpgrade = true;
                    isWait = false;
                }
                else if (isSaleGoldToUpgrade)
                {
                    Utilities.openMenu(39);
                    Service.gI().confirmMenu(39, 0);
                    Service.gI().confirmMenu(39, 0);

                    solanSale = soThoiVangBan;
                    timeSaleGold = 100;
                    lastTimeSaleGold = mSystem.currentTimeMillis();
                    isBanVang = true;
                }
                else
                {
                    toggle(false);
                    GameScr.info1.addInfo("Thiếu vàng", 0);
                    return;
                }
            }

            Item item = FindItemUpgrade(listUpgrade[0]);
            currItemUpgrade = listUpgrade[0];

            if (item == null)
            {
                isPLH = false;
                isShowListUpgrade = false;
                GameScr.info1.addInfo("Chưa có item nâng cấp", 0);
                toggle(false);
                return;
            }

            int maxStart = getMaxStart(item);

            if (maxStart >= starUpgrade)
            {
                listUpgrade.RemoveAt(0);

                if (listUpgrade.Count <= 0)
                {
                    isPLH = false;
                    isShowListUpgrade = false;
                    GameScr.info1.addInfo("Xong", 0);
                    toggle(false);
                    return;
                }
                item = FindItemUpgrade(listUpgrade[0]);
                currItemUpgrade = listUpgrade[0];
            }
            MyVector myVector = new MyVector();
            myVector.addElement(item);
            Service.gI().combine(1, myVector);
            Service.gI().confirmMenu(idBaHatMit, (sbyte)menuPLH);
        }

        public static void KhamSPL()
        {
            if (listUpgrade.Count <= 0)
            {
                isKham = false;
                GameScr.info1.addInfo("Bạn cần chọn vật phẩm", 0);
                toggle(false);
                return;
            }

            if (Char.myCharz().luong <= 1000)
            {
                isKham = false;
                GameScr.info1.addInfo("Thiếu ngọc", 0);
                toggle(false);
                return;
            }

            Item item = FindItemUpgrade(listUpgrade[0]);
            var indexSPL = Utilities.getIndexItemBag(idSPL);
            Item spl = (indexSPL != -1) ? global::Char.myCharz().arrItemBag[indexSPL] : null;

            if (item == null || spl == null)
            {
                isKham = false;
                GameScr.info1.addInfo("Chưa có item khảm", 0);
                toggle(false);
                return;
            }

            if (getStartSlot(item) == getMaxStart(item))
            {
                listUpgrade.RemoveAt(0);
                if (listUpgrade.Count <= 0)
                {
                    isKham = false;
                    listUpgrade.Clear();
                    GameScr.info1.addInfo("Xong", 0);
                    toggle(false);
                    return;
                }
                item = FindItemUpgrade(listUpgrade[0]);
            }

            MyVector myVector = new MyVector();
            myVector.addElement(item);
            myVector.addElement(spl);
            Service.gI().combine(1, myVector);
            if (ServerListScreen.nameServer[Utilities.server].ToLower().Contains("kingcool"))
                GameCanvas.gI().keyPressedz(-5);
            else
                Service.gI().confirmMenu(idBaHatMit, 0);
        }

        public static void EpNR()
        {
            if (SoNRBanDau + SoLuongEp <= FindQuatity((short)idNgocRong))
            {
                isEpNR = false;
                idBag = 0;
                GameScr.info1.addInfo("Xong", 0);
                toggle(false);
                return;
            }
            if (idBag >= global::Char.myCharz().arrItemBag.Length)
            {
                isEpNR = false;
                idBag = 0;
                GameScr.info1.addInfo("Không tìm thấy ngọc rồng " + (idNgocRong - 12) + " sao", 0);
                toggle(false);
                return;
            }
            if (global::Char.myCharz().arrItemBag[idBag] == null)
            {
                idBag++;
                return;
            }
            if (global::Char.myCharz().arrItemBag[idBag].template.id == (short)(idNgocRong + 1)
                && global::Char.myCharz().arrItemBag[idBag].quantity < 7
            )
            {
                idBag++;
                return;
            }
            if (global::Char.myCharz().arrItemBag[idBag].template.id == (short)(idNgocRong + 1))
            {
                GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag]);
                MyVector myVector = new MyVector();
                myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
                Service.gI().combine(1, myVector);
                Service.gI().confirmMenu(21, 0);
                GameCanvas.panel.vItemCombine.removeAllElements();
                return;
            }
            idBag++;
        }

        public static void NCTB()
        {
            if (idBag2 >= global::Char.myCharz().arrItemBag.Length)
            {
                isNCTB = false;
                idBag1 = 0;
                idBag2 = 0;
                GameScr.info1.addInfo("Không tìm thấy đá nâng cấp", 0);
                toggle(false);
                return;
            }
            if (global::Char.myCharz().arrItemBag[idBag2] == null)
            {
                idBag2++;
                return;
            }
            if (global::Char.myCharz().arrItemBag[idBag2].template.id == (short)idDaNangCap
                && global::Char.myCharz().arrItemBag[idBag2].quantity < 25
            )
            {
                idBag2++;
                return;
            }
            if (isUseDBV)
            {
                if (idBag1 >= global::Char.myCharz().arrItemBag.Length)
                {
                    isNCTB = false;
                    idBag1 = 0;
                    idBag2 = 0;
                    GameScr.info1.addInfo("Không tìm thấy đá bảo vệ", 0);
                    toggle(false);
                    return;
                }
                if (global::Char.myCharz().arrItemBag[idBag1] == null)
                {
                    idBag1++;
                    return;
                }
            }
            if (global::Char.myCharz().arrItemBag[idBag2].template.id == (short)idDaNangCap)
            {
                Item item = FindItemUpgrade(listUpgrade[0]);
                if (isUseDBV)
                {
                    if (global::Char.myCharz().arrItemBag[idBag1].template.id != (short)idDaBaoVe)
                    {
                        idBag1++;
                        return;
                    }
                    GameCanvas.panel.vItemCombine.addElement(item);
                    GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag2]);
                    GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag1]);
                    MyVector myVector = new MyVector();
                    myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
                    myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
                    myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(2));
                    Service.gI().combine(1, GameCanvas.panel.vItemCombine);
                    Service.gI().confirmMenu(21, 0);
                    GameCanvas.panel.vItemCombine.removeAllElements();
                    return;
                }
                GameCanvas.panel.vItemCombine.addElement(item);
                GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag2]);
                MyVector myVector1 = new MyVector();
                myVector1.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
                myVector1.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
                Service.gI().combine(1, myVector1);
                Service.gI().confirmMenu(21, 0);
                GameCanvas.panel.vItemCombine.removeAllElements();
                return;
            }
            idBag2++;
        }

        private static int currMap = -1;

        public static void MoCSBT()
        {
            Item item = FindItemUpgrade(listUpgrade[0]);
            if (paramPorata(item, optionPorataID) >= paramPorataRequest)
            {
                isMoCSBT = false;
                idBag1 = 0;
                idBag2 = 0;
                GameScr.info1.addInfo("Hoàn thành!", 0);
                toggle(false);
                return;
            }
            if (idBag2 >= global::Char.myCharz().arrItemBag.Length || idBag1 >= global::Char.myCharz().arrItemBag.Length)
            {
                isMoCSBT = false;
                idBag1 = 0;
                idBag2 = 0;
                GameScr.info1.addInfo("Không tìm thấy item", 0);
                toggle(false);
                return;
            }
            if (Char.myCharz().luong <= 10000 || isWait)
            {
                if (isNhanNgoc)
                {
                    if (!isWait)
                    {
                        npcHome = Char.myCharz().cgender switch
                        {
                            1 => 2,
                            2 => 1,
                            _ => 0
                        };
                        if (TileMap.mapID != Char.myCharz().cgender + 21)
                        {
                            currMap = TileMap.mapID;
                            if (!XmapController.gI.IsActing)
                                XmapController.start(Char.myCharz().cgender + 21);
                            return;
                        }

                        Utilities.openMenu(npcHome);
                        Service.gI().confirmMenu(npcHome, npcNNCF);
                    }

                    if (TileMap.mapID != currMap && currMap != -1)
                    {
                        if (!XmapController.gI.IsActing)
                            XmapController.start(currMap);
                        isWait = true;
                        return;

                    }

                    Utilities.openMenu(idBaHatMit);
                    Service.gI().confirmMenu(idBaHatMit, npccsbt);
                    Service.gI().confirmMenu(idBaHatMit, npccsbt);
                    isWait = false;
                }
                else
                {
                    toggle(false);
                    isMoCSBT = false;
                    idBag1 = 0;
                    idBag2 = 0;
                    GameScr.info1.addInfo("Thiếu ngọc", 0);
                    return;
                }
            }
            if (global::Char.myCharz().arrItemBag[idBag2] == null)
            {
                idBag2++;
                return;
            }
            if (global::Char.myCharz().arrItemBag[idBag2].template.id == 934
                && global::Char.myCharz().arrItemBag[idBag2].quantity < 99
            )
            {
                idBag2++;
                return;
            }
            if (global::Char.myCharz().arrItemBag[idBag1] == null)
            {
                idBag1++;
                return;
            }
            if (global::Char.myCharz().arrItemBag[idBag2].template.id == 934 || global::Char.myCharz().arrItemBag[idBag1].template.id == 935)
            {
                if (global::Char.myCharz().arrItemBag[idBag1].template.id != 935)
                {
                    idBag1++;
                    return;
                }
                if (global::Char.myCharz().arrItemBag[idBag2].template.id != 934)
                {
                    idBag2++;
                    return;
                }
                GameCanvas.panel.vItemCombine.addElement(item);
                GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag2]);
                GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[idBag1]);
                MyVector myVector = new MyVector();
                myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
                myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
                myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(2));
                Service.gI().combine(1, GameCanvas.panel.vItemCombine);
                Service.gI().confirmMenu(21, 0);
                GameCanvas.panel.vItemCombine.removeAllElements();
                return;
            }
            idBag1++;
            idBag2++;
        }

        public static int paramPorata(Item item, int optionID)
        {
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                ItemOptionTemplate optionTemplate = item.itemOption[i].optionTemplate;
                if (optionTemplate.id == optionID)
                    return item.itemOption[i].param;
            }
            return 0;
        }

        public static bool isNCGTS;
        public static void NCGTS()
        {
            var indexMangGTS = Utilities.getIndexItemBag(1219);
            var indexGHD = Utilities.getIndexItemBag(657);
            if (indexMangGTS == -1 || indexGHD == -1)
            {
                isNCGTS = false;
                GameScr.info1.addInfo("Không tìm thấy item", 0);
                toggle(false);
                return;
            }
            GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[indexMangGTS]);
            GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[indexGHD]);
            MyVector myVector = new MyVector();
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
            Service.gI().combine(1, GameCanvas.panel.vItemCombine);
            Service.gI().confirmMenu(21, 0);
            GameCanvas.panel.vItemCombine.removeAllElements();
        }

        public static bool isHHTB;
        public static void HacHacTB()
        {
            var indexDaHacHoa = Utilities.getIndexItemBag(2092);
            Item item = FindItemUpgrade(listUpgrade[0]);
            if (checkParamHacHoa(item) >= 18)
            {
                isHHTB = false;
                GameScr.info1.addInfo("Hoàn thành!", 0);
                toggle(false);
                return;
            }
            if (indexDaHacHoa == -1)
            {
                return;
            }
            if (item == null)
            {
                isHHTB = false;
                GameScr.info1.addInfo("Không tìm thấy item", 0);
                toggle(false);
                return;
            }
            if (checkTimeHacHoa(item) == 8)
            {
                isHHTB = false;
                TayHacHoa();
                return;
            }
            GameCanvas.panel.vItemCombine.addElement(item);
            GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[indexDaHacHoa]);
            MyVector myVector = new MyVector();
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
            Service.gI().combine(1, GameCanvas.panel.vItemCombine);
            Service.gI().confirmMenu(21, 0);
            GameCanvas.panel.vItemCombine.removeAllElements();
        }

        public static bool isTayHHTB;
        public static void TayHacHacTB()
        {
            var indexDaHacHoa = Utilities.getIndexItemBag(2093);
            Item item = FindItemUpgrade(listUpgrade[0]);
            if (indexDaHacHoa == -1 || item == null)
            {
                isTayHHTB = false;
                GameScr.info1.addInfo("Không tìm thấy item", 0);
                toggle(false);
                return;
            }
            GameCanvas.panel.vItemCombine.addElement(item);
            GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[indexDaHacHoa]);
            MyVector myVector = new MyVector();
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
            Service.gI().combine(1, GameCanvas.panel.vItemCombine);
            Service.gI().confirmMenu(21, 0);
            GameCanvas.panel.vItemCombine.removeAllElements();
            isTayHHTB = false;
            //GameScr.info1.addInfo("Hoàn thành!", 0);
            //toggle(false);
            HacHoa();
            return;
        }

        private static int checkTimeHacHoa(Item item)
        {
            int solan = 0;
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                ItemOptionTemplate optionTemplate = item.itemOption[i].optionTemplate;
                if (optionTemplate.id == 217)
                {
                    solan = item.itemOption[i].param;
                    return solan;
                }
            }
            return solan;
        }

        private static int checkParamHacHoa(Item item)
        {
            int solan = 0;
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                ItemOptionTemplate optionTemplate = item.itemOption[i].optionTemplate;
                if (optionTemplate.id == 215 || optionTemplate.id == 216)
                {
                    solan = item.itemOption[i].param;
                    return solan;
                }
            }
            return solan;
        }

        public static bool isNCSKH;

        private static sbyte[] getIndexItemUpgradeSKH(params short[] templatesId)
        {
            var myChar = Char.myCharz();
            int length = myChar.arrItemBag.Length;
            List<sbyte> arr = new();
            for (sbyte i = 0; i < length; i++)
            {
                var item = myChar.arrItemBag[i];
                if (item != null && templatesId.Contains(item.template.id)
                    && !arr.Any(x => myChar.arrItemBag[x].template.id == item.template.id))
                    arr.Add(i);
                if (arr.Count == 2) return arr.ToArray();
            }

            return null;
        }

        public static void NangCapSKH()
        {
            var indexGHD = Utilities.getIndexItemBag(listMainItem);
            sbyte[] arr = getIndexItemUpgradeSKH(listSubItem);
            if (indexGHD == -1 || arr == null)
            {
                isNCSKH = false;
                GameScr.info1.addInfo("Không tìm thấy item", 0);
                toggle(false);
                return;
            }
            var indexAHD = arr[0];
            var indexWHD = arr[1];
            GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[indexGHD]);
            GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[indexWHD]);
            GameCanvas.panel.vItemCombine.addElement(global::Char.myCharz().arrItemBag[indexAHD]);
            MyVector myVector = new MyVector();
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(0));
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(1));
            myVector.addElement(GameCanvas.panel.vItemCombine.elementAt(2));
            Service.gI().combine(1, GameCanvas.panel.vItemCombine);
            Service.gI().confirmMenu(21, 0);
            GameCanvas.panel.vItemCombine.removeAllElements();
        }

        public static Item FindItemUpgrade(ItemUpgrade item)
        {
            if (item == null)
                return null;
            Item[] arrItemBag = Char.myCharz().arrItemBag;
            for (int i = 0; i < arrItemBag.Length; i++)
            {
                if (arrItemBag[i] == null) continue;
                if (i == item.id && arrItemBag[i].template.type == item.type && item.name == arrItemBag[i].template.name)
                    return arrItemBag[i];
            }
            return null;
        }

        public static int getStartSlot(Item item)
        {
            for (int i = 0; i < item.itemOption.Length; i++)
                if (item.itemOption[i].optionTemplate.id == 102)
                    return item.itemOption[i].param;
            return -1;
        }

        public static int getMaxStart(Item item)
        {
            for (int i = 0; i < item.itemOption.Length; i++)
                if (item.itemOption[i].optionTemplate.id == 107)
                    return item.itemOption[i].param;
            return 0;
        }

        public static void UpdateListUpgrade()
        {
            listShow.Clear();
            foreach (ItemUpgrade item in listUpgrade)
                listShow.Add(item);
        }

        public static void PaintListUpgrade(mGraphics g)
        {
            if (!isShowListUpgrade) return;
            int x = GameCanvas.w / 2;
            int y;
            y = isSaleGoldToUpgrade ? 65 : 55;
            if (isSaleGoldToUpgrade)
            {
                mFont.tahoma_7b_yellow.drawString(g, $"Danh sách item nâng cấp ({starUpgrade} sao)", x, y - 40, mFont.CENTER, mFont.tahoma_7_greySmall);
                mFont.tahoma_7b_yellow.drawString(g, $"Ngọc xanh: {Char.myCharz().luongStr} - Ngọc hồng: {Char.myCharz().luongKhoaStr}", x, y - 30, mFont.CENTER, mFont.tahoma_7_greySmall);
                mFont.tahoma_7b_yellow.drawString(g, $"Vàng: {Char.myCharz().xuStr} - Thỏi vàng: {NinjaUtil.getMoneys(FindQuatity(idThoiVang))}", x, y - 20, mFont.CENTER, mFont.tahoma_7_greySmall);
                mFont.tahoma_7b_yellow.drawString(g, $"Bán thỏi vàng khi <{NinjaUtil.getMoneys(goldSell)} - Số lượng: {soThoiVangBan}", x, y - 10, mFont.CENTER, mFont.tahoma_7_greySmall);
            }
            else
            {
                mFont.tahoma_7b_yellow.drawString(g, $"Danh sách item nâng cấp ({starUpgrade} sao)", x, y - 30, mFont.CENTER, mFont.tahoma_7_greySmall);
                mFont.tahoma_7b_yellow.drawString(g, $"Ngọc xanh: {Char.myCharz().luongStr} - Ngọc hồng: {Char.myCharz().luongKhoaStr}", x, y - 20, mFont.CENTER, mFont.tahoma_7_greySmall);
                mFont.tahoma_7b_yellow.drawString(g, $"Vàng: {Char.myCharz().xuStr} - Thỏi vàng: {NinjaUtil.getMoneys(FindQuatity(idThoiVang))}", x, y - 10, mFont.CENTER, mFont.tahoma_7_greySmall);
            }
            foreach (ItemUpgrade item in listShow)
            {
                Item it = FindItemUpgrade(item);
                if (it != null)
                {
                    if (item.type == currItemUpgrade.type && item.name == currItemUpgrade.name)
                        mFont.tahoma_7b_red.drawString(g, it.template.name + ": " + getMaxStart(it).ToString() + " sao", x, y, mFont.CENTER, mFont.tahoma_7_greySmall);
                    else
                        mFont.tahoma_7b_white.drawString(g, it.template.name + ": " + getMaxStart(it).ToString() + " sao", x, y, mFont.CENTER, mFont.tahoma_7_greySmall);
                    y += 10;
                }
            }
        }

        public static bool CheckSKH(Item item)
        {
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                if (item.itemOption[i].optionTemplate.name.StartsWith("$"))
                {
                    nameTypeSKH = item.template.type switch
                    {
                        1 => "Quần",
                        2 => "Găng",
                        3 => "Giày",
                        4 => "Rada",
                        _ => "Áo",
                    };
                    nameSKH = nameTypeSKH + " " + NinjaUtil.replace(item.itemOption[i - 1].optionTemplate.name, "Set ", string.Empty);
                    return true;
                }
            }
            return false;
        }

        public static void addListUpgrade(Item item, int id)
        {
            foreach (ItemUpgrade it in listUpgrade)
            {
                if (it.id == id && item.template.name == it.name && it.type == item.template.type)
                {
                    listUpgrade.Remove(it);
                    //GameScr.info1.addInfo("Đã xoá " + item.template.name + " khỏi danh sách nâng cấp", 0);
                }
            }
            listUpgrade.Add(new ItemUpgrade(item, id));
            //GameScr.info1.addInfo("Đã thêm " + item.template.name + " vào danh sách nâng cấp", 0);

        }

        public static int FindQuatity(short id)
        {
            sbyte itemID = 0;
            int sl = 0;
            while (itemID < global::Char.myCharz().arrItemBag.Length)
            {
                if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                {
                    itemID++;
                    continue;
                }
                if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == id)
                {
                    sl += global::Char.myCharz().arrItemBag[(int)itemID].quantity;
                }
                itemID++;
            }
            return sl;
        }

        public static void sellGold()
        {
            if (solanSale <= 0)
            {
                isBanVang = false;
                GameScr.info1.addInfo("Auto bán thỏi vàng dừng", 0);
                Utilities.openMenu(idBaHatMit);
                Service.gI().confirmMenu(idBaHatMit, 1);
                Service.gI().confirmMenu(idBaHatMit, 1);
                GameCanvas.menu.doCloseMenu();
                ChatPopup.currChatPopup = null;
                isPLH = true;
                isShowListUpgrade = true;
                return;
            }
            if (solanSale > 0 && mSystem.currentTimeMillis() - lastTimeSaleGold > timeSaleGold)
            {
                var index = Utilities.getIndexItemBag(idThoiVang);
                if (index != -1)
                {
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
                else
                {
                    isBanVang = false;
                    toggle(false);
                    GameScr.info1.addInfo("Không tìm thấy thỏi vàng", 0);
                    return;
                }
            }
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    Item[] arrItemBody = Char.myCharz().arrItemBody;
                    int len = GameCanvas.panel.selected - arrItemBody.Length;
                    addListUpgrade(GameCanvas.panel.currItem, len);
                    break;
            }
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && string.IsNullOrEmpty(text))
                return;
            if (to == inputModMenuItemInts[6][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 30) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[6];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi số sao pha lê hoá tối đa!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Số sao pha lê không hợp lệ!");
                }
            }
            else if (to == inputModMenuItemInts[7][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 1500) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[7];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi id thỏi vàng!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("ID thỏi vàng không hợp lệ!");
                }
            }
            else if (to == inputModMenuItemInts[8][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 90000000 || value > 1000000000) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[8];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi số vàng bắt đầu bán!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg($"Số vàng phải lớn hơn {mSystem.numberTostring(90000000)} và nhỏ hơn {mSystem.numberTostring(1000000000)}!");
                }
            }
            else if (to == inputModMenuItemInts[9][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 200) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[9];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi id npc nhận vàng tại đảo kame!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("ID NPC không hợp lệ!");
                }
            }
            else if (to == inputModMenuItemInts[10][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 20) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[10];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi menu confirm nhận vàng!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Menu confirm không hợp lệ!");
                }
            }
            else if (to == inputModMenuItemInts[11][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 20) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[11];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi menu confirm nhận ngọc!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Menu confirm không hợp lệ!");
                }
            }
            else if (to == inputModMenuItemInts[12][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 20) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[12];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi menu confirm mở chỉ số bông tai!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Menu confirm không hợp lệ!");
                }
            }
            else if (to == inputModMenuItemInts[13][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 1000000) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[13];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay số lượng thỏi vàng sẽ bán!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Số lượng không hợp lệ!");
                }
            }
            else if (to == inputModMenuItemInts[14][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 20) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[14];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi menu pha lê hoá cấp 2!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Menu confirm không hợp lệ!");
                }
            }
            else if (to == inputModMenuItemInts[15][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value < 0 || value > 20) throw new Exception();
                    ModMenuItemInt menuItem = (ModMenuItemInt)tabItem[15];
                    menuItem.setValue(value);
                    GameScr.info1.addInfo("Đã thay đổi menu confirm pha lê hoá!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Menu confirm không hợp lệ!");
                }
            }
            else if (to == epNRCaption)
            {
                try
                {
                    new Thread(delegate ()
                    {
                        while (TileMap.mapID != Char.myCharz().cgender + 42)
                            if (!XmapController.gI.IsActing)
                                XmapController.start(Char.myCharz().cgender + 42);

                        Thread.Sleep(500);
                        Utilities.openMenu(21);
                        Thread.Sleep(250);
                        sbyte id = 0;
                        while (id < GameCanvas.menu.menuItems.size())
                        {
                            Command command = (Command)GameCanvas.menu.menuItems.elementAt((int)id);
                            if (command != null && command.caption.ToLower().Contains("ngọc rồng"))
                            {
                                Service.gI().confirmMenu(21, id);
                                GameCanvas.menu.doCloseMenu();
                                ChatPopup.currChatPopup = null;
                                SoLuongEp = int.Parse(text.Trim());
                                isEpNR = true;
                                toggle(true);
                                break;
                            }
                            id++;
                        }
                    })
                    {
                        IsBackground = true,
                        Name = "GotoVachNui"
                    }.Start();

                }
                catch (Exception)
                {
                    GameScr.info1.addInfo("Đã xảy ra lỗi!", 0);
                }
            }
            else if (to == csbtCaption)
            {
                try
                {
                    Utilities.openMenu(21);
                    Service.gI().confirmMenu(21, npccsbt);
                    paramPorataRequest = int.Parse(text.Trim());
                    isMoCSBT = true;
                    toggle(true);

                }
                catch (Exception)
                {
                    GameScr.info1.addInfo("Đã xảy ra lỗi!", 0);
                }
            }
            else if (to == "Nhập id vật phẩm dùng để khảm")
            {
                try
                {
                    idSPL = (short)int.Parse(text.Trim());
                    Utilities.openMenu(21);
                    Service.gI().confirmMenu(21, 0);
                    GameCanvas.menu.doCloseMenu();
                    ChatPopup.currChatPopup = null;
                    isKham = true;
                    toggle(true);

                    //new Thread(delegate ()
                    //{
                    //    while (TileMap.mapID != 5)
                    //        if (!XmapController.gI.IsActing)
                    //            XmapController.start(5);

                    //    while (Char.isLoadingMap || Char.ischangingMap)
                    //        Thread.Sleep(500);

                    //    Thread.Sleep(500);
                    //    Utilities.openMenu(21);
                    //    Service.gI().confirmMenu(21, 0);
                    //    GameCanvas.menu.doCloseMenu();
                    //    ChatPopup.currChatPopup = null;
                    //    isKham = true;
                    //    toggle(true);
                    //})
                    //{
                    //    IsBackground = true,
                    //    Name = "GotoDaoKame"
                    //}.Start();

                }
                catch (Exception)
                {
                    GameScr.info1.addInfo("Đã xảy ra lỗi!", 0);
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
    }
}
