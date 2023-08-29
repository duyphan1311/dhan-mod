using Mod.ModHelper;
using Mod.ModHelper.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using static Mod.Auto.AutoChat.Setup;

namespace Mod.Auto
{
    internal class AutoCrackBall : ThreadActionUpdate<AutoCrackBall>, IChatable
    {
        public static bool isGetItem;

        public static bool isNotEnough;

        public static bool isSellGold;

        public static sbyte menuConfirm1 = 1;

        public static sbyte menuConfirm2 = 0;

        public static sbyte menuConfirmGetItem = 1;

        public static int goldSell = 200000000;

        public static int thoivang = 10;

        public int typenhando = 0;

        public override int Interval => 500;

        public static void ShowMenu()
        {
            new MenuBuilder()
                .addItem("Bắt đầu", new(() =>
                {
                    gI.toggle(true);
                    GameScr.info1.addInfo("Tự động vòng quay bắt đầu", 0);
                }))
                .addItem("Nhận đồ\n[" + (isGetItem ? "On]" : "Off]"), new(() =>
                {
                    isGetItem = !isGetItem;
                    ShowMenu();
                    GameScr.info1.addInfo("Tự động nhận đồ vòng quay: " + (isGetItem ? "On" : "Off"), 0);
                }))
                .addItem("Bán vàng:\n[" + (isSellGold ? "On]" : "Off]"), new(() =>
                {
                    isSellGold = !isSellGold;
                    ShowMenu();
                    GameScr.info1.addInfo("Tự động bán vàng khi không đủ: " + (isGetItem ? "On" : "Off"), 0);
                }))
                .addItem($"Menu Thượng đế:\n[{menuConfirm1}]", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "Menu select";
                    ChatTextField.gI().startChat2(gI, "Nhập menu thượng đế");
                }))
                .addItem($"Menu vòng quay:\n[{menuConfirm2}]", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "Menu select";
                    ChatTextField.gI().startChat2(gI, "Nhập menu vòng quay");
                }))
                .addItem($"Menu nhận đồ:\n[{menuConfirmGetItem}]", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "Menu select";
                    ChatTextField.gI().startChat2(gI, "Nhập menu nhận đồ");
                }))
                //.addItem($"Bán vàng\nkhi thấp hơn:\n[{NinjaUtil.getMoneys(goldSell)}]", new(() =>
                //{
                //    ChatTextField.gI().strChat = string.Empty;
                //    ChatTextField.gI().tfChat.name = "Value";
                //    ChatTextField.gI().startChat2(gI, "Nhập số lượng vàng sẽ bán khi ít hơn");
                //}))
                .addItem($"Số thỏi vàng\nsẽ bán:\n[{thoivang}]", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "Value";
                    ChatTextField.gI().startChat2(gI, "Nhập số lượng thỏi vàng sẽ bán");
                }))
                .start();
        }

        protected override void update()
        {
            Utilities.openMenu(19);
            Thread.Sleep(500);
            Service.gI().confirmMenu(19, menuConfirm1);
            Thread.Sleep(500);
            Service.gI().confirmMenu(19, menuConfirm2);
            Thread.Sleep(500);
            ChatPopup.currChatPopup = null;
            Effect2.vEffect2.removeAllElements();
            Effect2.vEffect2Outside.removeAllElements();
            InfoDlg.hide();
            GameCanvas.menu.doCloseMenu();
            Thread.Sleep(500);
            while (gI.IsActing && GameCanvas.currentScreen == CrackBallScr.instance)
            {
                if (Input.GetKey((KeyCode)120))
                {
                    gI.toggle(false);
                    isNotEnough = false;
                    CrackBallScr.gI().doClickSkill(1);
                    CrackBallScr.gI().doClickSkill(1);
                    GameScr.info1.addInfo("Kết thúc", 0);
                    break;
                }
                for (int i = 0; i < 7; i++)
                {
                    CrackBallScr.gI().doClickBall(i);
                    Thread.Sleep(100);
                    if (isNotEnough)
                        break;
                }
                Thread.Sleep(100);
                CrackBallScr.gI().doClickSkill(0);
                CrackBallScr.gI().doClickSkill(0);
                Thread.Sleep(500);
                if (isNotEnough)
                {
                    if (isSellGold)
                    {
                        for(var i = 0; i < thoivang; i++)
                        {
                            var index = Utilities.getIndexItemBag(457);
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
                            }
                            else
                            {
                                gI.toggle(false);
                                isNotEnough = false;
                                CrackBallScr.gI().doClickSkill(1);
                                CrackBallScr.gI().doClickSkill(1);
                                GameScr.info1.addInfo("Không tìm thấy thỏi vàng", 0);
                                break;
                            }
                            Thread.Sleep(100);
                        }
                    }
                    else
                    {
                        gI.toggle(false);
                        isNotEnough = false;
                        CrackBallScr.gI().doClickSkill(1);
                        CrackBallScr.gI().doClickSkill(1);
                        GameScr.info1.addInfo("Kết thúc", 0);
                        break;
                    }
                }
            }
        }

        public static void infoMe(string s)
        {
            if (s.Contains(mResources.not_enough_money_1) && gI.IsActing)
            {
                isNotEnough = true;
            }
            if (s.ToLower().Contains("đã đầy") || s.ToLower().Contains("rương phụ"))
            {
                if (isGetItem)
                {
                    gI.toggle(false);
                    CrackBallScr.gI().doClickSkill(1);
                    CrackBallScr.gI().doClickSkill(1);
                    new Thread(nhanAllDo).Start();
                }
                else
                {
                    gI.toggle(false);
                    isNotEnough = false;
                    CrackBallScr.gI().doClickSkill(1);
                    CrackBallScr.gI().doClickSkill(1);
                    GameScr.info1.addInfo("Kết thúc", 0);
                }
            }
        }

        public static void nhanAllDo()
        {
            Thread.Sleep(500);
            Utilities.openMenu(19);
            Thread.Sleep(500);
            Service.gI().confirmMenu(19, menuConfirm1);
            ChatPopup.currChatPopup = null;
            Effect2.vEffect2.removeAllElements();
            Effect2.vEffect2Outside.removeAllElements();
            InfoDlg.hide();
            GameCanvas.menu.doCloseMenu();
            GameCanvas.panel.cp = null;
            Thread.Sleep(500);
            Service.gI().confirmMenu(19, menuConfirmGetItem);
            Thread.Sleep(500);
            ChatPopup.currChatPopup = null;
            Effect2.vEffect2.removeAllElements();
            Effect2.vEffect2Outside.removeAllElements();
            InfoDlg.hide();
            GameCanvas.menu.doCloseMenu();
            GameCanvas.panel.cp = null;
            while (Char.myCharz().arrItemShop[0].Length > 0)
            {
                Service.gI().buyItem(2, 0, 0);
                Thread.Sleep(1000);
            }
            GameCanvas.panel.hide();
            Thread.Sleep(1000);
            GameCanvas.panel.hide();
            Thread.Sleep(1000);
            gI.toggle(true);
        }

        public static void SaveData()
        {
            Utilities.saveRMSBool("isSellGoldForCrackBall", isSellGold);
            Utilities.saveRMSBool("isGetItemWhenCrackBall", isGetItem);

            Utilities.saveRMSInt("menuThuongDe", menuConfirm1);
            Utilities.saveRMSInt("menuVongQuay", menuConfirm2);
            Utilities.saveRMSInt("menuNhanDo", menuConfirmGetItem);
            Utilities.saveRMSInt("soThoiVangBanCrackBall", thoivang);
        }

        public static void LoadData()
        {
            try
            {
                if (Utilities.isFirstTimeLoad) return;
                isSellGold = Utilities.loadRMSBool("isSellGoldForCrackBall");
                isGetItem = Utilities.loadRMSBool("isGetItemWhenCrackBall");

                menuConfirm1 = (sbyte)Utilities.loadRMSInt("menuThuongDe");
                menuConfirm2 = (sbyte)Utilities.loadRMSInt("menuVongQuay");
                menuConfirmGetItem = (sbyte)Utilities.loadRMSInt("menuNhanDo");
                thoivang = (sbyte)Utilities.loadRMSInt("soThoiVangBanCrackBall");
            }
            catch (Exception) { }
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && string.IsNullOrEmpty(text))
                return;

            if (to == "Nhập menu thượng đế")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception();
                    if (value < 0 || value > 50) throw new Exception();
                    menuConfirm1 = (sbyte)value;
                    ShowMenu();
                    GameScr.info1.addInfo($"Thay đổi menu thượng đế thành công!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else if (to == "Nhập menu vòng quay")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception();
                    if (value < 0 || value > 50) throw new Exception();
                    menuConfirm2 = (sbyte)value;
                    ShowMenu();
                    GameScr.info1.addInfo($"Thay đổi menu vòng quay thành công!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else if (to == "Nhập menu nhận đồ")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception();
                    if (value < 0 || value > 50) throw new Exception();
                    menuConfirmGetItem = (sbyte)value;
                    ShowMenu();
                    GameScr.info1.addInfo("Thay đổi menu nhận đồ thành công!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else if (to == "Nhập số lượng thỏi vàng sẽ bán")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception();
                    if (value < 0 || value > 1000000) throw new Exception();
                    thoivang = value;
                    ShowMenu();
                    GameScr.info1.addInfo("Thay đổi số lượng thành công!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Có lỗi xãy ra vui lòng thử lại!");
                }
            }
            else
                ChatTextField.gI().isShow = false;

            ChatTextField.gI().ResetTF();
        }

        public void onCancelChat()
        {
            ChatTextField.gI().isShow = false;
            ChatTextField.gI().ResetTF();
        }
    }
}
