using AssemblyCSharp.Mod.Other;
using AssemblyCSharp.Mod.Xmap;
using System.Reflection;

namespace Mod.Auto
{
    internal class AutoGa
    {
        public static bool isAutoGa;

        public static bool isAutoGaEnabled;

        public static long lastTimeFollow;

        private static bool isWaitKM;
        private static bool isWaitAru;
        public static void toggleGa()
        {
            isAutoGaEnabled = !isAutoGaEnabled;
            GameScr.info1.addInfo("Auto dẫn thần kê " + (isAutoGaEnabled ? "bắt đầu" : "kết thúc"), 0);
            if (!isAutoGaEnabled) isAutoGa = false;
        }
        public static void openMenu(sbyte id)
        {
            Npc npc = GameScr.findNPCInMap(id);
            //int distance = Res.distance(Char.myCharz().cx, Char.myCharz().cy, npc.cx, npc.cy);
            if (npc != null)
            {
                //if (distance > 60 && distance < 100)
                //    Char.myCharz().currentMovePoint = new MovePoint(npc.cx, npc.cy);
                //if (distance >= 100)
                XmapController.MoveMyChar(npc.cx, npc.cy);
            }
            Service.gI().openMenu(id);
        }

        public static void update()
        {
            if (mSystem.currentTimeMillis() - lastTimeFollow > 1000L)
            {
                isAutoGa = true;
                if (TileMap.mapID == 0 /*&& !isWaitKM*/)
                {
                    isWaitKM = true;
                    if (isWaitAru)
                    {
                        isWaitAru = false;
                        lastTimeFollow = mSystem.currentTimeMillis();
                        return;
                    }
                    openMenu(7);
                    Service.gI().confirmMenu(7, 2);
                    Service.gI().confirmMenu(7, 2);
                    if (Pk9rXmap.IsXmapRunning)
                        XmapController.FinishXmap();
                    if (!Pk9rXmap.IsXmapRunning)
                        XmapController.StartRunToMapId(5);
                    lastTimeFollow = mSystem.currentTimeMillis();
                    return;
                }
                if (Pk9rXmap.IsXmapRunning)
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
                if (TileMap.mapID == 5/* && !isWaitAru*/)
                {
                    if (isWaitKM)
                    {
                        isWaitKM = false;
                        lastTimeFollow = mSystem.currentTimeMillis();
                        return;
                    }
                    isWaitAru = true;
                    //if (Pk9rXmap.IsXmapRunning)
                    //    XmapController.FinishXmap();
                    //if (!Pk9rXmap.IsXmapRunning)
                    //    XmapController.StartRunToMapId(0);
                    Service.gI().requestMapSelect(0);
                    lastTimeFollow = mSystem.currentTimeMillis();
                    return;
                }

                if (Pk9rXmap.IsXmapRunning)
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
