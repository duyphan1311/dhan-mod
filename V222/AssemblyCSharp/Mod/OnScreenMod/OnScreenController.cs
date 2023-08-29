using AssemblyCSharp.Mod.Options;
using AssemblyCSharp.Mod.PickMob;
using AssemblyCSharp.Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UglyBoy;
using UnityEngine;

namespace AssemblyCSharp.Mod.OnScreenMod
{

    public class OnScreenController
    {

        #region dctt on screen
        public static Char[] saveChar = new Char[30];

        public static void UpdateKeyTouchControl()
        {
            if (OnScreen.nhanVat)
            {
                int number = 90;
                for (int a = 0; a < saveChar.Length; a++)
                {
                    if (saveChar[a] != null)
                    {
                        if (GameCanvas.isPointerHoldIn(GameCanvas.w - 120, number, 150, 10))
                        {
                            XmapController.MoveMyChar(saveChar[a].cx, saveChar[a].cy);
                            global::Char.myCharz().npcFocus = null;
                            global::Char.myCharz().mobFocus = null;
                            global::Char.myCharz().charFocus = null;
                            global::Char.myCharz().itemFocus = null;
                            global::Char.myCharz().charFocus = saveChar[a];
                            SoundMn.gI().buttonClick();
                            global::Char.myCharz().currentMovePoint = null;
                            GameCanvas.clearAllPointerEvent();
                            GameCanvas.clearKeyHold();
                            GameCanvas.clearKeyPressed();
                        }
                        number += 10;
                    }

                }
            }
            string mname;
            if (OnScreen.viewBoss)
            {
                int number = 20;
                foreach (string text in OnScreen.listBoss.AsEnumerable<string>().Reverse<string>())
                {
                    string[] array = text.Split(new char[]
                    {
                        '-'
                    });
                    mname = array[1].Trim();
                    if (text != null)
                    {
                        if (GameCanvas.isPointerHoldIn(GameCanvas.w - 110, number, 100, 10))
                        {
                            for (int i = 0; i < TileMap.mapNames.Length; i++)
                            {
                                if (TileMap.mapNames[i] == mname && TileMap.mapID != i)
                                {
                                    XmapController.StartRunToMapId(i);
                                    break;
                                }
                            }
                            global::Char.myCharz().currentMovePoint = null;
                            SoundMn.gI().buttonClick();
                            GameCanvas.clearAllPointerEvent();
                            GameCanvas.clearKeyHold();
                            GameCanvas.clearKeyPressed();
                        }
                        number += 10;
                    }

                }
            }
        }

        public static void DSToChar(mGraphics g)
        {
            if (OnScreen.nhanVat)
            {
                int numY = 90;
                int stt = 1;
                for (int i = 0; i < saveChar.Length; i++)
                {
                    saveChar[i] = null;
                }
                for (int j = 0; j < GameScr.vCharInMap.size(); j++)
                {
                    mFont mfont;
                    Char @char = (Char)GameScr.vCharInMap.elementAt(j);
                    string classHT;
                    switch (@char.nClass.classId)
                    {
                        case 0:
                            classHT = "TĐ";
                            break;
                        case 1:
                            classHT = "NM";
                            break;
                        case 2:
                            classHT = "XD";
                            break;
                        default:
                            classHT = "Boss";
                            break;
                    }
                    if (@char == null || @char.cName.Trim() == "" 
                        || @char.isPet || @char.isMiniPet 
                        || @char.cNameGoc.StartsWith("#") || @char.cNameGoc.StartsWith("$")
                        || @char.cName.StartsWith("#") || @char.cName.StartsWith("$"))
                    {
                        continue;
                    }
                    if (@char == Char.myCharz().charFocus || @char.cTypePk == 5)
                    {
                        mfont = mFont.nameFontRed;
                    }
                    else if (char.IsUpper(@char.cName[0]) && @char.cTypePk != 5)
                    {
                        mfont = mFont.nameFontRed2;
                    }
                    else if (@char.cungBang)
                    {
                        mfont = mFont.nameFontGreen;
                    }
                    else
                    {
                        mfont = mFont.nameFontOrange;
                    }
                    mfont.drawString(g, string.Concat(new object[]
                    {
                            stt,
                            ". ",
                            @char.cName,
                            ": ",
                            mSystem.numberTostring(long.Parse(@char.cHP.ToString())),
                            " - " + classHT
                    }), GameCanvas.w - 110, numY, 0, mFont.tahoma_7_greySmall);
                    saveChar[j] = @char;
                    numY += 10;
                    stt++;
                }
            }

        }


