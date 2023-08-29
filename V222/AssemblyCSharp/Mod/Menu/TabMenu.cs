using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp.Mod.Menu
{
    internal class TabMenu : IActionListener
    {
        private static TabMenu instance;

		public static string MenuMod = "Menu Mod";

		public static string[] strMenuMod = new string[]
		{
				"Auto login",
				"Auto tan sat",
				"Trở lại khu cũ sau khi mất kết nối",
				"Hiển thi thông tin sư phụ"
		};

		public static string[] strModStatus = new string[]
		{

		};

		public static TabMenu gI()
        {
            return (instance != null) ? instance : (instance = new TabMenu());
        }
		public void setTypeMenuMod()
		{
			GameCanvas.panel.type = 27;
			GameCanvas.panel.setType(0);
			setTabMenuMod();
			GameCanvas.panel.cmx = (GameCanvas.panel.cmtoX = 0);
		}

		public void setTabMenuMod()
		{
			GameCanvas.isTouch = true;
			GameCanvas.panel.currentListLength = strMenuMod.Length;
			GameCanvas.panel.ITEM_HEIGHT = 24;
			GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
			if (GameCanvas.panel.cmyLim < 0)
			{
				GameCanvas.panel.cmyLim = 0;
			}
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex]);
			if (GameCanvas.panel.cmy < 0)
			{
				GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
			}
			if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim)
			{
				GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim);
			}
			GameCanvas.panel.selected = GameCanvas.isTouch ? (-1) : 0;
		}

		public void paintMenuMod(mGraphics g)
		{
			g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
			g.translate(0, -GameCanvas.panel.cmy);
			for (int i = 0; i < strMenuMod.Length; i++)
			{
				int x = GameCanvas.panel.xScroll;
				int num = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
				int num2 = GameCanvas.panel.wScroll - 1;
				int h = GameCanvas.panel.ITEM_HEIGHT - 1;
				if ((num - GameCanvas.panel.cmy <= GameCanvas.panel.yScroll + GameCanvas.panel.hScroll) && (num - GameCanvas.panel.cmy >= GameCanvas.panel.yScroll - GameCanvas.panel.ITEM_HEIGHT))
				{
					g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
					g.fillRect(x, num, num2, h);
					mFont.tahoma_7_green2.drawString(g, i + ". " + strMenuMod[i], x + 5, num + 1, mFont.LEFT);
					mFont.tahoma_7_blue.drawString(g, strMenuMod[i], x + 5, num + 11, mFont.LEFT);
				}
			}
			GameCanvas.panel.paintScrollArrow(g);
		}
		public void paintMenuModInfo(mGraphics g)
		{
			mFont.tahoma_7b_white.drawString(g, mResources.dragon_ball + " " + GameMidlet.VERSION, 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
			mFont.tahoma_7_yellow.drawString(g, "Mod by Duy Phan", 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, mResources.character + ": " + Char.myCharz().cName, 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, "TK: " + GameCanvas.loginScr.tfUser.getText() + " Server: " + ServerListScreen.nameServer[ServerListScreen.ipSelect], 60, 39, mFont.LEFT, mFont.tahoma_7_grey);
		}
		public void doFieMenuMod()
		{
			if (GameCanvas.panel.selected < 0)
			{
				return;
			}
			switch (GameCanvas.panel.selected)
			{
				case 0:
					GameScr.info1.addInfo(strMenuMod[GameCanvas.panel.selected], 0);
					break;
				case 1:
					GameScr.info1.addInfo(strMenuMod[GameCanvas.panel.selected], 0);
					break;
				case 2:
					GameScr.info1.addInfo(strMenuMod[GameCanvas.panel.selected], 0);
					break;
				case 3:
					GameScr.info1.addInfo(strMenuMod[GameCanvas.panel.selected], 0);
					break;
				case 4:
					GameScr.info1.addInfo(strMenuMod[GameCanvas.panel.selected], 0);
					break;
			}
		}
		public void updateKeyMenuMod()
		{
			GameCanvas.panel.updateKeyScrollView();
		}
		public void perform(int idAction, object p)
        {
            throw new NotImplementedException();
        }
    }
}
