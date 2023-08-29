using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mod.Auto
{
    internal class AutoCallDragon : ThreadActionUpdate<AutoCallDragon>
    {
        public override int Interval => 100;

        protected override void update()
        {
            var index = Utilities.getIndexItemBag(14);

            if(index == -1 || GameScr.gI().isRongThanXuatHien)
            {
                gI.toggle(false);
                while (GameCanvas.menu.showMenu)
                {
                    GameCanvas.menu.doCloseMenu();
                    ChatPopup.currChatPopup = null;
                }
                GameScr.info1.addInfo("Auto gọi rồng tắt", 0);
                return;
            }

            Service.gI().useItem(0, 1, index, -1);
            Service.gI().confirmMenu(5, 1);
            GameCanvas.menu.doCloseMenu();
            ChatPopup.currChatPopup = null;
        }

        [ChatCommand("nr")]
        public static void toggleCallDragon()
        {
            gI.toggle();
            GameScr.info1.addInfo("Auto gọi rồng " + (gI.IsActing ? "bắt đầu" : "kết thúc"), 0);
        }
    }
}
