using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.Menu;
using Mod.ModHelper;
using System.IO;
using Mod.CustomPanel;
using System;
using System.Collections.Generic;

namespace Mod.Auto.AutoChat
{
    public class AutoChat : ThreadActionUpdate<AutoChat>
    {
        public override int Interval => /*Res.random(5000, 10000)*/ Setup.delayAutoChat;

        protected override void update()
        {
            //string filePath = Utilities.PathAutoChat;
            string content = Setup.currentChat.content;


            Service.gI().chat("[ATC" + Res.random(10, 100) + "]: " + content);


        }
        [ChatCommand("atc")]
        public static void showMenu()
        {
            new MenuBuilder()
                //.addItem("Auto chat\n" + (gI.IsActing ? "[On]" : "[Off]"), new(() =>
                //{
                //    gI.toggle();
                //    if (!gI.IsActing)
                //    {
                //        Setup.clearStringTrash();
                //        GameScr.info1.addInfo("Tắt tự động chat ", 0);
                //    }
                //}))
                .addItem(ifCondition: gI.IsActing, "Tắt\nauto chat", new(() =>
                {
                    gI.toggle(false);
                    Setup.clearStringTrash();
                    GameScr.info1.addInfo("Tự động chat kết thúc", 0);
                }))
                .addItem("Nhập nội dung mới", new(() =>
                {
                    ChatTextField.gI().strChat = string.Empty;
                    ChatTextField.gI().tfChat.name = Setup.inputTextAutoChat[1];
                    ChatTextField.gI().startChat2(Setup.gI, Setup.inputTextAutoChat[0]);
                }))
                //.addItem("Delay:\n" + (float)(Setup.delayAutoChat) / 1000 + " giây", new(() =>
                //{
                //    ChatTextField.gI().strChat = Setup.inputDelayAutoChat[0];
                //    ChatTextField.gI().tfChat.name = Setup.inputDelayAutoChat[1];
                //    ChatTextField.gI().startChat2(Setup.gI, string.Empty);
                //}))
                .addItem("Danh sách nội dung", new(Setup.showAutoChatPanel))
                .addItem(ifCondition: Setup.ChatList.Count > 0, "Xóa tất\ncả", new(() =>
                {
                    Setup.ChatList.Clear();
                    Setup.SaveData();
                    GameScr.info1.addInfo("Đã xóa toàn bộ nội dung đã lưu!", 0);
                }))
                .start();
        }
    }
}
