using Mod.PickMob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.Auto
{
    internal class StopAuto
    {
        public static void update()
        {
            if (Input.GetKey((KeyCode)120))
            {
                if(AutoGa.isAutoGaEnabled)
                {
                    AutoGa.isAutoGaEnabled = false;
                    AutoGa.isAutoGa = false;
                    GameScr.info1.addInfo("Đã Dừng", 0);
                }
                if (AutoApTrung.gI.IsActing)
                {
                    AutoApTrung.gI.toggle(false);
                    AutoApTrung.isApLinhThu = false;
                    AutoApTrung.isApPet = false;
                    GameScr.info1.addInfo("Đã Dừng", 0);
                }
                if(AutoUpgrade.isUpgrade)
                {
                    AutoUpgrade.toggle(false);
                    if (AutoUpgrade.isNCGTS)
                    {
                        AutoUpgrade.isNCGTS = false;
                        GameScr.info1.addInfo("Đã Dừng", 0);
                    }
                    if (AutoUpgrade.isNCSKH)
                    {
                        AutoUpgrade.isNCSKH = false;
                        GameScr.info1.addInfo("Đã Dừng", 0);
                    }
                    if (AutoUpgrade.isHHTB)
                    {
                        AutoUpgrade.isHHTB = false;
                        GameScr.info1.addInfo("Đã Dừng", 0);
                    }
                    if (AutoUpgrade.isMoCSBT)
                    {
                        AutoUpgrade.isMoCSBT = false;
                        GameScr.info1.addInfo("Đã Dừng", 0);
                    }
                    if (AutoUpgrade.isPLH)
                    {
                        AutoUpgrade.isPLH = false;
                        AutoUpgrade.isShowListUpgrade = false;
                        GameScr.info1.addInfo("Đã Dừng", 0);
                    }
                    if (AutoUpgrade.isKham)
                    {
                        AutoUpgrade.isKham = false;
                        AutoUpgrade.listKham.Clear();
                        GameScr.info1.addInfo("Đã Dừng", 0);
                    }
                    if (AutoUpgrade.isNCTB)
                    {
                        AutoUpgrade.isNCTB = false;
                        GameScr.info1.addInfo("Đã Dừng", 0);
                    }
                    if (AutoUpgrade.isEpNR)
                    {
                        AutoUpgrade.isEpNR = false;
                        AutoUpgrade.idBag = 0;
                        GameScr.info1.addInfo("Đã Dừng", 0);
                    }
                }
                if (AutoUseItem.isUseItem)
                {
                    AutoUseItem.isUseItem = false;
                    AutoUseItem.listItemUse.Clear();
                    GameScr.info1.addInfo("Auto sử dụng item đã Dừng", 0);
                }
                if (AutoBuy.isBuyItem)
                {
                    AutoBuy.isBuyItem = false;
                    AutoBuy.listItemBuy.Clear();
                    GameScr.info1.addInfo("Auto mua đã Dừng", 0);
                }
                if (AutoSpecialSkill.gI().IsActing)
                {
                    AutoSpecialSkill.gI().toggle(false);
                    GameScr.info1.addInfo("Đã Dừng", 0);
                }
                if (AutoPlusPoint.isAutoPlusPoint())
                {
                    AutoPlusPoint.isPlusPointHP = false;
                    AutoPlusPoint.isPlusPointMP = false;
                    AutoPlusPoint.isPlusPointSD = false;
                    AutoPlusPoint.isPlusPointDef = false;
                    GameScr.info1.addInfo("Đã Dừng", 0);
                }
                if (AutoSellGold.isBanVang)
                {
                    AutoSellGold.isBanVang = false;
                    GameScr.info1.addInfo("Auto bán thỏi vàng dừng", 0);
                }
            }
        }
    }
}
