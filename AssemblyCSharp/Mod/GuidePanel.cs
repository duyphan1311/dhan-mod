using Mod.CustomPanel;
using Mod.ModHelper.CommandMod.Chat;
using System.Collections.Generic;

namespace Mod
{
    internal class GuidePanel
    {
        public static readonly Dictionary<int, string[]> chatCommands = new Dictionary<int, string[]>()
        {
            { 1, new string[]{"ak", "Bât/Tắt tự động đánh"} },
            { 2, new string[]{"ts", "Bât/Tắt tàn sát"} },
            { 3, new string[]{"add", "Thêm/Xoá quái/item vào danh sách"} },
            { 4, new string[]{"addt", "Thêm/Xoá loại quái/item vào danh sách" } },
            { 5, new string[]{"clrm", "Đặt lại danh sách quái khi tàn sát về mặc định"} },
            { 6, new string[]{"vdh", "Bât/Tắt vượt địa hình"} },
            { 7, new string[]{"nsq", "Bât/Tắt né siêu quái"} },
            { 8, new string[]{"anhat", "Bât/Tắt tự động nhặt"} },
            { 9, new string[]{"itm", "Bât/Tắt lọc không nhặt vật phẩm của người khác" } },
            { 10, new string[]{"sln", "Bât/Tắt giới hạn số lần nhặt"} },
            { 11, new string[]{"clri", "Đặt danh sách nhặt về mặc định"} },
            { 12, new string[]{"cnn", "Bât/Tắt chỉ nhặt ngọc"} },
            { 13, new string[]{"skill", "Thêm/Xoá kỹ năng đang trỏ vào danh sách sử dụng khi tàn sát"} },
            { 14, new string[]{"clrs", "Đặt danh sách kỹ năng sử dụng khi tàn sát về mặc định" } },
            { 15, new string[]{"blocki", "Thêm/Xoá item vào danh sách không tự động nhặt item" } },
            { 16, new string[]{"set", "Mở menu set đồ"} },
            { 17, new string[]{"tele", "Mở menu teleport"} },
            { 18, new string[]{"s", "Điều chỉnh tốc độ chạy X", "X"} },
            { 19, new string[]{"td", "Điều chỉnh tốc độ game X", "X"} },
            { 20, new string[]{"hsme", "Bât/Tắt tự động sử kỹ năng hồi sinh của namec"} },
            { 21, new string[]{"csb", "Mở menu capsule"} },
            { 22, new string[]{"bt", "Hợp thể sử dụng bông tai porata"} },
            { 23, new string[]{"tele", "Dịch chuyển tức thời đến người chơi có ID: X", "X"} },
            { 24, new string[]{"out", "Đổi tài khoản"} },
            { 25, new string[]{"atf", "Bât/Tắt tự động cờ chống PK"} },
            { 26, new string[]{"abf", "Điều chỉnh hp và mp khi up đệ", "X", "Y"} },
            { 27, new string[]{"k", "Đổi khu X", "X"} },
            { 28, new string[]{"ahs", "Bât/Tắt tự động hồi sinh"} },
            { 29, new string[]{"u", "Dịch lên", "X"} },
            { 30, new string[]{"d", "Dịch xuống", "X"} },
            { 31, new string[]{"r", "Dịch phải", "X"} },
            { 32, new string[]{"l", "Dịch trái", "X"} },
            { 33, new string[]{"xmp", "Mở menu XMap"} },
            { 34, new string[]{"xmp", "Di chuyển đến map X", "X"} },
            { 35, new string[]{"8sk", "Load lại các ô kỹ năng"} },
            { 36, new string[]{"nr", "Bât/Tắt tự động gọi rồng"} },
            { 37, new string[]{"f", "Bật cờ thứ X", "X"} },
            //{ 38, new string[]{"buy", "Tự động mua vật phẩm ID: x, số lượng: Y và khoảng cách mỗi lần mua 100ms", "X", "Y"} },
            //{ 39, new string[]{"buy", "Tự động mua vật phẩm ID: x, số lượng: Y và khoảng cách mỗi lần mua: Z", "X", "Y", "Z"} },
        };

