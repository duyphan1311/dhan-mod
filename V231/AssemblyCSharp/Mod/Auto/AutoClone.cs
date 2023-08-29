using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static Mod.Auto.AutoChat.Setup;

namespace Mod.Auto
{
    internal class AutoClone : ThreadActionUpdate<AutoClone>
    {
        public override int Interval => 5000;

        [ChatCommand("anb")]
        public static void toggleAutoClone()
        {
            gI.toggle();
            AutoAttack.gI.toggle(gI.IsActing);
            GameScr.info1.addInfo("Tự động nhân bản " + (gI.IsActing ? "bắt đầu!" : "kết thúc!"), 0);
        }

        protected override void update()
        {
            if (CharExtensions.findCharInMap($"Nhân Bản{Char.myCharz().cName}") == null)
            {
                Utilities.openMenu(62);
                Thread.Sleep(500);
                Service.gI().confirmMenu(62, 0);
                Thread.Sleep(2000);
                Service.gI().requestChangeZone(TileMap.zoneID, -1);
                Thread.Sleep(500);
            }
            Thread.Sleep(2000);
            while (CharExtensions.findCharInMap($"Nhân Bản{Char.myCharz().cName}") != null)
            {
                Char @char = CharExtensions.findCharInMap($"Nhân Bản{Char.myCharz().cName}");
                global::Char.myCharz().npcFocus = null;
                global::Char.myCharz().mobFocus = null;
                global::Char.myCharz().charFocus = null;
                global::Char.myCharz().itemFocus = null;
                global::Char.myCharz().charFocus = @char;
                Thread.Sleep(100);
            }
        }
    }
}
