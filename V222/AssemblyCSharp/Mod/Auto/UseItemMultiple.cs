using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp.Mod.Auto
{
    internal class UseItemMultiple
    {
        public static bool isUseItemMulti = false;

        public static List<ItemUseMulti> itemList = new();

        public static Item curr;

        public struct ItemUseMulti
        {
            public int quatity;
            public int timeUse;
            public long lastTimeUse;
            public Item item;

            public ItemUseMulti(int quatity, int timeUse, long lastTimeUse, Item item)
            {
                this.quatity = quatity;
                this.timeUse = timeUse;
                this.lastTimeUse = lastTimeUse;
                this.item = item;
            }
        }

        public static void update()
        {
            if(GameCanvas.gameTick % 20 == 0 && isUseItemMulti)
            {
                foreach(ItemUseMulti item in itemList)
                {
                    if(item.quatity == 0)
                    {
                        itemList.Remove(item);
                        GameScr.info1.addInfo($"Auto dùng item {item.item.template.name} đã dừng", 0);
                        continue;
                    }
                    if(item.quatity > 0 && mSystem.currentTimeMillis() - item.lastTimeUse > item.timeUse)
                    {
                        useItem(item.item);
                        ItemUseMulti i = new();
                        i.quatity = item.quatity - 1;
                        i.timeUse = item.timeUse;
                        i.lastTimeUse = mSystem.currentTimeMillis();
                        i.item = item.item;
                        RefreshListItem(i);
                    }
                }
            }
            if (GameCanvas.gameTick % 20 == 0 && isAutoMuaDo)
            {
                //AutoMuaDo();
                foreach (ItemBuyMulti item in listItemBuy)
                {
                    if (item.soLanMua == 0)
                    {
                        listItemBuy.Remove(item);
                        GameScr.info1.addInfo($"Auto dùng item {item.item.template.name} đã dừng", 0);
                        continue;
                    }
                    if (item.soLanMua > 0 && mSystem.currentTimeMillis() - item.lastTimeMua > item.timeBuy)
                    {
                        buyItem(item);
                        ItemBuyMulti i = new();
                        i.typeBuy = item.typeBuy;
                        i.soLanMua = item.soLanMua - 1;
                        i.timeBuy = item.timeBuy;
                        i.lastTimeMua = mSystem.currentTimeMillis();
                        i.item = item.item;
                        RefreshListItem(i);
                    }
                }
            }
        }
        public static List<ItemUseMulti> RefreshListItem(ItemUseMulti item)
        {
            int index = itemList.FindIndex(i => i.item.template.id == item.item.template.id);
            if (index != -1)
                itemList[index] = item;
            return itemList;
        }
        public static void useItem(Item i)
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
                    if (global::Char.myCharz().arrItemBag[(int)itemID].template.id == i.template.id)
                    {
                        Service.gI().useItem(0, 1, itemID, -1);
                        GameScr.info1.addInfo("Đã sử dụng item " + i.template.name, 0);
                        break;
                    }
                    itemID++;
                }

            }
            catch (Exception ex)
            {
                isUseItemMulti = false;
                itemList.Clear();
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }


        // auto mua do

        public struct ItemBuyMulti
        {
            public int soLanMua;
            public int timeBuy;
            public long lastTimeMua;
            public sbyte typeBuy;
            public Item item;

            public ItemBuyMulti(int soLanMua, int timeBuy, long lastTimeMua, Item item, sbyte typeBuy)
            {
                this.soLanMua = soLanMua;
                this.timeBuy = timeBuy;
                this.lastTimeMua = lastTimeMua;
                this.item = item;
                this.typeBuy = typeBuy;
            }
        }

        public static bool isAutoMuaDo = false;

        public static long lastTimeMua;

        public static int timeBuy = 1000;

        public static int soLanMua;

        public static Item itemAutoMua;

        public static sbyte typeBuy;

        public static void AutoMuaDo()
        {
            if (soLanMua == 0)
            {
                GameScr.info1.addInfo($"Auto mua item {itemAutoMua.template.name} dừng", 0);
                isAutoMuaDo = false;
            }
            if (soLanMua > 0 && mSystem.currentTimeMillis() - lastTimeMua > timeBuy)
            {
                Service.gI().buyItem(typeBuy, itemAutoMua.template.id, 0);
                soLanMua--;
                lastTimeMua = mSystem.currentTimeMillis();
            }
        }

        public static void buyItem(ItemBuyMulti i)
        {
            try
            {
                Service.gI().buyItem(i.typeBuy, i.item.template.id, 0);
            }
            catch (Exception ex)
            {
                isAutoMuaDo = false;
                listItemBuy.Clear();
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }

        public static List<ItemBuyMulti> listItemBuy = new();

        public static List<ItemBuyMulti> RefreshListItem(ItemBuyMulti item)
        {
            int index = listItemBuy.FindIndex(i => i.item.template.id == item.item.template.id);
            if (index != -1)
                listItemBuy[index] = item;
            return listItemBuy;
        }
    }
}
