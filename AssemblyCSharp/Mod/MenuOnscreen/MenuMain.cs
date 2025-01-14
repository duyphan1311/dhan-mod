using Mod.Auto;
using Mod.Auto.AutoChat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.PickMob;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mod.MenuOnscreen
{
    internal class MenuMain : IChatable, IActionListener
    {
        [HotkeyCommand('z')]
        public static void show()
        {
            new MenuBuilder()
                .addItem(ifCondition: Pk9rPickMob.IsTanSat,
                    "Tắt\ntàn sát", new(() =>
                    {
                        Pk9rPickMob.IsTanSat = false;
                        if (Pk9rPickMob.TypeMobsTanSat.Count > 0 && !Pk9rPickMob.IsTanSat) Pk9rPickMob.TypeMobsTanSat.Clear();
                        GameScr.info1.addInfo("Đã tắt tàn sát!", 0);
                    }))
                .addItem("Tàn sát", new(showMenuTanSat))
                .addItem("Nâng cấp", new(AutoUpgrade.showMenu))
                .addItem("Chat", new(AutoChat.showMenu))
                .addItem("Vứt vật phẩm", new(AutoGetItemOut.ShowMenu))
                .addItem("Đậu", new(showMenuPean))
                .addItem("Đệ tử", new(showMenuPet))
                .addItem("Vòng quay", new(AutoCrackBall.ShowMenu))
                .addItem("Rương đồ", new(() =>
                {
                    GameCanvas.panel.setTypeBox();
                    GameCanvas.panel.show();
                }))
                .addItem("Tuỳ chỉnh", new(showMenuTool))
                .start();

        }

        public static void showMenuTool()
        {
            new MenuBuilder()
                .addItem($"Tốc độ game:\n{Utilities.speedGame}", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "Speed";
                    ChatTextField.gI().startChat2(new MenuMain(), "Nhập tốc độ game");
                }))
                .addItem($"Tốc độ chạy:\n{Utilities.speedRun}", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "Speed";
                    ChatTextField.gI().startChat2(new MenuMain(), "Nhập tốc độ chạy");
                }))
                .start();
        }

        public static void showMenuPet()
        {
            new MenuBuilder()
                .addItem("Auto up đệ:\n[" + (AutoPet.mode == AutoPet.AutoPetMode.Disabled ? "Off]" : "On]"), new(() =>
                {
                    if (AutoPet.mode == AutoPet.AutoPetMode.Disabled) AutoPet.setState(1);
                    else AutoPet.setState(0);
                    GameScr.info1.addInfo("Tự động up đệ: " + (AutoPet.mode == AutoPet.AutoPetMode.Disabled ? "Off" : "On"), 0);
                }))
                .addItem("Bật cờ\nchống PK:\n[" + (Utilities.IsAutoFlag ? "On]" : "Off]"), new(() =>
                {
                    Utilities.toggleAutoFlag();
                    GameScr.info1.addInfo("Bật cờ chống PK: " + (Utilities.IsAutoFlag ? "On" : "Off"), 0);
                }))
                .addItem($"HP Buff: {Utilities.pHPBuff}%", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "%HP";
                    ChatTextField.gI().startChat2(new MenuMain(), "Nhập giá trị %HP của đệ tử");
                }))
                .addItem($"MP Buff: {Utilities.pMPBuff}%", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "%MP";
                    ChatTextField.gI().startChat2(new MenuMain(), "Nhập giá trị %MP của đệ tử");
                }))
                .start();
        }

        public static void showMenuPean()
        {
            new MenuBuilder()
                .addItem("Auto buff đậu", new(showMenuBuffPean))
                .addItem("Cho đậu:\n[" + (AutoPean.isAutoDonateEnabled ? "On]" : "Off]"), new(() => AutoPean.SetAutoDonateState(true)))
                .addItem("Xin đậu:\n[" + (AutoPean.isAutoRequestEnabled ? "On]" : "Off]"), new(() => AutoPean.SetAutoRequestState(true)))
                .addItem("Thu đậu:\n[" + (AutoPean.isAutoHarvestEnabled ? "On]" : "Off]"), new(() => AutoPean.SetAutoHarvestState(true)))
                .start();
        }

        public static void showMenuBuffPean()
        {
            new MenuBuilder()
                .addItem(Utilities.isAutoBuffPean ? "Kết thúc" : "Bắt đầu", new(() =>
                {
                    Utilities.isAutoBuffPean = !Utilities.isAutoBuffPean;
                    GameScr.info1.addInfo("Tự động buff đậu: " + (Utilities.isAutoBuffPean ? "On" : "Off"), 0);
                }))
                .addItem($"HP Buff: {Utilities.pHPBuff}%", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "%HP";
                    ChatTextField.gI().startChat2(new MenuMain(), "Nhập giá trị %HP");
                }))
                .addItem($"MP Buff: {Utilities.pMPBuff}%", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = "%MP";
                    ChatTextField.gI().startChat2(new MenuMain(), "Nhập giá trị %MP");
                }))
                .start();
        }

        [HotkeyCommand('t')]
        public static void showMenuTanSat()
        {
            var menuBuilder = new MenuBuilder();
            menuBuilder.addItem(ifCondition: Pk9rPickMob.IsTanSat,
                    "Tắt\ntàn sát", new(() =>
                    {
                        Pk9rPickMob.IsTanSat = false;
                        if (Pk9rPickMob.TypeMobsTanSat.Count > 0 && !Pk9rPickMob.IsTanSat) Pk9rPickMob.TypeMobsTanSat.Clear();
                        GameScr.info1.addInfo("Đã tắt tàn sát!", 0);
                    }));
            menuBuilder.addItem(ifCondition: GameScr.vMob.size() > 0,
                "Tất cả", new(() =>
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
            if (string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && string.IsNullOrEmpty(text))
                return;

            if (to == "Nhập giá trị %HP")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception("Giá trị nhập vào phải là số tự nhiên!");
                    if (value < 0 || value > 100) throw new Exception("Giá trị phải lớn hơn 0 và nhỏ hơn 100!");
                    Utilities.pHPBuff = value;
                    showMenuBuffPean();
                }
                catch (Exception e)
                {
                    GameCanvas.startOKDlg(e.Message);
                }
            }
            else if (to == "Nhập giá trị %MP")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception("Giá trị nhập vào phải là số tự nhiên!");
                    if (value < 0 || value > 100) throw new Exception("Giá trị phải lớn hơn 0 và nhỏ hơn 100!");
                    Utilities.pMPBuff = value;
                    showMenuBuffPean();
                }
                catch (Exception e)
                {
                    GameCanvas.startOKDlg(e.Message);
                }
            }
            else if (to == "Nhập giá trị %HP của đệ tử")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception("Giá trị nhập vào phải là số tự nhiên!");
                    if (value < 0 || value > 100) throw new Exception("Giá trị phải lớn hơn 0 và nhỏ hơn 100!");
                    AutoPet.hpBuff = value;
                    showMenuPet();
                }
                catch (Exception e)
                {
                    GameCanvas.startOKDlg(e.Message);
                }
            }
            else if (to == "Nhập giá trị %MP của đệ tử")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception("Giá trị nhập vào phải là số tự nhiên!");
                    if (value < 0 || value > 100) throw new Exception("Giá trị phải lớn hơn 0 và nhỏ hơn 100!");
                    AutoPet.mpBuff = value;
                    showMenuPet();
                }
                catch (Exception e)
                {
                    GameCanvas.startOKDlg(e.Message);
                }
            }
            else if (to == "Nhập tốc độ game")
            {
                try
                {
                    if (!float.TryParse(text, out float value)) throw new Exception("Giá trị nhập vào phải là số!");
                    if (value < 0 || value > 100) throw new Exception("Giá trị phải lớn hơn 0 và nhỏ hơn 100!");
                    Utilities.setSpeedGame(value);
                    showMenuTool();
                }
                catch (Exception e)
                {
                    GameCanvas.startOKDlg(e.Message);
                }
            }
            else if (to == "Nhập tốc độ chạy")
            {
                try
                {
                    if (!int.TryParse(text, out int value)) throw new Exception("Giá trị nhập vào phải là số tự nhiên!");
                    if (value < 0 || value > 100) throw new Exception("Giá trị phải lớn hơn 0 và nhỏ hơn 100!");
                    Utilities.setSpeedRun(value);
                    showMenuTool();
                }
                catch (Exception e)
                {
                    GameCanvas.startOKDlg(e.Message);
                }
            }
            else
                ChatTextField.gI().isShow = false;

            ChatTextField.gI().ResetTF();
        }

        public void onCancelChat()
        {
            ChatTextField.gI().isShow = false;
            ChatTextField.gI().ResetTF();
        }

        public void perform(int idAction, object p)
        {

            if (idAction == 0)
            {
                Item item = (Item)p;
                bool HasItemInListUse = AutoUseItem.listItemUse.Any(x => (item.template.id == x.item.template.id && item.GetFullInfo() == x.item.GetFullInfo()));
                bool HasItemInListUpgrade = AutoUpgrade.listUpgrade.Any(x => ((GameCanvas.panel.selected - Char.myCharz().arrItemBody.Length) == x.id && item.template.type == x.type && item.template.name == x.name));

                var menuBuilder = new MenuBuilder();
                if (item.isTypeBody())
                {

                    if (item.template.type == 32 || item.template.type == 11 ||
                        item.template.type == 72 || item.template.type == 23 || item.template.type == 27 ||
                        (item.template.type >= 0 && item.template.type <= 5))
                    {
                        if (HasItemInListUpgrade)
                            menuBuilder.addItem("Loại khỏi\nList Upgrade", new(() => AutoUpgrade.gI.perform(1, item)));
                        else
                            menuBuilder.addItem("Thêm vào\nList Upgrade", new(() => AutoUpgrade.gI.perform(1, item)));
                    }
                }
                else
                {

                    if (AutoUseItem.listItemUse.Count <= 0)
                    {
                        menuBuilder.addItem("Dùng liên tục", new(() => AutoUseItem.gI.perform(1, item)));
                    }
                    else
                    {
                        if (HasItemInListUse)
                            menuBuilder.addItem("Kết thúc", new(() => AutoUseItem.gI.perform(2, item)));
                        else
                            menuBuilder.addItem("Dùng liên tục", new(() => AutoUseItem.gI.perform(1, item)));
                    }
                    if (item.template.id == 457)
                    {
                        if (AutoSellGold.isBanVang)
                            menuBuilder.addItem("Kết thúc", new(() => AutoSellGold.gI.perform(2, null)));
                        else
                            menuBuilder.addItem("Auto bán", new(() => AutoSellGold.gI.perform(1, null)));
                    }
                    if ((item.template.type == 11 || item.template.type == 72 || item.template.type == 23 || item.template.type == 27) && item.template.id != 457)
                    {
                        if (HasItemInListUpgrade)
                            menuBuilder.addItem("Loại khỏi\nList Upgrade", new(() => AutoUpgrade.gI.perform(1, item)));
                        else
                            menuBuilder.addItem("Thêm vào\nList Upgrade", new(() => AutoUpgrade.gI.perform(1, item)));
                    }
                }
                menuBuilder.start();
            }
        }
    }
}
