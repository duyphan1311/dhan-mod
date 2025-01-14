using Mod.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mod.OnScreenPaint
{
    public class LogKillBoss
    {
        public string name;

        public string bossName;

        public static List<LogKillBoss> listLogs = new List<LogKillBoss>();

        public static bool isEnabled;

        public static int distanceBetweenLines = 8;

        static int offset = 0;

        public static int x = 15 - 9;

        public static int y = 50;

        static int maxLength = 0;

        static int lastLog = -1;

        public static bool isCollapsed;

        public static readonly int ORIGINAL_X = 6;

        public static readonly int MAX_LOG_DISPLAY = 5;

        static readonly int MAX_LOG = 100;

        static readonly string LIST_LOG = "Log boss killed";

        static int titleWidth;

        static bool isOffsetX;

        public DateTime logTime;

        LogKillBoss(string name, string bossName)
        {
            this.name = name;
            this.bossName = bossName;
            logTime = DateTime.Now;
        }

        public static void update()
        {
            x = ORIGINAL_X;
            if (ListBossInMap.isEnabled)
            {
                if (ListBossInMap.isCollapsed || ListBossInMap.listBoss.Count == 0)
                {
                    if (y != ListBossInMap.y + ListBossInMap.distanceBetweenLines + 3)
                        y = ListBossInMap.y + ListBossInMap.distanceBetweenLines + 3;
                }
                else if (y != ListBossInMap.y + 5 + ListBossInMap.distanceBetweenLines * ListBossInMap.listBoss.Count + 10)
                    y = ListBossInMap.y + 5 + ListBossInMap.distanceBetweenLines * ListBossInMap.listBoss.Count + 10;
            }
            else if (ListCharsInMap.isEnabled)
            {
                if (ListCharsInMap.isCollapsed || ListCharsInMap.listChars.Count == 0)
                {
                    if (y != ListCharsInMap.y + ListCharsInMap.distanceBetweenLines + 3)
                        y = ListCharsInMap.y + ListCharsInMap.distanceBetweenLines + 3;
                }
                else if (y != ListCharsInMap.y + 5 + ListCharsInMap.distanceBetweenLines * Mathf.Clamp(ListCharsInMap.listChars.Count, 0, ListCharsInMap.MAX_CHAR) + 10)
                    y = ListCharsInMap.y + 5 + ListCharsInMap.distanceBetweenLines * Mathf.Clamp(ListCharsInMap.listChars.Count, 0, ListCharsInMap.MAX_CHAR) + 10;
            }
            else if (Boss.isEnabled && Boss.listBosses.Count > 0)
            {
                if (Boss.isCollapsed)
                {
                    if (y != Boss.y + Boss.distanceBetweenLines + 3)
                        y = Boss.y + Boss.distanceBetweenLines + 3;
                }
                else if (y != Boss.y + 5 + Boss.distanceBetweenLines * Mathf.Clamp(Boss.listBosses.Count, 0, Boss.MAX_BOSS_DISPLAY) + 10)
                    y = Boss.y + 5 + Boss.distanceBetweenLines * Mathf.Clamp(Boss.listBosses.Count, 0, Boss.MAX_BOSS_DISPLAY) + 10;
            }
            else if (y != 50)
                y = 50;
            if (!isEnabled)
                return;
            if (offset >= listLogs.Count - MAX_LOG_DISPLAY)
            {
                if (listLogs.Count - MAX_LOG_DISPLAY > 0)
                    offset = listLogs.Count - MAX_LOG_DISPLAY;
                else
                    offset = 0;
            }
            if (GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1, maxLength, 8 * MAX_LOG_DISPLAY))
            {
                if (GameCanvas.pXYScrollMouse > 0)
                    if (offset < listLogs.Count - MAX_LOG_DISPLAY)
                        offset++;
                if (GameCanvas.pXYScrollMouse < 0)
                    if (offset > 0)
                        offset--;
            }
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listLogs.Count > MAX_LOG_DISPLAY)
                x += scrollBarWidth;
        }

        public static void AddLog(string chatVip)
        {
            if (chatVip.StartsWith("BOSS") || chatVip.ToLower().Contains("nhân bản"))
                return;
            if (Boss.strBossHasBeenKilled.Any(chatVip.Contains))
            {
                Boss.strBossHasBeenKilled.ForEach(s => chatVip = chatVip.Replace(s, "|"));
                string[] array = chatVip.Split('|');
                listLogs.Add(new LogKillBoss(array[0].Trim(), array[1].Trim()));
                if (listLogs.Count > MAX_LOG)
                    listLogs.RemoveAt(0);
            }
        }

        public override string ToString()
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(logTime);
            string result = $"{name} đã tiêu diệt {bossName} - ";
            int hours = (int)System.Math.Floor((decimal)timeSpan.TotalHours);
            if (hours > 0)
                result += $"{hours}h";
            if (timeSpan.Minutes > 0)
                result += $"{timeSpan.Minutes}m";
            result += $"{timeSpan.Seconds}s";
            return result;
        }

        public string ToString(bool enableRichText)
        {
            if (!enableRichText)
                return ToString();
            TimeSpan timeSpan = DateTime.Now.Subtract(logTime);
            string colorName = "yellow";
            string colorBossName = "red";
            string result = $"<color={colorName}>{name}</color> đã tiêu diệt <color={colorBossName}>{bossName}</color> - ";
            int hours = (int)System.Math.Floor((decimal)timeSpan.TotalHours);
            if (hours > 0)
                result += $"<color=orange>{hours}</color>h";
            if (timeSpan.Minutes > 0)
                result += $"<color=orange>{timeSpan.Minutes}</color>m";
            result += $"<color=orange>{timeSpan.Seconds}</color>s";
            return result;
        }

        public static void paint(mGraphics g)
        {
            if (!isEnabled)
                return;
            if (listLogs.Count <= 0)
                return;
            if (offset >= listLogs.Count - MAX_LOG_DISPLAY)
            {
                if (listLogs.Count - MAX_LOG_DISPLAY > 0)
                    offset = listLogs.Count - MAX_LOG_DISPLAY;
                else
                    offset = 0;
            }
            maxLength = 0;
            if (!isCollapsed)
            {
                PaintListLogs(g);
                PaintScroll(g);
            }
            PaintRect(g);
        }

        static void PaintListLogs(mGraphics g)
        {
            List<KeyValuePair<string, GUIStyle>> logDescriptions = new List<KeyValuePair<string, GUIStyle>>();
            int start = 0;
            if (listLogs.Count > MAX_LOG_DISPLAY)
                start = listLogs.Count - MAX_LOG_DISPLAY;
            for (int i = start - offset; i < listLogs.Count - offset; i++)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 6 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.UpperRight,
                    richText = true
                };
                LogKillBoss log = listLogs[i];
                string logDesc = $"{i + 1}. {log.ToString(true)}";
                logDescriptions.Add(new KeyValuePair<string, GUIStyle>(logDesc, style));
                maxLength = Math.max(maxLength, Utilities.getWidth(logDescriptions[i - start + offset].Value, logDesc));

            }
            FillBackground(g);
            int xDraw = GameCanvas.w - x - maxLength;
            for (int i = start - offset; i < listLogs.Count - offset; i++)
            {
                int yDraw = y + distanceBetweenLines * (i - start + offset);
                int offsetPaint = 0;
                LogKillBoss log = listLogs[i];
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7))
                    g.setColor(new Color(.2f, .2f, .2f, .7f));
                g.fillRect(xDraw, yDraw + 1, maxLength, 7);
                if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7))
                    CustomGraphics.fillRect(xDraw + 1, yDraw + 7, (maxLength - 2) * mGraphics.zoomLevel + 2, 1, Color.white);
                g.drawString(logDescriptions[i - start + offset].Key, -x - offsetPaint, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), logDescriptions[i - start + offset].Value);
            }
        }

        private static void FillBackground(mGraphics g)
        {
            if (!isCollapsed && listLogs.Count > 0)
            {
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
                getScrollBar(out int scrollBarWidth, out _, out _);
                if (listLogs.Count <= MAX_LOG_DISPLAY)
                    scrollBarWidth = 0;
                int w = maxLength + 5 + (scrollBarWidth > 0 ? (scrollBarWidth + 2) : 0);
                int h = distanceBetweenLines * Math.min(MAX_LOG_DISPLAY, listLogs.Count) + 7;
                g.fillRect(GameCanvas.w - x - maxLength - 3, y - 5, w, h);
            }
        }

        static void PaintScroll(mGraphics g)
        {
            if (listLogs.Count > MAX_LOG_DISPLAY)
            {
                getButtonUp(out int buttonUpX, out int buttonUpY);
                getButtonDown(out int buttonDownX, out int buttonDownY);
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                g.fillRect(buttonUpX, buttonUpY, 9, scrollBarHeight + 6 * 2);
                g.drawRegion(Mob.imgHP, 0, (offset < listLogs.Count - MAX_LOG_DISPLAY ? 18 : 54), 9, 6, 1, buttonUpX, buttonUpY, 0);
                g.drawRegion(Mob.imgHP, 0, (offset > 0 ? 18 : 54), 9, 6, 0, buttonDownX, buttonDownY, 0);
                //draw thumb
                g.setColor(new Color(.2f, .2f, .2f, .7f));
                g.fillRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listLogs.Count * (listLogs.Count - offset - MAX_LOG_DISPLAY)), scrollBarWidth, scrollBarThumbHeight);
                g.setColor(new Color(.7f, .7f, 0f, 1f));
                g.drawRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listLogs.Count * (listLogs.Count - offset - MAX_LOG_DISPLAY)), scrollBarWidth - 1, scrollBarThumbHeight - 1);
            }
        }

        static void PaintRect(mGraphics g)
        {
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listLogs.Count <= MAX_LOG_DISPLAY)
                scrollBarWidth = 0;
            int w = maxLength + 5 + (scrollBarWidth > 0 ? (scrollBarWidth + 2) : 0);
            int h = distanceBetweenLines * Math.min(MAX_LOG_DISPLAY, listLogs.Count) + 7;
            string str = $"<color=white>{LIST_LOG}</color>";
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 7 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperRight,
                richText = true
            };
            style.normal.textColor = Color.white;
            titleWidth = Utilities.getWidth(style, str);
            g.setColor(new Color(.2f, .2f, .2f, .7f));
            g.fillRect(GameCanvas.w - x - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8);
            if (GameCanvas.isMouseFocus(GameCanvas.w - x - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
            {
                g.setColor(style.normal.textColor);
                g.fillRect(GameCanvas.w - x - titleWidth + scrollBarWidth, y - 1, titleWidth - 1, 1);
            }
            g.drawString(str, -x + scrollBarWidth, y - distanceBetweenLines - 2, style);
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            g.drawRegion(Mob.imgHP, 0, 18, 9, 6, (isCollapsed ? 5 : 4), collapseButtonX, collapseButtonY, 0);
            if (isCollapsed || listLogs.Count <= 0)
                return;
            g.setColor(Color.yellow);
            g.fillRect(GameCanvas.w - x - maxLength - 3, y - 5, w - titleWidth - 9 - (scrollBarWidth > 0 ? 2 : 0), 1);
            g.fillRect(GameCanvas.w - x + scrollBarWidth, y - 5, 3 + (scrollBarWidth > 0 ? 1 : 0), 1);
            g.fillRect(GameCanvas.w - x - maxLength - 3, y - 5, 1, h);
            g.fillRect(GameCanvas.w - x - maxLength - 3 + w, y - 5, 1, h + 1);
            g.fillRect(GameCanvas.w - x - maxLength - 3, y - 5 + h, w + 1, 1);
        }

        public static void updateTouch()
        {
            if (!isEnabled)
                return;
            try
            {
                if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                    return;
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                getCollapseButton(out int collapseButtonX, out int collapseButtonY);
                if (GameCanvas.isPointerHoldIn(collapseButtonX, collapseButtonY, 9, 6) || GameCanvas.isPointerHoldIn(GameCanvas.w - x - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        isCollapsed = !isCollapsed;
                        Utilities.saveRMSBool("isCollapsedLogKillBoss", isCollapsed);
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
                if (listLogs.Count > MAX_LOG_DISPLAY)
                {
                    getButtonUp(out int buttonUpX, out int buttonUpY);
                    if (GameCanvas.isPointerMove && GameCanvas.isPointerDown && GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, scrollBarWidth, scrollBarHeight))
                    {
                        float increment = scrollBarHeight / (float)listLogs.Count;
                        float newOffset = (GameCanvas.pyMouse - buttonUpY) / increment;
                        if (float.IsNaN(newOffset))
                            return;
                        offset = Mathf.Clamp(listLogs.Count - Mathf.RoundToInt(newOffset), 0, listLogs.Count - MAX_LOG_DISPLAY);
                        return;
                    }
                    if (GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, 9, 6))
                    {
                        GameCanvas.isPointerJustDown = false;
                        GameScr.gI().isPointerDowning = false;
                        if (GameCanvas.isPointerClick)
                        {
                            if (offset < listLogs.Count - MAX_LOG_DISPLAY)
                                offset++;
                        }
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                    getButtonDown(out int buttonDownX, out int buttonDownY);
                    if (GameCanvas.isPointerHoldIn(buttonDownX, buttonDownY, 9, 6))
                    {
                        GameCanvas.isPointerJustDown = false;
                        GameScr.gI().isPointerDowning = false;
                        if (GameCanvas.isPointerClick)
                        {
                            if (offset > 0)
                                offset--;
                        }
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                }
            }
            catch (Exception) { }
        }

        static void getButtonUp(out int buttonUpX, out int buttonUpY)
        {
            buttonUpX = GameCanvas.w - x + 2;
            buttonUpY = y + 1;
        }

        static void getButtonDown(out int buttonDownX, out int buttonDownY)
        {
            buttonDownX = GameCanvas.w - x + 2;
            buttonDownY = y + 2 + distanceBetweenLines * (MAX_LOG_DISPLAY - 1);
        }

        static void getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight)
        {
            scrollBarWidth = 9;
            scrollBarHeight = MAX_LOG_DISPLAY * distanceBetweenLines - 1 - 6 * 2;
            scrollBarThumbHeight = Mathf.CeilToInt((float)MAX_LOG_DISPLAY / listLogs.Count * scrollBarHeight);
        }

        static void getCollapseButton(out int collapseButtonX, out int collapseButtonY)
        {
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listLogs.Count <= MAX_LOG_DISPLAY)
                scrollBarWidth = 0;
            collapseButtonX = GameCanvas.w - x - titleWidth + scrollBarWidth - 8;
            collapseButtonY = y - distanceBetweenLines + 1;
        }

        public static void SaveData()
        {
            try
            {
                Utilities.saveRMSBool("isCollapsedLogKillBoss", isCollapsed);
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void LoadData()
        {
            try
            {
                isCollapsed = Utilities.loadRMSBool("isCollapsedLogKillBoss");
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void setState(bool value) => isEnabled = value;
    }
}