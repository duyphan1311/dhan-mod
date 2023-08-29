using Mod.ModHelper;
using Mod.ModHelper.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Mod.Auto
{
    internal class AutoCrackBall : ThreadActionUpdate<AutoCrackBall>
    {
        public static bool isGetItem;

        public static bool isNotEnough;

        public static bool isAuto;

        public int typenhando = 0;

        public override int Interval => 500;

        public static void ShowMenu()
        {
            //MyVector myVector = new MyVector();
            //myVector.addElement(new Command("Mở thường", gI, 1, 1));
            //myVector.addElement(new Command("Mở đặc\nbiệt", gI, 1, 2));
            //myVector.addElement(new Command("Nhận đồ", gI, 2, null));
            //GameCanvas.menu.startAt(myVector, 3);
            new MenuBuilder()
                .addItem("Bắt đầu", new(() =>
                {
                    gI.toggle(true);
                    isAuto = true;
                    GameScr.info1.addInfo("Tự động vòng quay bắt đầu", 0);
                }))
                .addItem("Nhận đồ\n[" + (isGetItem ? "On]" : "Off]"), new(() =>
                {
                    isGetItem = !isGetItem;
                    ShowMenu();
                    GameScr.info1.addInfo("Tự động nhận đồ vòng quay: " + (isGetItem ? "On" : "Off"), 0);
                }))
                .addItem("Nhận đồ", new(() =>
                {

                }))
                .start();
        }

        protected override void update()
        {
            if (isAuto)
            {
                Service.gI().openMenu(19);
                Thread.Sleep(500);
                Service.gI().confirmMenu(19, 1);
                Thread.Sleep(500);
                Service.gI().confirmMenu(19, 0);
                Thread.Sleep(500);
                ChatPopup.currChatPopup = null;
                Effect2.vEffect2.removeAllElements();
                Effect2.vEffect2Outside.removeAllElements();
                InfoDlg.hide();
                GameCanvas.menu.doCloseMenu();
            }
            while (isAuto)
            {
                try
                {
                    if (Input.GetKey((KeyCode)120))
                    {
                        gI.toggle(false);
                        isAuto = false;
                        isNotEnough = false;
                        CrackBallScr.gI().doClickSkill(1);
                        CrackBallScr.gI().doClickSkill(1);
                        GameScr.info1.addInfo("Kết thúc", 0);
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        CrackBallScr.gI().doClickBall(i);
                        if (isNotEnough)
                            break;
                    }
                    Thread.Sleep(100);
                    CrackBallScr.gI().doClickSkill(0);
                    CrackBallScr.gI().doClickSkill(0);
                    Thread.Sleep(500);
                    if (isNotEnough)
                    {
                        gI.toggle(false);
                        isAuto = false;
                        isNotEnough = false;
                        CrackBallScr.gI().doClickSkill(1);
                        CrackBallScr.gI().doClickSkill(1);
                        GameScr.info1.addInfo("Kết thúc", 0);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static void infoMe(string s)
        {
            if (s.Contains(mResources.not_enough_money_1) && isAuto)
            {
                isNotEnough = true;
            }
            if (s.ToLower().Contains("đã đầy"))
            {
                if (isGetItem)
                {
                    isAuto = false;
                    CrackBallScr.gI().doClickSkill(1);
                    CrackBallScr.gI().doClickSkill(1);
                    //GameScr.info1.addInfo("Xong.", 0);
                    new Thread(nhanAllDo).Start();
                }
                else
                {
                    gI.toggle(false);
                    isAuto = false;
                    isNotEnough = false;
                    CrackBallScr.gI().doClickSkill(1);
                    CrackBallScr.gI().doClickSkill(1);
                    GameScr.info1.addInfo("Kết thúc", 0);
                }
            }
        }

        //public void perform(int idAction, object p)
        //{
        //    if (idAction == 1)
        //    {
        //        type = (int)p;
        //        isauto = true;
        //        new Thread(gI().update).Start();
        //    }
        //    if (idAction == 2)
        //    {
        //        MyVector myVector = new MyVector();
        //        myVector.addElement(new Command("Nhận vàng", gI(), 3, 9));
        //        myVector.addElement(new Command("Nhận bùa", gI(), 3, 13));
        //        myVector.addElement(new Command("Nhận đồ", gI(), 3, -1));
        //        myVector.addElement(new Command("Nhận cải trang", gI(), 3, 5));
        //        GameCanvas.menu.startAt(myVector, 3);
        //    }
        //    if (idAction == 3)
        //    {
        //        typenhando = (int)p;
        //        new Thread(runnhando).Start();
        //    }
        //}
        public static void nhanAllDo()
        {
            Service.gI().openMenu(19);
            Thread.Sleep(500);
            Service.gI().confirmMenu(19, 1);
            ChatPopup.currChatPopup = null;
            Effect2.vEffect2.removeAllElements();
            Effect2.vEffect2Outside.removeAllElements();
            InfoDlg.hide();
            GameCanvas.menu.doCloseMenu();
            GameCanvas.panel.cp = null;
            Thread.Sleep(500);
            //if (GameCanvas.menu.menuItems.size() == 5)
            //{
                Service.gI().confirmMenu(19, 1);
                Thread.Sleep(500);
                ChatPopup.currChatPopup = null;
                Effect2.vEffect2.removeAllElements();
                Effect2.vEffect2Outside.removeAllElements();
                InfoDlg.hide();
                GameCanvas.menu.doCloseMenu();
                GameCanvas.panel.cp = null;
                Service.gI().buyItem(2, 0, 0);
                Thread.Sleep(1000);
                GameCanvas.panel.hide();
            Thread.Sleep(1000);
            GameCanvas.panel.hide();
            Thread.Sleep(1000);
            isAuto = true;
            //}
            //else
            //{
            //    GameScr.info1.addInfo("Không có đồ.", 0);
            //}
        }
        public void runnhando()
        {
            Service.gI().openMenu(19);
            Thread.Sleep(700);
            Service.gI().confirmMenu(19, (sbyte)(GameCanvas.menu.menuItems.size() - 1));
            ChatPopup.currChatPopup = null;
            Effect2.vEffect2.removeAllElements();
            Effect2.vEffect2Outside.removeAllElements();
            InfoDlg.hide();
            GameCanvas.menu.doCloseMenu();
            GameCanvas.panel.cp = null;
            Thread.Sleep(700);
            if (GameCanvas.menu.menuItems.size() == 5)
            {
                Service.gI().confirmMenu(19, 3);
                Thread.Sleep(500);
                ChatPopup.currChatPopup = null;
                Effect2.vEffect2.removeAllElements();
                Effect2.vEffect2Outside.removeAllElements();
                InfoDlg.hide();
                GameCanvas.menu.doCloseMenu();
                GameCanvas.panel.cp = null;
                if (typenhando == -1)
                {
                    for (int i = 0; i < Char.myCharz().arrItemShop[0].Length; i++)
                    {
                        Item item = Char.myCharz().arrItemShop[0][i];
                        if (item != null && (item.template.type == 0 || item.template.type == 1 || item.template.type == 2 || item.template.type == 3 || item.template.type == 4))
                        {
                            Service.gI().buyItem(0, i, 0);
                        }
                    }
                    return;
                }
                int num = 0;
                while (num < Char.myCharz().arrItemShop[0].Length)
                {
                    Item item2 = Char.myCharz().arrItemShop[0][num];
                    if (item2 != null && item2.template.type == typenhando)
                    {
                        Service.gI().buyItem(0, num, 0);
                        Thread.Sleep(500);
                    }
                    else
                    {
                        num++;
                    }
                }
            }
            else
            {
                GameScr.info1.addInfo("Không có đồ.", 0);
            }
        }
    }
}
