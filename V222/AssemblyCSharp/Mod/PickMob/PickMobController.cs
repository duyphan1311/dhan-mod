using AssemblyCSharp.Mod.Auto;
using AssemblyCSharp.Mod.OnScreenMod;
using AssemblyCSharp.Mod.Xmap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UglyBoy;
using UnityEngine;

namespace AssemblyCSharp.Mod.PickMob
{
    public class PickMobController
    {
        private
        const int TIME_REPICKITEM = 50;
        private
        const int TIME_DELAY_TANSAT = 500;
        private
        const int ID_ICON_ITEM_TDLT = 4387;
        private static readonly sbyte[] IdSkillsMelee = {
            0,
            9,
            2,
            17,
            4
        };
        private static readonly sbyte[] IdSkillsCanNotAttack = {
            10,
            11,
            14,
            23,
            7
        };

        private static readonly PickMobController _Instance = new();

        public static bool IsPickingItems;

        private static bool IsWait;
        private static long TimeStartWait;
        private static long TimeWait;

        public static List<ItemMap> ItemPicks = new();
        public static List<SetDo1> ListSet1 = new List<SetDo1>();
        public static List<SetDo2> ListSet2 = new List<SetDo2>();
        public static int IndexItemPick = 0;
        public static bool findMobCoplete;
        public static int XNhat;
        public static int YNhat;
        public static long timeAK;
        public static int timeSkill3 = 25000;
        public static bool DangGiaoDich;
        public static int TimeGiaoDich;
        public static string NameInFile;
        public static bool Traded;
        public static string[] ListIDItem;
        public static string[] ListTenNVGD;

        private static void Move(int x, int y)
        {
            Char myChar = Char.myCharz();
            if (!PickMob.IsVuotDiaHinh)
            {
                myChar.currentMovePoint = new MovePoint(x, y);
                return;
            }
            int[] vs = GetPointYsdMax(myChar.cx, x);
            if (vs[1] >= y || (vs[1] >= myChar.cy && (myChar.statusMe == 2 || myChar.statusMe == 1)))
            {
                vs[0] = x;
                vs[1] = y;
            }
            myChar.currentMovePoint = new MovePoint(vs[0], vs[1]);
        }

        #region Get data pick item
        private static TpyePickItem GetTpyePickItem(ItemMap itemMap)
        {
            Char myChar = Char.myCharz();
            bool isMyItem = (itemMap.playerId == myChar.charID || itemMap.playerId == -1);
            if (PickMob.IsItemMe && !isMyItem)
                return TpyePickItem.CanNotPickItem;

            if (PickMob.IsLimitTimesPickItem && itemMap.countAutoPick > PickMob.TimesAutoPickItemMax)
                return TpyePickItem.CanNotPickItem;

            if (!FilterItemPick(itemMap))
                return TpyePickItem.CanNotPickItem;

            if (Res.abs(myChar.cx - itemMap.xEnd) < 60 && Res.abs(myChar.cy - itemMap.yEnd) < 60)
                return TpyePickItem.PickItemNormal;

            if (ItemTime.isExistItem(ID_ICON_ITEM_TDLT))
                return TpyePickItem.PickItemTDLT;

            if (PickMob.IsTanSat)
                return TpyePickItem.PickItemTanSat;

            return TpyePickItem.CanNotPickItem;
        }

        private static bool FilterItemPick(ItemMap itemMap)
        {
            if (PickMob.IdItemPicks.Count != 0 && !PickMob.IdItemPicks.Contains(itemMap.template.id))
                return false;

            if (PickMob.IdItemBlocks.Count != 0 && PickMob.IdItemBlocks.Contains(itemMap.template.id))
                return false;

            if (PickMob.TypeItemPicks.Count != 0 && !PickMob.TypeItemPicks.Contains(itemMap.template.type))
                return false;

            if (PickMob.TypeItemBlock.Count != 0 && PickMob.TypeItemBlock.Contains(itemMap.template.type))
                return false;

            return true;
        }

        private enum TpyePickItem
        {
            CanNotPickItem,
            PickItemNormal,
            PickItemTDLT,
            PickItemTanSat
        }
        #endregion

        #region Get data tan sat
        private static Mob GetMobTanSat()
        {
            Mob mobDmin = null;
            int d;
            int dmin = int.MaxValue;
            Char myChar = Char.myCharz();
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                d = (mob.xFirst - myChar.cx) * (mob.xFirst - myChar.cx) + (mob.yFirst - myChar.cy) * (mob.yFirst - myChar.cy);
                if (IsMobTanSat(mob) && d < dmin)
                {
                    mobDmin = mob;
                    dmin = d;
                }
            }
            return mobDmin;
        }

