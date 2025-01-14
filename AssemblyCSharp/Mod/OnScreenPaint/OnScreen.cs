using Mod.Auto;
using Mod.ModMenu;
using Mod.PickMob;
using System;
using UnityEngine;

namespace Mod.OnScreenPaint
{
    internal class OnScreen
    {
        public static bool IsThongTinCN = true;
        public static int zoneInsert = -1;
        public static bool isAutoChange = false;
        public static bool isFuncBtnSelected;

        public static Image imgFunc = GameCanvas.loadImage("/mainImage/imgModFuc.png");
        static Image imgFuncF = GameCanvas.loadImage("/mainImage/imgModFucF.png");

        public static void paint(mGraphics g, int cmx, int cmy)
        {
            try
            {
                LineBoss(g, cmx, cmy);
                paintFuncBtn(g);
                //ThongTinChucNang(g);
            }
            catch (Exception e)
            {
                WriteLog.write("log_paint.txt", e.Message, "OnScreen");
            }
        }
        public static void LineBoss(mGraphics g, int cmx, int cmy)
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
                if (@char.isNormalChar(true) && @char.isBoss())
                {
                    g.setColor(Color.yellow);
                    g.drawLine(global::Char.myCharz().cx - cmx, global::Char.myCharz().cy - cmy, @char.cx - cmx, @char.cy - cmy);

                }
            }
        }

        public static void paintFuncBtn(mGraphics g)
        {
            if (GameCanvas.isMouseFocus(155, 5, imgFunc.getHeight(), imgFunc.getWidth()) && !GameCanvas.panel.isShow)
            {
                g.drawImage(imgFuncF, 155, 5, 0);
            }
            else
            {
                g.drawImage(imgFunc, 155, 5, 0);
            }
            if (isFuncBtnSelected)
            {
                g.drawImage(ItemMap.imageFlare, 155 + imgFunc.getWidth() / 2, 5 + imgFunc.getHeight() / 2, 3);
            }
        }

        public static void ThongTinChucNang(mGraphics g)
        {
            mFont.nameFontOrange.drawString(g, "Cheat: " + Time.timeScale, 158, 0, mFont.LEFT, mFont.tahoma_7b_dark);
            mFont.nameFontOrange.drawString(g, "Speed: " + Char.myCharz().cspeed, 158, 10, mFont.LEFT, mFont.tahoma_7b_dark);
            string modeTS = PickMobController.mode switch
            {
                PickMobController.TanSatMod.TeleToMob => "Tele đánh quái",
                PickMobController.TanSatMod.TanSatPlayer => "Tàn sát người chơi",
                PickMobController.TanSatMod.TanSatBoss => "Auto đánh boss",
                _ => "Di chuyển đánh quái",
            };
            string modeAutoNhat = PickMobController.modePickItem switch
            {
                PickMobController.PickItemMode.OnlyPickDiamond => "Chỉ nhặt ngọc",
                _ => "Nhặt tất cả",
            };
            if (IsThongTinCN)
            {
                mFont.nameFontOrange.drawString(g,
                    "Auto nhặt: " + (Pk9rPickMob.IsAutoPickItems ? "On" : "Off")
                    + " - Tàn sát: " + (Pk9rPickMob.IsTanSat ? "On" : "Off")
                    + " - AHS: " + (Utilities.IsRevive ? "On" : "Off")
                    + " - Săn boss: " + (AutoFindBoss.isFindBoss ? "On" : "Off")
                , 205, 0, mFont.LEFT, mFont.tahoma_7b_dark);
                mFont.nameFontOrange.drawString(g,
                    "Mode tàn sát: " + modeTS
                    + " - Mode auto nhặt: " + modeAutoNhat
                , 205, 10, mFont.LEFT, mFont.tahoma_7b_dark);
                if (Utilities.isChenKhu || AutoFindBoss.isStart)
                {
                    if (AutoFindBoss.isStart) zoneInsert = AutoFindBoss.khusb;
                    mFont.nameFontYellow2.drawString(g, "Đang chèn vào khu: "
                        + zoneInsert
                        + " - số người: "
                        + GameScr.gI().numPlayer[zoneInsert]
                        + "/"
                        + GameScr.gI().maxPlayer[zoneInsert], 205, 20, mFont.LEFT, mFont.tahoma_7b_dark);
                }
            }
        }

        public static void paintAutoZone(mGraphics g, int zone, int x, int y, int w, int h)
        {
            if (!Utilities.isChenKhu || (Utilities.isChenKhu && zoneInsert != zone))
            {
                g.setColor(new Color(0f, 128f / 255f, 255f / 255f));
                g.fillRect(x, y, w, h);
                mFont.tahoma_7_yellow.drawString(g, "Auto", x + w / 2 + 1, y + 6, mFont.CENTER);
            }
            else
            {
                g.setColor(16742263);
                g.fillRect(x, y, w, h);
                mFont.tahoma_7_yellow.drawString(g, "Huỷ", x + w / 2 + 1, y + 6, mFont.CENTER);
            }
        }

        public static void updateTouch()
        {
            try
            {
                if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                    return;
                if (GameCanvas.isPointerHoldIn(155, 5, imgFunc.getHeight(), imgFunc.getWidth()))
                {
                    isFuncBtnSelected = true;
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                        Utilities.OpenTool();
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
                isFuncBtnSelected = false;
                return;
            }
            catch { }
        }
    }
}
