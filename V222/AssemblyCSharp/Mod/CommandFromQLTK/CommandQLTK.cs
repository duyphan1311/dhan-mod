using AssemblyCSharp.Mod.PickMob;
using AssemblyCSharp.Mod.Xmap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssemblyCSharp.Mod.CommandFromQLTK
{
    class CommandQLTK
    {
        public static string chatCMD = "Data\\chatCmd.ini";
        public static void GetCMDInFile()
        {
            string[] text = File.ReadAllText(chatCMD).Split(new char[]
            {
                ';'
            });
            if(text != null )
            {
                for (int i = 0; i < text.Length; i++)
                {
                    PickMob.PickMob.Chat(text[i]);
                    Pk9rXmap.Chat(text[i]);
                    OnScreen.Chat(text[i]);
                }
            }
            
        }
        public static string configuration = "Data\\configuration.ini";
        public static void GetConfiguration()
        {
            string text = File.ReadAllText(configuration);
            if(int.Parse(text) == 1)
            {
                mGraphics.zoomLevel = 1;
                Rms.saveRMSInt("lowGraphic", 1);
                OnScreen.IsXoaMap = true;
            }
            else
            {
                mGraphics.zoomLevel = 2;
                Rms.saveRMSInt("lowGraphic", 0);
            }
        }
    }
}
