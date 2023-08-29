using Mod.Auto;
using Mod.Auto.AutoChat;
using Mod.Graphics;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.PickMob;
using Mod.Set;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.MenuOnscreen
{
    internal class MenuMain : IChatable
    {
        [HotkeyCommand('z')]
        public static void show()
        {
            new MenuBuilder()
                .addItem(ifCondition: Pk9rPickMob.IsTanSat,
                    "Tắt\ntàn sát", new(() =>
                    {
                        Pk9rPickMob.IsTanSat = false;
                        GameScr.info1.addInfo("Đã tắt tàn sát!", 0);
                    }))
                .addItem("Tàn sát", new(showMenuTanSat))
                .addItem("Nâng cấp\ntrang bị", new(AutoUpgrade.showMenu))
                //.addItem("XMap", new(Pk9rXmap.showXmapMenu))
                //.addItem("Teleport", new(TeleportMenu.TeleportMenu.ShowMenu))
                //.addItem("Set đồ", new(SetDo.ShowMenu))
                .addItem("Auto chat", new(AutoChat.showMenu))
                .addItem("Auto vứt\nvật phẩm", new(AutoGetItemOut.ShowMenu))
                .addItem("Vòng quay", new(AutoCrackBall.ShowMenu))
                //.addItem("NPC", new(Utilities.showMenuTeleNpc))
                //.addItem("Rương đồ", new(() => Service.gI().openMenu(3)))
                .start();

        }

        public static void showMenuTanSat()
        {
            var menuBuilder = new MenuBuilder();
            menuBuilder.addItem(ifCondition: Pk9rPickMob.IsTanSat,
                    "Tắt\ntàn sát", new(() =>
                    {
                        Pk9rPickMob.IsTanSat = false;
                        GameScr.info1.addInfo("Đã tắt tàn sát!", 0);
                    }));
            if (GameScr.vMob.size() > 0)
                menuBuilder.addItem("Tất cả", new(() =>
                {
                    Pk9rPickMob.IsTanSat = true;
                    if (Pk9rPickMob.TypeMobsTanSat.Count > 0) Pk9rPickMob.TypeMobsTanSat.Clear();
                    GameScr.info1.addInfo("Tự động đánh quái: Bật", 0);
                }));
            List<sbyte> idMob = new();
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                if (!idMob.Contains(mob.getTemplate().mobTemplateId))
                    menuBuilder.addItem($"{mob.getTemplate().name}\n[{NinjaUtil.getMoneys(mob.getTemplate().hp)}]", new(() =>
                    {
                        if (Pk9rPickMob.TypeMobsTanSat.Contains(mob.getTemplate().mobTemplateId))
                        {
                            Pk9rPickMob.TypeMobsTanSat.Remove(mob.getTemplate().mobTemplateId);
                            GameScr.info1.addInfo($"Đã xoá loại mob: {Mob.arrMobTemplate[mob.getTemplate().mobTemplateId].name}[{mob.getTemplate().mobTemplateId}]", 0);
                        }
                        else
                        {
                            Pk9rPickMob.TypeMobsTanSat.Add(mob.getTemplate().mobTemplateId);
                            GameScr.info1.addInfo($"Đã thêm loại mob: {Mob.arrMobTemplate[mob.getTemplate().mobTemplateId].name}[{mob.getTemplate().mobTemplateId}]", 0);
                        }
                        Pk9rPickMob.IsTanSat = true;
                        GameScr.info1.addInfo("Tự động đánh quái: Bật", 0);
                    }));
                idMob.Add(mob.getTemplate().mobTemplateId);
            }
            menuBuilder.addItem("Cài đặt", new(Pk9rPickMob.ShowMenu));
            menuBuilder.start();
        }

        public void onChatFromMe(string text, string to)
        {
            if (!string.IsNullOrEmpty(text))
            {

            }
            else
                GameCanvas.panel.chatTField.isShow = false;
            GameCanvas.panel.chatTField.ResetTF();
        }

        public void onCancelChat()
        {
            GameCanvas.panel.chatTField.ResetTF();
        }
    }
}
