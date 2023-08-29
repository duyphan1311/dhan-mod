using Mod.CustomPanel;
using Mod.ModHelper.CommandMod.Chat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Mod
{
    internal class GuidePanel
    {
        private static readonly Dictionary<int, string[]> chatCommands = new Dictionary<int, string[]>()
        {
            { 1, new string[]{"Nhập số sao pha lê hoá tối đa", "Số sao"} },
            { 2, new string[]{"Nhập id thỏi vàng", "ID thỏi vàng"} },
            { 3, new string[]{"Nhập số vàng bắt đầu bán", "Số vàng"} },
            { 4, new string[]{"Nhập id npc nhận vàng tại đảo kame", "ID NPC" } },
            { 5, new string[]{"Nhập menu confirm nhận vàng", "Menu confirm" } },
            { 6, new string[]{"Nhập menu confirm nhận ngọc", "Menu confirm" } },
            { 7, new string[]{ "Nhập menu confirm mở chỉ số bông tai", "Menu confirm" } },
        };

        private static readonly Dictionary<int, string[]> keyCommands = new Dictionary<int, string[]>()
        {
            { 1, new string[]{"Nhập số sao pha lê hoá tối đa", "Số sao 1"} },
            { 2, new string[]{"Nhập id thỏi vàng", "ID thỏi vàng 1"} },
            { 3, new string[]{"Nhập số vàng bắt đầu bán", "Số vàng 1"} },
            { 4, new string[]{"Nhập id npc nhận vàng tại đảo kame", "ID NPC 1" } },
            { 5, new string[]{"Nhập menu confirm nhận vàng", "Menu confirm 1" } },
            { 6, new string[]{"Nhập menu confirm nhận ngọc", "Menu confirm 1" } },
            { 7, new string[]{ "Nhập menu confirm mở chỉ số bông tai", "Menu confirm 1" } },
        };

        [ChatCommand("guide")]
        private static void showTabGuidePanel() => CustomPanelMenu.show(setTabGuide, null, paintTabGuideHeader, paintTabGuide);

        public static void setTabGuide(Panel panel)
        {
            panel.ITEM_HEIGHT = 16;

            panel.currentListLength = chatCommands.Count + keyCommands.Count + 3;

            panel.selected = GameCanvas.isTouch ? (-1) : 0;

            panel.cmyLim = panel.currentListLength * panel.ITEM_HEIGHT - panel.hScroll;
            if (panel.cmyLim < 0) panel.cmyLim = 0;

            panel.cmy = panel.cmtoY = panel.cmyLast[panel.currentTabIndex];
            if (panel.cmy < 0) panel.cmy = panel.cmtoY = 0;
            if (panel.cmy > panel.cmyLim) panel.cmy = panel.cmtoY = panel.cmyLim;
        }

        private static void paintTabGuideHeader(Panel panel, mGraphics g) => PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Hướng dẫn sử dụng mod");

        public static void paintTabGuide(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            mFont.tahoma_7b_dark.drawString(g, "Lệnh cơ bản: (X, Y là các giá trị thay đổi)", panel.xScroll + 5, panel.yScroll + 6, mFont.LEFT);
            for (int i = 0; i < chatCommands.Count; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + (i + 1) * 15;
                int num3 = panel.wScroll - 1;
                int num4 = panel.ITEM_HEIGHT - 1;
                if (num2 - panel.cmy <= panel.yScroll + panel.hScroll && num2 - panel.cmy >= panel.yScroll - panel.ITEM_HEIGHT)
                {
                    mFont.tahoma_7b_dark.drawString(g, " " + chatCommands[i + 1][1], panel.xScroll + 5, num2 + 6, mFont.LEFT);
                }
            }
            mFont.tahoma_7b_dark.drawString(g, "Phím tắt", panel.xScroll + 5, panel.yScroll + (chatCommands.Count + 2) * 15 + 6, mFont.LEFT);
            for (int i = 0; i < keyCommands.Count; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + (chatCommands.Count + 2) * 15 + (i + 1) * 15;
                int num3 = panel.wScroll - 1;
                int num4 = panel.ITEM_HEIGHT - 1;
                if (num2 - panel.cmy <= panel.yScroll + panel.hScroll && num2 - panel.cmy >= panel.yScroll - panel.ITEM_HEIGHT)
                {
                    mFont.tahoma_7b_dark.drawString(g, " " + keyCommands[i + 1][1], panel.xScroll + 5, num2 + 6, mFont.LEFT);
                }
            }
            panel.paintScrollArrow(g);
        }
    }
}
