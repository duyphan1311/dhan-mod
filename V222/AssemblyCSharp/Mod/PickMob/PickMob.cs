using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.IO;
using System.Collections;
using AssemblyCSharp.Mod.Menu;
using AssemblyCSharp.Mod.Xmap;
using UglyBoy;
using AssemblyCSharp.Mod.OnScreenMod;
using AssemblyCSharp.Mod.Other;
using Mod.ModHelper;
using Mod.Auto;

namespace AssemblyCSharp.Mod.PickMob
{
    public class PickMob
    {
        private
        const int ID_ITEM_GEM = 77;
        private
        const int ID_ITEM_GEM_LOCK = 861;
        private
        const int DEFAULT_HP_BUFF = 20;
        private
        const int DEFAULT_MP_BUFF = 20;
        private static readonly sbyte[] IdSkillsBase = {
      0,
      2,
      17,
      4
    };
        private static readonly short[] IdItemBlockBase = {
      225,
      353,
      354,
      355,
      356,
      357,
      358,
      359,
      360,
      362
    };

        public static bool IsTanSat = false;
        public static bool IsNeSieuQuai = true;
        public static bool IsTSSQ = false;
        public static bool IsVuotDiaHinh = true;
        public static List<int> IdMobsTanSat = new();
        public static List<int> TypeMobsTanSat = new();
        public static List<sbyte> IdSkillsTanSat = new(IdSkillsBase);

        public static bool IsAutoPickItems = true;
        public static bool IsItemMe = true;
        public static bool IsLimitTimesPickItem = true;
        public static int TimesAutoPickItemMax = 7;
        public static List<short> IdItemPicks = new();
        public static List<short> IdItemBlocks = new(IdItemBlockBase);
        public static List<sbyte> TypeItemPicks = new();
        public static List<sbyte> TypeItemBlock = new();

        public static int HpBuff = 0;
        public static int MpBuff = 0;

        public static bool IsCSDT;
        public static bool viewBoss = true;
        public static List<string> listBoss = new List<string>();

        public static bool IsAutoLogin = true;
        public static bool IshideNShowCSDT = true;
        public static bool IshideNShowCSSP = true;
        public static bool IshideNShowKhuNMap = true;

        public static bool IsminiMap = true;
        public static int tdc = 9;
        public static float speed = 1.5f;

        public static bool IsXinDau;
        public static bool IsChoDau;
        public static bool lineBoss = true;

        public static bool ukhu;