        private static Mob GetMobNext()
        {
            Mob mobTmin = null;
            long tmin = mSystem.currentTimeMillis();
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                if (IsMobNext(mob) && mob.timeLastDie < tmin)
                {
                    mobTmin = mob;
                    tmin = mob.timeLastDie;
                }
            }
            return mobTmin;
        }

        private static bool IsMobTanSat(Mob mob)
        {
            if (mob.status == 0 || mob.status == 1 || mob.hp <= 0 || mob.isMobMe)
                return false;

            bool checkNeSieuQuai = PickMob.IsNeSieuQuai && !ItemTime.isExistItem(ID_ICON_ITEM_TDLT);
            if (PickMob.IsTSSQ && mob.levelBoss == 0)
                return false;
            if (mob.levelBoss != 0 && checkNeSieuQuai)
                return false;

            if (!FilterMobTanSat(mob))
                return false;

            return true;
        }

        private static bool IsMobNext(Mob mob)
        {
            if (mob.isMobMe)
                return false;

            if (!FilterMobTanSat(mob))
                return false;

            if (PickMob.IsNeSieuQuai && !ItemTime.isExistItem(ID_ICON_ITEM_TDLT) && mob.getTemplate().hp >= 3000)
            {
                if (mob.levelBoss != 0)
                {
                    Mob mobNextSieuQuai = null;
                    bool isHaveMob = false;
                    for (int i = 0; i < GameScr.vMob.size(); i++)
                    {
                        mobNextSieuQuai = (Mob)GameScr.vMob.elementAt(i);
                        if (mobNextSieuQuai.countDie == 10 && (mobNextSieuQuai.status == 0 || mobNextSieuQuai.status == 1))
                        {
                            isHaveMob = true;
                            break;
                        }
                    }
                    if (!isHaveMob)
                    {
                        return false;
                    }
                    mob.timeLastDie = mobNextSieuQuai.timeLastDie;
                }
                else if (mob.countDie == 10 && (mob.status == 0 || mob.status == 1))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool FilterMobTanSat(Mob mob)
        {
            if (PickMob.IdMobsTanSat.Count != 0 && !PickMob.IdMobsTanSat.Contains(mob.mobId))
                return false;

            if (PickMob.TypeMobsTanSat.Count != 0 && !PickMob.TypeMobsTanSat.Contains(mob.templateId))
                return false;

            return true;
        }

        private static Skill GetSkillAttack()
        {
            Skill skill = null;
            Skill nextSkill;
            SkillTemplate skillTemplate = new();
            foreach (var id in PickMob.IdSkillsTanSat)
            {
                skillTemplate.id = id;
                nextSkill = Char.myCharz().getSkill(skillTemplate);
                if (IsSkillBetter(nextSkill, skill))
                {
                    skill = nextSkill;
                }
            }
            return skill;
        }

        private static bool IsSkillBetter(Skill SkillBetter, Skill skill)
        {
            if (SkillBetter == null)
                return false;

            if (!CanUseSkill(SkillBetter))
                return false;

            bool isPrioritize = (SkillBetter.template.id == 17 && skill.template.id == 2) ||
              (SkillBetter.template.id == 9 && skill.template.id == 0);
            if (skill != null && skill.coolDown >= SkillBetter.coolDown && !isPrioritize)
                return false;

            return true;
        }

        private static bool CanUseSkill(Skill skill)
        {
            if (mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill > skill.coolDown)
                skill.paintCanNotUseSkill = false;

            if (skill.paintCanNotUseSkill && !IdSkillsMelee.Contains(skill.template.id))
                return false;

            if (IdSkillsCanNotAttack.Contains(skill.template.id))
                return false;

            if (Char.myCharz().cMP < GetManaUseSkill(skill))
                return false;

            return true;
        }

        private static int GetManaUseSkill(Skill skill)
        {
            if (skill.template.manaUseType == 2)
                return 1;
            else if (skill.template.manaUseType == 1)
                return (skill.manaUse * Char.myCharz().cMPFull / 100);
            else
                return skill.manaUse;
        }

        private static int GetYsd(int xsd)
        {
            Char myChar = Char.myCharz();
            int dmin = TileMap.pxh;
            int d;
            int ysdBest = -1;
            for (int i = 24; i < TileMap.pxh; i += 24)
            {
                if (TileMap.tileTypeAt(xsd, i, 2))
                {
                    d = Res.abs(i - myChar.cy);
                    if (d < dmin)
                    {
                        dmin = d;
                        ysdBest = i;
                    }
                }
            }
            return ysdBest;
        }

        private static int[] GetPointYsdMax(int xStart, int xEnd)
        {
            int ysdMin = TileMap.pxh;
            int x = -1;

            if (xStart > xEnd)
            {
                for (int i = xEnd; i < xStart; i += 24)
                {
                    int ysd = GetYsd(i);
                    if (ysd < ysdMin)
                    {
                        ysdMin = ysd;
                        x = i;
                    }
                }
            }
            else
            {
                for (int i = xEnd; i > xStart; i -= 24)
                {
                    int ysd = GetYsd(i);
                    if (ysd < ysdMin)
                    {
                        ysdMin = ysd;
                        x = i;
                    }
                }
            }
            int[] vs = {
        x,
        ysdMin
      };
            return vs;
        }
        #endregion

        #region Control update
        private static void Wait(int time)
        {
            IsWait = true;
            TimeStartWait = mSystem.currentTimeMillis();
            TimeWait = time;
        }

        private static bool IsWaiting()
        {
            if (IsWait && (mSystem.currentTimeMillis() - TimeStartWait >= TimeWait))
                IsWait = false;
            return IsWait;
        }
        #endregion
        #region mặc đồ theo set
        public static string fileTypeSet1 = "Data\\setDo1.ini";
        public static string fileInfoSet1 = "Data\\infosetDo1.ini";
        public static string fileTypeSet2 = "Data\\setDo2.ini";
        public static string fileInfoSet2 = "Data\\infosetDo2.ini";
        public struct SetDo1
        {
            public string info;
            public int type;

            public SetDo1(string info, int type)
            {
                this.info = info;
                this.type = type;
            }
        }
        public struct SetDo2
        {
            public string info2;
            public int type2;

            public SetDo2(string info2, int type2)
            {
                this.info2 = info2;
                this.type2 = type2;
            }
        }
        public static void addSet1(Item item)
        {
            foreach (SetDo1 setDo1 in ListSet1)
            {
                if (setDo1.type == item.template.type)
                {
                    ListSet1.Remove(setDo1);
                }

            }
            ListSet1.Add(new SetDo1(item.info, item.template.type));
            GameScr.info1.addInfo("Đã thêm " + item.template.name + " vào set 1", 0);
        }
        public static void addSet2(Item item)
        {
            foreach (SetDo2 setDo2 in ListSet2)
            {
                if (setDo2.type2 == item.template.type)
                {
                    ListSet2.Remove(setDo2);
                }

            }
            ListSet2.Add(new SetDo2(item.info, item.template.type));
            GameScr.info1.addInfo("Đã thêm " + item.template.name + " vào set 2", 0);
        }
        public static void MacSet1()
        {
            bool flag = false;
            foreach (SetDo1 setDo1 in ListSet1)
            {
                Item[] arrItemBag = Char.myCharz().arrItemBag;
                try
                {
                    for (int i = 0; i < arrItemBag.Length; i++)
                    {
                        if (flag == false && arrItemBag[i].template.type == setDo1.type && arrItemBag[i].info == setDo1.info)
                        {
                            Service.gI().getItem(4, (sbyte)i);
                            Thread.Sleep(700);
                        }
                    }
                    flag = true;
                }
                catch
                {

                }

            }
            GameScr.info1.addInfo("Đã mặc set 1", 0);
        }
        public static void MacSet2()
        {
            bool flag = false;
            foreach (SetDo2 setDo2 in ListSet2)
            {
                Item[] arrItemBag2 = Char.myCharz().arrItemBag;
                try
                {
                    for (int i = 0; i < arrItemBag2.Length; i++)
                    {
                        if (flag == false && arrItemBag2[i].template.type == setDo2.type2 && arrItemBag2[i].info == setDo2.info2)
                        {
                            Service.gI().getItem(4, (sbyte)i);
                            Thread.Sleep(700);
                        }
                    }
                    flag = true;
                }
                catch
                {

                }

            }
            GameScr.info1.addInfo("Đã mặc set 2", 0);
        }
        public static void actionPerFormSetDo(int idAction)
        {
            if (idAction == 25551)
            {
                new Thread(MacSet1).Start();
            }
            if (idAction == 25552)
            {
                new Thread(MacSet2).Start();
            }
        }
        public static void vectorUseEqui()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Mặc set 1", 25551));
            myVector.addElement(new Command("Mặc set 2", 25552));
            GameCanvas.menu.startAt(myVector, 3);
        }
        #endregion
        public static void findMobForPet()
        {
            findMobCoplete = false;
            MyVector vt = new MyVector();
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob md = (Mob)GameScr.vMob.elementAt(i);
                if (global::Math.abs(md.x - Char.myCharz().cx) > 350)
                {
                    findMobCoplete = true;
                    vt.addElement(md);
                    GameScr.gI().doUseSkillNotFocus(GameScr.onScreenSkill[0]);
                    Service.gI().sendPlayerAttack(vt, new MyVector(), 1);
                    return;
                }

            }
            if (findMobCoplete == false)
            {
                MyVector myVector = new MyVector();
                Mob mob = (Mob)GameScr.vMob.elementAt(0);
                myVector.addElement(mob);
                GameScr.gI().doUseSkillNotFocus(GameScr.onScreenSkill[0]);
                Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
            }
        }
        public static void updateZone()
        {
            if (TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23)
            {
                Service.gI().openUIZone();
            }
            else
            {

            }
        }
        public static void startOKDlg(string info)
        {
            if (info.Contains("Không thể đổi khu vực trong map này"))
                return;
            //else if (info.Contains("Chưa thể đổi khu vực%"))
            //    return;
            else if (info.Contains("Có lỗi xảy ra%"))
                return;
        }
        public static bool isCTInBag = false;
        public static sbyte itemCTDC;
        public static bool IsCanDctt()
        {
            if (
                global::Char.myCharz().arrItemBody[5].template.id == 591 ||
                global::Char.myCharz().arrItemBody[5].template.id == 592 ||
                global::Char.myCharz().arrItemBody[5].template.id == 593 ||
                global::Char.myCharz().arrItemBody[5].template.id == 594 ||
                global::Char.myCharz().arrItemBody[5].template.id == 905 ||
                global::Char.myCharz().arrItemBody[5].template.id == 907 ||
                global::Char.myCharz().arrItemBody[5].template.id == 911
                )
            {
                return true;
            }
            itemCTDC = 0;
            while (itemCTDC < global::Char.myCharz().arrItemBag.Length)
            {
                if (global::Char.myCharz().arrItemBag[(int)itemCTDC] == null)
                {
                    itemCTDC++;
                    continue;
                }
                if (global::Char.myCharz().arrItemBag[(int)itemCTDC].template.id == 591 ||
                    global::Char.myCharz().arrItemBag[(int)itemCTDC].template.id == 592 ||
                    global::Char.myCharz().arrItemBag[(int)itemCTDC].template.id == 593 ||
                    global::Char.myCharz().arrItemBag[(int)itemCTDC].template.id == 594 ||
                    global::Char.myCharz().arrItemBag[(int)itemCTDC].template.id == 905 ||
                    global::Char.myCharz().arrItemBag[(int)itemCTDC].template.id == 907 ||
                    global::Char.myCharz().arrItemBag[(int)itemCTDC].template.id == 911
                )
                {
                    isCTInBag = true;
                    return true;
                }
                itemCTDC++;
            }
            return false;
        }
        public static void DCTT(int id)
        {

            if (IsCanDctt())
            {
                if (isCTInBag)
                {
                    Service.gI().getItem(4, itemCTDC);
                    Service.gI().gotoPlayer(id);
                    Service.gI().getItem(4, itemCTDC);
                }
                else
                {
                    Service.gI().gotoPlayer(id);
                }
            }
        }

        //ban vang
        public static bool isBanVang = false;
        public static int solanSale;
        public static int timeSaleGold = 500;
        public static long lastTimeSaleGold;
        public static void banvang()
        {
            if(solanSale <= 0)
            {
                isBanVang = false;
                GameScr.info1.addInfo("Auto bán thỏi vàng dừng", 0);
                return;
            }
            if (solanSale > 0 && mSystem.currentTimeMillis() - lastTimeSaleGold > timeSaleGold)
            {
                if (UseItem.findItem(457) != -1)
                {
                    Service.gI().saleItem(1, 1, UseItem.findItem(457));
                    if (GameCanvas.currentDialog != null)
                    {
                        ItemObject itemObject = new ItemObject();
                        itemObject.type = 1;
                        itemObject.id = itemID;
                        itemObject.where = 1;
                        GameCanvas.panel.perform(3002, itemObject);
                    }
                    GameCanvas.endDlg();
                    solanSale--;
                    lastTimeSaleGold = mSystem.currentTimeMillis();
                }
                else
                {
                    isBanVang = false;
                    GameScr.info1.addInfo("Không tìm thấy thỏi vàng", 0);
                    return;
                }
            }
        }

        public static bool isVutDo = false;
        public static bool isFullBag1 = false;
        public static long lastTimeVutDo;
        public static int itemID = 0;

        public static void AutoVutDo()
        {
            if (!File.Exists("Ugly\\vutdo.txt"))
            {
                isVutDo = false;
                GameScr.info1.addInfo("Không tồn tại file Ugly\\vutdo.txt", 0);
                return;
            }
            string[] array = File.ReadAllText("Ugly\\vutdo.txt").Split(',');
            if (mSystem.currentTimeMillis() - lastTimeVutDo > 1000)
            {

                if (itemID >= global::Char.myCharz().arrItemBag.Length)
                {
                    itemID = 0;
                    isFullBag1 = false;
                    GameScr.info1.addInfo("Vứt đồ off", 0);
                }
                if (Char.myCharz().arrItemBag[itemID] == null)
                {
                    itemID++;
                    return;
                }
                for (int j = 0; j < array.Length; j++)
                {
                    if (Char.myCharz().arrItemBag[itemID].template.name.ToLower().Contains(array[j].Trim().ToLower()))
                    {
                        Service.gI().useItem(1, 1, (sbyte)itemID, -1);
                        if (GameCanvas.currentDialog != null)
                        {
                            ItemObject itemObject = new ItemObject();
                            itemObject.type = 1;
                            itemObject.id = itemID;
                            itemObject.where = 1;
                            GameCanvas.panel.perform(2004, itemObject);
                        }
                        GameCanvas.endDlg();
                        return;
                    }
                }
                itemID++;
            }
        }

        public static short npcHome;

        public static int mapHome;

        public static bool isLoadGame;

        public static long tgpick;

        public static int isLoginTime = 0;


        public static void Update()
        {
            if (IsWaiting())
                return;
            if(PickMob.IsSkill3 && GameCanvas.gameTick % 20 == 0)
            {
                AutoSkill3();
            }
            if (PickMob.IsSkill5 && GameCanvas.gameTick % 20 == 0)
            {
                AutoSkill5();
            }
            if (isUpPetNroStar && GameCanvas.gameTick % 20 == 0)
            {
                upPetStar();
            }
            if (PickMob.tlkhu && isLoadGame && GameCanvas.gameTick % 20 == 0)
            {
                if(GameCanvas.currentScreen != GameCanvas.loginScr && GameCanvas.currentScreen != GameCanvas.serverScreen)
                {
                    KhuAutoLogin();
                    isLoadGame = false;
                }
            }
            if (PickMob.IsAutoTSBoss && GameCanvas.gameTick % 20 == 0)
            {
                AutoTSBoss();
            }
            if (OnScreen.IsSKH && GameCanvas.gameTick % 20 == 0)
            {
                OnScreenController.HienSKH();
            }
            if (OnScreen.showItem && GameCanvas.gameTick % 20 == 0)
            {
                OnScreenController.HienItem();
            }
            if (AutoUpGrade.IsShowListUpgrade && GameCanvas.gameTick % 20 == 0)
            {
                AutoUpGrade.ShowListUpgrade();
            } 
            if (PickMobController.IsDapDo && GameCanvas.gameTick % 20 == 0)
            {
                PickMobController.DapDo();
            }
            if (AutoUpGrade.isupgrade && GameCanvas.gameTick % 20 == 0)
            {
                AutoUpGrade.update();
            }
            if (AutoUpGrade.isKham && GameCanvas.gameTick % 20 == 0)
            {
                AutoUpGrade.updateA();
            }
            if (AutoUpGrade.isKham2 && GameCanvas.gameTick % 20 == 0)
            {
                AutoUpGrade.updateB();
            }
            if (isGohomeGetGold && GameCanvas.gameTick % 20 == 0)
            {
                GoHomeGetGold();
            }
            if (isDoiVang && GameCanvas.gameTick % 20 == 0)
            {
                DoiVang();
            }
            //if (AutoCrackBall.isauto && GameCanvas.gameTick % 20 == 0)
            //{
            //    AutoCrackBall.gI().update();
            //}
                Char myChar = Char.myCharz();
            Char myPet = Char.myPetz();
            if(myPet.cHP == 0 && myChar.nClass.classId == 1 && PickMob.IsAutoHSDeTu)
            {
                Skill skill = GameScr.onScreenSkill[2];
                GameScr.gI().doUseSkill(skill, true);
                GameScr.gI().doUseSkill(skill, true);
            }
            if (myChar.statusMe == 14 || myChar.cHP <= 0)
                return;

            if (myChar.cHP <= myChar.cHPFull * PickMob.HpBuff / 100 || myChar.cMP <= myChar.cMPFull * PickMob.MpBuff / 100)
                GameScr.gI().doUseHP();
            if (PickMob.IsUpDe)
            {
                if (myPet.cHP <= myPet.cHPFull * PickMob.HpBuff / 100 
                    || myPet.cMP <= myPet.cMPFull * PickMob.MpBuff / 100 
                    || myPet.cMP <= 100 
                    || global::Char.myPetz().cStamina < 5
                )
                    GameScr.gI().doUseHP();
            }

            if (PickMob.IsAutoPickItems && PickMob.IsNhatSKH)
            {
                if (mSystem.currentTimeMillis() - PickMobController.tgpick > 500L)
                {
                    if (PickMobController.IndexItemPick >= GameScr.vItemMap.size())
                    {
                        PickMobController.IndexItemPick = 0;
                    }
                    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(PickMobController.IndexItemPick);
                    //if (PickMobController.GetTpyePickItem(itemMap) != PickMobController.TpyePickItem.CanNotPickItem)
                    //{
                    Char.myCharz().cx = itemMap.xEnd;
                    Char.myCharz().cy = itemMap.yEnd;
                    Service.gI().charMove();
                    Service.gI().pickItem(itemMap.itemMapID);
                    PickMobController.tgpick = mSystem.currentTimeMillis();
                    //}
                    PickMobController.IndexItemPick++;
                }
            }
            bool isUseTDLT = ItemTime.isExistItem(ID_ICON_ITEM_TDLT);
            bool isTanSatTDLT = PickMob.IsTanSat && isUseTDLT;
            if (PickMob.IsAutoPickItems)
            {
                if (mSystem.currentTimeMillis() - tgpick > 500L)
                {
                    for (int i = 0; i < GameScr.vItemMap.size(); i++)
                    {
                        ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                        if (GetTpyePickItem(itemMap) != TpyePickItem.CanNotPickItem)
                        {
                            Service.gI().pickItem(itemMap.itemMapID);
                            tgpick = mSystem.currentTimeMillis();
                        }
                    }
                }
                //if (IsPickingItems)
                //{
                //    if (IndexItemPick >= ItemPicks.Count)
                //    {
                //        IsPickingItems = false;
                //        return;
                //    }
                //    ItemMap itemMap = ItemPicks[IndexItemPick];
                //    switch (GetTpyePickItem(itemMap))
                //    {
                //        case TpyePickItem.PickItemTDLT:
                //            myChar.cx = itemMap.xEnd;
                //            myChar.cy = itemMap.yEnd;
                //            Service.gI().charMove();
                //            Service.gI().pickItem(itemMap.itemMapID);
                //            itemMap.countAutoPick++;
                //            IndexItemPick++;
                //            Wait(TIME_REPICKITEM);
                //            return;
                //        case TpyePickItem.PickItemTanSat:
                //            Move(itemMap.xEnd, itemMap.yEnd);
                //            myChar.mobFocus = null;
                //            Wait(TIME_REPICKITEM);
                //            return;
                //        case TpyePickItem.PickItemNormal:
                //            Service.gI().charMove();
                //            Service.gI().pickItem(itemMap.itemMapID);
                //            itemMap.countAutoPick++;
                //            IndexItemPick++;
                //            Wait(TIME_REPICKITEM);
                //            return;
                //        case TpyePickItem.CanNotPickItem:
                //            IndexItemPick++;
                //            return;
                //    }
                //}
                //ItemPicks.Clear();
                //IndexItemPick = 0;
                //for (int i = 0; i < GameScr.vItemMap.size(); i++)
                //{
                //    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                //    if (GetTpyePickItem(itemMap) != TpyePickItem.CanNotPickItem)
                //    {
                //        ItemPicks.Add(itemMap);
                //    }
                //}
                //if (ItemPicks.Count > 0)
                //{
                //    IsPickingItems = true;
                //    return;
                //}
            }
            if (PickMob.IsTanSatNguoi && myChar.cFlag != 0)
            {
                if (myChar.isCharge)
                {
                    Wait(TIME_DELAY_TANSAT);
                    return;
                }
                //myChar.clearFocus(0);
                if (myChar.charFocus != null && myChar.charFocus.cHP <= 0)
                    myChar.charFocus = null;
                if (myChar.charFocus == null)
                {
                    for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                    {
                        Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                        if (@char == null || @char.cName.Trim() == ""
                            || @char.isPet || @char.isMiniPet
                            || @char.cNameGoc.StartsWith("#") || @char.cNameGoc.StartsWith("$")
                            || @char.cName.StartsWith("#") || @char.cName.StartsWith("$")
                            )
                        {
                            continue;
                        }
                        if (@char.cFlag != 0 && @char.charID > 0 && (@char.cFlag != myChar.cFlag || myChar.cFlag == 8))
                        {
                            myChar.npcFocus = null;
                            myChar.mobFocus = null;
                            myChar.charFocus = null;
                            myChar.itemFocus = null;
                            myChar.charFocus = @char;
                            break;
                        }
                    }
                }
                if (myChar.charFocus != null)
                {
                    if (myChar.skillInfoPaint() == null)
                    {
                        Skill skill = GetSkillAttack();
                        if (skill != null && !skill.paintCanNotUseSkill)
                        {
                            Char @char = myChar.charFocus;
                            GameScr.gI().doSelectSkill(skill, true);
                            if (Res.distance(myChar.cx, myChar.cy, @char.cx, @char.cy) <= 48)
                            {
                                GameScr.gI().MyDoDoubleClickToObj(@char);
                            }
                            else
                            {
                                Char.myCharz().cx = @char.cx;
                                Char.myCharz().cy = @char.cy;
                                Service.gI().charMove();
                            }
                        }
                    }
                }
                Wait(TIME_DELAY_TANSAT);
            }
            if (PickMob.IsTanSatBoss)
            {
                if (myChar.isCharge)
                {
                    Wait(TIME_DELAY_TANSAT);
                    return;
                }
                //myChar.clearFocus(0);
                if (myChar.charFocus != null && myChar.charFocus.cHP <= 0)
                    myChar.charFocus = null;
                if (myChar.charFocus == null)
                {
                    for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                    {
                        Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                        if (@char == null || @char.cName.Trim() == ""
                            || @char.isPet || @char.isMiniPet
                            || @char.cNameGoc.StartsWith("#") || @char.cNameGoc.StartsWith("$")
                            || @char.cName.StartsWith("#") || @char.cName.StartsWith("$")
                            || char.IsLower(@char.cName[0])
                            )
                        {
                            continue;
                        }
                        if (char.IsUpper(@char.cName[0]) && @char.cTypePk == 5)
                        {
                            myChar.npcFocus = null;
                            myChar.mobFocus = null;
                            myChar.charFocus = null;
                            myChar.itemFocus = null;
                            myChar.charFocus = @char;
                            break;
                        }
                    }
                }
                if (myChar.charFocus != null)
                {
                    if (myChar.skillInfoPaint() == null)
                    {
                        Skill skill = GetSkillAttack();
                        if (skill != null && !skill.paintCanNotUseSkill)
                        {
                            Char @char = myChar.charFocus;
                            GameScr.gI().doSelectSkill(skill, true);
                            if (Res.distance(myChar.cx, myChar.cy, @char.cx, @char.cy) <= 48)
                            {
                                GameScr.gI().MyDoDoubleClickToObj(@char);
                            }
                            else
                            {
                                Char.myCharz().cx = @char.cx;
                                Char.myCharz().cy = @char.cy;
                                Service.gI().charMove();
                            }
                        }
                    }
                }
                Wait(TIME_DELAY_TANSAT);
            }
            if (PickMob.IsTanSat)
            {
                if (myChar.isCharge)
                {
                    Wait(TIME_DELAY_TANSAT);
                    return;
                }
                myChar.clearFocus(0);
                if (myChar.mobFocus != null && !IsMobTanSat(myChar.mobFocus))
                    myChar.mobFocus = null;
                if (myChar.mobFocus == null)
                {
                    myChar.mobFocus = GetMobTanSat();
                    if (/*isUseTDLT &&*/ myChar.mobFocus != null)
                    {
                        myChar.cx = myChar.mobFocus.xFirst - 24;
                        myChar.cy = myChar.mobFocus.yFirst;
                        Service.gI().charMove();
                    }
                }
                if (myChar.mobFocus != null)
                {
                    if (myChar.skillInfoPaint() == null)
                    {
                        Skill skill = GetSkillAttack();
                        if (skill != null && !skill.paintCanNotUseSkill)
                        {
                            Mob mobFocus = myChar.mobFocus;
                            mobFocus.x = mobFocus.xFirst;
                            mobFocus.y = mobFocus.yFirst;
                            GameScr.gI().doSelectSkill(skill, true);
                            if (Res.distance(mobFocus.xFirst, mobFocus.yFirst, myChar.cx, myChar.cy) <= 48)
                            {
                                GameScr.gI().MyDoDoubleClickToObj(mobFocus);
                            }
                            else
                            {
                                Move(mobFocus.xFirst, mobFocus.yFirst);
                            }
                        }
                    }
                }
                //else if (!isUseTDLT)
                //{
                //    Mob mob = GetMobNext();
                //    if (mob != null)
                //    {
                //        Move(mob.xFirst - 24, mob.yFirst);
                //    }
                //}
                Wait(TIME_DELAY_TANSAT);
            }
        }

        public static bool isWait = false;

        public static bool isGohomeGetGold = false;

        public static bool isGobackOldmap = false;

        public static int npcnvid = 0;

        public static int idmaphome = 0;

        public static int oldmap = 0;

        public static void GoHomeGetGold()
        {
            if (Char.myCharz().xu <= 100000000 || isWait)
            {
                switch (Char.myCharz().nClass.classId)
                {
                    case 0:
                        idmaphome = 21;
                        npcnvid = 0;
                        break;
                    case 1:
                        idmaphome = 22;
                        npcnvid = 2;
                        break;
                    default:
                        idmaphome = 23;
                        npcnvid = 1;
                        break;
                }
                if (TileMap.mapID != idmaphome && !isWait)
                {
                    oldmap = TileMap.mapID;
                    XmapController.StartRunToMapId(idmaphome);
                }
                if (!isWait)
                {
                    Npc npc = GameScr.findNPCInMap((short)npcnvid);
                    if (npc != null) XmapController.MoveMyChar(npc.cx, npc.cy);
                    for (int i = 0; i < 4; i++)
                    {
                        Service.gI().openMenu(npcnvid);
                        Service.gI().confirmMenu((short)npcnvid, (sbyte)OnScreen.npcNVCF);
                    }
                }
                if (isGobackOldmap)
                {
                    if (TileMap.mapID != oldmap)
                    {
                        XmapController.StartRunToMapId(oldmap);
                        isWait = true;
                        return;
                    }
                }
                isWait = false;

            }
        }
        // auto doi vang
        public static bool isDoiVang = false;

        public static void DoiVang()
        {
            Service.gI().openMenu(70);
            Service.gI().confirmMenu(70, 0);
            Service.gI().confirmMenu(70, 0);
            Service.gI().confirmMenu(70, 0);
            Service.gI().confirmMenu(70, 5);
        }

        // auto cong chi so
        public static void AutoCS()
        {
            if (PickMob.IsCCS)
            {
                if (!File.Exists("Data\\chiso.ini"))
                {
                    File.Create("Data\\chiso.ini").Close();
                    File.WriteAllText("Data\\chiso.ini", "550000|550000|25000");
                }
            }
            string text = File.ReadAllText("Data\\chiso.ini");
            string[] array = text.Split(new char[]
                {
                    '|'
                });
            int hp = int.Parse(array[0].Trim());
            int mp = int.Parse(array[1].Trim());
            int sd = int.Parse(array[2].Trim());
            int flag = 0;
            while (PickMob.IsCCS)
            {
                if(Char.myCharz().cHPGoc < hp)
                {
                    if(Char.myCharz().cHPGoc <= hp - 2000)
                    {
                        Service.gI().upPotential(0, 100);
                    }
                    else if (Char.myCharz().cHPGoc <= hp - 200)
                    {
                        Service.gI().upPotential(0, 10);
                    }
                    else if (Char.myCharz().cHPGoc <= hp - 20)
                    {
                        Service.gI().upPotential(0, 1);
                    }
                    else
                    {
                        flag++;
                    }
                }
                if (Char.myCharz().cMPGoc < mp)
                {
                    if (Char.myCharz().cMPGoc <= mp - 2000)
                    {
                        Service.gI().upPotential(1, 100);
                    }
                    else if (Char.myCharz().cMPGoc <= mp - 200)
                    {
                        Service.gI().upPotential(1, 10);
                    }
                    else if (Char.myCharz().cMPGoc <= mp - 20)
                    {
                        Service.gI().upPotential(1, 1);
                    }
                    else
                    {
                        flag++;
                    }
                }
                if (Char.myCharz().cDamGoc < sd)
                {
                    if (Char.myCharz().cDamGoc <= sd - 100)
                    {
                        Service.gI().upPotential(2, 100);
                    }
                    else if (Char.myCharz().cDamGoc <= sd - 10)
                    {
                        Service.gI().upPotential(2, 10);
                    }
                    else if (Char.myCharz().cDamGoc <= sd - 1)
                    {
                        Service.gI().upPotential(2, 1);
                    }
                    else
                    {
                        flag++;
                    }
                }
                if((Char.myCharz().cHPGoc == hp && Char.myCharz().cMPGoc == mp && Char.myCharz().cDamGoc == sd) || flag == 3)
                {
                    PickMob.IsCCS = false;
                    GameScr.info1.addInfo("Auto cộng chỉ số tắt", 0);
                    return;
                }
                Thread.Sleep(1000);
            }
        }

        //check skh
        public static bool CheckSKH(Item item)
        {
            for(int i = 0; i < item.itemOption.Length; i++)
            {
                if(item.itemOption[i].optionTemplate.id == 107)
                {
                    return true;
                }
                if (item.itemOption[i].optionTemplate.name.StartsWith("$"))
                {
                    return true;
                }
                if(item.itemOption[i].optionTemplate.type < 4)
                {
                    return true;
                }
            }
            return false;
        }
        //check so luong sao pha le
        public static int SoLuongSPL(Item item)
        {
            for(int i = 0; i < item.itemOption.Length; i++)
            {
                if(item.itemOption[i].optionTemplate.id == 107)
                {
                    return item.itemOption[i].param;
                }
            }
            return 0;
        }

        // auto pha ma bao ve
        public static void AutoPhaMaBV()
        {
            int pass = PickMob.MBV;
            if (PickMob.IsPhaMBV)
            {
                File.Create("Data\\duy.txt").Close();
            }
            while (PickMob.IsPhaMBV)
            {
                File.AppendAllText("Data\\duy.txt", pass.ToString() + " ");
                if(PickMob.MBV == 100000)
                {
                    Service.gI().setLockInventory(pass);
                    pass++;
                    Thread.Sleep(50);
                }
                else
                {
                    if(pass > PickMob.MBV + 20000)
                    {
                        PickMob.IsPhaMBV = false;
                        GameScr.info1.addInfo("Auto phá mã bảo vệ tắt", 0);
                        break;
                    }
                    Service.gI().setLockInventory(pass);
                    pass++;
                    Thread.Sleep(50);
                }
            }
        }
        // auto danh
        public static void CharAK(Char @char)
        {
            try
            {
                MyVector myVector = new MyVector();
                myVector.addElement(@char);
                Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
            }
            catch
            {

            }
        }
        public static void goiRong()
        {
            sbyte itemID = 0;
            new Thread(delegate ()
            {
                while (PickMob.IsNR)
                {
                    if (GameScr.gI().isRongThanXuatHien)
                    {
                        PickMob.IsNR = false;
                        GameScr.info1.addInfo("Auto gọi rồng tắt", 0);
                        break;
                    }
                    while (itemID < global::Char.myCharz().arrItemBag.Length)
                    {
                        if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == 14)
                        {
                            Service.gI().useItem(0, 1, itemID, -1);
                            Thread.Sleep(50);
                            break;
                        }

                        itemID++;
                    }
                }
            }).Start();
                
        }
        public static void MobAK(Mob @mob)
        {
            try
            {
                MyVector myVector = new MyVector();
                myVector.addElement(@mob);
                Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
            }
            catch
            {

            }
        }
        public static void AutoAK(Mob @mob)
        {

            if (Char.myCharz().mobFocus != null)
            {
                MobAK(Char.myCharz().mobFocus);
            }
            if (Char.myCharz().charFocus != null)
            {
                CharAK(Char.myCharz().charFocus);
            }

        }

        public static void KhuAutoLogin()
        {
            if (PickMob.tlkhu == true)
            {
                if (TileMap.zoneID != PickMob.oldKhu && TileMap.mapName.Equals(PickMob.amap))
                {
                    OnScreen.zoneChen = PickMob.oldKhu;
                    new Thread(delegate ()
                    {
                        while (TileMap.zoneID != PickMob.oldKhu && TileMap.mapName.Equals(PickMob.amap))
                        {
                            GameScr.isChenKhu = true;
                            Service.gI().requestChangeZone(PickMob.oldKhu, -1);
                            Thread.Sleep(1000);
                        }
                        GameScr.isChenKhu = false;
                        while (PickMob.IsUpDe && Char.myPetz().petStatus != 1 && Char.myPetz().petStatus != 2)
                        {
                            Service.gI().petStatus(2);
                            Thread.Sleep(1000);
                        }
                    }).Start();
                }
            }
        }

        public static int timeDelay = 1000;
        public static long lastTimeLog = 0;
        public static bool isReconnect = false;
        public static void autoLogin()
        {
            try
            {
                if (isReconnect && mSystem.currentTimeMillis() - lastTimeLog > timeDelay)
                {
                    if (GameCanvas.currentScreen == GameCanvas.loginScr 
                        || GameCanvas.currentScreen == GameCanvas.serverScreen)
                    {
                        if (GameCanvas.currentDialog != null)
                        {
                            GameCanvas.endDlg();
                        }
                        loadAccount();
                    }
                    lastTimeLog = mSystem.currentTimeMillis();
                }
            }
            catch (Exception)
            {
            }
        }
        public static void loadAccount()
        {
            if (GameCanvas.loginScr == null)
            {
                GameCanvas.loginScr = new LoginScr();
            }
            GameCanvas.loginScr.switchToMe();
            Service.gI().login(ALogin.Account, ALogin.Password, GameMidlet.VERSION, 0);
        }
        //nhat do de tu roi
        public static void NhatDoUpDT()
        {
            for (int i = 0; i < GameScr.vItemMap.size(); i++)
            {
                ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                Char.myCharz().itemFocus = itemMap;
                if (itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1 && Math.abs(itemMap.x - Char.myCharz().cx) < 200 && Math.abs(itemMap.y - Char.myCharz().cy) < 48)
                {
                    Char.myCharz().currentMovePoint = new MovePoint(itemMap.x, itemMap.y);
                    Char.myCharz().itemFocus = itemMap;
                    Thread.Sleep(1000);
                    Char.myCharz().currentMovePoint = new MovePoint(XNhat, YNhat);
                }
            }
        }
        #region autokok
        public static void MoveLeft()
        {
            //GameScr.gI().checkClickMoveTo(global::Char.myCharz().cx + 11, global::Char.myCharz().cy);
            global::Char.myCharz().cx -= 10;
            Service.gI().charMove();
        }
        public static void MoveRight()
        {
            //GameScr.gI().checkClickMoveTo(global::Char.myCharz().cx - 10, global::Char.myCharz().cy);
            global::Char.myCharz().cx += 10;
            Service.gI().charMove();
        }
        // up de nrostar
        public static bool isUpPetNroStar = false;
        public static long lastTimeChangeZoneU;
        public static long timeChangZoneU = 1500000L;
        public static void upPetStar()
        {
            if (mSystem.currentTimeMillis() - lastTimeChangeZoneU > timeChangZoneU)
            {
                Service.gI().requestChangeZone(TileMap.zoneID, -1);
                lastTimeChangeZoneU = mSystem.currentTimeMillis();
            }
        }
        // auto ts boss
        public static int hpBoss;
        public static int skTSBoss;
        public static int hpBossDef = 80000000;
        public static string FileHPBoss = "Data\\hpBossTS.ini";
        public static void AutoTSBoss()
        {
            if (Char.myCharz().nClass.classId != 0)
            {
                if (!File.Exists(FileHPBoss))
                {
                    File.Create(FileHPBoss).Close();
                    File.WriteAllText(FileHPBoss, hpBossDef.ToString());
                }
                hpBoss = Int32.Parse(File.ReadAllText(FileHPBoss));
                if (Char.myCharz().nClass.classId == 2)
                {
                    skTSBoss = 5;
                }
                if (Char.myCharz().nClass.classId == 1)
                {
                    skTSBoss = 4;
                }
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                    char name = char.Parse(@char.cName.Substring(0, 1));
                    if (@char == null || @char.cName.Trim() == ""
                        || @char.isPet || @char.isMiniPet
                        || @char.cNameGoc.StartsWith("#") || @char.cNameGoc.StartsWith("$")
                        || @char.cName.StartsWith("#") || @char.cName.StartsWith("$"))
                    {
                        continue;
                    }
                    if (char.IsUpper(name))
                    {
                        if (@char.cTypePk == 5 && @char.cHP <= hpBoss)
                        {
                            Skill skill = GameScr.onScreenSkill[skTSBoss];
                            GameScr.gI().doUseSkillNotFocus(skill);
                            GameScr.gI().doUseSkillNotFocus(skill);
                            return;
                        }
                    }
                }
            }
        }
        public static void AutoKOK()
        {
            Char myChar = Char.myCharz();
            while (PickMob.IsAKOK)
            {
                if (PickMob.IsKOKMove)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                }
                Thread.Sleep(500);
                PickMob.IsKOKMove = !PickMob.IsKOKMove;
            }
        }
        #endregion
        //auto skill 3
        public static bool isTTNL;
        public static void AutoSkill3()
        {
            Skill skill3 = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[2]);
            Skill skill1 = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[0]);
            if (skill3.point > 0 && mSystem.currentTimeMillis() - skill3.lastTimeUseThisSkill > skill3.coolDown)
            {
                if ((Char.myPetz().cHP /** 100 / Char.myPetz().cHPFull*/ <= 0 || Char.myPetz().cMP * 100 / Char.myPetz().cMPFull < 10 || Char.myCharz().cHP * 100 / Char.myCharz().cHPFull < 10 || Char.myCharz().cMP * 100 / Char.myCharz().cMPFull < 10) && Char.myCharz().cgender == 1 && skill3.manaUse < Char.myCharz().cMP)
                {
                    if (skill3.point > 1 && !PickMob.IsAutoHSDeTu) buffMe();
                    else
                    {
                        MyVector vecPet = new MyVector();
                        vecPet.addElement(GameScr.findCharInMap(-Char.myCharz().charID));
                        Service.gI().selectSkill(skill3.template.id);
                        Service.gI().sendPlayerAttack(new MyVector(), vecPet, 2);
                        Service.gI().selectSkill(skill1.template.id);
                    }
                    skill3.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                    return;
                }
                if (/*(Char.myCharz().cHP * 100 / Char.myCharz().cHPFull < 10 || Char.myCharz().cMP * 100 / Char.myCharz().cMPFull < 10) &&*/ Char.myCharz().cgender == 2 || Char.myCharz().cgender == 0)
                {
                    GameScr.gI().doUseSkillNotFocus(skill3);
                    isTTNL = true;
                    skill3.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                    return;
                }
            }
            if (Char.myCharz().cgender == 2 && isTTNL && mSystem.currentTimeMillis() - skill3.lastTimeUseThisSkill > 10000)
            {
                Char.myCharz().myskill = skill1;
                isTTNL = false;
            }

        }
        public static bool isTS;
        public static void AutoSkill5()
        {
            Skill skill5 = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]);
            Skill skill1 = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[0]);
            if (skill5.point > 0 && mSystem.currentTimeMillis() - skill5.lastTimeUseThisSkill > skill5.coolDown)
            {
                //if ((Char.myPetz().cHP /** 100 / Char.myPetz().cHPFull*/ <= 0 || Char.myPetz().cMP * 100 / Char.myPetz().cMPFull < 10 || Char.myCharz().cHP * 100 / Char.myCharz().cHPFull < 10 || Char.myCharz().cMP * 100 / Char.myCharz().cMPFull < 10) && Char.myCharz().cgender == 1 && skill5.manaUse < Char.myCharz().cMP)
                //{
                //    if (skill5.point > 1 && !PickMob.IsAutoHSDeTu) buffMe();
                //    else
                //    {
                //        MyVector vecPet = new MyVector();
                //        vecPet.addElement(GameScr.findCharInMap(-Char.myCharz().charID));
                //        Service.gI().selectSkill(skill3.template.id);
                //        Service.gI().sendPlayerAttack(new MyVector(), vecPet, 2);
                //        Service.gI().selectSkill(skill1.template.id);
                //    }
                //    skill5.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                //    return;
                //}
                if (Char.myCharz().cgender == 2)
                {
                    GameScr.gI().doUseSkillNotFocus(skill5);
                    isTS = true;
                    skill5.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                    return;
                }
            }
            if (Char.myCharz().cgender == 2 && isTS && mSystem.currentTimeMillis() - skill5.lastTimeUseThisSkill > 10000)
            {
                Char.myCharz().myskill = skill1;
                isTS = false;
            }

        }
        // xin dau
        public static void buffMe()
        {
            if (!canBuffMe(out Skill skillBuff))
            {
                GameScr.info1.addInfo("Không tìm thấy kỹ năng Trị thương", 0);
                return;
            }

            // Đổi sang skill hồi sinh
            Service.gI().selectSkill(ID_SKILL_BUFF);

            // Tự tấn công vào bản thân
            Service.gI().sendPlayerAttack(new MyVector(), getMyVectorMe(), -1);

            // Trả về skill cũ
            Service.gI().selectSkill(Char.myCharz().myskill.template.id);

            // Đặt thời gian hồi cho skill
            skillBuff.lastTimeUseThisSkill = mSystem.currentTimeMillis();
        }
        public static MyVector getMyVectorMe()
        {
            var vMe = new MyVector();
            vMe.addElement(Char.myCharz());
            return vMe;
        }
        public const sbyte ID_SKILL_BUFF = 7;
        /// <summary>
        /// Kiểm tra khả năng sử dụng skill Trị thương vào bản thân.
        /// </summary>
        /// <param name="skillBuff">Skill trị thương.</param>
        /// <returns>true nếu có thể sử dụng skill trị thương vào bản thân.</returns>
        public static bool canBuffMe(out Skill skillBuff)
        {
            skillBuff = Char.myCharz().
                getSkill(new SkillTemplate { id = ID_SKILL_BUFF });

            if (skillBuff == null)
            {
                return false;
            }

            return true;
        }
        public static void xinDau()
        {

            while (PickMob.IsXinDau)
            {
                Service.gI().clanMessage(1, "", -1);
                Thread.Sleep(302000);
               
            }

        }

        public static int findItemInBag(string ItemName)
        {
            try
            {
                sbyte itemID = 0;
                while (itemID < global::Char.myCharz().arrItemBag.Length)
                {
                    if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                    {
                        itemID++;
                        continue;
                    }
                    if (global::Char.myCharz().arrItemBag[(int)itemID].template.name.ToLower().Contains(ItemName))
                    {
                        return itemID;
                    }
                    itemID++;
                }

            }
            catch (Exception ex)
            {
                GameScr.info1.addInfo(ex.Message, 0);
                return -1;
            }
            return -1;
        }
        
        //auto dap do
        public static bool IsDapDo;

        public static int sosao = 7;

        public static void DapDo()
        {
            if (AutoUpGrade.isSaleGoldToUpgrade && Char.myCharz().xu <= 500000000)
            {
                IsDapDo = false;
                PickMobController.solanSale = 4;
                PickMobController.timeSaleGold = 1000;
                PickMobController.lastTimeSaleGold = mSystem.currentTimeMillis();
                PickMobController.isBanVang = true;
                IsDapDo = true;
            }
            for (int i = 0; i < GameCanvas.panel.vItemCombine.size(); i++)
            {
                if (GameCanvas.panel.vItemCombine.elementAt(i) != null)
                {
                    Service.gI().combine(1, GameCanvas.panel.vItemCombine);
                    GameCanvas.gI().keyPressedz(-5);
                }
            }
        }

        private static int getMaxStart(Item item)
        {
            int result = 0;
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                bool flag = item.itemOption[i].optionTemplate.id == 107;
                if (flag)
                {
                    result = item.itemOption[i].param;
                }
            }
            return result;
        }

        public static Item findItemBagWithIndexUI(int index)
        {
            foreach (Item item in global::Char.myCharz().arrItemBag)
            {
                bool flag = item != null && item.indexUI == index;
                if (flag)
                {
                    return item;
                }
            }
            return null;
        }

        //tele
        public static void TeleToPlayer(string IDTele)
        {
            if (string.IsNullOrEmpty(IDTele))
            {
                GameScr.info1.addInfo("Chưa nhập id người chơi", 0);
                return;
            }
            DCTT(int.Parse(IDTele));

        }

        //Auto sanboss

        public static List<int> listOldZone = new();

        public static List<int> listFileZone = new();

        public static int khusb;

        public static int proc1;

        public static int waitTime = 1;

        public static bool canAutoPlay;

        public static bool sanboss = false;

        public static bool testBoss = false;

        public static string FileBossName = "Data\\bossName.txt";

        public static string FileBoss = "Data\\SanBoss.txt";

        public static string BossIni = "Data\\SB.txt";

        public static string KhuBoss = "Data\\khuBoss.ini";

        public static int sbDef = 0;

        public static int maxNumberPlayer;

        public static bool isGoToMapBoss = false;
        public static void SanBoss()
        {
            int[] zones = GameScr.gI().zones;
            //string[] bossNames = File.ReadAllText(FileBossName).Split(',');
            if (sanboss && isGoToMapBoss)
            {
                FindMapBoss(/*bossNames*/);
            }

            if (testBoss || sanboss)
            {
                if (!File.Exists(BossIni))
                {
                    File.Create(BossIni).Close();
                    File.WriteAllText(BossIni, sbDef.ToString());
                }
                int sbb = Int32.Parse(File.ReadAllText(BossIni));
                if (sbb <= 0)
                {
                    listOldZone.Clear();
                    if (File.Exists(FileBoss))
                    {
                        File.Delete(FileBoss);
                    }
                    if (File.Exists(BossIni))
                    {
                        File.Create(BossIni).Close();
                        File.WriteAllText(BossIni, sbDef.ToString());
                    }
                }
                int proc = Int32.Parse(File.ReadAllText(BossIni));
                proc++;
                File.WriteAllText(BossIni, proc.ToString());
            }

            while (sanboss || testBoss)
            {

                try
                {
                    if (!Pk9rXmap.IsXmapRunning)
                    {
                        if (!File.Exists(FileBoss))
                        {
                            File.Create(FileBoss).Close();
                        }

                        if (!File.Exists(KhuBoss))
                        {
                            File.Create(KhuBoss).Close();
                            File.WriteAllText(KhuBoss, "-1|0");
                        }

                        proc1 = Int32.Parse(File.ReadAllText(BossIni));

                        List<string> list1 = File.ReadAllLines(FileBoss).ToList();

                        listFileZone = list1.Select(s => int.Parse(s)).ToList();

                        string[] str = File.ReadAllText(KhuBoss).Split('|');

                        int khuBoss = Int32.Parse(str[0]);
                        int id = Int32.Parse(str[1]);

                        if (khuBoss != -1)
                        {
                            sanboss = false;
                            listOldZone.Clear();
                            GameScr.info1.addInfo("Tìm thấy " + FindBoss() + " ở khu " + khuBoss, 0);
                            if (khuBoss != TileMap.zoneID)
                            {
                                if (Char.myCharz().charID != id && IsCanDctt())
                                {
                                    DCTT(id);
                                }
                                else
                                {
                                    OnScreen.zoneChen = khuBoss;
                                    GameScr.chenKhu(khuBoss);
                                }
                            }
                            proc1 = Int32.Parse(File.ReadAllText(BossIni));
                            proc1--;
                            File.WriteAllText(BossIni, proc1.ToString());
                            if (proc1 <= 0)
                            {
                                if (File.Exists(FileBoss))
                                {
                                    File.Delete(FileBoss);
                                }
                                if (File.Exists(BossIni))
                                {
                                    File.Create(BossIni).Close();
                                    File.WriteAllText(BossIni, sbDef.ToString());
                                }
                                if (File.Exists(KhuBoss))
                                {
                                    File.Create(KhuBoss).Close();
                                    File.WriteAllText(KhuBoss, "-1|0");
                                }
                                return;
                            }
                            continue;
                        }

                        if (proc1 <= 0)
                        {
                            sanboss = false;
                            listOldZone.Clear();
                            if (File.Exists(FileBoss))
                            {
                                File.Delete(FileBoss);
                            }
                            if (File.Exists(BossIni))
                            {
                                File.Create(BossIni).Close();
                                File.WriteAllText(BossIni, sbDef.ToString());
                            }
                            GameScr.info1.addInfo("Săn boss: " + (sanboss ? "Bật" : "Tắt"), 0);
                            return;
                        }
                        if (!listOldZone.Any() || khusb == TileMap.zoneID)
                        {
                            if (listFileZone.Count == zones.Length || listOldZone.Count == zones.Length)
                            {
                                sanboss = false;
                                listOldZone.Clear();
                                if (File.Exists(FileBoss))
                                {
                                    File.Delete(FileBoss);
                                }
                                if (File.Exists(BossIni))
                                {
                                    File.Create(BossIni).Close();
                                    File.WriteAllText(BossIni, sbDef.ToString());
                                }
                                GameScr.info1.addInfo("Không tìm thấy boss", 0);
                                GameScr.info1.addInfo("Săn boss: " + (sanboss ? "Bật" : "Tắt"), 0);
                                return;
                            }
                            if (FindBoss() != "-1")
                            {
                                sanboss = false;
                                GameScr.info1.addInfo("Tìm thấy " + FindBoss() + " ở khu " + TileMap.zoneID, 0);
                                listOldZone.Clear();
                                if (proc1 == 1)
                                {
                                    if (File.Exists(KhuBoss))
                                    {
                                        File.Create(KhuBoss).Close();
                                        File.WriteAllText(KhuBoss, "-1|0");
                                    }
                                }
                                proc1 = Int32.Parse(File.ReadAllText(BossIni));
                                proc1--;
                                File.WriteAllText(BossIni, proc1.ToString());
                                if (proc1 <= 0)
                                {
                                    if (File.Exists(FileBoss))
                                    {
                                        File.Delete(FileBoss);
                                    }
                                    if (File.Exists(BossIni))
                                    {
                                        File.Create(BossIni).Close();
                                        File.WriteAllText(BossIni, sbDef.ToString());
                                    }
                                    if (File.Exists(KhuBoss))
                                    {
                                        File.Create(KhuBoss).Close();
                                        File.WriteAllText(KhuBoss, "-1|0");
                                    }
                                }
                                return;
                                //if (canAutoPlay)
                                //{
                                //    Service.gI().useItem(0, 1, -1, 521);
                                //}
                                //return;
                            }
                            //if (findItemInBag("Tự động luyện tập") != -1 && !canAutoPlay)
                            //{
                            //    Service.gI().useItem(0, 1, -1, 521);
                            //    waitTime = 10;
                            //}
                            //else
                            //{
                            //    if (!canAutoPlay)
                            //    {
                            //        waitTime = 30;
                            //    }
                            //    else
                            //    {
                            //        waitTime = 10;
                            //    }
                            //}
                            maxNumberPlayer = -1;
                            if (listOldZone.IndexOf(TileMap.zoneID) == -1 && listFileZone.IndexOf(TileMap.zoneID) == -1)
                            {
                                listOldZone.Add(TileMap.zoneID);
                                File.AppendAllText(FileBoss, TileMap.zoneID.ToString() + Environment.NewLine);
                            }
                            for (int i = 0; i < zones.Length; i++)
                            {
                                if (listFileZone.IndexOf(zones[i]) == -1 && listOldZone.IndexOf(zones[i]) == -1)
                                {
                                    if (GameScr.gI().numPlayer[zones[i]] >= GameScr.gI().maxPlayer[zones[i]])
                                    {
                                        continue;
                                    }
                                    if (maxNumberPlayer < GameScr.gI().numPlayer[zones[i]])
                                    {
                                        maxNumberPlayer = GameScr.gI().numPlayer[zones[i]];
                                        khusb = zones[i];
                                    }
                                }
                            }
                            listOldZone.Add(khusb);
                            string op = khusb.ToString();
                            File.AppendAllText(FileBoss, op + Environment.NewLine);
                            Service.gI().requestChangeZone(khusb, -1);
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Service.gI().requestChangeZone(khusb, -1);
                            Thread.Sleep(1000);
                        }
                    }
                }
                catch (Exception e)
                {
                    sanboss = false;
                    listOldZone.Clear();
                    if (File.Exists(FileBoss))
                    {
                        File.Delete(FileBoss);
                    }
                    if (File.Exists(BossIni))
                    {
                        File.Create(BossIni).Close();
                        File.WriteAllText(BossIni, sbDef.ToString());
                    }
                    //GameScr.info1.addInfo(e.Message, 0);
                    GameScr.info1.addInfo("Săn boss: " + (sanboss ? "Bật" : "Tắt"), 0);
                    return;
                }
            }

        }

        public static int khuhientai;
        public static int khudo;
        public static void AutoDoKhuBoss()
        {
            int khu = khudo;
            string[] bossNames = File.ReadAllText(FileBossName).Split(',');
            while (PickMob.IsDoKhu)
            {
                try
                {
                    if (khu < khudo + 10)
                    {
                        if (khu == TileMap.zoneID)
                        {
                            if (FindBoss() != "-1")
                            {
                                PickMob.IsDoKhu = !PickMob.IsDoKhu;
                                GameScr.info1.addInfo("Tìm thấy " + FindBoss(), 0);
                                return;
                            }
                            if (TileMap.zoneID == khudo)
                            {
                                khu++;
                                if (khu == khuhientai) khu++;
                            }
                            Service.gI().requestChangeZone(khu, -1);
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Service.gI().requestChangeZone(khu, -1);
                            Thread.Sleep(1000);
                        }
                    }
                }
                catch (Exception e)
                {
                    PickMob.IsDoKhu = false;
                    GameScr.info1.addInfo("Săn boss: " + (PickMob.IsDoKhu ? "Bật" : "Tắt"), 0);
                    return;
                }
            }
        }

        public static bool isStop = false;
        public static string mn = string.Empty;
        public static int mid;
        public static void FindMapBoss()
        {
            string text = OnScreen.listBoss.ElementAt<string>(OnScreen.listBoss.Count - 1);
            if (text != null)
            {
                string[] array = text.Split(new char[]
                {
                    '-'
                });

                mn = array[1].Trim();
                mid = OnScreenController.ShowBoss.MapID(mn);
                //for (int b = 0; b < bossNames.Length; b++)
                //{
                if (TileMap.mapName.Equals(mn)/* && array[0].Replace("BOSS", "").ToLower().Contains(bossNames[b].ToLower())*/)
                {
                    //isStop = true;
                    GameScr.info1.addInfo("Đã đến map boss", 0);
                    //break;
                }
                if (TileMap.mapID != mid/* && array[0].Replace("BOSS", "").ToLower().Contains(bossNames[b].ToLower())*/)
                {
                    XmapController.StartRunToMapId(mid);
                    //isStop = true;
                    GameScr.info1.addInfo("Đã đến map boss", 0);
                    //break;
                }
                //}
                //if (isStop)
                //{
                //    GameScr.info1.addInfo("Đã đến map boss", 0);
                //}
            }

        }
        //public static string FindBoss(string[] bossNames)
        public static string FindBoss()
        {
            //if (!testBoss)
            //{
            //    if (!OnScreen.listBoss.Any())
            //    {
            //        return "1";
            //    }
            //}
            //string text = OnScreen.listBoss.ElementAt<string>(OnScreen.listBoss.Count - 1);
            //if (text != null)
            //{
            //    string[] array = text.Split(new char[]
            //    {
            //        '-'
            //    });

            //    mn = array[1].Trim();
            //    mid = OnScreenController.ShowBoss.MapID(mn);

            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char boss = (Char)GameScr.vCharInMap.elementAt(i);
                if (boss == null || boss.cName.Trim() == ""
                    || boss.isPet || boss.isMiniPet
                    || boss.cNameGoc.StartsWith("#") || boss.cNameGoc.StartsWith("$")
                    || boss.cName.StartsWith("#") || boss.cName.StartsWith("$")
                    || boss.cName.Contains("Đệ tử")
                    || boss.cHP <= 0)
                {
                    continue;
                }
                if (char.IsUpper(boss.cName[0]) && boss.cTypePk == 5)
                {
                    File.WriteAllText(KhuBoss, TileMap.zoneID.ToString() + "|" + Char.myCharz().charID.ToString());
                    return boss.cName;
                }

                //for (int j = 0; j < bossNames.Length; j++)
                //{
                //    if (boss.cName.ToLower().Contains(bossNames[j].ToLower()))
                ////    {
                //File.WriteAllText(KhuBoss, TileMap.zoneID.ToString() + "|" + Char.myCharz().charID.ToString());
                //            return boss.cName;
                //    }
                //}
            }
            //}
            return "-1";
        }
        public static void choDau()
        {
            while (PickMob.IsChoDau)
            {
                for (int i = 0; i < ClanMessage.vMessage.size(); i++)
                {
                    ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(i);
                    if (clanMessage.maxCap != 0 && clanMessage.playerName != global::Char.myCharz().cName && clanMessage.recieve != clanMessage.maxCap)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Service.gI().clanDonate(clanMessage.id);
                            Thread.Sleep(150);
                        }
                    }
                }
                Thread.Sleep(150);
            }
        }
        public static void thuDau()
        {
            if (TileMap.mapID == 21 || TileMap.mapID == 22 || TileMap.mapID == 23)
            {
                int soLuong = 0;
                for (int i = 0; i < Char.myCharz().arrItemBox.Length; i++)
                {
                    Item item = Char.myCharz().arrItemBox[i];
                    if (item != null && item.template.type == 6)
                    {
                        soLuong += item.quantity;

                    }
                }
                if (soLuong < 20 && GameCanvas.gameTick % 200 == 0)
                {
                    for (int i2 = 0; i2 < Char.myCharz().arrItemBox.Length; i2++)
                    {
                        Item item2 = Char.myCharz().arrItemBox[i2];
                        if (item2 != null && item2.template.type == 6)
                        {
                            Service.gI().getItem(1, (sbyte)i2);
                        }
                    }
                }
                if (GameScr.gI().magicTree.currPeas > 0 && GameScr.hpPotion < 10 || soLuong < 20 && GameCanvas.gameTick % 100 == 0)
                {
                    Service.gI().openMenu(4);
                    Service.gI().confirmMenu(4, 0);
                }
            }
        }
        public static void autodt()
        {
            if (TileMap.mapID != 27)
            {
                PickMob.IsDT = false;
                return;
            }
            if ( GameCanvas.gameTick % (20 * (int)Time.timeScale) == 0)
            {
                Service.gI().openMenu(25);
                Service.gI().confirmMenu(25,0);
                GameScr.info1.addInfo("Đang auto doanh trại", 0);
            }

        }
        public static void Revive()
        {
            if (PickMob.IsRevive && global::Char.myCharz().meDead)
            {
                Service.gI().wakeUpFromDead();
            }
        }
        private static bool haveCSKB;
        public static void AutoCSKB()
        {
            while (PickMob.IsAutoCSKB)
            {
                try
                {
                    sbyte itemID = 0;

                    haveCSKB = false;

                    while (itemID < global::Char.myCharz().arrItemBag.Length)
                    {
                        if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                        {
                            itemID++;
                            continue;
                        }
                        if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == 380)
                        {
                            Service.gI().useItem(0, 1, itemID, -1);
                            Thread.Sleep(1000);
                            haveCSKB = true;
                            break;
                        }
                        itemID++;
                    }
                    if (!haveCSKB)
                    {
                        PickMob.IsAutoCSKB = false;
                        GameScr.info1.addInfo("Không tìm thấy Viên Capsule kì bí", 0);
                        return;
                    }
                    //if (PickMobController.findItemInBag("Viên Capsule kì bí") != -1)
                    //{
                    //    Service.gI().useItem(0, 1, -1, 380);
                    //    waitTime = 10;
                    //}
                    //else
                    //{
                    //    PickMob.IsAutoCSKB = false;
                    //    GameScr.info1.addInfo("Không tìm thấy Viên Capsule kì bí", 0);
                    //    return;
                    //}
                    //Thread.Sleep(1000);

                }
                catch (Exception e)
                {
                    PickMob.IsAutoCSKB = false;
                    GameScr.info1.addInfo(e.Message, 0);
                    GameScr.info1.addInfo("Auto sử dụng Viên Capsule kì bí: " + (PickMob.IsAutoCSKB ? "Bật" : "Tắt"), 0);
                    return;
                }
            }
        }
        //Auto focus
        public static void AutoFocus()
        {
            if (PickMob.CharAutoFocus == null)
            {
                if (Char.myCharz().charFocus != null)
                {
                    PickMob.CharAutoFocus = Char.myCharz().charFocus;
                    GameScr.info1.addInfo("Auto focus: " + PickMob.CharAutoFocus.cName, 0);
                }
            }
            else
            {
                PickMob.CharAutoFocus = null;
                GameScr.info1.addInfo("Tắt auto focus", 0);
            }

        }
        public static void AutoFocusBoss()
        {
            
            if (PickMob.IsAutoFocusBoss)
            {
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                    if (@char == null || @char.cName.Trim() == ""
                        || @char.isPet || @char.isMiniPet
                        || @char.cNameGoc.StartsWith("#") || @char.cNameGoc.StartsWith("$")
                        || @char.cName.StartsWith("#") || @char.cName.StartsWith("$")
                        || @char.cName.Contains("Đệ tử")
                        || @char.cHP <= 0)
                    {
                        continue;
                    }
                    if (char.IsUpper(@char.cName[0]) && @char.cTypePk == 5)
                    {
                        global::Char.myCharz().npcFocus = null;
                        global::Char.myCharz().mobFocus = null;
                        global::Char.myCharz().charFocus = null;
                        global::Char.myCharz().itemFocus = null;
                        global::Char.myCharz().charFocus = @char;
                        break;
                    }
                }
            }
        }
        public static void AutoNeBoss()
        {
            while (PickMob.IsAutoNeBoss)
            {
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                    char name = char.Parse(@char.cName.Substring(0, 1));
                    if (name >= 'A' && name < 'Z' && !@char.cName.StartsWith("Đệ tử") && !@char.cName.StartsWith("Ăn trộm"))
                    {
                        Service.gI().requestChangeZone(-1, -1);
                    }
                }

                Thread.Sleep(1000);
            }
        }
        public static bool FlagInMap()
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                if (@char.cFlag != 0 && @char.charID > 0)
                {
                    return true;
                }

            }
            return false;
        }
        public static void AutoFlag()
        {
            if (PickMob.IsAutoFlag && GameCanvas.gameTick % (20 * (int)Time.timeScale) == 0)
            {
                if (!FlagInMap() && Char.myCharz().cFlag == 0)
                {
                    Service.gI().getFlag(1, 8);
                }
                if (FlagInMap() && Char.myCharz().cFlag == 8)
                {
                    Service.gI().getFlag(1, 0);
                }
            }
          
        }
        public static void autoAnDuiGa()
        {
            if (PickMob.IsAnDuiGa)
            {
                if (TileMap.mapID == 21 || TileMap.mapID == 22 || TileMap.mapID == 23 && GameCanvas.gameTick % (20 * (int)Time.timeScale)==0)
                {
                    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(0);
                    if(itemMap != null)
                    {
                        Service.gI().pickItem(itemMap.itemMapID);
                    }
                }
            }

        }
        // auto enter
        public static void AutoEnter()
        {
            if (PickMob.IsAEnter)
            {
                while (PickMob.IsAEnter)
                {
                    GameCanvas.keyPressed[13] = true;
                }
            }
        }
        // AK
        public static void MOB()
        {
            try
            {
                MyVector myVector = new MyVector();
                myVector.addElement(Char.myCharz().mobFocus);
                Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
            }
            catch
            {

            }
        }
        public static void CHARs()
        {
            try
            {
                MyVector myVector = new MyVector();
                myVector.addElement(Char.myCharz().charFocus);
                Service.gI().sendPlayerAttack(myVector, new MyVector(), 2);
            }
            catch
            {

            }
        }
        public static void AK()
        {
            
                if (mSystem.currentTimeMillis() - timeAK > Char.myCharz().myskill.coolDown + 100)
                {
                MyVector myVector = new MyVector();
                MyVector myVector2 = new MyVector();
                if (global::Char.myCharz().charFocus != null)
                {
                    myVector2.addElement(global::Char.myCharz().charFocus);
                }
                if (global::Char.myCharz().mobFocus != null)
                {
                    myVector.addElement(global::Char.myCharz().mobFocus);
                }
                if (myVector.size() != 0)
                {
                    Service.gI().sendPlayerAttack(myVector, myVector2, 1);
                    timeAK = mSystem.currentTimeMillis();
                }
                if (myVector2.size() != 0)
                {
                    Service.gI().sendPlayerAttack(myVector, myVector2, 2);
                    timeAK = mSystem.currentTimeMillis();
                }
                //if (Char.myCharz().mobFocus != null && GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus) && Math.abs(Char.myCharz().mobFocus.x
                //    - Char.myCharz().cx) < Char.myCharz().myskill.dx * 1.5)
                //{
                //    MOB();
                //    timeAK = mSystem.currentTimeMillis();
                //    return;
                //}
                //if (Char.myCharz().charFocus != null && Char.myCharz().isMeCanAttackOtherPlayer(Char.myCharz().charFocus)
                //    && Math.abs(Char.myCharz().charFocus.xSd
                //    - Char.myCharz().cx) < Char.myCharz().myskill.dx * 1.5)
                //{
                //    CHARs();
                //    timeAK = mSystem.currentTimeMillis();
                //}
            }

            
        }
        ////////////
        //auto ban do kho bau
        public static void AutoBDKB()
        {
            if (TileMap.mapID != 5)
            {
                PickMob.IsABDKB = false;
                return;
            }
            if (!GameScr.isPaint)
            {
                PickMob.IsABDKB = false;
                return;
            }
            if(GameCanvas.gameTick %(20*(int)Time.timeScale) == 0)
            {
                Service.gI().confirmMenu((short)PickMob.idNPC.template.npcTemplateId, 0);
            }
        }
        // tu sat
        public static void TuSat()
        {

            while(Char.myCharz().cHP > 0)
            {
                Service.gI().getFlag(1, 8);
                MyVector tuSat = new MyVector();
                tuSat.addElement(Char.myCharz());
                Service.gI().sendPlayerAttack(new MyVector(), tuSat, -1);
                Thread.Sleep(1000);
            }
        }
        public static void TuHS()
        {
            if (PickMob.IsTuHS) {
                while (Char.myCharz().cHP > 0)
                {
                    //Service.gI().getFlag(1, 8);
                    MyVector tuSat = new MyVector();
                    tuSat.addElement(Char.myCharz());
                    GameScr.gI().doUseSkillNotFocus(GameScr.onScreenSkill[2]);
                    Service.gI().sendPlayerAttack(new MyVector(), tuSat, -1);
                    Thread.Sleep(PickMob.TuHSTime);
                }
            }
            
        }
        public static void GoiDeTu()
        {
            Char.myCharz().cy--;
            Service.gI().charMove();
            Char.myCharz().cy++;
            Service.gI().charMove();
        }
        public static void KhoaViTri()
        {
            if (PickMob.kvt)
            {
                Char.myCharz().isLockMove = true ;
            }
            else
            {
                Char.myCharz().isLockMove = false;
            }
        }
        public static int aHP = 100000;
        public static int aMP = 50000;
        public static void ASkill3HPMP()
        {
            if (PickMob.IsAutoSkill3HPMP)
            {
                
               if(Char.myCharz().cHP <= aHP || Char.myCharz().cMP <= aMP)
                {
                    Skill skill = GameScr.onScreenSkill[2];
                    GameScr.gI().doUseSkillNotFocus(skill);
                    GameScr.gI().doUseSkillNotFocus(skill);
                }  
            }
            
            
        }
    }
}