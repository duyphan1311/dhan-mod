using Mod.ModHelper.CommandMod.Chat;
using Mod.Xmap;

namespace Mod.Auto
{
    internal class AutoGa
    {
        public static bool isAutoGa;

        public static bool isAutoGaEnabled;

        public static long lastTimeFollow;

        private static bool isWaitKM;
        private static bool isWaitAru;

        [ChatCommand("thanke")]
        public static void toggleGa()
        {
            isAutoGaEnabled = !isAutoGaEnabled;
            GameScr.info1.addInfo("Auto dẫn thần kê " + (isAutoGaEnabled ? "bắt đầu" : "kết thúc"), 0);
            if (!isAutoGaEnabled) isAutoGa = false;
        }

        public static void update()
        {
            if (mSystem.currentTimeMillis() - lastTimeFollow > 2000L)
            {
                isAutoGa = true;
                if (TileMap.mapID == 0)
                {
                    isWaitKM = true;
                    if (isWaitAru)
                    {
                        isWaitAru = false;
                        lastTimeFollow = mSystem.currentTimeMillis();
                        return;
                    }
                    Utilities.openMenu(7);
                    Service.gI().confirmMenu(7, 2);
                    Service.gI().confirmMenu(7, 2);
                    if (XmapController.gI.IsActing)
                        XmapController.finishXmap();
                    if (!XmapController.gI.IsActing)
                        XmapController.start(5);
                    lastTimeFollow = mSystem.currentTimeMillis();
                    return;
                }
                if (XmapController.gI.IsActing)
                {
                    lastTimeFollow = mSystem.currentTimeMillis();
                    return;
                }
                if (Char.ischangingMap || Char.isLoadingMap)
                {
                    lastTimeFollow = mSystem.currentTimeMillis();
                    return;
                }

                isAutoGa = false;
                if (TileMap.mapID == 5)
                {
                    if (isWaitKM)
                    {
                        isWaitKM = false;
                        lastTimeFollow = mSystem.currentTimeMillis();
                        return;
                    }
                    isWaitAru = true;
                    if (XmapController.gI.IsActing)
                        XmapController.finishXmap();
                    if (!XmapController.gI.IsActing)
                        XmapController.start(0);
                    lastTimeFollow = mSystem.currentTimeMillis();
                    return;
                }

                if (XmapController.gI.IsActing)
                {
                    lastTimeFollow = mSystem.currentTimeMillis();
                    return;
                }
                if (Char.ischangingMap || Char.isLoadingMap)
                {
                    lastTimeFollow = mSystem.currentTimeMillis();
                    return;
                }
                lastTimeFollow = mSystem.currentTimeMillis();
            }
        }
    }
}