        #endregion

        #region Boss on screen
        public class ShowBoss
        {

            public static int mapid;
            public static string mapName1;
            public static int MapID(string a)// lay map id
            {
                for (int i = 0; i < TileMap.mapNames.Length; i++)
                {
                    if (TileMap.mapNames[i].Equals(a))
                    {
                        return i;
                    }
                }
                return -1;
            }
            //danh sach boss
            public static void checkBoss(mGraphics g)
            {
                if (OnScreen.viewBoss)
                {
                    try
                    {
                        int num15 = 20;
                        int i = 0;
                        //mFont.bigNumber_While.drawString(g, "*Danh sách boss:", GameCanvas.w - 2, GameCanvas.h - 285, mFont.RIGHT, mFont.tahoma_7b_dark);
                        foreach (string text in OnScreen.listBoss.AsEnumerable<string>().Reverse<string>())
                        {
                            string[] array = text.Split(new char[]
                            {
                            '-'
                            });
                            DateTime value = Convert.ToDateTime(array[2]);
                            TimeSpan timeSpan = DateTime.Now.Subtract(value);
                            int num16 = (int)timeSpan.TotalSeconds;
                            string timeBoss;
                            if (num16 > 3600)
                            {
                                timeBoss = timeSpan.Hours + "h";
                            }
                            else if (num16 > 60)
                            {
                                timeBoss = timeSpan.Minutes + "m";
                            }
                            else
                            {
                                timeBoss = num16 + "s";
                            }
                            mFont mFont;
                            mapName1 = array[1].Trim();
                            mapid = MapID(mapName1);
                            if (array[1].Trim().Contains(TileMap.mapName))
                            {

                                mFont = mFont.nameFontRed;

                            }

                            else
                            {
                                mFont = mFont.nameFontOrange;

                            }
                            mFont.drawString(g, string.Concat(new string[]
                            {
                                array[0].Contains("Super Broly") ? array[0] : array[0].Replace("BOSS",""),
                                " - ",
                                array[1].Replace("zona","khu"),

                                //$"[{mapid}]",
                                " - ",
                                timeBoss
                            }), GameCanvas.w - 2, num15, mFont.RIGHT, mFont.tahoma_7_greySmall);
                            //g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
                            //g.fillRect(GameCanvas.w - 20, GameCanvas.h - num15, 20, 10);
                            num15 += 10;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            public static void chatVip(string chatVip)
            {
                if (OnScreen.viewBoss && (chatVip.Contains("BOSS") || chatVip.Contains("vừa xuất hiện tại")))
                {
                    if (OnScreen.listBoss.Count > 5)
                    {
                        OnScreen.listBoss.RemoveAt(0);
                    }
                    if (chatVip.Contains("ở khu vực nào thì mọi người tự tìm nhé"))
                    {
                        chatVip = chatVip.Replace("ở khu vực nào thì mọi người tự tìm nhé!", "");
                        OnScreen.listBoss.Add(chatVip.Replace(" vừa xuất hiện tại map", "-") + "-" + DateTime.Now.ToString("HH:mm:ss"));
                    }
                    else
                    {
                        OnScreen.listBoss.Add(chatVip.Replace(" vừa xuất hiện tại", "-") + "-" + DateTime.Now.ToString("HH:mm:ss"));
                    }
                }
            }
        }
        #endregion

        #region thong tin de tu
        public static string[] strStatus = new string[6]
        {
            mResources.follow,
            mResources.defend,
            mResources.attack,
            mResources.gohome,
            mResources.fusion,
            mResources.fusionForever
        };
        public static void csdt(mGraphics g)
        {
            OnScreen.IshideNShowCSSP = Option.LoadOptInt("cssp") == 1;
            OnScreen.IshideNShowCSDT = Option.LoadOptInt("csdt") == 1;
            OnScreen.viewBoss = Option.LoadOptInt("vb") == 1;
            OnScreen.nhanVat = Option.LoadOptInt("nv") == 1;
            OnScreen.IsThongTinCN = Option.LoadOptInt("tt") == 1;

            if (OnScreen.IshideNShowCSDT)
            {
                if (Char.myCharz().cgender == 1)
                {
                    strStatus = new string[6]
                    {
                        mResources.follow,
                        mResources.defend,
                        mResources.attack,
                        mResources.gohome,
                        mResources.fusion,
                        mResources.fusionForever
                    };
                }
                else
                {
                    strStatus = new string[5]
                    {
                        mResources.follow,
                        mResources.defend,
                        mResources.attack,
                        mResources.gohome,
                        mResources.fusion
                    };
                }
                mFont.tahoma_7_whiteSmall.drawString(g, "*Đệ tử: " + Char.myPetz().cName, 10, GameCanvas.h - 150, mFont.LEFT, mFont.tahoma_7_greySmall);
                //HP
                mFont.nameFontOrange.drawString(g, "HP: " + mSystem.numberTostring(long.Parse(Char.myPetz().cHP.ToString())) + "(" + (Mathf.Round((float)global::Char.myPetz().cHP / (float)global::Char.myPetz().cHPFull * 100f))
                + "%" + ") " +
                //MP
                "- MP: " + mSystem.numberTostring(long.Parse(Char.myPetz().cMP.ToString())) + "(" +
                (Mathf.Round((float)global::Char.myPetz().cMP / (float)global::Char.myPetz().cMPFull * 100f)) + "%)"
                , 10, GameCanvas.h - 140, mFont.LEFT, mFont.tahoma_7_greySmall);
                //Sức mạnh
                mFont.nameFontOrange.drawString(g, "SM: " + mSystem.numberTostring(long.Parse(Char.myPetz().cPower.ToString())) + " - SĐ: " + NinjaUtil.getMoneys(long.Parse(Char.myPetz().cDamFull.ToString())), 10, GameCanvas.h - 130, mFont.LEFT, mFont.tahoma_7_greySmall);
                //Tiềm năng
                mFont.nameFontOrange.drawString(g, "TN: " + mSystem.numberTostring(long.Parse(Char.myPetz().cTiemNang.ToString())) + " - Giáp: " + NinjaUtil.getMoneys(long.Parse(Char.myPetz().cDefull.ToString())), 10, GameCanvas.h - 120, mFont.LEFT, mFont.tahoma_7_greySmall);
                //Thể lực
                mFont.nameFontOrange.drawString(g, "TL: " + Mathf.Round((float)global::Char.myPetz().cStamina / (float)global::Char.myPetz().cMaxStamina * 100f) + "% - "
                    + strStatus[global::Char.myPetz().petStatus], 10, GameCanvas.h - 110, mFont.LEFT, mFont.tahoma_7_greySmall);

            }
            if (OnScreen.IshideNShowCSSP)
            {
                mFont.tahoma_7_whiteSmall.drawString(g, "*Sư phụ: " + Char.myCharz().cName, 10, GameCanvas.h - 100, mFont.LEFT, mFont.tahoma_7_greySmall);
                //HP
                mFont.nameFontOrange.drawString(g, "HP: " + mSystem.numberTostring(long.Parse(Char.myCharz().cHP.ToString())) + "(" + (Mathf.Round((float)global::Char.myCharz().cHP / (float)global::Char.myCharz().cHPFull * 100f))
                + "%" + ") " +
                //MP
                "- MP: " + mSystem.numberTostring(long.Parse(Char.myCharz().cMP.ToString())) + "(" +
                (Mathf.Round((float)global::Char.myCharz().cMP / (float)global::Char.myCharz().cMPFull * 100f)) + "%)"
                , 10, GameCanvas.h - 90, mFont.LEFT, mFont.tahoma_7_greySmall);
                //Sức mạnh
                mFont.nameFontOrange.drawString(g, "SM: " + mSystem.numberTostring(long.Parse(Char.myCharz().cPower.ToString())) + " - SĐ: " + NinjaUtil.getMoneys(long.Parse(Char.myCharz().cDamFull.ToString())), 10, GameCanvas.h - 80, mFont.LEFT, mFont.tahoma_7_greySmall);
                //Tiềm năng
                mFont.nameFontOrange.drawString(g, "TN: " + mSystem.numberTostring(long.Parse(Char.myCharz().cTiemNang.ToString())) + " - TL: " + Mathf.Round((float)global::Char.myCharz().cStamina / (float)global::Char.myCharz().cMaxStamina * 100f) + "%", 10, GameCanvas.h - 70, mFont.LEFT, mFont.tahoma_7_greySmall);
                //Thể lực
                //mFont.nameFontOrange.drawString(g, "TL: " + Mathf.Round((float)global::Char.myCharz().cStamina / (float)global::Char.myCharz().cMaxStamina * 100f) + "%"
                //    , 10, GameCanvas.h - 70, mFont.LEFT, mFont.tahoma_7_greySmall);
            }
        }
        #endregion
        public static void HienThongTin(mGraphics g)
        {
            return;
            if (GameCanvas.gameTick / (int)Time.timeScale % 10 != 0 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 1 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 2 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 3 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 4)
            {
                if (OnScreen.isHienThongTin)
                {
                    mFont.tahoma_7b_green.drawString(g, "Z: Mở menu tool", 10, GameCanvas.h - 65, mFont.LEFT);
                }
            }
            else
            {
                if (OnScreen.isHienThongTin)
                {
                    mFont.tahoma_7b_white.drawString(g, "Z: Mở menu tool", 10, GameCanvas.h - 65, mFont.LEFT);
                }
            }
        }
        //public static void CopyRight(mGraphics g)
        //{

        //    if (GameCanvas.gameTick / (int)Time.timeScale % 10 != 0 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 1 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 2 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 3 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 4)
        //    {
        //        mFont.bigNumber_red.drawString(g, "L Ê  D U Y", 200, 0, 0);
        //    }
        //    else
        //    {
        //        mFont.tahoma_7b_white.drawString(g, "L Ê  D U Y", 200, 0, 0);
        //    }
        //}
        public static void HP(mGraphics g, int x, int y)
        {
            mFont.bigNumber_red.drawString(g, NinjaUtil.getMoneys(long.Parse(global::Char.myCharz().cHP.ToString())), x, y, mFont.LEFT, mFont.tahoma_7b_dark);
            //mFont.bigNumber_yellow.drawString(g, GameScr.cvip, 150, 50, mFont.LEFT, mFont.tahoma_7b_dark);
        }
        public static void MP(mGraphics g, int x, int y)
        {
            mFont.bigNumber_yellow.drawString(g, NinjaUtil.getMoneys(long.Parse(global::Char.myCharz().cMP.ToString())), x, y, mFont.LEFT, mFont.tahoma_7b_dark);
        }
        public static class HotKey
        {
            public static void ThongTinChucNang(mGraphics g)
            {
                mFont.nameFontOrange.drawString(g, "Cheat: " + Time.timeScale, 158, 0, mFont.LEFT, mFont.tahoma_7b_dark);
                mFont.nameFontOrange.drawString(g, "Speed: " + Char.myCharz().cspeed, 158, 10, mFont.LEFT, mFont.tahoma_7b_dark);
                if (OnScreen.IsThongTinCN)
                {
                    mFont.nameFontOrange.drawString(g,
                        "Auto nhặt: " + (PickMob.PickMob.IsAutoPickItems ? "On" : "Off")
                        + " - Tàn sát: " + (PickMob.PickMob.IsTanSat ? "On" : "Off")
                        + " - Săn boss: " + (PickMobController.sanboss ? "On" : "Off")
                        + " - Auto hồi sinh: " + (PickMob.PickMob.IsRevive ? "On" : "Off")
                    , 205, 0, mFont.LEFT, mFont.tahoma_7b_dark);
                    if (GameScr.isChenKhu)
                    {
                        mFont.nameFontOrange.drawString(g, "Đang chèn vào khu: "
                            + OnScreen.zoneChen
                            + " - số người: "
                            + GameScr.gI().numPlayer[OnScreen.zoneChen]
                            + "/"
                            + GameScr.gI().maxPlayer[OnScreen.zoneChen], 205, 10, mFont.LEFT, mFont.tahoma_7b_dark);
                    }
                }
            }
        }
        public static void MapNKhu(mGraphics g)
        {
            //if (OnScreen.IshideNShowKhuNMap)
            //{
            //mFont.nameFontOrange.drawString(g, DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), 10, GameCanvas.h - 220, mFont.LEFT, mFont.tahoma_7_greySmall);
            mFont.nameFontOrange.drawString(g, string.Concat(new object[]
            {
                TileMap.mapName,
                " - Khu: " + TileMap.zoneID,
            }), 10, GameCanvas.h - 220, mFont.LEFT, mFont.tahoma_7_greySmall);

            if (TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23
                && (TileMap.mapID < 45 || TileMap.mapID > 50)
                && (TileMap.mapID < 53 || TileMap.mapID > 62)
                && (TileMap.mapID < 135 || TileMap.mapID > 154)
                )
            {
                mFont.nameFontOrange.drawString(g, "Số người: "
                                + GameScr.gI().numPlayer[TileMap.zoneID]
                                + "/"
                                + GameScr.gI().maxPlayer[TileMap.zoneID], 10, GameCanvas.h - 210, mFont.LEFT, mFont.tahoma_7_greySmall);
            }
            //int num = 180;
            //for(int d = 0; d < GameScr.vCharInMap.size(); d++)
            //{
            //    global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(num19);
            //    mFont.bigNumber_yellow.drawString(g, string.Concat(new object[]
            //    {
            //        @char.cName,
            //        " - ",
            //        @char.holder,
            //        ": ",
            //        NinjaUtil.getMoneys(long.Parse(Mathf.Round((float)@char.cHP).ToString())),
            //        " | HP: ",
            //        Mathf.Round((float)@char.cHP / Mathf.Round((float)@char.cHPFull) * 100f),
            //        " %"
            //    }), GameCanvas.w - 10, GameCanvas.h - num, mFont.RIGHT, mFont.tahoma_7_greySmall);
            //    num -= 10;
            //}
            //}
        }
        public static void coordinates(mGraphics g)
        {
            //if (OnScreen.isCoordinates)
            //{
            mFont.nameFontOrange.drawString(g, string.Concat(new object[]
            {
                "Tọa độ X: ",
                global::Char.myCharz().cx,
                " - Y: ",
                global::Char.myCharz().cy
            }), 10, GameCanvas.h - 220, mFont.LEFT, mFont.tahoma_7_greySmall);
                    //}
        }
        public static void Square(mGraphics f)
        {
            if (OnScreen.IsSquare)
            {

                f.setColor(UnityEngine.Color.yellow);
                f.drawRect(Char.myCharz().cx - 40 - GameScr.cmx, Char.myCharz().cy - 40 - GameScr.cmy, 70, 70);
                for (int i = 0; i < GameScr.vItemMap.size(); i++)
                {
                    ItemMap item = (ItemMap)GameScr.vItemMap.elementAt(i);
                    if (item != null)
                    {
                        f.setColor(Color.yellow);
                        f.drawRect(item.x - 10 - GameScr.cmx, item.y - 7 - GameScr.cmy, 20, 20);
                    }
                }
            }

        }
        public static void LineBoss(mGraphics g, int cmx, int cmy)
        {
            if (OnScreen.lineBoss)
            {
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
                    if (@char != null && @char.cTypePk == 5)
                    {
                        g.setColor(Color.yellow);
                        g.drawLine(global::Char.myCharz().cx - cmx, global::Char.myCharz().cy - cmy, @char.cx - cmx, @char.cy - cmy);

                    }
                }
            }

        }
        //nhan vat trong khu
        public static void PlayerInZone(mGraphics g)
        {
            if (OnScreen.nhanVat)
            {

                mFont.bigNumber_While.drawString(g, "*Nhân vật trong khu: ", GameCanvas.w - 10, GameCanvas.h - 160, mFont.RIGHT, mFont.tahoma_7b_dark);
                int num18 = 150;
                for (int num19 = 0; num19 < GameScr.vCharInMap.size(); num19++)
                {
                    global::Char char7 = (global::Char)GameScr.vCharInMap.elementAt(num19);
                    mFont.nameFontYellow.drawString(g, string.Concat(new object[]
                    {
                    num19,
                    " - ",
                    char7.cName,
                    ": ",
                    NinjaUtil.getMoneys(long.Parse(Mathf.Round((float)char7.cHP).ToString())),
                    " | HP: ",
                    Mathf.Round((float)char7.cHP / Mathf.Round((float)char7.cHPFull) * 100f),
                    " %"
                    }), GameCanvas.w - 10, GameCanvas.h - num18, mFont.RIGHT, mFont.tahoma_7_greySmall);
                    num18 -= 10;
                }
            }
        }
        #region Basic Frame Counter

        public static int lastTick;
        public static int lastFrameRate;
        public static int frameRate;

        public static int CalculateFrameRate()
        {
            if (System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate;
        }
        #endregion
        public static void Skin()
        {
            if (global::Char.myCharz().charFocus != null)
            {
                global::Char.myCharz().head = global::Char.myCharz().charFocus.head;
                global::Char.myCharz().body = global::Char.myCharz().charFocus.body;
                global::Char.myCharz().leg = global::Char.myCharz().charFocus.leg;
                GameScr.info1.addInfo("Copy", 0);
                return;
            }
            GameScr.info1.addInfo("Chỉ vào trước", 0);
        }
        public static void AutoEnter()
        {
            GameCanvas.gI().keyPressedz(-5);
        }
        public static string sl;
        public static int numxskh = 50;
        public static int numyskh = 150;
        public static List<string> saveSKH = new List<string>();
        public static void HienSKH()
        {
            saveSKH.Clear();
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                if (Char.myCharz().arrItemBag[i] == null)
                {
                    continue;
                }
                Item item = Char.myCharz().arrItemBag[i];
                if (AutoUpGrade.CheckSKH(item))
                {
                    saveSKH.Add(AutoUpGrade.nameSKH);
                }
            }
        }
        public static void hienItem(mGraphics g)
        {
            numxskh = 40;
            numyskh = 200;
            int flag = 0;
            foreach (string item in saveSKH)
            {
                mFont.tahoma_7b_white.drawString(g, item, numyskh, numxskh, mFont.LEFT, mFont.tahoma_7_greySmall);
                numxskh += 10;
                flag++;
                if (flag % 10 == 0 && flag != 0)
                {
                    numyskh += 100;
                    numxskh = 40;
                }
            }
        }

        public static List<string> saveItem = new List<string>();
        public static void HienItem()
        {
            saveItem.Clear();
            string str;
            string[] array = OnScreen.arrayItemShow.Trim().Split(new char[]
            {
                ' '
            });
            int num;
            for (int i = 0; i < array.Length; i++)
            {
                sbyte itemID = 0;
                num = int.Parse(array[i]);
                while (itemID < global::Char.myCharz().arrItemBag.Length)
                {
                    if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                    {
                        itemID++;
                        continue;
                    }
                    if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == num)
                    {
                        str = Char.myCharz().arrItemBag[(int)itemID].template.name + ": " + Char.myCharz().arrItemBag[(int)itemID].quantity.ToString();
                        saveItem.Add(str);
                        itemID++;
                        continue;
                    }
                    itemID++;
                }
            }
        }
        public static void hienItemRaMH(mGraphics g)
        {
            int y = 40;
            foreach (string item in saveItem)
            {
                mFont.tahoma_7b_white.drawString(g, item, GameCanvas.w / 2 - 50, y, mFont.LEFT, mFont.tahoma_7_greySmall);
                y += 10;
            }
        }
    }
}
