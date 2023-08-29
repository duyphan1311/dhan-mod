using AssemblyCSharp.Mod.PickMob;
using System;


namespace AssemblyCSharp.Mod.Xmap
{
    public class Pk9rXmap
    {
        public static bool IsXmapRunning = false;
        public static bool IsMapTransAsXmap = false;
        public static bool IsShowPanelMapTrans = true;
        public static bool IsUseCapsuleNormal = false;
        public static bool IsUseCapsuleVip = true;
        public static int IdMapCapsuleReturn = -1;
        public static bool IsMoveToBoss;
      
        public static bool Chat(string txt)
        {
            if (!txt.StartsWith("/"))
                return false;

            txt = txt.Substring(1);
            string[] array = txt.Trim().Split(';');
            for (int idx = 0; idx < array.Length; idx++)
            {
                string text = array[idx].Trim();
                if (text == "xmp")
                {
                    if (IsXmapRunning)
                    {
                        XmapController.FinishXmap();
                        GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                    }
                    else
                    {
                        XmapController.ShowXmapMenu();
                    }
                }
                else if (IsGetInfoChat<int>(text, "xmp"))
                {
                    if (IsXmapRunning)
                    {
                        XmapController.FinishXmap();
                        GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                    }
                    else
                    {
                        int idMap = GetInfoChat<int>(text, "xmp");
                        XmapController.StartRunToMapId(idMap);
                    }
                }
                else if (text == "csb")
                {
                    IsUseCapsuleNormal = !IsUseCapsuleNormal;
                    GameScr.info1.addInfo("Sử dụng capsule thường Xmap: " + (IsUseCapsuleNormal ? "Bật" : "Tắt"), 0);
                }
                else if (text == "csdb")
                {
                    IsUseCapsuleVip = !IsUseCapsuleVip;
                    GameScr.info1.addInfo("Sử dụng capsule đặc biệt Xmap: " + (IsUseCapsuleVip ? "Bật" : "Tắt"), 0);
                }
                else if (text == "j")
                {
                    if (XmapController.getX(0) > 0 && XmapController.getY(0) > 0)
                    {
                        XmapController.MoveMyChar(XmapController.getX(0), XmapController.getY(0));
                    }
                }
                else if (text == "k")
                {
                    if (XmapController.getX(1) > 0 && XmapController.getY(1) > 0)
                    {
                        XmapController.MoveMyChar(XmapController.getX(1), XmapController.getY(1));
                        //Service.gI().getMapOffline();
                        Service.gI().requestChangeMap();
                    }
                }
                else if (text == "l")
                {
                    if (XmapController.getX(2) > 0 && XmapController.getY(2) > 0)
                    {
                        XmapController.MoveMyChar(XmapController.getX(2), XmapController.getY(2));

                    }
                }
                else if (text == "i")
                {
                    if (XmapController.getX(3) > 0 && XmapController.getY(3) > 0)
                    {
                        XmapController.MoveMyChar(XmapController.getX(3), XmapController.getY(3));

                    }
                }
                else if (text == "tl")
                {
                    Chat("xmp102");
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static bool HotKeys()
        {
            switch (GameCanvas.keyAsciiPress)
            {
                case 'x':
                    Chat("/xmp");
                    break;
                //case 'c':
                //    Chat("csb");
                //    break;
                case 'j':
                    Chat("/j");
                    break;
                case 'k':
                    Chat("/k");
                    break;
                case 'l':
                    Chat("/l");
                    break;
                case 'i':
                    Chat("/i");
                    break;
                default:
                    return false;
            }
            return true;
        }
       
        public static void Update()
        {
            if (XmapData.Instance().IsLoading)
                XmapData.Instance().Update();
            if (IsXmapRunning)
                XmapController.Update();
        }

        public static void Info(string text)
        {
            if (text.Equals("Bạn chưa thể đến khu vực này"))
                XmapController.FinishXmap();
        }

        public static bool XoaTauBay(Object obj)
        {
            Teleport teleport = (Teleport)obj;
            if (teleport.isMe)
            {
                Char.myCharz().isTeleport = false;
                if (teleport.type == 0)
                {
                    Controller.isStopReadMessage = false;
                    Char.ischangingMap = true;
                }
                Teleport.vTeleport.removeElement(teleport);
                return true;
            }
            return false;
        }

        public static void SelectMapTrans(int selected)
        {
            if (IsMapTransAsXmap)
            {
                XmapController.HideInfoDlg();
                string mapName = GameCanvas.panel.mapNames[selected];
                int idMap = XmapData.GetIdMapFromPanelXmap(mapName);
                XmapController.StartRunToMapId(idMap);
                return;
            }
            XmapController.SaveIdMapCapsuleReturn();
            Service.gI().requestMapSelect(selected);
        }

        public static void ShowPanelMapTrans()
        {
            IsMapTransAsXmap = false;
            if (IsShowPanelMapTrans)
            {
                GameCanvas.panel.setTypeMapTrans();
                GameCanvas.panel.show();
                return;
            }
            IsShowPanelMapTrans = true;
        }

        public static void FixBlackScreen()
        {
            Controller.gI().loadCurrMap(0);
            Service.gI().finishLoadMap();
            Char.isLoadingMap = false;
        }

        #region Không cần liên kết với game
        public static bool IsGetInfoChat<T>(string text, string s)
        {
            if (!text.StartsWith(s))
            {
                return false;
            }
            try
            {
                Convert.ChangeType(text.Substring(s.Length), typeof(T));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static T GetInfoChat<T>(string text, string s)
        {
            return (T)Convert.ChangeType(text.Substring(s.Length), typeof(T));
        }
        #endregion
    }
}
