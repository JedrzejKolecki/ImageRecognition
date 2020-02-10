using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;


namespace RO
{
    static class PhotoReader
    {
        public static List<BasicImage> TrainingImages()
        {
            List<BasicImage> Photos = new List<BasicImage>();

            string[] photosBrush = Directory.GetFiles("train/brush");
            foreach (string name in photosBrush)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for(int i = 0; i < bmp.Height; i++)
                    for(int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "brush"));
            }

            string[] photosClamp = Directory.GetFiles("train/clamp");
            foreach (string name in photosClamp)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "clamp" +
                    ""));
            }

            string[] photosHook = Directory.GetFiles("train/hook");
            foreach (string name in photosHook)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "hook"));
            }

            string[] photosKnife = Directory.GetFiles("train/knife");
            foreach (string name in photosKnife)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "knife"));
            }

            string[] photosPencil = Directory.GetFiles("train/pencil");
            foreach (string name in photosPencil)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "pencil"));
            }

            string[] photosPliers = Directory.GetFiles("train/pliers");
            foreach (string name in photosPliers)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "pliers"));
            }

            string[] photosScissors = Directory.GetFiles("train/scissors");
            foreach (string name in photosScissors)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "scissors"));
            }

            string[] photosScrewdriver = Directory.GetFiles("train/screwdriver");
            foreach (string name in photosScrewdriver)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "screwdriver"));
            }

            string[] photosSpanner = Directory.GetFiles("train/spanner");
            foreach (string name in photosSpanner)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "spanner"));
            }

            string[] photosString = Directory.GetFiles("train/string");
            foreach (string name in photosString)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, "string"));
            }

            return Photos;
        }

        public static List<BasicImage> TestImagesPlain()
        {
            List<BasicImage> Photos = new List<BasicImage>();

            string[] photoNames = Directory.GetFiles("test_plain");
            foreach (string name in photoNames)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, name.Substring(11, name.LastIndexOf("_") - 11)));
            }

            return Photos;
        }

        public static List<BasicImage> TestImagesLight()
        {
            List<BasicImage> Photos = new List<BasicImage>();

            string[] photoNames = Directory.GetFiles("test_light");
            foreach (string name in photoNames)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, name.Substring(11, name.LastIndexOf("_") - 11)));
            }

            return Photos;
        }

        public static List<BasicImage> TestImagesLight30()
        {
            List<BasicImage> Photos = new List<BasicImage>();

            string[] photoNames = Directory.GetFiles("test_30st_light");
            foreach (string name in photoNames)
            {
                Bitmap bmp = new Bitmap(name);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R/255.0;
                    }

                Photos.Add(new PhotoImage(photo, name.Substring(16, name.LastIndexOf("_") - 16)));
            }

            return Photos;
        }
    }
}
