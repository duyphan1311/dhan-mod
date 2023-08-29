using LitJson;
using Mod.CustomGroupBox;
using Mod.Graphics;
using Mod.Set;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.OnScreenPaint
{
    public class ListBossInMap
    {
        public static List<Char> listBoss = new List<Char>();

        public static bool isEnabled;

        public static bool isShowPet;

        public static int maxLength = 0;

        public static int x = 15 - 9;

        public static int y = 50;

        public static readonly int ORIGINAL_X = 6;

        public static int distanceBetweenLines = 8;

        public static bool isCollapsed;

        public static int titleWidth;

        public static void update()
        {
            x = ORIGINAL_X;
            if (ListCharsInMap.isEnabled)
            {
                if (ListCharsInMap.isCollapsed || ListCharsInMap.listChars.Count == 0)
                //{
                //    if (y != ListCharsInMap.y + ListCharsInMap.distanceBetweenLines + 3)
                        y = ListCharsInMap.y + ListCharsInMap.distanceBetweenLines + 3;
                //}
                else /*if (y != ListCharsInMap.y + 5 + ListCharsInMap.distanceBetweenLines * Mathf.Clamp(ListCharsInMap.listChars.Count, 0, ListCharsInMap.MAX_CHAR) + 10)*/
                    y = ListCharsInMap.y + 5 + ListCharsInMap.distanceBetweenLines * Mathf.Clamp(ListCharsInMap.listChars.Count, 0, ListCharsInMap.MAX_CHAR) + 10;
            }
            else if (Boss.isEnabled && Boss.listBosses.Count > 0)
            {
                if (Boss.isCollapsed)
                //{
                //    if (y != Boss.y + Boss.distanceBetweenLines + 3)
                        y = Boss.y + Boss.distanceBetweenLines + 3;
                //}
                else /*if (y != Boss.y + 5 + Boss.distanceBetweenLines * Mathf.Clamp(Boss.listBosses.Count, 0, Boss.MAX_BOSS_DISPLAY) + 10)*/
                    y = Boss.y + 5 + Boss.distanceBetweenLines * Mathf.Clamp(Boss.listBosses.Count, 0, Boss.MAX_BOSS_DISPLAY) + 10;
            }
            else if (y != 50)
                y = 50;
            if (!isEnabled)
                return;
            listBoss.Clear();
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.isBoss())
                    listBoss.Add(ch);
            }
        }

        public static void paint(mGraphics g)
        {
            if (!isEnabled)
                return;
            maxLength = 0;
            if (!isCollapsed && TileMap.mapID != Char.myCharz().cgender + 21)
            {
                PaintListBoss(g);
            }
            PaintRect(g);
        }

        static string formatHP(Char ch)
        {
            int hp = ch.cHP;
            int hpFull = ch.cHPFull;
            float ratio = hp / (float)hpFull;
            Color color = new Color(Mathf.Clamp(2 - ratio * 2, 0, 1), Mathf.Clamp(ratio * 2, 0, 1), 0);
            string hexColor = $"#{(int)(color.r * 255):x2}{(int)(color.g * 255):x2}{(int)(color.b * 255):x2}{(int)(color.a * 255):x2}";
            return $"<color=white>[<color={hexColor}>{NinjaUtil.getMoneys(ch.cHP)}</color>/<color=lime>{NinjaUtil.getMoneys(ch.cHPFull)}</color>]</color>";
        }

        static void PaintListBoss(mGraphics g)
        {
            List<KeyValuePair<string, GUIStyle>> charDescriptions = new List<KeyValuePair<string, GUIStyle>>();
            for (int i = 0; i < listBoss.Count; i++)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 6 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.UpperRight,
                    richText = true
                };
                #region Format
                Char ch = listBoss[i];
                string charDesc = $"{ch.getNameWithoutClanTag(true)} {formatHP(ch)}";
                if ((Char.myCharz().isStandAndCharge || (!Char.myCharz().isDie && Char.myCharz().cgender == 2 && Char.myCharz().myskill == Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]))) && SuicideRange.mapObjsInMyRange.Contains(ch))
                {
                    if (GameCanvas.gameTick % 40 > 20)
                        charDesc += " - <color=red>Trong tầm</color>";
                    else
                        charDesc += " - <color=yellow>Trong tầm</color>";
                }
                charDesc = $"{i + 1}. <color=red>{charDesc}</color>";
                charDescriptions.Add(new KeyValuePair<string, GUIStyle>(charDesc, style));
                maxLength = Math.max(maxLength, Utilities.getWidth(charDescriptions[i].Value, charDesc) + (ch.cFlag != 0 ? (distanceBetweenLines + 1) : 0));
                #endregion
            }
            FillBackground(g);
            for (int i = 0; i < listBoss.Count; i++)
            {
                Char ch = listBoss[i];
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.4f));
                if (GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * i, maxLength, distanceBetweenLines - 1))
                    g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.7f));
                if (Char.myCharz().charFocus == listBoss[i])
                    g.setColor(new Color(1f, .5f, 0f, .5f));
                if (SuicideRange.isShowSuicideRange && SuicideRange.mapObjsInMyRange.Contains(listBoss[i]))
                {
                    g.setColor(new Color(0.5f, 0.5f, 0f, 1f));
                    if (Char.myCharz().isStandAndCharge && GameCanvas.gameTick % 10 >= 5)
                        g.setColor(new Color(1f, 0f, 0f, 1f));
                }
                g.fillRect(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * i, maxLength, distanceBetweenLines - 1);
                if (GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * i, maxLength, distanceBetweenLines - 1))
                {
                    int length = Utilities.getWidth(charDescriptions[i].Value, charDescriptions[i].Key);
                    g.setColor(Color.white);
                    g.fillRect(GameCanvas.w - x - length + 1, y + distanceBetweenLines * i + 7, length - 2, 1);
                    int hp = ch.cHP;
                    int hpFull = ch.cHPFull;
                    float ratio = hp / (float)hpFull;
                    Color color = new Color(Mathf.Clamp(2 - ratio * 2, 0, 1), Mathf.Clamp(ratio * 2, 0, 1), 0);
                    g.setColor(color);
                    g.fillRect(GameCanvas.w - x - length + 1, y + distanceBetweenLines * i + 7, (int)(ratio * (length - 2)), 1);
                }
                g.drawString(charDescriptions[i].Key, -x, mGraphics.zoomLevel - 4 + y + distanceBetweenLines * i, charDescriptions[i].Value);
            }
        }

        private static void FillBackground(mGraphics g)
        {
            if (!isCollapsed && listBoss.Count > 0)
            {
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
                int w = maxLength + 5;
                int h = distanceBetweenLines * listBoss.Count + 7;
                g.fillRect(GameCanvas.w - x - maxLength - 3, y - 5, w, h);
            }
        }

        static void PaintRect(mGraphics g)
        {
            int w = maxLength + 5;
            int h = distanceBetweenLines * listBoss.Count + 7;
            string str = $"<color=white>Boss trong khu</color>";
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
            g.fillRect(GameCanvas.w - x - titleWidth, y - distanceBetweenLines, titleWidth, 8);
            if (GameCanvas.isMouseFocus(GameCanvas.w - x - titleWidth, y - distanceBetweenLines, titleWidth, 8))
            {
                g.setColor(style.normal.textColor);
                g.fillRect(GameCanvas.w - x - titleWidth, y - 1, titleWidth - 1, 1);
            }
            g.drawString(str, -x, y - distanceBetweenLines - 2, style);
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            g.drawRegion(Mob.imgHP, 0, 18, 9, 6, (isCollapsed ? 5 : 4), collapseButtonX, collapseButtonY, 0);
            if (isCollapsed || listBoss.Count <= 0)
                return;
            g.setColor(Color.yellow);
            g.fillRect(GameCanvas.w - x - maxLength - 3, y - 5, w - titleWidth - 9, 1);
            g.fillRect(GameCanvas.w - x, y - 5, 3, 1);
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
                getCollapseButton(out int collapseButtonX, out int collapseButtonY);
                if (GameCanvas.isPointerHoldIn(collapseButtonX, collapseButtonY, 9, 6) || GameCanvas.isPointerHoldIn(GameCanvas.w - x - titleWidth, y - distanceBetweenLines, titleWidth, 8))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        isCollapsed = !isCollapsed;
                        Utilities.saveRMSBool("isCollapsedListBoss", isCollapsed);
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
                if (isCollapsed) return;
                for (int i = 0; i < listBoss.Count; i++)
                {
                    if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength - (listBoss[i].cFlag != 0 ? (distanceBetweenLines + 1) : 0), y + 1 + distanceBetweenLines * i, maxLength, distanceBetweenLines - 1))
                    {
                        GameCanvas.isPointerJustDown = false;
                        GameScr.gI().isPointerDowning = false;
                        if (GameCanvas.isPointerClick)
                        {
                            global::Char.myCharz().npcFocus = null;
                            global::Char.myCharz().mobFocus = null;
                            global::Char.myCharz().itemFocus = null;
                            if (Char.myCharz().charFocus != listBoss[i])
                                Char.myCharz().charFocus = listBoss[i];
                            else Utilities.teleportMyChar(listBoss[i]);
                        }
                        Char.myCharz().currentMovePoint = null;
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                }
            }
            catch (Exception) { }
        }

        static void getCollapseButton(out int collapseButtonX, out int collapseButtonY)
        {
            collapseButtonX = GameCanvas.w - x - titleWidth - 8;
            collapseButtonY = y - distanceBetweenLines + 1;
        }

        public static void LoadData()
        {
            try
            {
                isCollapsed = Utilities.loadRMSBool("isCollapsedListBoss");
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void SaveData()
        {
            try
            {
                Utilities.saveRMSBool("isCollapsedListBoss", isCollapsed);
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void setState(bool value) => isEnabled = value;
    }
}