using System;

namespace Mod.Auto
{
    internal class AutoPlusPoint : IActionListener, IChatable
    {
        public static AutoPlusPoint gI { get; } = new AutoPlusPoint();

        private static string plusPointHPTitle = "Nhập số HP tăng thêm";
        private static string plusPointMPTitle = "Nhập số KI tăng thêm";
        private static string plusPointSDTitle = "Nhập số sức đánh tăng thêm";
        private static string plusPointDefTitle = "Nhập số giáp tăng thêm";

        public static bool isPlusPointHP;
        public static bool isPlusPointMP;
        public static bool isPlusPointSD;
        public static bool isPlusPointDef;

        public static int timePlus = 1000;

        public static long lastTimePlusHP;
        public static long lastTimePlusMP;
        public static long lastTimePlusSD;
        public static long lastTimePlusDef;

        public static int hpPlused;
        public static int mpPlused;
        public static int sdPlused;
        public static int defPlused;

        public static void update()
        {
            if (isPlusPointHP && mSystem.currentTimeMillis() - lastTimePlusHP > timePlus)
            {
                if (Char.myCharz().cHPGoc < hpPlused)
                {
                    if (Char.myCharz().cHPGoc <= hpPlused - 2000)
                        Service.gI().upPotential(0, 100);
                    if (Char.myCharz().cHPGoc <= hpPlused - 200)
                        Service.gI().upPotential(0, 10);
                    if (Char.myCharz().cHPGoc <= hpPlused - 20)
                        Service.gI().upPotential(0, 1);
                }
                else
                {
                    isPlusPointHP = false;
                    GameScr.info1.addInfo("Hoàn thành!", 0);
                    return;
                }
                lastTimePlusHP = mSystem.currentTimeMillis();
            }
            if (isPlusPointMP && mSystem.currentTimeMillis() - lastTimePlusMP > timePlus)
            {
                if (Char.myCharz().cMPGoc < mpPlused)
                {
                    if (Char.myCharz().cMPGoc <= mpPlused - 2000)
                        Service.gI().upPotential(1, 100);
                    if (Char.myCharz().cMPGoc <= mpPlused - 200)
                        Service.gI().upPotential(1, 10);
                    if (Char.myCharz().cMPGoc <= mpPlused - 20)
                        Service.gI().upPotential(1, 1);
                }
                else
                {
                    isPlusPointMP = false;
                    GameScr.info1.addInfo("Hoàn thành!", 0);
                    return;
                }
                lastTimePlusMP = mSystem.currentTimeMillis();
            }
            if (isPlusPointSD && mSystem.currentTimeMillis() - lastTimePlusSD > timePlus)
            {
                if (Char.myCharz().cDamGoc < sdPlused)
                {
                    if (Char.myCharz().cDamGoc < sdPlused)
                    {
                        if (Char.myCharz().cDamGoc <= sdPlused - 100)
                            Service.gI().upPotential(2, 100);
                        if (Char.myCharz().cDamGoc <= sdPlused - 10)
                            Service.gI().upPotential(2, 10);
                        if (Char.myCharz().cDamGoc <= sdPlused - 1)
                            Service.gI().upPotential(2, 1);
                    }
                    else
                    {
                        isPlusPointSD = false;
                        GameScr.info1.addInfo("Hoàn thành!", 0);
                        return;
                    }
                }
                else
                {
                    isPlusPointSD = false;
                    GameScr.info1.addInfo("Hoàn thành!", 0);
                    return;
                }
                lastTimePlusSD = mSystem.currentTimeMillis();
            }
            if (isPlusPointDef && mSystem.currentTimeMillis() - lastTimePlusDef > timePlus)
            {
                if (Char.myCharz().cDefGoc < defPlused)
                {
                    if (Char.myCharz().cDefGoc < defPlused)
                    {
                        if (Char.myCharz().cDefGoc <= defPlused - 100)
                            Service.gI().upPotential(3, 100);
                        if (Char.myCharz().cDefGoc <= defPlused - 10)
                            Service.gI().upPotential(3, 10);
                        if (Char.myCharz().cDefGoc <= defPlused - 1)
                            Service.gI().upPotential(3, 1);
                    }
                    else
                    {
                        isPlusPointDef = false;
                        GameScr.info1.addInfo("Hoàn thành!", 0);
                        return;
                    }
                }
                else
                {
                    isPlusPointDef = false;
                    GameScr.info1.addInfo("Hoàn thành!", 0);
                    return;
                }
                lastTimePlusDef = mSystem.currentTimeMillis();
            }
        }

        public static bool isAutoPlusPoint()
        {
            return isPlusPointHP || isPlusPointMP || isPlusPointSD || isPlusPointDef;
        }

        public static void startChat(string name, string to)
        {
            GameCanvas.panel.chatTField = new ChatTextField();
            GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
            GameCanvas.panel.chatTField.initChatTextField();
            GameCanvas.panel.chatTField.strChat = string.Empty;
            GameCanvas.panel.chatTField.tfChat.name = name;
            GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
            GameCanvas.panel.chatTField.startChat2(new AutoPlusPoint(), to);
        }
        
        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    startChat("HP", plusPointHPTitle);
                    break;
                case 2:
                    startChat("KI", plusPointMPTitle);
                    break;
                case 3:
                    startChat("SĐ", plusPointSDTitle);
                    break;
                case 4:
                    startChat("Giáp", plusPointDefTitle);
                    break;
            }
        }

        public void onChatFromMe(string text, string to)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (to == plusPointHPTitle)
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value < 0 || value > 1000000000) throw new Exception();
                        hpPlused = Char.myCharz().cHPGoc + value;
                        isPlusPointHP = true;
                        GameScr.info1.addInfo("Auto cộng HP bắt đâu!", 0);
                    }
                    catch
                    {
                        GameCanvas.startOKDlg($"Chỉ số HP tăng thêm phải lớn hơn 0 và nhỏ hơn {mSystem.numberTostring(1000000000)}!");
                    }
                }
                else if (to == plusPointMPTitle)
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value < 0 || value > 1000000000) throw new Exception();
                        mpPlused = Char.myCharz().cMPGoc + value;
                        isPlusPointMP = true;
                        GameScr.info1.addInfo("Auto cộng KI bắt đâu!", 0);
                    }
                    catch
                    {
                        GameCanvas.startOKDlg($"Chỉ số KI tăng thêm phải lớn hơn 0 và nhỏ hơn {mSystem.numberTostring(1000000000)}!");
                    }
                }
                else if (to == plusPointSDTitle)
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value < 0 || value > 1000000) throw new Exception();
                        sdPlused = Char.myCharz().cDamGoc + value;
                        isPlusPointSD = true;
                        GameScr.info1.addInfo("Auto cộng sức đánh bắt đâu!", 0);
                    }
                    catch
                    {
                        GameCanvas.startOKDlg($"Chỉ số sức đánh tăng thêm phải lớn hơn 0 và nhỏ hơn {mSystem.numberTostring(1000000)}!");
                    }
                }
                else if (to == plusPointDefTitle)
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value < 0 || value > 10000) throw new Exception();
                        defPlused = Char.myCharz().cDefGoc + value;
                        isPlusPointDef = true;
                        GameScr.info1.addInfo("Auto cộng giáp bắt đâu!", 0);
                    }
                    catch
                    {
                        GameCanvas.startOKDlg($"Chỉ số giáp tăng thêm phải lớn hơn 0 và nhỏ hơn {mSystem.numberTostring(10000)}!");
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
