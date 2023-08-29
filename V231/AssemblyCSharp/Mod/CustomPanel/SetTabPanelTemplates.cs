﻿using Mod.ModMenu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.CustomPanel
{
    public static class SetTabPanelTemplates
    {
        public static void setTabListTemplate(Panel panel, params int[] lengths)
        {
            // Set the item height
            panel.ITEM_HEIGHT = 24;

            // Set the current list length
            if (lengths.Length > 1)
                panel.currentListLength = lengths[panel.currentTabIndex];
            else
                panel.currentListLength = lengths[0];

            // Set the selected index
            panel.selected = GameCanvas.isTouch ? (-1) : 0;

            // Set the scroll limit
            panel.cmyLim = panel.currentListLength * panel.ITEM_HEIGHT - panel.hScroll;
            if (panel.cmyLim < 0) panel.cmyLim = 0;

            // Set the scroll position
            panel.cmy = panel.cmtoY = panel.cmyLast[panel.currentTabIndex];
            if (panel.cmy < 0) panel.cmy = panel.cmtoY = 0;
            if (panel.cmy > panel.cmyLim) panel.cmy = panel.cmtoY = panel.cmyLim;
        }

        public static void setTabListTemplate(Panel panel, params ICollection[] collections)
        {
            var lengths = (from x in collections select x.Count).ToArray();
            setTabListTemplate(panel, lengths);
        }
    }
}
