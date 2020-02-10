using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RO
{
    public static class GrapesImageReader
    {
        public static List<GrapesImage> ReadGrapesImages()
        {
            List<GrapesImage> images = new List<GrapesImage>();

            string[] grapesImages = Directory.GetFiles("grapes");
            foreach (string name in grapesImages)
            {
                Bitmap bmp = new Bitmap(name);
                Color[,] photo = new Color[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j].R = bmp.GetPixel(j, i).R;
                        photo[i, j].G = bmp.GetPixel(j, i).G;
                        photo[i, j].B = bmp.GetPixel(j, i).B;
                    }

                Bitmap bmp2 = bmp.Clone(new RectangleF(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                if (name == "grapes\\count1.bmp") images.Add(new GrapesImage(photo, "count1", 18, 23, bmp, bmp2));
                if (name == "grapes\\count2.bmp") images.Add(new GrapesImage(photo, "count2", 16, 25, bmp, bmp2));
                if (name == "grapes\\count3.bmp") images.Add(new GrapesImage(photo, "count3", 19, 22, bmp, bmp2));
                if (name == "grapes\\count4.bmp") images.Add(new GrapesImage(photo, "count4", 23, 25, bmp, bmp2));
                if (name == "grapes\\count5.bmp") images.Add(new GrapesImage(photo, "count5", 22, 5, bmp, bmp2));
                if (name == "grapes\\count6.bmp") images.Add(new GrapesImage(photo, "count6", 11, 9, bmp, bmp2));
                if (name == "grapes\\count7.bmp") images.Add(new GrapesImage(photo, "count7", 20, 23, bmp, bmp2));
                if (name == "grapes\\count8.bmp") images.Add(new GrapesImage(photo, "count8", 23, 24, bmp, bmp2));
                if (name == "grapes\\count9.bmp") images.Add(new GrapesImage(photo, "count9", 18, 18, bmp, bmp2));
                if (name == "grapes\\count10.bmp") images.Add(new GrapesImage(photo, "count10", 24, 13, bmp, bmp2));
                if (name == "grapes\\count11.bmp") images.Add(new GrapesImage(photo, "count11", 15, 14, bmp, bmp2));
                if (name == "grapes\\count12.bmp") images.Add(new GrapesImage(photo, "count12", 30, 30, bmp, bmp2));
                if (name == "grapes\\count13.bmp") images.Add(new GrapesImage(photo, "count13", 29, 28, bmp, bmp2));
                if (name == "grapes\\count14.bmp") images.Add(new GrapesImage(photo, "count14", 21, 24, bmp, bmp2));
                if (name == "grapes\\count15.bmp") images.Add(new GrapesImage(photo, "count15", 24, 22, bmp, bmp2));
                //break;
            }

            return images;
        }
    }
}
