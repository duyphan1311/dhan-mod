using System.IO;
using System.Reflection;

namespace Mod.Graphics
{
    public class ModImages
    {
        public static Image infinitySymbol;

        static ModImages()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Mod.Resources.infinityChar-x{mGraphics.zoomLevel}.png");
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            infinitySymbol = Image.createImage(buffer);
        }
    }
}
