using Mod.Auto.AutoChat;
using Mod.Graphics;
using Mod.ModHelper;
using Mod.ModMenu;
using Mod.PickMob;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mod.Auto
{
    internal class AutoSpecialSkill : ThreadActionUpdate<AutoSpecialSkill>, IActionListener, IChatable
    {
        public override int Interval => 250;

        public static string specialSkillName;

        public static sbyte type = 1;

        public static bool openMax = false;

        public static int max = -1;

        public static int[] chiso = new int[2];

        private static AutoSpecialSkill instance;

        public static string caption = string.Empty;

        public static new AutoSpecialSkill gI()
        {
            return (instance != null) ? instance : (instance = new AutoSpecialSkill());
        }

        protected override void update()
        {
            //if (Char.myCharz().cPower < 10000000000L)
            //{
            //    GameScr.info1.addInfo("Cần 10 tỷ sức mạnh để mở.", 0);
            //    openMax = false;
            //    return;
            //}
            if (openMax && max == -1)
            {
                gI().toggle(false);
                if (!gI().IsActing)
                    GameScr.info1.addInfo("Tắt tự động mở nội tại ", 0);
                openMax = false;
                return;
            }
            Service.gI().speacialSkill(0);
            if (Panel.specialInfo.Contains(specialSkillName))
            {
                if (!openMax)
                {
                    gI().toggle(false);
                    GameScr.info1.addInfo("Xong", 0);
                    return;
                }
                int num = Panel.specialInfo.IndexOf("%");
                string text = Panel.specialInfo.Substring(0, num);
                int num2 = text.LastIndexOf(' ');
                string s = CutString(num2 + 1, num - 1, Panel.specialInfo);
                int num3 = int.Parse(s);
                if (num3 >= max)
                {
                    gI().toggle(false);
                    openMax = false;
                    GameScr.info1.addInfo("Xong", 0);
                    return;
                }
            }
            Thread.Sleep(250);
            Service.gI().confirmMenu(5, type);
            Thread.Sleep(250);
            Service.gI().confirmMenu(5, 0);
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    {
                        string text2 = (string)p;
                        int length2 = text2.Substring(0, text2.IndexOf('%')).LastIndexOf(' ');
                        specialSkillName = text2.Substring(0, length2);
                        gI().toggle(true);
                        type = (sbyte)idAction;
                        GameCanvas.panel.hide();
                        //new Thread(open).Start();
                        break;
                    }
                case 2:
                    {
                        string text = (string)p;
                        int length = text.Substring(0, text.IndexOf('%')).LastIndexOf(' ');
                        specialSkillName = text.Substring(0, length);
                        gI().toggle(true);
                        type = (sbyte)idAction;
                        GameCanvas.panel.hide();
                        //new Thread(open).Start();
                        break;
                    }

                case 3:
                    {
                        openMax = false;
                        MyVector myVector = new MyVector();
                        myVector.addElement(new Command("Mở Vip", gI(), 2, p));
                        myVector.addElement(new Command("Mở Thường", gI(), 1, p));
                        GameCanvas.menu.startAt(myVector, 3);
                        break;
                    }
                case 4:
                    {
                        string text3 = (string)p;
                        openMax = true;
                        int num = text3.IndexOf("đến ");
                        int length3 = text3.Substring(num + 4).IndexOf("%");
                        max = int.Parse(text3.Substring(num + 4, length3));
                        MyVector myVector2 = new MyVector();
                        myVector2.addElement(new Command("Mở Vip", gI(), 2, p));
                        myVector2.addElement(new Command("Mở Thường", gI(), 1, p));
                        GameCanvas.menu.startAt(myVector2, 3);
                        break;
                    }
                case 5:
                    {
                        string text4 = (string)p;
                        int length4 = text4.Substring(0, text4.IndexOf('%')).LastIndexOf(' ');
                        specialSkillName = text4.Substring(0, length4);
                        int num2 = text4.IndexOf("%");
                        int num3 = text4.IndexOf("đến ");
                        int start = text4.Substring(0, num2).LastIndexOf(' ');
                        int num4 = text4.LastIndexOf('%');
                        chiso[0] = int.Parse(CutString(start, num2 - 1, text4));
                        chiso[1] = int.Parse(CutString(num3 + 4, num4 - 1, text4));
                        string text5 = CutString(start, num4, text4);
                        caption = "Nhập chỉ số bạn muốn chọn trong khoảng " + text5;
                        MyVector myVector4 = new MyVector();
                        myVector4.addElement(new Command("Mở Thường", gI(), 6, 1));
                        myVector4.addElement(new Command("Mở Vip", gI(), 6, 2));
                        GameCanvas.menu.startAt(myVector4, 3);
                        break;
                    }
                case 6:
                    {
                        int num5 = (int)p;
                        type = (sbyte)num5;
                        GameCanvas.panel.chatTField = new ChatTextField();
                        GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                        GameCanvas.panel.chatTField.initChatTextField();
                        GameCanvas.panel.chatTField.strChat = string.Empty;
                        GameCanvas.panel.chatTField.tfChat.name = "Nhập chỉ số";
                        GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                        GameCanvas.panel.chatTField.startChat2(new AutoSpecialSkill(), caption);
                        //mPanel.startChat(1, mPanel.noitai);
                        break;
                    }
            }
        }

        public string CutString(int start, int end, string s)
        {
            string text = "";
            for (int i = start; i <= end; i++)
            {
                text += s[i];
            }
            return text;
        }

        public void onChatFromMe(string text, string to)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (to == caption)
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value != -1 && value >= chiso[0] && value <= chiso[1])
                        {
                            max = value;
                            openMax = true; 
                            gI().toggle(true);
                            GameCanvas.panel.hide();
                            //new Thread(AutoNoiTai.gI().open).Start();
                            GameCanvas.panel.chatTField.isShow = false;
                            GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                        }
                        else
                        {
                            GameCanvas.startOKDlg(caption);
                        }
                    }
                    catch (Exception)
                    {
                        //GameCanvas.panel.chatTField.isShow = false;
                        GameCanvas.startOKDlg(mResources.input_quantity_wrong);
                        //GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                        //return;
                    }
                }
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