        public static bool isCharAutoFocus;
        public static global::Char CharAutoFocus;
        public static bool isHideP;
        public static bool IsStart = true;
        public static bool IsAutoChat;
        public static bool IsAutoFocusBoss;
        public static bool IsAutoNeBoss;
        public static bool IsAutoFlag;
        public static bool IsAutoThuDau;
        public static bool IsAnDuiGa = false;
        public static bool IsAK;
        public static bool IsDT;
        public static bool IsNhatDoUpDT;
        public static bool IsRevive = true;
        public static bool IsAutoCSKB;
        public static bool IsACN, IsABH, IsABK, IsAGX, IsAFood;
        public static bool IsAKOK;
        public static bool IsKOKMove;
        public static bool IsAutoTSBoss = false;
        public static bool IsSkill3;
        public static bool IsSkill5;
        public static bool IsPaint;
        public static bool IsAcGiaoDich;
        public static bool IsKhoaMap;
        public static bool IsKhoaKhu;
        public static bool IsAEnter;
        public static bool IsABDKB;
        public static Npc idNPC;
        public static bool IsTuHS;
        public static int TuHSTime = 44000;
        public static int cx;
        public static int cy;
        public static bool kvt = false;
        public static bool isEnter = false;
        public static bool isXinDauV;
        public static string idC = null;
        public static bool IsAutoSkill3HPMP;
        public static bool IsUpDe = false;
        public static bool IsNR = false;
        public static bool IsAutoDapDo = false;
        public static bool IsDoKhu = false;
        public static bool IsPhaMBV = false;
        public static bool IsCCS = false;
        public static bool IsKhauTrang = false;
        public static int MBV = 100000;
        public static bool IsAutoHSDeTu = false;
        public static bool isNVdd = false;
        public static bool isNDVQ = false;
        public static bool IsTanSatBoss = false;
        public static bool IsTanSatNguoi = false;
        public static bool isGHNT = false;
        #region Chat
        public static bool Chat(string txt)
        {
            if (!txt.StartsWith("/"))
                return false;

            txt = txt.Substring(1);
            string[] array = txt.Trim().Split(';');

            for(int idx = 0; idx < array.Length; idx++)
            {
                string text = array[idx].Trim();
                if(text == "asy")
                {
                    var myChar = Char.myCharz();
                    var myPet = Char.myPetz();

                    if (myChar.cName == "")
                        break;

                    SocketClient.gI.sendMessage(new
                    {
                        action = "updateInfo",
                        Utilities.status,
                        myChar.charID,
                        myChar.cName,
                        myChar.cgender,
                        TileMap.mapName,
                        TileMap.mapID,
                        TileMap.zoneID,
                        myChar.cx,
                        myChar.cy,
                        myChar.cHP,
                        myChar.cHPFull,
                        myChar.cMP,
                        myChar.cMPFull,
                        myChar.cStamina,
                        myChar.cMaxStamina,
                        myChar.cPower,
                        myChar.cTiemNang,
                        myChar.cHPGoc,
                        myChar.cMPGoc,
                        myChar.cDefGoc,
                        myChar.cCriticalGoc,
                        myChar.cDamFull,
                        myChar.cDefull,
                        myChar.cCriticalFull,
                        cPetName = myPet.cName,
                        cPetGender = myPet.cgender,
                        cPetHP = myPet.cHP,
                        cPetHPFull = myPet.cHPFull,
                        cPetMP = myPet.cMP,
                        cPetMPFull = myPet.cMPFull,
                        cPetStamina = myPet.cStamina,
                        cPetMaxStamina = myPet.cMaxStamina,
                        cPetPower = myPet.cPower,
                        cPetTiemNang = myPet.cTiemNang,
                        cPetDamFull = myPet.cDamFull,
                        cPetDefull = myPet.cDefull,
                        cPetCriticalFull = myPet.cCriticalFull,
                        petStatus = OnScreenController.strStatus[myPet.petStatus],
                        myChar.xu,
                        myChar.luong,
                        myChar.luongKhoa
                    });
                }
                if (text == "add")
                {
                    Mob mob = Char.myCharz().mobFocus;
                    ItemMap itemMap = Char.myCharz().itemFocus;
                    if (mob != null)
                    {
                        if (IdMobsTanSat.Contains(mob.mobId))
                        {
                            IdMobsTanSat.Remove(mob.mobId);
                            GameScr.info1.addInfo("Đã xoá mob: " + mob.mobId, 0);
                        }
                        else
                        {
                            IdMobsTanSat.Add(mob.mobId);
                            GameScr.info1.addInfo("Đã thêm mob: " + mob.mobId, 0);
                        }
                    }
                    else if (itemMap != null)
                    {
                        if (IdItemPicks.Contains(itemMap.template.id))
                        {
                            IdItemPicks.Remove(itemMap.template.id);
                            GameScr.info1.addInfo($"Đã xoá khỏi danh sách chỉ tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                        }
                        else
                        {
                            IdItemPicks.Add(itemMap.template.id);
                            GameScr.info1.addInfo($"Đã thêm vào danh sách chỉ tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                        }
                    }
                    else
                    {
                        GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
                    }
                }
                else if (text == "ado")
                {
                    IsAutoDapDo = !IsAutoDapDo;
                    GameScr.info1.addInfo("Tự động đập đồ: " + (IsAutoDapDo ? "Bật" : "Tắt"), 0);
                }
                else if (text == "ghnt")
                {
                    isGHNT = !isGHNT;
                    GameScr.info1.addInfo("Bo gioi han noi tai: " + (isGHNT ? "Bật" : "Tắt"), 0);
                }
                else if (text == "ndvq")
                {
                    isNDVQ = !isNDVQ;
                    GameScr.info1.addInfo("Tự động nhận đồ vòng quay: " + (isNDVQ ? "Bật" : "Tắt"), 0);
                }
                else if (text == "ghgg")
                {
                    PickMobController.isGohomeGetGold = !PickMobController.isGohomeGetGold;
                    GameScr.info1.addInfo("Tự động nhận về nhà nhận vàng: " + (PickMobController.isGohomeGetGold ? "Bật" : "Tắt"), 0);
                }
                else if (text == "gbgg")
                {
                    PickMobController.isGobackOldmap = !PickMobController.isGobackOldmap;
                    GameScr.info1.addInfo("Tự động quay lại map cũ khi về nhà nhận vàng: " + (PickMobController.isGobackOldmap ? "Bật" : "Tắt"), 0);
                }
                else if (text == "naa")
                {
                    isNVdd = !isNVdd;
                    GameScr.info1.addInfo("Tự động mở: " + (isNVdd ? "Bật" : "Tắt"), 0);
                }
                else if (text == "addt")
                {
                    Mob mob = Char.myCharz().mobFocus;
                    ItemMap itemMap = Char.myCharz().itemFocus;
                    if (mob != null)
                    {
                        if (TypeMobsTanSat.Contains(mob.templateId))
                        {
                            TypeMobsTanSat.Remove(mob.templateId);
                            GameScr.info1.addInfo($"Đã xoá loại mob: {mob.getTemplate().name}[{mob.templateId}]", 0);
                        }
                        else
                        {
                            TypeMobsTanSat.Add(mob.templateId);
                            GameScr.info1.addInfo($"Đã thêm loại mob: {mob.getTemplate().name}[{mob.templateId}]", 0);
                        }
                    }
                    else if (itemMap != null)
                    {
                        if (TypeItemPicks.Contains(itemMap.template.type))
                        {
                            TypeItemPicks.Remove(itemMap.template.type);
                            GameScr.info1.addInfo("Đã xoá khỏi danh sách chỉ tự động nhặt loại item:" + itemMap.template.type, 0);
                        }
                        else
                        {
                            TypeItemPicks.Add(itemMap.template.type);
                            GameScr.info1.addInfo("Đã thêm vào danh sách chỉ tự động nhặt loại item:" + itemMap.template.type, 0);
                        }
                    }
                    else
                    {
                        GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
                    }
                }
                else if (text == "anhat")
                {
                    IsAutoPickItems = !IsAutoPickItems;
                    GameScr.info1.addInfo("Tự động nhặt vật phẩm: " + (IsAutoPickItems ? "Bật" : "Tắt"), 0);
                }
                else if (text == "itm")
                {
                    IsItemMe = !IsItemMe;
                    GameScr.info1.addInfo("Lọc không nhặt vật phẩm của người khác: " + (IsItemMe ? "Bật" : "Tắt"), 0);
                }
                else if (text == "sln")
                {
                    IsLimitTimesPickItem = !IsLimitTimesPickItem;
                    StringBuilder builder = new();
                    builder.Append($"Giới hạn số lần nhặt là ");
                    builder.Append(TimesAutoPickItemMax);
                    builder.Append(IsLimitTimesPickItem ? ": Bật" : ": Tắt");
                    GameScr.info1.addInfo(builder.ToString(), 0);
                }
                else if (IsGetInfoChat<int>(text, "sln"))
                {
                    TimesAutoPickItemMax = GetInfoChat<int>(text, "sln");
                    GameScr.info1.addInfo("Số lần nhặt giới hạn là: " + TimesAutoPickItemMax, 0);
                }
                else if (IsGetInfoChat<string>(text, "chat"))
                {
                    string str = GetInfoChat<string>(text, "chat");
                    File.WriteAllText("Data\\chat.ini", str);
                }
                else if (IsGetInfoChat<int>(text, "gs"))
                {
                    int str = GetInfoChat<int>(text, "gs");
                    str *= 1000000;
                    File.WriteAllText("Data\\goldSell.txt", str.ToString());
                }
                else if (IsGetInfoChat<short>(text, "addi"))
                {
                    short id = GetInfoChat<short>(text, "addi");
                    if (IdItemPicks.Contains(id))
                    {
                        IdItemPicks.Remove(id);
                        GameScr.info1.addInfo($"Đã xoá khỏi danh sách chỉ tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                    }
                    else
                    {
                        IdItemPicks.Add(id);
                        GameScr.info1.addInfo($"Đã thêm vào danh sách chỉ tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                    }
                }
                else if (text == "blocki")
                {
                    ItemMap itemMap = Char.myCharz().itemFocus;
                    if (itemMap != null)
                    {
                        if (IdItemBlocks.Contains(itemMap.template.id))
                        {
                            IdItemBlocks.Remove(itemMap.template.id);
                            GameScr.info1.addInfo($"Đã xoá khỏi danh sách không tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                        }
                        else
                        {
                            IdItemBlocks.Add(itemMap.template.id);
                            GameScr.info1.addInfo($"Đã thêm vào danh sách không tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                        }
                    }
                    else
                    {
                        GameScr.info1.addInfo("Cần trỏ vào vật phẩm cần chặn khi auto nhặt", 0);
                    }
                }
                else if (IsGetInfoChat<short>(text, "blocki"))
                {
                    short id = GetInfoChat<short>(text, "blocki");
                    if (IdItemBlocks.Contains(id))
                    {
                        IdItemBlocks.Remove(id);
                        GameScr.info1.addInfo($"Đã thêm vào danh sách không tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                    }
                    else
                    {
                        IdItemBlocks.Add(id);
                        GameScr.info1.addInfo($"Đã xoá khỏi danh sách không tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                    }
                }
                else if (IsGetInfoChat<sbyte>(text, "addti"))
                {
                    sbyte type = GetInfoChat<sbyte>(text, "addti");
                    if (TypeItemPicks.Contains(type))
                    {
                        TypeItemPicks.Remove(type);
                        GameScr.info1.addInfo("Đã xoá khỏi danh sách chỉ tự động nhặt loại item: " + type, 0);
                    }
                    else
                    {
                        TypeItemPicks.Add(type);
                        GameScr.info1.addInfo("Đã thêm vào danh sách chỉ tự động nhặt loại item: " + type, 0);
                    }
                }
                else if (IsGetInfoChat<sbyte>(text, "blockti"))
                {
                    sbyte type = GetInfoChat<sbyte>(text, "blockti");
                    if (TypeItemBlock.Contains(type))
                    {
                        TypeItemBlock.Remove(type);
                        GameScr.info1.addInfo("Đã xoá khỏi danh sách không tự động nhặt loại item: " + type, 0);
                    }
                    else
                    {
                        TypeItemBlock.Add(type);
                        GameScr.info1.addInfo("Đã thêm vào danh sách không tự động nhặt loại item: " + type, 0);
                    }
                }
                else if (text == "clri")
                {
                    IdItemPicks.Clear();
                    TypeItemPicks.Clear();
                    TypeItemBlock.Clear();
                    IdItemBlocks.Clear();
                    IdItemBlocks.AddRange(IdItemBlockBase);
                    GameScr.info1.addInfo("Danh sách lọc item đã được đặt lại mặc định", 0);
                }
                else if (text == "cnn")
                {
                    IdItemPicks.Clear();
                    TypeItemPicks.Clear();
                    TypeItemBlock.Clear();
                    IdItemBlocks.Clear();
                    IdItemBlocks.AddRange(IdItemBlockBase);
                    IdItemPicks.Add(ID_ITEM_GEM);
                    IdItemPicks.Add(ID_ITEM_GEM_LOCK);
                    GameScr.info1.addInfo("Đã cài đặt chỉ nhặt ngọc", 0);
                }
                else if (text == "ts")
                {
                    IsTanSat = !IsTanSat;
                    if (TypeMobsTanSat.Count > 0 && !IsTanSat) TypeMobsTanSat.Clear();
                    GameScr.info1.addInfo("Tự động đánh quái: " + (IsTanSat ? "Bật" : "Tắt"), 0);
                }
                else if (text == "nsq")
                {
                    IsNeSieuQuai = !IsNeSieuQuai;
                    GameScr.info1.addInfo("Tàn sát né siêu quái: " + (IsNeSieuQuai ? "Bật" : "Tắt"), 0);
                }
                else if (text == "tssq")
                {
                    IsTSSQ = !IsTSSQ;
                    IsNeSieuQuai = false;
                    GameScr.info1.addInfo("Tàn sát siêu quái: " + (IsTSSQ ? "Bật" : "Tắt"), 0);
                }
                else if (IsGetInfoChat<int>(text, "addm"))
                {
                    int id = GetInfoChat<int>(text, "addm");
                    if (IdMobsTanSat.Contains(id))
                    {
                        IdMobsTanSat.Remove(id);
                        GameScr.info1.addInfo("Đã xoá mob: " + id, 0);
                    }
                    else
                    {
                        IdMobsTanSat.Add(id);
                        GameScr.info1.addInfo("Đã thêm mob: " + id, 0);
                    }
                }
                else if (IsGetInfoChat<int>(text, "addtm"))
                {
                    int id = GetInfoChat<int>(text, "addtm");
                    if (TypeMobsTanSat.Contains(id))
                    {
                        TypeMobsTanSat.Remove(id);
                        GameScr.info1.addInfo($"Đã xoá loại mob: {Mob.arrMobTemplate[id].name}[{id}]", 0);
                    }
                    else
                    {
                        TypeMobsTanSat.Add(id);
                        GameScr.info1.addInfo($"Đã thêm loại mob: {Mob.arrMobTemplate[id].name}[{id}]", 0);
                    }
                }
                else if (text == "clrm")
                {
                    IdMobsTanSat.Clear();
                    TypeMobsTanSat.Clear();
                    GameScr.info1.addInfo("Đã xoá danh sách đánh quái", 0);
                }
                else if (text == "sk")
                {
                    SkillTemplate template = Char.myCharz().myskill.template;
                    if (IdSkillsTanSat.Contains(template.id))
                    {
                        IdSkillsTanSat.Remove(template.id);
                        GameScr.info1.addInfo($"Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                    }
                    else
                    {
                        IdSkillsTanSat.Add(template.id);
                        GameScr.info1.addInfo($"Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                    }
                }
                else if (IsGetInfoChat<int>(text, "sk"))
                {
                    int index = GetInfoChat<int>(text, "sk") - 1;
                    SkillTemplate template = Char.myCharz().nClass.skillTemplates[index];
                    if (IdSkillsTanSat.Contains(template.id))
                    {
                        IdSkillsTanSat.Remove(template.id);
                        GameScr.info1.addInfo($"Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                    }
                    else
                    {
                        IdSkillsTanSat.Add(template.id);
                        GameScr.info1.addInfo($"Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                    }
                }
                else if (IsGetInfoChat<sbyte>(text, "skid"))
                {
                    sbyte id = GetInfoChat<sbyte>(text, "skid");
                    if (IdSkillsTanSat.Contains(id))
                    {
                        IdSkillsTanSat.Remove(id);
                        GameScr.info1.addInfo("Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: " + id, 0);
                    }
                    else
                    {
                        IdSkillsTanSat.Add(id);
                        GameScr.info1.addInfo("Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: " + id, 0);
                    }
                }
                else if (text == "clrs")
                {
                    IdSkillsTanSat.Clear();
                    IdSkillsTanSat.AddRange(IdSkillsBase);
                    GameScr.info1.addInfo("Đã đặt danh sách skill sử dụng tự động đánh quái về mặc định", 0);
                }
                else if (text == "abf")
                {
                    if (HpBuff == 0 && MpBuff == 0)
                    {
                        GameScr.info1.addInfo("Tự động sử dụng đậu thần: Tắt", 0);
                    }
                    else
                    {
                        HpBuff = DEFAULT_HP_BUFF;
                        MpBuff = DEFAULT_MP_BUFF;
                        GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%, MP dưới {MpBuff}%", 0);
                    }
                }
                else if (IsGetInfoChat<int>(text, "abf"))
                {
                    HpBuff = GetInfoChat<int>(text, "abf");
                    MpBuff = 0;
                    GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%", 0);
                }
                else if (IsGetInfoChat<int>(text, "abf", 2))
                {
                    int[] vs = GetInfoChat<int>(text, "abf", 2);
                    HpBuff = vs[0];
                    MpBuff = vs[1];
                    GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%, MP dưới {MpBuff}%", 0);
                }
                // ban vang
                else if(text == "bv")
                {
                    PickMobController.isBanVang = false;
                    GameScr.info1.addInfo("Auto bán thỏi vàng dừng", 0);
                }
                else if (IsGetInfoChat<int>(text, "bv"))
                {
                    PickMobController.solanSale = GetInfoChat<int>(text, "bv");
                    PickMobController.lastTimeSaleGold = mSystem.currentTimeMillis();
                    PickMobController.isBanVang = true;
                    GameScr.info1.addInfo($"Auto bán {PickMobController.solanSale} thỏi vàng", 0);
                }
                else if (IsGetInfoChat<int>(text, "bv", 2))
                {
                    int[] vs = GetInfoChat<int>(text, "bv", 2);
                    PickMobController.solanSale = vs[0];
                    PickMobController.timeSaleGold = vs[1];
                    PickMobController.lastTimeSaleGold = mSystem.currentTimeMillis();
                    PickMobController.isBanVang = true;
                    GameScr.info1.addInfo($"Auto bán {PickMobController.solanSale} thỏi vàng delay mỗi lần bán {PickMobController.timeSaleGold}ms", 0);
                }
                else if (text == "vdh")
                {
                    IsVuotDiaHinh = !IsVuotDiaHinh;
                    GameScr.info1.addInfo("Tự động đánh quái vượt địa hình: " + (IsVuotDiaHinh ? "Bật" : "Tắt"), 0);
                }
                else if (text == "mktb")
                {
                    AutoUpGrade.isMKTB = !AutoUpGrade.isMKTB;
                    GameScr.info1.addInfo("Tự động mở khoá trang bị: " + (AutoUpGrade.isMKTB ? "Bật" : "Tắt"), 0);
                }
                //else if (IsGetInfoChat<int>(text, "k"))
                //{
                //    // Lấy số nguyên sau lệnh chat kz (khu cần chuyển)
                //    int zone = GetInfoChat<int>(text, "k");
                //    Service.gI().requestChangeZone(zone, -1);

                //}
                else if (text.Equals("odt"))
                {
                    // id NPC Ca Lích là 38
                    Service.gI().openMenu(25);
                    Service.gI().confirmMenu(4, 0);
                }
                else if (IsGetInfoChat<int>(text, "npc"))
                {
                    int idNpc = GetInfoChat<int>(text, "npc");
                    Npc npc = GameScr.findNPCInMap((short)idNpc);
                    if (npc != null) XmapController.MoveMyChar(npc.cx, npc.cy);
                    Service.gI().openMenu(idNpc);

                }
                // kiểm tra số thực sau lệnh chat spd
                else if (IsGetInfoChat<float>(text, "td"))
                {
                    speed = GetInfoChat<float>(text, "td");
                }
                else if (IsGetInfoChat<float>(text, "tele"))
                {
                    string id = GetInfoChat<string>(text, "tele");
                    PickMobController.TeleToPlayer(id);
                }
                //giao dich
                else if (text == "trade")
                {
                    int id = global::Char.myCharz().charFocus.charID;
                    string name = global::Char.myCharz().charFocus.cName;
                    Service.gI().giaodich(0, id, -1, -1);
                    GameScr.info1.addInfo($"Đã mời {name} [Id:{id}] giao dịch", 0);
                }
                //hop the
                else if (text == "ht1")
                {
                    UseItem.usePorataAndPetFollow();

                }
                else if (text == "ht3")
                {
                    UseItem.usePorataAndPetAttack();

                }
                else if (text == "ht2")
                {
                    UseItem.usePorataAndPetDefense();

                }
                //menu
                else if (text == "menu")
                {
                    TabMenu.gI().setTypeMenuMod();
                    GameCanvas.panel.show();
                }
                //auto up de
                else if (text == "adt")
                {
                    IsUpDe = !IsUpDe;
                    GameScr.info1.addInfo("Auto up đệ tử: " + (IsUpDe ? "Bật" : "Tắt"), 0);
                }
                //auto goi rong
                else if (text == "nr")
                {
                    IsNR = !IsNR;
                    PickMobController.goiRong();
                    GameScr.info1.addInfo("Auto gọi rồng thần: " + (IsNR ? "Bật" : "Tắt"), 0);
                }
                else if (text == "dbv")
                {
                    AutoUpGrade.isUseDBV = !AutoUpGrade.isUseDBV;
                    GameScr.info1.addInfo("Sử dụng đá bảo vệ: " + (AutoUpGrade.isUseDBV ? "Bật" : "Tắt"), 0);
                }
                // khau trang
                else if (text == "ukt")
                {
                    UseItem.UseKhauTrang();
                }
                else if (text == "akt")
                {
                    IsKhauTrang = !IsKhauTrang;
                    new Thread(new ThreadStart(UseItem.AutoUseKhauTrang)).Start();
                    GameScr.info1.addInfo("Auto sử dụng khẩu trang: " + (IsACN ? "Bật" : "Tắt"), 0);
                }
                // su dung cuong no
                else if (text == "ucn")
                {
                    UseItem.useCN();
                }
                else if (text == "acn")
                {
                    IsACN = !IsACN;
                    new Thread(new ThreadStart(UseItem.AutoUseCN)).Start();
                    GameScr.info1.addInfo("Auto sử dụng cuồng nộ: " + (IsACN ? "Bật" : "Tắt"), 0);
                }
                // su dung giap xen
                else if (text == "ugx")
                {
                    UseItem.useGX();
                }
                else if (text == "agx")
                {
                    IsAGX = !IsAGX;
                    new Thread(new ThreadStart(UseItem.AutoUseGX)).Start();
                    GameScr.info1.addInfo("Auto sử dụng giáp xên: " + (IsAGX ? "Bật" : "Tắt"), 0);
                }
                // su dung bo khi
                else if (text == "ubk")
                {
                    UseItem.useBK();
                }
                else if (text == "abk")
                {
                    IsABK = !IsABK;
                    new Thread(new ThreadStart(UseItem.AutoUseBK)).Start();
                    GameScr.info1.addInfo("Auto sử dụng bổ khí: " + (IsABK ? "Bật" : "Tắt"), 0);
                }
                else if (text == "ubh")
                {
                    UseItem.useBH();
                }
                else if (text == "abh")
                {
                    IsABH = !IsABH;
                    new Thread(new ThreadStart(UseItem.AutoUseBH)).Start();
                    GameScr.info1.addInfo("Auto sử dụng bổ khí: " + (IsABH ? "Bật" : "Tắt"), 0);
                }
                //else if (text == "uitem")
                //{
                //    UseItem.useItem(text);
                //}
                //auto login
                else if (text == "alogin")
                {
                    IsAutoLogin = !IsAutoLogin;
                    GameScr.info1.addInfo("Tự động đăng nhập sau 25s: " + (IsAutoLogin ? "Bật" : "Tắt"), 0);
                }
                //mini map
                else if (text == "mmap")
                {
                    IsminiMap = !IsminiMap;
                    GameScr.info1.addInfo("Mini Map: " + (IsminiMap ? "Bật" : "Tắt"), 0);
                }
                else if (IsGetInfoChat<int>(text, "s"))
                {
                    tdc = GetInfoChat<int>(text, "s");
                    GameScr.info1.addInfo("Tốc độ chạy: " + tdc, 0);
                }
                else if (IsGetInfoChat<int>(text, "td"))
                {
                    speed = GetInfoChat<int>(text, "td");
                    GameScr.info1.addInfo("Tốc độ game: " + speed, 0);
                }
                //san boss
                else if (text == "vb")
                {
                    OnScreen.viewBoss = !OnScreen.viewBoss;
                    GameScr.info1.addInfo($"Hiển thị boss: " + (OnScreen.viewBoss ? "Bật" : "Tắt"), 0);
                }
                else if (text == "sb")
                {
                    PickMobController.sanboss = !PickMobController.sanboss;
                    //if (PickMobController.sanboss == false && PickMobController.canAutoPlay == true)
                    //{
                    //    Service.gI().useItem(0, 1, -1, 521);
                    //}
                    int sb1;
                    if (!PickMobController.sanboss)
                    {
                        PickMobController.listOldZone.Clear();
                        sb1 = Int32.Parse(File.ReadAllText(PickMobController.BossIni));
                        sb1--;
                        File.WriteAllText(PickMobController.BossIni, sb1.ToString());
                        if (File.Exists(PickMobController.FileBoss) && sb1 <= 0)
                        {
                            File.Delete(PickMobController.FileBoss);
                        }
                        if (File.Exists(PickMobController.BossIni) && sb1 <= 0)
                        {
                            File.Create(PickMobController.BossIni).Close();
                            File.WriteAllText(PickMobController.BossIni, PickMobController.sbDef.ToString());
                        }
                    }
                    GameScr.info1.addInfo("Săn boss: " + (PickMobController.sanboss ? "Bật" : "Tắt"), 0);
                    text = "";
                    //PickMobController.khusb = TileMap.zoneID;
                    new Thread(new ThreadStart(PickMobController.SanBoss)).Start();
                }
                else if (text == "testb")
                {
                    PickMobController.testBoss = !PickMobController.testBoss;
                    //if (PickMobController.sanboss == false && PickMobController.canAutoPlay == true)
                    //{
                    //    Service.gI().useItem(0, 1, -1, 521);
                    //}
                    int sb1;
                    if (!PickMobController.testBoss)
                    {
                        PickMobController.listOldZone.Clear();
                        sb1 = Int32.Parse(File.ReadAllText(PickMobController.BossIni));
                        sb1--;
                        File.WriteAllText(PickMobController.BossIni, sb1.ToString());

                        if (File.Exists(PickMobController.FileBoss) && sb1 <= 0)
                        {
                            File.Delete(PickMobController.FileBoss);
                        }
                        if (File.Exists(PickMobController.BossIni) && sb1 <= 0)
                        {
                            File.Create(PickMobController.BossIni).Close();
                            File.WriteAllText(PickMobController.BossIni, PickMobController.sbDef.ToString());
                        }
                    }
                    GameScr.info1.addInfo("Test săn boss: " + (PickMobController.testBoss ? "Bật" : "Tắt"), 0);
                    text = "";
                    //PickMobController.khusb = TileMap.zoneID;
                    new Thread(new ThreadStart(PickMobController.SanBoss)).Start();
                }
                //up cskb
                else if (text == "upcskb")
                {
                    UseItem.upCSKB = !UseItem.upCSKB;
                    new Thread(new ThreadStart(UseItem.AutoUseMayDoCSKB)).Start();
                    GameScr.info1.addInfo("Auto up cskb: " + (UseItem.upCSKB ? "Bật" : "Tắt"), 0);
                }
                //auto tdlt
                else if (text == "tdlt")
                {
                    if (UseItem.findItem(521) != -1)
                    {
                        Service.gI().useItem(0, 1, UseItem.findItem(521), -1);
                    }
                    else
                    {
                        GameScr.info1.addInfo("Không tìm thấy tự động luyện tập", 0);
                    }
                }
                //xindau
                else if (text == "xd")
                {
                    IsXinDau = !IsXinDau;
                    new Thread(new ThreadStart(PickMobController.xinDau)).Start();
                    GameScr.info1.addInfo($"Xin đậu" + (IsXinDau ? " Bật" : " Tắt"), 0);
                }
                //chodau
                else if (text == "cd")
                {
                    IsChoDau = !IsChoDau;
                    new Thread(new ThreadStart(PickMobController.choDau)).Start();
                    GameScr.info1.addInfo($"Cho đậu" + (IsChoDau ? " Bật" : " Tắt"), 0);
                }
                else if (text == "minfo")
                {
                    IshideNShowKhuNMap = !IshideNShowKhuNMap;
                    GameScr.info1.addInfo($"Hiện thị thông tin map" + (IshideNShowKhuNMap ? " Bật" : " Tắt"), 0);
                }
                // mở khu nhanh
                else if (text == "zone")
                {
                    if (TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23)
                    {
                        Service.gI().openUIZone();
                        GameCanvas.panel.setTypeZone();
                        GameCanvas.panel.show();
                    }

                }
                else if (text == "ukhu")
                {
                    ukhu = !ukhu;

                    GameScr.info1.addInfo($"Cập nhật khu liên tục: " + (ukhu ? " Bật" : " Tắt"), 0);
                }
                else if (text == "focus")
                {
                    //isCharAutoFocus = !isCharAutoFocus;
                    charAutoFocus();
                }
                else if (text == "hidep")
                {
                    isHideP = !isHideP;
                    GameScr.info1.addInfo($"Ẩn người chơi: " + (isHideP ? " Bật" : " Tắt"), 0);
                }
                else if (text == "cpuf")
                {
                    if (IsStart)
                        Application.runInBackground = !Application.runInBackground;
                }
                else if (text == "atc")
                {
                    IsAutoChat = !IsAutoChat;
                    new Thread(new ThreadStart(AutoChat)).Start();
                    GameScr.info1.addInfo($"Auto chat: " + (IsAutoChat ? " Bật" : " Tắt"), 0);
                }
                else if (text == "focusboss")
                {
                    IsAutoFocusBoss = !IsAutoFocusBoss;
                    AutoFocusBoss();
                }
                else if (text == "tsboss")
                {
                    IsTanSatBoss = !IsTanSatBoss;
                    GameScr.info1.addInfo($"Auto tàn sát boss: " + (IsTanSatBoss ? " Bật" : " Tắt"), 0);
                }
                else if (text == "tsng")
                {
                    IsTanSatNguoi = !IsTanSatNguoi;
                    GameScr.info1.addInfo($"Auto tàn sát người: " + (IsTanSatNguoi ? " Bật" : " Tắt"), 0);
                }
                else if (text == "friend")
                {
                    Service.gI().friend(0, 1);
                }
                else if (text == "autoneboss")
                {
                    IsAutoNeBoss = !IsAutoNeBoss;
                    new Thread(new ThreadStart(AutoNeBoss)).Start();
                    GameScr.info1.addInfo($"Auto né boss: " + (IsAutoNeBoss ? " Bật" : " Tắt"), 0);
                }
                else if (text == "atf")
                {
                    IsAutoFlag = !IsAutoFlag;
                    new Thread(new ThreadStart(AutoFlag)).Start();
                    GameScr.info1.addInfo($"Auto cờ: " + (IsAutoFlag ? " Bật" : " Tắt"), 0);
                }
                else if (text == "hsdt")
                {
                    IsAutoHSDeTu = !IsAutoHSDeTu;
                    GameScr.info1.addInfo($"Auto hồi sinh đệ tử: " + (IsAutoHSDeTu ? " Bật" : " Tắt"), 0);
                }
                else if (IsGetInfoChat<int>(text, "f"))
                {
                    int flag = GetInfoChat<int>(text, "f");
                    Service.gI().getFlag(1, (sbyte)flag);
                }
                else if (text == "set1")
                {

                    new Thread(PickMobController.MacSet1).Start();
                    GameScr.info1.addInfo($"Đã mặc set 1", 0);
                }
                //else if (text == "xoaset1")
                //{

                //    PickMobController.XoaSetDo1();
                //    GameScr.info1.addInfo($"Đã xoá set đồ 1", 0);
                //}
                else if (text == "set2")
                {

                    new Thread(PickMobController.MacSet2).Start();
                    GameScr.info1.addInfo($"Đã mặc set 2", 0);
                }
                //else if (text == "xoaset2")
                //{

                //    PickMobController.XoaSetDo2();
                //    GameScr.info1.addInfo($"Đã xoá set đồ 2", 0);
                //}
                else if (text == "thudau")
                {

                    IsAutoThuDau = !IsAutoThuDau;
                    GameScr.info1.addInfo($"Auto thu đậu: " + (IsAutoThuDau ? " Bật" : " Tắt"), 0);
                }
                else if (text == "duiga")
                {

                    IsAnDuiGa = !IsAnDuiGa;
                    GameScr.info1.addInfo($"Auto ăn đùi gà: " + (IsAnDuiGa ? " Bật" : " Tắt"), 0);
                }
                else if (text == "adt")
                {
                    IsDT = !IsDT;
                    new Thread(new ThreadStart(autodt)).Start();
                    GameScr.info1.addInfo($"Auto dt: " + (IsDT ? " Bật" : " Tắt"), 0);
                }
                else if (text == "ndt")
                {
                    IsNhatDoUpDT = !IsNhatDoUpDT;
                    if (IsNhatDoUpDT)
                    {
                        PickMobController.XNhat = Char.myCharz().cx;
                        PickMobController.YNhat = Char.myCharz().cy;
                    }

                    GameScr.info1.addInfo($"Nhặt đồ up đệ: " + (IsNhatDoUpDT ? " Bật" : " Tắt"), 0);
                }
                else if (text == "out")
                {
                    Controller.isDisconnected = true;
                }
                else if (text == "revive")
                {
                    Service.gI().wakeUpFromDead();
                }
                else if (text == "home")
                {
                    if (Char.myCharz().statusMe == 14 || Char.myCharz().cHP <= 0)
                        Service.gI().returnTownFromDead();
                    else
                    {
                        if (Char.myCharz().nClass.classId == 0)
                            Xmap.XmapController.StartRunToMapId(21);
                        if (Char.myCharz().nClass.classId == 1)
                            Xmap.XmapController.StartRunToMapId(22);
                        if (Char.myCharz().nClass.classId == 2)
                            Xmap.XmapController.StartRunToMapId(23);
                    }
                }
                else if(text == "tke")
                {
                    AutoGa.toggleGa();
                }
                else if (text == "ahs")
                {
                    IsRevive = !IsRevive;
                    GameScr.info1.addInfo($"Auto hồi sinh: " + (IsRevive ? " Bật" : " Tắt"), 0);
                }
                else if (text == "acskb")
                {
                    IsAutoCSKB = !IsAutoCSKB;
                    new Thread(new ThreadStart(PickMobController.AutoCSKB)).Start();
                    GameScr.info1.addInfo($"Auto mở cskb: " + (IsAutoCSKB ? " Bật" : " Tắt"), 0);
                }
                else if (text == "akok")
                {
                    IsAKOK = !IsAKOK;
                    new Thread(new ThreadStart(PickMobController.AutoKOK)).Start();
                    GameScr.info1.addInfo($"Auto kok: " + (IsAKOK ? " Bật" : " Tắt"), 0);
                }
                else if (text == "ps")
                {
                    PickMobController.isUpPetNroStar = !PickMobController.isUpPetNroStar;
                    PickMobController.lastTimeChangeZoneU = mSystem.currentTimeMillis();
                    GameScr.info1.addInfo("Auto up đệ Star: " + (PickMobController.isUpPetNroStar ? " Bật" : " Tắt"), 0);
                }
                else if (text == "xoapaint")
                {
                    IsPaint = !IsPaint;
                }
                else if (text == "agd")
                {
                    IsAcGiaoDich = !IsAcGiaoDich;
                    GameScr.info1.addInfo($"Auto giao dịch" + (IsAcGiaoDich ? " Bật" : " Tắt"), 0);
                }
                else if (text == "kmap")
                {
                    IsKhoaMap = !IsKhoaMap;
                    GameScr.info1.addInfo($"Khoá map: " + (IsKhoaMap ? " Bật" : " Tắt"), 0);
                }
                else if (text == "kkhu")
                {
                    IsKhoaKhu = !IsKhoaKhu;
                    GameScr.info1.addInfo($"Khoá khu: " + (IsKhoaKhu ? " Bật" : " Tắt"), 0);
                }
                else if (text == "aenter")
                {
                    IsAEnter = !IsAEnter;
                    new Thread(new ThreadStart(PickMobController.AutoEnter)).Start();
                    GameScr.info1.addInfo($"Auto enter: " + (IsAEnter ? " Bật" : " Tắt"), 0);
                }

                else if (text == "abdkb")
                {
                    IsABDKB = !IsABDKB;
                    GameScr.info1.addInfo($"Auto BDKB: " + (IsABDKB ? " Bật" : " Tắt"), 0);
                }
                else if (text == "tusat")
                {
                    new Thread(new ThreadStart(PickMobController.TuSat)).Start();
                    GameScr.info1.addInfo($"Đang chết ...", 0);
                }
                else if (IsGetInfoChat<int>(text, "tuhs"))
                {
                    IsTuHS = !IsTuHS;
                    PickMob.TuHSTime = GetInfoChat<int>(text, "tuhs");
                    new Thread(new ThreadStart(PickMobController.TuHS)).Start();
                    GameScr.info1.addInfo($"Auto tự hs trong {TuHSTime}: " + (IsTuHS ? " Bật" : " Tắt"), 0);
                }
                else if (text == "tuhs")
                {
                    IsTuHS = !IsTuHS;
                    new Thread(new ThreadStart(PickMobController.TuHS)).Start();
                    GameScr.info1.addInfo($"Đang tự hs", 0);
                }
                else if (text == "kvt")
                {
                    kvt = !kvt;
                    GameScr.info1.addInfo($"Khoá vị trí" + (kvt ? " Bật" : " Tắt"), 0);
                }
                else if (text == "8sk")
                {
                    GameScr.xskill = true;
                    GameScr.info1.addInfo($"Load lại 8 skill", 0);
                }
                else if (text == "enter")
                {
                    if (!isEnter)
                    {
                        GameCanvas.panel.hideNow();
                    }
                    isEnter = !isEnter;
                    GameScr.info1.addInfo($"Auto Enter" + (isEnter ? " Bật" : " Tắt"), 0);
                }
                else if (text == "ccs")
                {
                    IsCCS = !IsCCS;
                    new Thread(new ThreadStart(PickMobController.AutoCS)).Start();
                    GameScr.info1.addInfo($"Auto cộng chỉ số" + (IsCCS ? " Bật" : " Tắt"), 0);
                }
                else if (text == "mbv")
                {
                    IsPhaMBV = !IsPhaMBV;
                    if (!IsPhaMBV)
                    {
                        File.Delete("Data\\duy.txt");
                    }
                    new Thread(new ThreadStart(PickMobController.AutoPhaMaBV)).Start();
                    GameScr.info1.addInfo($"Auto phá mã bảo vệ" + (IsPhaMBV ? " Bật" : " Tắt"), 0);
                }
                else if (IsGetInfoChat<int>(text, "mbv"))
                {
                    IsPhaMBV = !IsPhaMBV;
                    if (!IsPhaMBV)
                    {
                        File.Delete("Data\\duy.txt");
                    }
                    MBV = GetInfoChat<int>(text, "mbv");
                    new Thread(new ThreadStart(PickMobController.AutoPhaMaBV)).Start();
                    GameScr.info1.addInfo($"Auto phá mã bảo vệ" + (IsPhaMBV ? " Bật" : " Tắt"), 0);
                }
                else if (text == "xindauv")
                {
                    isXinDauV = !isXinDauV;
                    GameScr.info1.addInfo($"Xin đậu vip" + (isXinDauV ? " Bật" : " Tắt"), 0);
                    try
                    {
                        Service.gI().clanMessage(1, "", -1);
                    }
                    catch
                    {

                    }
                    finally
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                        {
                            Thread.Sleep(3000);
                            PickMob.Chat("out");
                        }));
                    }
                }
                else if (IsGetInfoChat<int>(text, "hp"))
                {
                    int hp = GetInfoChat<int>(text, "hp");
                    File.Create(PickMobController.FileHPBoss).Close();
                    File.WriteAllText(PickMobController.FileHPBoss, hp.ToString());
                    GameScr.info1.addInfo($"Auto tự sát boss khi hp dưới " + hp, 0);
                }
                else if (text == "atsb")
                {
                    IsAutoTSBoss = !IsAutoTSBoss;
                    //new Thread(new ThreadStart(PickMobController.AutoTSBoss)).Start();
                    GameScr.info1.addInfo($"Auto tự sát Boss" + (IsAutoTSBoss ? " Bật" : " Tắt"), 0);
                }
                else if (text == "ask3")
                {
                    IsSkill3 = !IsSkill3;
                    //new Thread(new ThreadStart(PickMobController.AutoSkill3)).Start();
                    GameScr.info1.addInfo($"Auto skill 3" + (IsSkill3 ? " Bật" : " Tắt"), 0);
                }
                else if (text == "ask5")
                {
                    IsSkill5 = !IsSkill5;
                    //new Thread(new ThreadStart(PickMobController.AutoSkill3)).Start();
                    GameScr.info1.addInfo($"Auto skill 5" + (IsSkill5 ? " Bật" : " Tắt"), 0);
                }
                //else if (IsGetInfoChat<int>(text, "ask3"))
                //{
                //    IsSkill3 = !IsSkill3;
                //    PickMobController.timeSkill3 = GetInfoChat<int>(text, "askill3");
                //    new Thread(new ThreadStart(PickMobController.AutoSkill3)).Start();

                //    GameScr.info1.addInfo($"Auto skill 3 trong " + PickMobController.timeSkill3 + (IsSkill3 ? " Bật" : " Tắt"), 0);
                //}
                else if (IsGetInfoChat<int>(text, "z"))
                {
                    IsDoKhu = !IsDoKhu;
                    int zones = GetInfoChat<int>(text, "z");
                    new Thread(new ThreadStart(PickMobController.AutoDoKhuBoss)).Start();
                    PickMobController.khudo = zones;
                    PickMobController.khuhientai = TileMap.zoneID;
                    GameScr.info1.addInfo($"Auto dò từ khu " + zones + " đến khu " + (zones + 9) + (IsDoKhu ? " Bật" : " Tắt"), 0);
                }
                else if (IsGetInfoChat<string>(text, "idc"))
                {
                    idC = GetInfoChat<string>(text, "idc");

                    GameScr.info1.addInfo($"Đã đổi id thành " + idC, 0);
                }
                else if (text == "ak")
                {
                    IsAK = !IsAK;
                    GameScr.info1.addInfo($"AK" + (IsAK ? " Bật" : " Tắt"), 0);
                }
                else if (text == "as3")
                {
                    IsAutoSkill3HPMP = !IsAutoSkill3HPMP;
                    GameScr.info1.addInfo($"Auto theo hp và mp: " + (IsAutoSkill3HPMP ? " Bật" : " Tắt"), 0);
                }
                else if (text == "akhu")
                {
                    tlkhu = true;
                    GameScr.info1.addInfo("Trở lại " + TileMap.mapName + " [" + TileMap.mapID + "] Khu: " + TileMap.zoneID + " khi mất kết nối", 0);
                    oldKhu = TileMap.zoneID;
                    amap = TileMap.mapName;
                }
                else if (text == "hakhu")
                {
                    tlkhu = false;
                    GameScr.info1.addInfo("Auto trở lại khu tắt", 0);
                }
                else if (text == "uskh")
                {
                    IsNhatSKH = !IsNhatSKH;
                    GameScr.info1.addInfo("Nhặt set kích hoạt: " + (IsNhatSKH ? "Bật" : "Tắt"), 0);
                }
                else if (text == "dv")
                {
                    PickMobController.isDoiVang = !PickMobController.isDoiVang;
                    GameScr.info1.addInfo("Auto doi vang: " + (PickMobController.isDoiVang ? "Bật" : "Tắt"), 0);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsNhatSKH = false;
        #endregion
        public static bool tlkhu = false;
        public static bool z5;
        public static string amap = string.Empty;
        // cập nhật khu
        public static void tdlt()
        {
            //Chat("sk8");
            if(Char.myCharz().nClass.classId == 2)
            {
                Chat("sk4");
            }
            if(Char.myCharz().nClass.classId == 1)
            {
                Chat("sk5");
            }
        }

        public static bool HotKeys()
        {
            switch (GameCanvas.keyAsciiPress)
            {
                case 't':
                    Chat("/ts");
                    tdlt();
                    break;
                case 'b':
                    Chat("/sb");
                    break;
                case 'n':
                    Chat("/anhat");
                    break;
                //case 'a':
                //    Chat("add");
                //    break;
                case 'h':
                    Chat("/ht3");
                    break;
                case 'v':
                    Chat("/menu");
                    break;
                case 'f':
                    Chat("/ht1");
                    break;
                case 'g':
                    Chat("/trade");
                    break;
                case 'm':
                    Chat("/zone");
                    break;
                case 'w':
                    Chat("/focus");
                    break;
                case 'e':
                    Chat("/focusboss");
                    break;
                case 'y':
                    Chat("/f8");
                    break;
                case 'u':
                    Chat("/f0");
                    break;
                //case '0':
                //    Chat("/xoapaint");
                //    break;
                //case '9':
                //    Chat("tusat");
                //    break;
                case 'z':
                    MyMenu.gI().loadMenu();
                    break;
                case 's':
                    Chat("/tdlt");
                    break;
                case 'o':
                    Chat("/tsboss");
                    break;
                //case 'd':
                //    Chat("/nr");
                //    break;
                //case 'p':
                //    Chat("/aenter");
                //    break;
                default:
                    return false;
            }
            return true;
        }
        public static int oldKhu = -1;
        public static void GameScrUpdate()
        {
            PickMob.UpdateNhatDoDT();
            //PickMob.AnDuiGa();
            PickMob.updateHideP();
            //PickMobController.AutoFocus();
            PickMob.UpdateAutoThudau();
            PickMob.updateZone();
            //PickMob.updateGD();
            UpdateAutoBDKB();
            updateDoanhTrai();
            //updateSkill3();
            updateKVT();
            updateAutoEnter();
            AutoFlag();
            AK();
            AS3();
        }
        public static void autodt()
        {
            PickMobController.autodt();
        }

        // auto tắt cờ
        public static void AutoFlag()
        {
            PickMobController.AutoFlag();
        }
        // focuss boss
        public static void AutoFocusBoss()
        {
            PickMobController.AutoFocusBoss();
        }

        // auto chat
        public static void AutoChat()
        {
            while (IsAutoChat)
            {
                string text = File.ReadAllText("Data\\chat.ini");
                Service.gI().chat(text);
                Thread.Sleep(2000);
            }
        }
        //focus char
        public static void charAutoFocus()
        {
            PickMobController.AutoFocus();
        }
        // tốc độ chạy
        public static void startOkDlg(string info)
        {
            PickMobController.startOKDlg(info);
        }
        public static void autoLogin()
        {
            if (IsAutoLogin)
            {
                new Thread(new ThreadStart(PickMobController.autoLogin)).Start();
            }

        }
        public static void khuAutoLogin()
        {
            if (tlkhu)
            {
                new Thread(new ThreadStart(PickMobController.KhuAutoLogin)).Start();
            }
        }
        public static void Update()
        {
            PickMobController.Update();
        }
        // auto né boss
        public static void AutoNeBoss()
        {
            PickMobController.AutoNeBoss();
        }
        // sao sư phụ không đánh
        public static void findMobForPet(string info)
        {
            if (info.ToLower().Contains("sao sư phụ không đánh"))
            {
                PickMobController.findMobForPet();
            }
        }
        public static void buffDauDT(string info)
        {
            if (info.ToLower().Contains("sư phụ cho con đậu thần"))
            {
                GameScr.gI().doUseHP();
            }
        }
        //vector mặc đồ
        public static void vectorUseEqui()
        {
            PickMobController.vectorUseEqui();
        }

        public static void performMacSet(int idAction)
        {
            PickMobController.actionPerFormSetDo(idAction);
        }

        
        #region gamescr update
        public static void updateAutoEnter()
        {
            if(isEnter && GameCanvas.gameTick % (20 * (int)Time.timeScale) == 0)
            {
                PickMobController.AutoEnter();
            }
            
        }
        public static void AS3()
        {
            if (GameCanvas.gameTick % 20 == 0)
            {
                PickMobController.ASkill3HPMP();
            }
            
        }
        public static void AK()
        {
            if(IsAK && GameCanvas.gameTick %20 == 0)
            {
                PickMobController.AK();
            }
            
        }
        public static void updateDoanhTrai()
        {
            if(IsDT && GameCanvas.gameTick % (20 * (int)Time.timeScale) == 0)
            {
                autodt();
            }
        }
        //skill 3
        public static void updateSkill3()
        {
            PickMobController.AutoSkill3();
        }
        //kvt
        public static void updateKVT() 
        {
            PickMobController.KhoaViTri();
        }
        //xindau
        public static void updateXinDau()
        {
            PickMobController.xinDau();
        }
        // update khu
        public static void updateZone()
        {
            if (ukhu && GameCanvas.gameTick % (30 * (int)Time.timeScale) == 0)
            {
                PickMobController.updateZone();
            }
        }
        //ẩn người
        public static void updateHideP()
        {
            if (isHideP)
            {
                GameScr.vCharInMap.removeAllElements();
            }

        }
        // update nhat do de tu
        public static void UpdateNhatDoDT()
        {
            if (IsNhatDoUpDT && GameCanvas.gameTick % (20 * (int)Time.timeScale) == 0)
            {
                new Thread(new ThreadStart(PickMobController.NhatDoUpDT)).Start();
            }
        }
        // update thu dau
        public static void UpdateAutoThudau()
        {
            if (IsAutoThuDau && GameCanvas.gameTick % (20 * (int)Time.timeScale) == 0)
            {
                PickMobController.thuDau();
            }
        }
        // auto ăn đùi gà
        public static void AnDuiGa()
        {
            PickMobController.autoAnDuiGa();
        }
        #endregion
        public static void MobStartDie(object obj)
        {
            Mob mob = (Mob)obj;
            if (mob.status != 1 && mob.status != 0)
            {
                mob.timeLastDie = mSystem.currentTimeMillis();
                mob.countDie++;
                if (mob.countDie > 10)
                    mob.countDie = 0;
            }
        }

        public static void UpdateCountDieMob(Mob mob)
        {
            if (mob.levelBoss != 0)
                mob.countDie = 0;
        }
        public static void UpdateAutoBDKB()
        {
            if (PickMob.IsABDKB)
            {
                PickMobController.AutoBDKB();
            }
        }

        #region Không cần liên kết với game
        private static bool IsGetInfoChat<T>(string text, string s)
        {
            if (!text.StartsWith(s))
            {
                return false;
            }
            try
            {
                Convert.ChangeType(text.Substring(s.Length), typeof(T));
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static T GetInfoChat<T>(string text, string s)
        {
            return (T)Convert.ChangeType(text.Substring(s.Length), typeof(T));
        }

        private static bool IsGetInfoChat<T>(string text, string s, int n)
        {
            if (!text.StartsWith(s))
            {
                return false;
            }
            try
            {
                string[] vs = text.Substring(s.Length).Split(' ');
                for (int i = 0; i < n; i++)
                {
                    Convert.ChangeType(vs[i], typeof(T));
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static T[] GetInfoChat<T>(string text, string s, int n)
        {
            T[] ts = new T[n];
            string[] vs = text.Substring(s.Length).Split(' ');
            for (int i = 0; i < n; i++)
            {
                ts[i] = (T)Convert.ChangeType(vs[i], typeof(T));
            }
            return ts;
        }
        #endregion
    }

}