//using AssemblyCSharp.Mod.OnScreenMod;
//using AssemblyCSharp.Mod.Other;
//using Mod.ModHelper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Mod
//{
//    public class ShareInfo : ThreadActionUpdate<ShareInfo>
//    {
//        public override int Interval => 1000;

//        protected override void update()
//        {
//            var myChar = Char.myCharz();
//            var myPet = Char.myPetz();

//            if (myChar.cName == "")
//                return;

//            SocketClient.gI.sendMessage(new
//            {
//                action = "updateInfo",
//                Utilities.status,
//                myChar.charID,
//                myChar.cName,
//                myChar.cgender,
//                TileMap.mapName,
//                TileMap.mapID,
//                TileMap.zoneID,
//                myChar.cx,
//                myChar.cy,
//                myChar.cHP,
//                myChar.cHPFull,
//                myChar.cMP,
//                myChar.cMPFull,
//                myChar.cStamina,
//                myChar.cMaxStamina,
//                myChar.cPower,
//                myChar.cTiemNang,
//                myChar.cHPGoc,
//                myChar.cMPGoc,
//                myChar.cDefGoc,
//                myChar.cCriticalGoc,
//                myChar.cDamFull,
//                myChar.cDefull,
//                myChar.cCriticalFull,
//                cPetName = myPet.cName,
//                cPetGender = myPet.cgender,
//                cPetHP = myPet.cHP,
//                cPetHPFull = myPet.cHPFull,
//                cPetMP = myPet.cMP,
//                cPetMPFull = myPet.cMPFull,
//                cPetStamina = myPet.cStamina,
//                cPetMaxStamina = myPet.cMaxStamina,
//                cPetPower = myPet.cPower,
//                cPetTiemNang = myPet.cTiemNang,
//                cPetDamFull = myPet.cDamFull,
//                cPetDefull = myPet.cDefull,
//                cPetCriticalFull = myPet.cCriticalFull,
//                petStatus = OnScreenController.strStatus[myPet.petStatus],
//                myChar.xu,
//                myChar.luong,
//                myChar.luongKhoa
//            });
//        }
//    }
//}
