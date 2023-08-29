using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using System;
using System.Security.Policy;
using System.Threading;

namespace Mod.Auto
{
    internal class AutoApTrung : ThreadActionUpdate<AutoApTrung>
    {
        public override int Interval => 6000;
        public static bool isApLinhThu = false;
        public static bool isApPet = false;
        private static readonly int delay = 1000;

        protected override void update()
        {
            if (isApLinhThu) ApLinhThu();
            if (isApPet) ApPet();
        }

        [ChatCommand("alt")]
        public static void toggleApLT()
        {
            isApLinhThu = !isApLinhThu;
            gI.toggle(true);
            GameScr.info1.addInfo("Ap linh thu bat dau", 0);
        }

        [ChatCommand("apet")]
        public static void toggleApPet()
        {
            isApPet = !isApPet;
            gI.toggle(true);
            GameScr.info1.addInfo("Ap pet bat dau", 0);
        }

        private static void ApLinhThu()
        {
            var index = Utilities.getIndexItemBag(1147);

            if (index == -1)
            {
                gI.toggle(false);
                isApLinhThu = false;
                GameScr.info1.addInfo("Ket thic ap linh thu", 0);
            }
            Utilities.openMenu(37);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(37, 0);
            Thread.Sleep(delay);
            GameCanvas.menu.doCloseMenu();
            Thread.Sleep(delay);
            MyVector myVector = new();
            myVector.addElement(Char.myCharz().arrItemBag[index]);
            Service.gI().combine(1, myVector);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(37, 0);
            Thread.Sleep(delay);
            Service.gI().requestChangeZone(0, -1);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(104);
            Service.gI().confirmMenu(104, 0);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(104, 0);
            Thread.Sleep(delay);
        }

        private static void ApPet()
        {
            var index = Utilities.getIndexItemBag(1148);

            if (index == -1)
            {
                gI.toggle(false);
                isApLinhThu = false;
                GameScr.info1.addInfo("Ket thic ap pet", 0);
            }
            Utilities.openMenu(37);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(37, 1);
            Thread.Sleep(delay);
            GameCanvas.menu.doCloseMenu();
            Thread.Sleep(delay);
            MyVector myVector = new();
            myVector.addElement(Char.myCharz().arrItemBag[index]);
            Service.gI().combine(1, myVector);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(37, 0);
            Thread.Sleep(delay);
            Service.gI().requestChangeZone(0, -1);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 1);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 2);
            Thread.Sleep(delay);
            Utilities.openMenu(105);
            Service.gI().confirmMenu(105, 0);
            Thread.Sleep(delay);
            Service.gI().confirmMenu(105, 0);
            Thread.Sleep(delay);
        }
    }
}