        public static readonly Dictionary<int, string[]> keyCommands = new Dictionary<int, string[]>()
        {
            { 1, new string[]{"Z", "Mở menu chức năng"} },
            { 2, new string[]{"V", "Mở tab điều chỉnh"} },
            { 3, new string[]{"B", "Bật/Tắt tự động tìm boss"} },
            { 4, new string[]{"Q", "Mở menu teleport" } },
            { 5, new string[]{"N", "Mở danh sách NPC trong map" } },
            { 6, new string[]{"C", "Sử dụng capsule" } },
            { 7, new string[]{"F", "Hợp thể sử dụng Porata" } },
            { 8, new string[]{"W", "Dịch lên 50" } },
            { 9, new string[]{"S", "Dịch xuống 50" } },
            { 10, new string[]{"A", "Dịch trái 50" } },
            { 11, new string[]{"D", "Dịch phải 50" } },
            { 12, new string[]{"J", "Dịch chuyển map trái" } },
            { 13, new string[]{"K", "Dịch chuyển map giữa" } },
            { 14, new string[]{"L", "Dịch chuyển map phải" } },
            { 15, new string[]{"U", "Bật cờ đen" } },
            { 16, new string[]{"O", "Tắt cờ" } },
            { 17, new string[]{"T", "Tàn sát" } },
            { 18, new string[]{"Y", "Mở tab tin nhắn" } },
            { 19, new string[]{"P", "Mở thông tin đệ tử" } },
            { 20, new string[]{"G", "Mời giao dịch nhanh" } },
            { 21, new string[]{"H", "Mở danh sách bạn bè" } },
            { 22, new string[]{"X", "Mở menu XMap" } },
            { 23, new string[]{"M", "Mở danh sách khu" } },
            { 24, new string[]{"`", "Mở menu set đồ" } },
        };

        [ChatCommand("guide")]
        public static void showTabGuidePanel() => CustomPanelMenu.show(setTabGuide, null, paintTabGuideHeader, paintTabGuide);

        public static void setTabGuide(Panel panel)
        {
            panel.ITEM_HEIGHT = 16;

            panel.currentListLength = chatCommands.Count + keyCommands.Count;

            panel.selected = GameCanvas.isTouch ? (-1) : 0;

            panel.cmyLim = panel.currentListLength * panel.ITEM_HEIGHT - panel.hScroll;
            if (panel.cmyLim < 0) panel.cmyLim = 0;

            panel.cmy = panel.cmtoY = panel.cmyLast[panel.currentTabIndex];
            if (panel.cmy < 0) panel.cmy = panel.cmtoY = 0;
            if (panel.cmy > panel.cmyLim) panel.cmy = panel.cmtoY = panel.cmyLim;
        }

        public static void paintTabGuideHeader(Panel panel, mGraphics g) => PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Hướng dẫn sử dụng mod");

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
                    if (chatCommands[i + 1].Length == 2)
                        mFont.tahoma_7b_dark.drawString(g, $"  {chatCommands[i + 1][0]}: {chatCommands[i + 1][1]}", panel.xScroll + 5, num2 + 6, mFont.LEFT);
                    else if (chatCommands[i + 1].Length == 3)
                    {
                        mFont.tahoma_7b_dark.drawString(g, $"  {chatCommands[i + 1][0]}", panel.xScroll + 5, num2 + 6, mFont.LEFT);
                        int x = mFont.tahoma_7b_dark.getWidth($"  {chatCommands[i + 1][0]}");
                        mFont.tahoma_7b_red.drawString(g, chatCommands[i + 1][2], panel.xScroll + 5 + x, num2 + 6, mFont.LEFT);
                        int x2 = mFont.tahoma_7b_red.getWidth($"{chatCommands[i + 1][2]}");
                        mFont.tahoma_7b_dark.drawString(g, $": {chatCommands[i + 1][1]}", panel.xScroll + 5 + x + x2, num2 + 6, mFont.LEFT);
                    }
                    else
                    {
                        mFont.tahoma_7b_dark.drawString(g, $"  {chatCommands[i + 1][0]}", panel.xScroll + 5, num2 + 6, mFont.LEFT);
                        int x = mFont.tahoma_7b_dark.getWidth($"  {chatCommands[i + 1][0]}");
                        mFont.tahoma_7b_red.drawString(g, chatCommands[i + 1][2], panel.xScroll + 5 + x, num2 + 6, mFont.LEFT);
                        int x2 = mFont.tahoma_7b_dark.getWidth($"{chatCommands[i + 1][2]}");
                        mFont.tahoma_7b_yellow.drawString(g, $" {chatCommands[i + 1][3]}", panel.xScroll + 5 + x + x2, num2 + 6, mFont.LEFT);
                        int x3 = mFont.tahoma_7b_yellow.getWidth($" {chatCommands[i + 1][3]}");
                        mFont.tahoma_7b_dark.drawString(g, $": {chatCommands[i + 1][1]}", panel.xScroll + 5 + x + x2 + x3, num2 + 6, mFont.LEFT);
                    }
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
                    mFont.tahoma_7b_dark.drawString(g, $"  {keyCommands[i + 1][0]}: {keyCommands[i + 1][1]}", panel.xScroll + 5, num2 + 6, mFont.LEFT);
                }
            }
            panel.paintScrollArrow(g);
        }
    }
}
