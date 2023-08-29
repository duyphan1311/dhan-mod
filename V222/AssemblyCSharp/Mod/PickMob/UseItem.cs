using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AssemblyCSharp.Mod.PickMob
{
    public class UseItem
    {

        public static void useItem(string text)
        {

            if (IsGetInfoChat<int>(text, "uitem"))
            {
                int item = GetInfoChat<int>(text, "uitem");
                try
                {
                    sbyte itemID = 0;

                    while (itemID < global::Char.myCharz().arrItemBag.Length)
                    {
                        if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                        {
                            itemID++;
                            continue;
                        }
                        if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == item)
                        {
                            Service.gI().useItem(0, 1, itemID, -1);
                            break;
                        }
                        itemID++;
                    }
                    GameScr.info1.addInfo("Đã sử dụng item ( " + item + " )", 0);

                }
                catch (Exception ex)
                {
                    GameScr.info1.addInfo(ex.Message, 0);
                }
            }
        }
        public static void usePorataAndPetFollow()
        {
            try
            {
                sbyte itemID = 0;
                while (itemID < global::Char.myCharz().arrItemBag.Length)
                {
                    if(global::Char.myCharz().arrItemBag[(int)itemID] == null)
                    {
                        itemID++;
                        continue;
                    }
                    if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == 454 || global::Char.myCharz().arrItemBag[(int)itemID].template.id == 921)
                    {
                        Service.gI().useItem(0, 1, itemID, -1);
                        Service.gI().petStatus(0);
                        break;
                    }
                    itemID++;
                }
                GameScr.info1.addInfo("Đã sử dụng item Porata", 0);

            }
            catch (Exception ex)
            {
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }
        public static void usePorataAndPetDefense()
        {
            try
            {
                sbyte itemID = 0;

                while (itemID < global::Char.myCharz().arrItemBag.Length)
                {
                    if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                    {
                        itemID++;
                        continue;
                    }
                    if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == 454 || global::Char.myCharz().arrItemBag[(int)itemID].template.id == 921)
                    {
                        Service.gI().useItem(0, 1, itemID, -1);
                        Service.gI().petStatus(1);
                        break;
                    }
                    itemID++;
                }
                GameScr.info1.addInfo("Đã sử dụng item Porata", 0);

            }
            catch (Exception ex)
            {
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }

        public static void usePorataAndPetAttack()
        {
            try
            {
                sbyte itemID = 0;

                while (itemID < global::Char.myCharz().arrItemBag.Length)
                {
                    if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                    {
                        itemID++;
                        continue;
                    }
                    if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == 454 || global::Char.myCharz().arrItemBag[(int)itemID].template.id == 921)
                    {
                        Service.gI().useItem(0, 1, itemID, -1);
                        Service.gI().petStatus(2);
                        break;
                    }
                    itemID++;
                }
                GameScr.info1.addInfo("Đã sử dụng item Porata", 0);

            }
            catch (Exception ex)
            {
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }

        public static sbyte findItem(short id)
        {
            try
            {
                sbyte itemID = 0;
                while (itemID < global::Char.myCharz().arrItemBag.Length)
                {
                    if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                    {
                        itemID++;
                        continue;
                    }
                    if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == id)
                    {
                        return itemID;
                    }
                    itemID++;
                }

            }
            catch (Exception ex)
            {
                upCSKB = false;
                GameScr.info1.addInfo(ex.Message, 0);
                return -1;
            }
            return -1;
        }

        public static int waitTime;
        public static void useCN()
        {
            try
            {
                if (findItem(381) != -1)
                {
                    Service.gI().useItem(0, 1, findItem(381), -1); 
                    GameScr.info1.addInfo("Đã sử dụng item " + global::Char.myCharz().arrItemBag[(int)findItem(381)].template.name, 0);
                }
                else
                {
                    PickMob.IsACN = false;
                    GameScr.info1.addInfo("Không tìm thấy Cuồng nộ", 0);
                    return;
                }

            }
            catch (Exception e)
            {
                PickMob.IsKhauTrang = false;
                GameScr.info1.addInfo(e.Message, 0);
                return;
            }
        }
        //Auto up cskb
        public static bool upCSKB = false;
        public static void UpCSKB()
        {
            try
            {
                if (findItem(379) != -1)
                {
                    Service.gI().useItem(0, 1, findItem(379), -1); 
                    GameScr.info1.addInfo("Đã sử dụng item " + global::Char.myCharz().arrItemBag[(int)findItem(379)].template.name, 0);
                }
                else
                {
                    upCSKB = false;
                    GameScr.info1.addInfo("Không tìm thấy máy dò capsule kì bí", 0);
                    return;
                }

            }
            catch (Exception e)
            {
                upCSKB = false;
                GameScr.info1.addInfo(e.Message, 0);
                return;
            }
        }
        public static void AutoUseMayDoCSKB()
        {
            while (upCSKB)
            {
                UpCSKB();
                Thread.Sleep(1801000);
            }
        }
        public static void useFood()
        {
            try
            {
                sbyte itemID = 0;
                while (itemID < global::Char.myCharz().arrItemBag.Length)
                {
                    if (global::Char.myCharz().arrItemBag[(int)itemID] == null)
                    {
                        itemID++;
                        continue;
                    }
                    if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == 663 
                        || global::Char.myCharz().arrItemBag[(int)itemID].template.id == 664
                        || global::Char.myCharz().arrItemBag[(int)itemID].template.id == 665
                        || global::Char.myCharz().arrItemBag[(int)itemID].template.id == 666
                        || global::Char.myCharz().arrItemBag[(int)itemID].template.id == 667
                    )
                    {
                        Service.gI().useItem(0, 1, itemID, -1);
                        break;
                    }
                    itemID++;
                }
                GameScr.info1.addInfo("Đã sử dụng item " + global::Char.myCharz().arrItemBag[(int)itemID].template.name, 0);

            }
            catch (Exception ex)
            {
                PickMob.IsAFood = false;
                GameScr.info1.addInfo(ex.Message, 0);
                return;
            }
        }
        public static void AutoUseFood()
        {
            while (PickMob.IsAFood)
            {
                useFood();
                Thread.Sleep(601000);
            }
        }
        public static void AutoUseCN()
        {
            while (PickMob.IsACN)
            {
                useCN();
                Thread.Sleep(601000);
            }
        }
        public static void AutoUseBH()
        {
            while (PickMob.IsABH)
            {
                useBH();
                Thread.Sleep(601000);
            }
        }
        public static void AutoUseGX()
        {
            while (PickMob.IsAGX)
            {
                useGX();
                Thread.Sleep(601000);
            }
        }
        public static void AutoUseKhauTrang()
        {
            while (PickMob.IsKhauTrang)
            {
                UseKhauTrang();
                Thread.Sleep(1801000);
            }
        }
        public static void AutoUseBK()
        {
            while (PickMob.IsABK)
            {
                useBK();
                Thread.Sleep(601000);
            }
        }
        public static void UseKhauTrang()
        {
            try
            {
                if (findItem(764) != -1)
                {
                    Service.gI().useItem(0, 1, findItem(764), -1);
                    GameScr.info1.addInfo("Đã sử dụng item " + global::Char.myCharz().arrItemBag[(int)findItem(764)].template.name, 0);
                }
                else
                {
                    PickMob.IsKhauTrang = false;
                    GameScr.info1.addInfo("Không tìm thấy Khẩu trang", 0);
                    return;
                }

            }
            catch (Exception e)
            {
                PickMob.IsKhauTrang = false;
                GameScr.info1.addInfo(e.Message, 0);
                return;
            }
        }
        public static void useBH()
        {
            try
            {
                if (findItem(382) != -1)
                {
                    Service.gI().useItem(0, 1, findItem(382), -1);
                    GameScr.info1.addInfo("Đã sử dụng item " + global::Char.myCharz().arrItemBag[(int)findItem(382)].template.name, 0);
                }
                else
                {
                    PickMob.IsABH = false;
                    GameScr.info1.addInfo("Không tìm thấy Bổ huyết", 0);
                    return;
                }

            }
            catch (Exception e)
            {
                PickMob.IsABH = false;
                GameScr.info1.addInfo(e.Message, 0);
                return;
            }
        }
        public static void useGX()
        {
            try
            {
                if (findItem(384) != -1)
                {
                    Service.gI().useItem(0, 1, findItem(384), -1);
                    GameScr.info1.addInfo("Đã sử dụng item " + global::Char.myCharz().arrItemBag[(int)findItem(384)].template.name, 0);
                }
                else
                {
                    PickMob.IsAGX = false;
                    GameScr.info1.addInfo("Không tìm thấy Giáp Xên bọ hung", 0);
                    return;
                }

            }
            catch (Exception e)
            {
                PickMob.IsAGX = false;
                GameScr.info1.addInfo(e.Message, 0);
                return;
            }
        }
        public static void useBK()
        {
            try
            {
                if (findItem(383) != -1)
                {
                    Service.gI().useItem(0, 1, findItem(383), -1);
                    GameScr.info1.addInfo("Đã sử dụng item " + global::Char.myCharz().arrItemBag[(int)findItem(382)].template.name, 0);
                }
                else
                {
                    PickMob.IsABK = false;
                    GameScr.info1.addInfo("Không tìm thấy Bổ khí", 0);
                    return;
                }

            }
            catch (Exception e)
            {
                PickMob.IsABK = false;
                GameScr.info1.addInfo(e.Message, 0);
                return;
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
