using Mod.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class ShareInfo
    {
        public static long lastTimeUpdate = 0;

        public static bool isShareInfo;

        public static string[] strStatus = new string[6]
        {
            mResources.follow,
            mResources.defend,
            mResources.attack,
            mResources.gohome,
            mResources.fusion,
            mResources.fusionForever
        };

        public static void LoadData()
        {
            try
            {
                isShareInfo = Utilities.loadRMSBool("isShareInfo");
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void SaveData()
        {
            try
            {
                Utilities.saveRMSBool("isShareInfo", isShareInfo);
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void setState(bool value) => isShareInfo = value;

        public static void sendInfo()
        {
            if (mSystem.currentTimeMillis() - lastTimeUpdate > 1500)
            {
                lastTimeUpdate = mSystem.currentTimeMillis();
                var myChar = Char.myCharz();
                var myPet = Char.myPetz();

                if (myChar.cName == "")
                    return;

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
                    petStatus = strStatus[myPet.petStatus],
                    myChar.xu,
                    myChar.luong,
                    myChar.luongKhoa
                });
            }
        }
    }
}
