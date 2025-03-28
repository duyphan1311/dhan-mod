﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod.Info
{
    public class CharInfo
    {
        public static bool isEnabled;

        public static int distanceBetweenLines = 9;

        public static int x = 15 - 9;

        public static int y = 55;

        public static bool isCollapsed;

        static readonly string title = "Sư phụ:";

        static int titleWidth;

        static int maxLength = 0;

        public static List<string> lines = new List<string>();

        public static void update()
        {
            if (!isCollapsed)
            {
                y = 55;
                if (y != y + 5 + distanceBetweenLines * lines.Count)
                    y = y + 5 + distanceBetweenLines * lines.Count;
            }
            else if (y != 55)
                y = 55;
            if (!isEnabled)
                return;
            lines.Clear();
            lines.Add($" <color=orange>HP: {formatHP(Char.myCharz())}</color> - <color=orange>MP: {formatMP(Char.myCharz())}</color>");
            lines.Add($" <color=orange>Sức mạnh: <color=cyan>{NinjaUtil.getMoneys(Char.myCharz().cPower)}</color></color>  - <color=orange>Sức đánh: <color=red>{NinjaUtil.getMoneys(Char.myCharz().cDamFull)}</color></color>");
            lines.Add($" <color=orange>Tiềm năng: <color=cyan>{NinjaUtil.getMoneys(Char.myCharz().cTiemNang)}</color></color> - <color=orange>Thể lực: {formatStamina(Char.myCharz())}</color>");
        }

        static string formatHP(Char ch)
        {
            long hp = ch.cHP;
            long hpFull = ch.cHPFull;
            float ratio = hp / (float)hpFull;
            Color color = new Color(Mathf.Clamp(2 - ratio * 2, 0, 1), Mathf.Clamp(ratio * 2, 0, 1), 0);
            string hexColor = $"#{(int)(color.r * 255):x2}{(int)(color.g * 255):x2}{(int)(color.b * 255):x2}{(int)(color.a * 255):x2}";
            return $"<color={hexColor}>{NinjaUtil.getMoneys(ch.cHP)}</color>/<color=lime>{NinjaUtil.getMoneys(ch.cHPFull)}</color><color=white> (<color={hexColor}>{Mathf.Round(ratio * 100f)}%</color>)</color>";
        }

        static string formatMP(Char ch)
        {
            long mp = ch.cMP;
            long mpFull = ch.cMPFull;
            float ratio = mp / (float)mpFull;
            Color startColor = new Color(0f, 128f / 255f, 255f / 255f);
            Color color = ratio >= 0.5f ? Color.Lerp(startColor, Color.yellow, Mathf.Clamp(2 - ratio * 2, 0, 1)) : Color.Lerp(Color.red, Color.yellow, Mathf.Clamp(ratio * 2, 0, 1));
            string hexColor = $"#{(int)(color.r * 255):x2}{(int)(color.g * 255):x2}{(int)(color.b * 255):x2}{(int)(color.a * 255):x2}";
            return $"<color={hexColor}>{NinjaUtil.getMoneys(ch.cMP)}</color>/<color=#0080ffff>{NinjaUtil.getMoneys(ch.cMPFull)}</color><color=white> (<color={hexColor}>{Mathf.Round(ratio * 100f)}%</color>)</color>";
        }

        static string formatStamina(Char ch)
        {
            int stamina = ch.cStamina;
            int maxStamina = ch.cMaxStamina;
            float ratio = stamina / (float)maxStamina;
            Color color = Color.Lerp(Color.yellow, Color.red, 1 - ratio);
            string hexColor = $"#{(int)(color.r * 255):x2}{(int)(color.g * 255):x2}{(int)(color.b * 255):x2}{(int)(color.a * 255):x2}";
            return $"<color={hexColor}>{NinjaUtil.getMoneys(ch.cStamina)}</color>/<color=yellow>{NinjaUtil.getMoneys(ch.cMaxStamina)}</color><color=white> (<color={hexColor}>{Mathf.Round(ratio * 100f)}%</color>)</color>";
        }

        public static void Paint(mGraphics g)
        {
            if (!isEnabled)
                return;

            if (!isCollapsed)
                paintInfoChar(g);
            PaintRect(g);
        }

        static void paintInfoChar(mGraphics g)
        {
            GUIStyle[] styles = new GUIStyle[lines.Count];
            for (int i = 0; i < lines.Count; i++)
            {
                styles[i] = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperLeft,
                    fontSize = 7 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Bold,
                    richText = true,
                };
                int length = Utilities.getWidth(styles[i], lines[i]);
                maxLength = Math.max(length, maxLength);
            }
            FillBackground(g);
            int xDraw = x;
            for (int i = 0; i < lines.Count; i++)
            {
                int yDraw = y - distanceBetweenLines * i + 2 - i;
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                g.fillRect(xDraw, GameCanvas.h - (mGraphics.zoomLevel - 3 + yDraw - 1), maxLength, 9);
                g.drawString(lines[i], x, GameCanvas.h - (mGraphics.zoomLevel - 3 + yDraw), styles[i]);
            }
        }

        static void PaintRect(mGraphics g)
        {
            int w = maxLength + 5;
            int h = distanceBetweenLines * lines.Count + 9;
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 7 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperLeft,
                richText = true
            };
            style.normal.textColor = Color.white;
            titleWidth = Utilities.getWidth(style, $" {title} {CharExtensions.getNameWithoutClanTag(Char.myCharz())} ");
            g.setColor(new Color(.2f, .2f, .2f, .7f));
            g.fillRect(x, GameCanvas.h - y - distanceBetweenLines, titleWidth, 8);
            if (GameCanvas.isMouseFocus(x, GameCanvas.h - y - distanceBetweenLines, titleWidth, 8))
            {
                g.setColor(style.normal.textColor);
                g.fillRect(x, GameCanvas.h - y - distanceBetweenLines + 7, titleWidth - 1, 1);
            }
            g.drawString($" <color=yellow>{title}</color> {CharExtensions.getNameWithoutClanTag(Char.myCharz())} ", x, GameCanvas.h - y - distanceBetweenLines - 2, style);
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            g.drawRegion(Mob.imgHP, 0, 18, 9, 6, (isCollapsed ? 4 : 5), collapseButtonX, collapseButtonY, 0);
            if (isCollapsed)
                return;
            g.setColor(Color.yellow);
            g.fillRect(x + titleWidth + 4, GameCanvas.h - y - 5, w - titleWidth - 7, 1);
            g.fillRect(x - 3, GameCanvas.h - y - 5, 3, 1);
            g.fillRect(x - 3, GameCanvas.h - y - 5, 1, h);
            g.fillRect(x + maxLength + 2, GameCanvas.h - y - 5, 1, h + 1);
            g.fillRect(x - 3, GameCanvas.h - y - 5 + h, w, 1);
        }

        private static void FillBackground(mGraphics g)
        {
            if (!isCollapsed)
            {
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
                int w = maxLength + 5;
                int h = distanceBetweenLines * lines.Count + 9;
                g.fillRect(x - 3, GameCanvas.h - y - 5, w, h);
            }
        }

        public static void LoadData()
        {
            try
            {
                isCollapsed = Utilities.loadRMSBool("isCollapsedCharInfo");
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void SaveData()
        {
            try
            {
                Utilities.saveRMSBool("isCollapsedCharInfo", isCollapsed);
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void UpdateTouch()
        {
            if (!isEnabled)
                return;
            if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                return;
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            if (GameCanvas.isPointerHoldIn(collapseButtonX, collapseButtonY, 6, 9)
                || GameCanvas.isMouseFocus(x, GameCanvas.h - y - distanceBetweenLines, titleWidth, 8))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    isCollapsed = !isCollapsed;
                    Utilities.saveRMSBool("isCollapsedCharInfo", isCollapsed);
                }
                Char.myCharz().currentMovePoint = null;
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }

        static void getCollapseButton(out int collapseButtonX, out int collapseButtonY)
        {
            collapseButtonX = x + titleWidth - 2;
            collapseButtonY = GameCanvas.h - y - distanceBetweenLines + 1;
        }

        public static void setState(bool value) => isEnabled = value;
    }
}
