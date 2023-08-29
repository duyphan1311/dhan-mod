using Mod.ModHelper.CommandMod.Hotkey;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mod.Auto
{
    internal class AutoFindBoss
    {
        public static bool isFindBoss;

        public static bool isGoToMapBoss;

        public static List<int> listOldZone = new();

        public static List<int> listZone = new();

        public static int khusb;

        public static int processes;

        public static string scanedZone = "Data\\zones";

        public static string NumCli = "Data\\numCli";

        public static string bossZone = "Data\\bossZone";

        public static int maxNumberPlayer;

        public static int[] zones;

        public static long lastTimeCheck = 0;

        public static bool isStart = false;

        public static void FindMapBoss()
        {
            if (Boss.listBosses.Count <= 0) return;
            Boss boss = Boss.listBosses[Boss.listBosses.Count - 1];
            if (boss.isDied)
            {
                isFindBoss = false;
                listOldZone.Clear();
                isStart = false;
                int num = int.Parse(File.ReadAllText(NumCli));
                num--;
                File.WriteAllText(NumCli, num.ToString());
                if (num <= 0)
                    resetFile();
                GameScr.info1.addInfo($"Boss đã {(string.IsNullOrEmpty(boss.killer) ? "chết" : $"bị {boss.killer} tiêu diệt")}!", 0);
            }
            else
            {
                if (TileMap.mapID != boss.mapId)
                {
                    if (XmapController.gI.IsActing)
                        XmapController.finishXmap();
                    XmapController.start(boss.mapId);
                    return;
                }
            }
        }

        public static string FindBoss()
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.isBoss())
                {
                    File.WriteAllText(bossZone, $"{TileMap.zoneID}|{Char.myCharz().charID}");
                    return ch.cName;
                }
            }
            return null;
        }

        [HotkeyCommand('b')]
        public static void toggleAutoSanBoss()
        {
            isFindBoss = !isFindBoss;
            //new Thread(delegate ()
            //{
            //    try
            //    {
            if (!isFindBoss)
            {
                listOldZone.Clear();
                isStart = false;
                int num = int.Parse(File.ReadAllText(NumCli));
                num--;
                File.WriteAllText(NumCli, num.ToString());
                if (num <= 0)
                    resetFile();
            }
            else
            {
                if (isGoToMapBoss) FindMapBoss();
                initAutoSanBoss();
                isStart = true;
            }
            //    }
            //    catch (Exception e)
            //    {
            //        string file = "Data\\a.txt";
            //        if (!File.Exists(file)) File.Create(file).Close();
            //        File.AppendAllText(file, e.ToString() + "\n");
            //    }
            //})
            //{
            //    IsBackground = true,
            //    Name = "StartSanBoss"
            //}.Start();
            GameScr.info1.addInfo("Săn boss: " + (isFindBoss ? "Bật" : "Tắt"), 0);
        }

        public static void onQuitGame()
        {
            if (File.Exists(NumCli))
            {
                int num = int.Parse(File.ReadAllText(NumCli));
                num--;
                File.WriteAllText(NumCli, num.ToString());
                if (num <= 0)
                    resetFile();
            }
        }

        private static void initAutoSanBoss()
        {
            int cli = 0;
            if (!File.Exists(NumCli))
                File.Create(NumCli).Close();
            else
            {
                cli = int.Parse(File.ReadAllText(NumCli));

                if (cli <= 0)
                    cli = 0;
            }
            cli++;
            File.WriteAllText(NumCli, cli.ToString());
            listOldZone.Clear();

            using FileStream fileStream = new(scanedZone, FileMode.Create, FileAccess.Write);

            if (!File.Exists(bossZone))
            {
                File.Create(bossZone).Close();
                File.WriteAllText(bossZone, "-1|-1");
            }
        }

        private static void resetFile()
        {
            if (File.Exists(scanedZone))
                File.Delete(scanedZone);
            if (File.Exists(bossZone))
                File.Delete(bossZone);
            if (File.Exists(NumCli))
                File.Delete(NumCli);
        }

        public static bool IsCanDctt()
        {
            if ((Char.myCharz().arrItemBody[5].template.id >= 591 && Char.myCharz().arrItemBody[5].template.id <= 594)
                || Char.myCharz().arrItemBody[5].template.id == 905 || Char.myCharz().arrItemBody[5].template.id == 905
                || Char.myCharz().arrItemBody[5].template.id == 911)
                return true;
            var index = Utilities.getIndexItemBag(591, 592, 593, 594, 905, 905, 911);
            if (index != -1) return true;
            return false;
        }

        public static void SanBoss()
        {
            if (!isStart || XmapController.gI.IsActing) return;
            try
            {

                if (mSystem.currentTimeMillis() - lastTimeCheck > 250)
                {
                    zones = GameScr.gI().zones;

                    processes = int.Parse(File.ReadAllText(NumCli));

                    listZone = File.ReadAllLines(scanedZone).ToList().Select(s => int.Parse(s)).ToList();

                    string[] str = File.ReadAllText(bossZone).Split('|');

                    int khuBoss = int.Parse(str[0]);
                    int id = int.Parse(str[1]);

                    if (khuBoss != -1)
                    {
                        isFindBoss = false;
                        isStart = false;
                        listOldZone.Clear();
                        GameScr.info1.addInfo("Tìm thấy " + FindBoss() + " ở khu " + khuBoss, 0);
                        if (khuBoss != TileMap.zoneID)
                        {
                            if (Char.myCharz().charID != id && IsCanDctt())
                                Service.gI().gotoPlayer(id);
                            else
                                Utilities.changeZone(khuBoss);
                        }
                        processes--;
                        File.WriteAllText(NumCli, processes.ToString());
                        if (processes <= 0)
                            resetFile();
                        return;
                    }

                    if (processes <= 0)
                    {
                        isFindBoss = false;
                        isStart = false;
                        listOldZone.Clear();
                        resetFile();
                        GameScr.info1.addInfo("Săn boss: " + (isFindBoss ? "Bật" : "Tắt"), 0);
                        return;
                    }

                    if (!listOldZone.Any() || khusb == TileMap.zoneID)
                    {
                        if (listZone.Count == zones.Length || listOldZone.Count == zones.Length)
                        {
                            isFindBoss = false;
                            isStart = false;
                            listOldZone.Clear();
                            processes--;
                            File.WriteAllText(NumCli, processes.ToString());
                            if (processes <= 0)
                                resetFile();
                            GameScr.info1.addInfo("Không tìm thấy boss\nAuto săn boss: " + (isFindBoss ? "Bật" : "Tắt"), 0);
                            return;
                        }
                        if (FindBoss() != null)
                        {
                            isFindBoss = false;
                            isStart = false;
                            GameScr.info1.addInfo("Tìm thấy " + FindBoss() + " ở khu " + TileMap.zoneID, 0);
                            listOldZone.Clear();
                            processes--;
                            File.WriteAllText(NumCli, processes.ToString());
                            if (processes <= 0)
                                resetFile();
                            return;
                        }
                        maxNumberPlayer = -1;
                        if (!listOldZone.Contains(TileMap.zoneID) && !listZone.Contains(TileMap.zoneID))
                        {
                            listOldZone.Add(TileMap.zoneID);
                            File.AppendAllText(scanedZone, $"{TileMap.zoneID}\n");
                        }
                        for (int i = 0; i < zones.Length; i++)
                        {
                            if (!listZone.Contains(zones[i]) && !listOldZone.Contains(zones[i]))
                            {
                                if (GameScr.gI().numPlayer[zones[i]] >= GameScr.gI().maxPlayer[zones[i]])
                                    continue;

                                if (maxNumberPlayer < GameScr.gI().numPlayer[zones[i]])
                                {
                                    maxNumberPlayer = GameScr.gI().numPlayer[zones[i]];
                                    khusb = zones[i];
                                }
                            }
                        }
                        listOldZone.Add(khusb);
                        File.AppendAllText(scanedZone, $"{khusb}\n");
                        Service.gI().requestChangeZone(khusb, -1);
                        lastTimeCheck = mSystem.currentTimeMillis();
                        return;
                    }
                    Service.gI().requestChangeZone(khusb, -1);
                    lastTimeCheck = mSystem.currentTimeMillis();
                }
            }
            catch
            {
                isFindBoss = false;
                isStart = false;
                listOldZone.Clear();
                resetFile();
                GameScr.info1.addInfo("Săn boss: " + (isFindBoss ? "Bật" : "Tắt"), 0);
                return;
            }
        }

        public static void setState(bool value) => isGoToMapBoss = value;
    }
}
