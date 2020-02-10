using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Imaging;
using Accord.Imaging.Filters;

namespace RO
{
    public class GrapesImage : BasicImage
    {
        private string name;
        private Color[,] Image;
        private Color[,] ImageOriginal;
        private Bitmap ImageBitmap;
        private Bitmap ImageBitmap8pp;
        private int GreenGrapesCount;
        private int VioletGrapesCount;

        private int GreenGrapesFound;
        private int VioletGrapesFound;

        public GrapesImage(Color[,] Image, string Name, int GreenGrapesCount, int VioletGrapesCount, Bitmap ImageBitmap, Bitmap ImageBitmap8pp)
        {
            this.Image = Image;
            this.ImageOriginal = Image;
            this.GreenGrapesCount = GreenGrapesCount;
            this.VioletGrapesCount = VioletGrapesCount;
            this.ImageBitmap = ImageBitmap;
            this.ImageBitmap8pp = ImageBitmap8pp;
            this.name = Name;
            GreenGrapesFound = 0;
            VioletGrapesFound = 0;
            //this.Image = RobertsCross();
            //SaveToFile();
            //HughCircleTransform();
            Count();
        }

        private Color[,] RobertsCross()
        {
            Color[,] img = new Color[Image.GetLength(0), Image.GetLength(1)];

            for (int i = 0; i < Image.GetLength(0) - 1; ++i)
                for (int j = 0; j < Image.GetLength(1) - 1; ++j)
                {
                    double col1 = (Image[i, j].R + Image[i, j].G + Image[i, j].B) / 3;
                    double col2 = (Image[i+1, j+1].R + Image[i + 1, j + 1].G + Image[i + 1, j + 1].B) / 3;

                    double col3 = (Image[i, j+1].R + Image[i, j + 1].G + Image[i, j + 1].B) / 3;
                    double col4 = (Image[i + 1, j].R + Image[i + 1, j].G + Image[i + 1, j].B) / 3;

                    int finalCol = (int)Math.Sqrt(Math.Pow(col1 - col2, 2) + Math.Pow(col3 - col4, 2));

                    if (finalCol > 80)
                        finalCol = 255;
                    else 
                        finalCol = 0;

                    img[i, j].R = finalCol;
                    img[i, j].G = finalCol;
                    img[i, j].B = finalCol;

                    System.Console.WriteLine(finalCol);
                }
            return img;
        }

        public Color[,] Filter(int size)
        {
            int[,] mask = new int[size, size];
            Color[,] finalImage = new Color[Image.GetLength(0), Image.GetLength(1)];
            List<Color> list = new List<Color>();
            Color median;

            for (int i = 0; i <= finalImage.GetLength(0) - mask.GetLength(0); i++)
            {
                for (int j = 0; j <= finalImage.GetLength(1) - mask.GetLength(1); j++)
                {
                    for (int x = i; x <= (mask.GetLength(0) - 1) + i; x++)
                    {
                        for (int y = j; y <= (mask.GetLength(1) - 1) + j; y++)
                        {
                            list.Add(Image[x, y]);
                        }
                    }

                    median = Median(list); //dla mediany
                    list.Clear();

                    finalImage[i, j].R = median.R;
                    finalImage[i, j].G = median.G;
                    finalImage[i, j].B = median.B;
                }
            }
            return finalImage;
        }

        private Color Median(List<Color> list)
        {
            Color col;
            //najpierw sortuj liste
            List<int> red = new List<int>();
            List<int> green = new List<int>();
            List<int> blue = new List<int>();

            int redMedian;
            int greenMedian;
            int blueMedian;

            //tworzenie list wszystkich wartosci poszczegolnych kanalow
            foreach (Color c in list)
            {
                red.Add(c.R);
                green.Add(c.G);
                blue.Add(c.B);
            }

            //sortowanie wszystkich wartosci w kanalach
            red.Sort();
            green.Sort();
            blue.Sort();

            //wyznaczenie elementu srodkowego
            redMedian = red.ElementAt(red.Count / 2);
            greenMedian = green.ElementAt(red.Count / 2);
            blueMedian = blue.ElementAt(red.Count / 2);

            //nowy kolor tworzony z elementu srodkowego kazdego z kanalow
            col.R = redMedian;
            col.G = greenMedian;
            col.B = blueMedian;

            return col;
        }

        public void SaveToFile()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Image.GetLength(0), Image.GetLength(1), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int i = 0; i < Image.GetLength(0); i++)
                for (int j = 0; j < Image.GetLength(1); j++)
                {

                    bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(Image[i,j].R, Image[i, j].G, Image[i, j].B));
                }

            bmp.Save(Label + this.GetHashCode().ToString() + ".png");
        }

        public void SaveToFileBlue()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Image.GetLength(0), Image.GetLength(1), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int i = 0; i < Image.GetLength(0); i++)
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(Image[i, j].B, Image[i, j].B, Image[i, j].B));
                }
            bmp.RotateFlip(RotateFlipType.Rotate270FlipY);
            //bmp.Save(Label + this.GetHashCode().ToString() + ".png");
            bmp.Save(name + "_edited.png");
        }


        public void Count()
        {
            Brightness(10);
            //Filtracja();
            Contrast(60); //80
            Image = Filter(5);
            //Filtracja();
            Regions();
            SaveToFileBlue();

            double scoreGreen = ((double)GreenGrapesFound / (double)GreenGrapesCount) * 100;
            double scoreViolet = ((double)VioletGrapesFound / (double)VioletGrapesCount) * 100;
            //System.Console.WriteLine("Skuteczność : Fioletowe: " + scoreViolet + "% , Zielone: " + scoreGreen + "%");
            System.Console.WriteLine("Nazwa pliku: " + name);
            System.Console.WriteLine("Fioletowe: " + VioletGrapesFound + "/" + VioletGrapesCount);
            System.Console.WriteLine("Zielone: " + GreenGrapesFound + "/" + GreenGrapesCount);
            System.Console.WriteLine();
        }

        bool[,] IfAlreadyChecked;
        Stack<XY> stack = new Stack<XY>();
        int FirstInGroup;

        private void Regions()
        {
            int rows = Image.GetLength(0);
            int columns = Image.GetLength(1);
            IfAlreadyChecked = new bool[Image.GetLength(0), Image.GetLength(1)];
            List<Tuple<XY, int>> WhiteRegions = new List<Tuple<XY, int>>();
            List<Tuple<XY, int>> BlackRegions = new List<Tuple<XY, int>>();

            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < columns; ++j)
                    IfAlreadyChecked[i, j] = false;

            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < columns; ++j)
                {
                    if (IfAlreadyChecked[i, j] == false)
                    {
                        int size = 0;
                        FirstInGroup = Image[i, j].B;
                        XY position = new XY(i, j);
                        stack.Push(new XY(i, j));
                        while (stack.Count != 0)
                        {
                            XY pixel = stack.Pop();
                            IfAlreadyChecked[pixel.X, pixel.Y] = true;
                            RegionCheckNeighbours(pixel.X, pixel.Y);
                            size++;
                        }
                        if (FirstInGroup == 0) BlackRegions.Add(new Tuple<XY, int>(position, size));
                        else WhiteRegions.Add(new Tuple<XY, int>(position, size));
                    }
                }

            /*
            for(int i = 0; i < BlackRegions.Count; i++)
            {
                Pen redPen = new Pen(System.Drawing.Color.Red, 3);
                using (var graphics = Graphics.FromImage(ImageBitmap))
                {
                    if(BlackRegions[i].Item2 > 10)
                    graphics.DrawEllipse(redPen, BlackRegions[i].Item1.Y, BlackRegions[i].Item1.X, 10, 10);
                }
            }
            */

            for (int i = 0; i < WhiteRegions.Count; i++)
            {
                Pen bluePen = new Pen(System.Drawing.Color.Blue, 7);
                Pen redPen = new Pen(System.Drawing.Color.Red, 7);

                using (var graphics = Graphics.FromImage(ImageBitmap))
                {
                    if (WhiteRegions[i].Item2 > 15 && WhiteRegions[i].Item2 < 500)
                    {
                        List<Color> pixelColors = new List<Color>();

                        if (InBounds(WhiteRegions[i].Item1.X - 10, Image.GetLength(0)))
                        {
                            Color up = ImageOriginal[WhiteRegions[i].Item1.X - 5, WhiteRegions[i].Item1.Y];
                            pixelColors.Add(up);
                        }

                        if (InBounds(WhiteRegions[i].Item1.X + 10, Image.GetLength(0)))
                        {
                            Color down = ImageOriginal[WhiteRegions[i].Item1.X + 5, WhiteRegions[i].Item1.Y];
                            pixelColors.Add(down);
                        }

                        if (InBounds(WhiteRegions[i].Item1.Y - 10, Image.GetLength(1)))
                        {
                            Color left = ImageOriginal[WhiteRegions[i].Item1.X, WhiteRegions[i].Item1.Y - 5];
                            pixelColors.Add(left);
                        }

                        if (InBounds(WhiteRegions[i].Item1.Y + 10, Image.GetLength(1)))
                        {
                            Color right = ImageOriginal[WhiteRegions[i].Item1.X, WhiteRegions[i].Item1.Y + 5];
                            pixelColors.Add(right);
                        }

                        int countGreen = 0;
                        int countViolet = 0;
                        
                        foreach(Color c in pixelColors)
                        {
                            if (c.G > 130)
                                countGreen++;
                            else
                                countViolet++;
                        }

                        if(countGreen>countViolet)
                        {
                            graphics.DrawEllipse(bluePen, WhiteRegions[i].Item1.Y, WhiteRegions[i].Item1.X, 10, 10);
                            GreenGrapesFound++;
                            graphics.DrawString("Zielone", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, WhiteRegions[i].Item1.Y-15, WhiteRegions[i].Item1.X+20);
                            //System.Console.WriteLine("Zielone");
                        }
                            
                        else
                        {
                            graphics.DrawEllipse(redPen, WhiteRegions[i].Item1.Y, WhiteRegions[i].Item1.X, 10, 10);
                            VioletGrapesFound++;
                            graphics.DrawString("Fioletowe", new Font("Arial", 8, FontStyle.Bold), Brushes.Red, WhiteRegions[i].Item1.Y-20, WhiteRegions[i].Item1.X + 20);
                            //System.Console.WriteLine("Fioletowe");
                        }
                            
                        //System.Console.WriteLine(red + " , " + green + " , " + blue);
                    }
                }
            }

            //ImageBitmap.Save(this.GetHashCode().ToString() + " testRegiony.bmp");
            ImageBitmap.Save(name + " testRegiony.bmp");
            stack.Clear();
        }

        private void RegionCheckNeighbours(int i, int j)
        {
            if (j - 1 >= 0)
            {
                if (IfAlreadyChecked[i, j - 1] == false)
                {
                    if (FirstInGroup - Image[i, j - 1].B == 0)
                    {
                        IfAlreadyChecked[i, j - 1] = true;
                        stack.Push(new XY(i, j - 1));
                    }
                }
            }
            if (i - 1 >= 0)
            {
                if (IfAlreadyChecked[i - 1, j] == false)
                {
                    if (FirstInGroup - Image[i - 1, j].B == 0)
                    {
                        IfAlreadyChecked[i - 1, j] = true;
                        stack.Push(new XY(i - 1, j));
                    }
                }
            }
            if (i + 1 < Image.GetLength(0))
            {
                if (IfAlreadyChecked[i + 1, j] == false)
                {
                    if (FirstInGroup - Image[i + 1, j].B == 0)
                    {
                        IfAlreadyChecked[i + 1, j] = true;
                        stack.Push(new XY(i + 1, j));
                    }
                }
            }
            if (j + 1 < Image.GetLength(1))
            {
                if (IfAlreadyChecked[i, j + 1] == false)
                {
                    if (FirstInGroup - Image[i, j + 1].B == 0)
                    {
                        IfAlreadyChecked[i, j + 1] = true;
                        stack.Push(new XY(i, j + 1));
                    }
                }
            }
        }

        public struct XY
        {
            public int X { get; set; }
            public int Y { get; set; }
            public XY(int x, int y)
            {
                X = x; Y = y;
            }
        }

        private bool InBounds(int x, int size)
        {
            if (x >= size || x <= 0)
            {
                return false;
            }

            return true;
        }

        private void Contrast(int x)
        {
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    int r = (Image[i, j].R - 128) * x + 128;
                    int g = (Image[i, j].G - 128) * x + 128;
                    int b = (Image[i, j].B - 128) * x + 128;

                    if (r > 255) r = 255; if (r < 0) r = 0;
                    if (g > 255) g = 255; if (g < 0) g = 0;
                    if (b > 255) b = 255; if (b < 0) b = 0;

                    Image[i, j].R = r;
                    Image[i, j].G = g;
                    Image[i, j].B = b;
                }
            }
        }

        private void Brightness(int x)
        {
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    int r = Image[i, j].R + x;
                    int g = Image[i, j].G + x;
                    int b = Image[i, j].B + x;

                    if (r > 255) r = 255; if (r < 0) r = 0;
                    if (g > 255) g = 255; if (g < 0) g = 0;
                    if (b > 255) b = 255; if (b < 0) b = 0;

                    Image[i, j].R = r;
                    Image[i, j].G = g;
                    Image[i, j].B = b;
                }
            }
        }

        public struct xy
        {
            public int x;
            public int y;
        };

        public void Filtracja()
        {
            int[] maska = new[] { 1,1,1, 1,0,1, 1,1,1};
            int WymiarMacierzy = 9;
            xy[] macierz = new xy[WymiarMacierzy];
            int srodek = (int)Math.Sqrt(WymiarMacierzy) / 2;
            int q = 0;

            for (int i = -srodek; i <= srodek; i++)
                for (int k = -srodek; k <= srodek; k++)
                {
                    macierz[q].x = k;
                    macierz[q].y = i;
                    q++;
                }

            double dzielnik = 0;
            for (int i = 0; i < maska.Length; i++)
            {
                dzielnik += maska[i];
            }
            if (dzielnik <= 0) dzielnik = 1.0;

            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    double r = 0.0;
                    double g = 0.0;
                    double b = 0.0;

                    for (int k = 0; k < WymiarMacierzy; k++)
                    {
                        int x = i + macierz[k].x;
                        int y = j + macierz[k].y;
                        if (x >= 0 && x < Image.GetLength(0) && y >= 0 && y < Image.GetLength(1))
                        {
                            r += Image[x, y].R * maska[k];
                            g += Image[x, y].G * maska[k];
                            b += Image[x, y].B * maska[k];
                        }
                    }

                    r = r / dzielnik;
                    g = g / dzielnik;
                    b = b / dzielnik;
                    if (r > 255) r = 255; if (r < 0) r = 0;
                    if (g > 255) g = 255; if (g < 0) g = 0;
                    if (b > 255) b = 255; if (b < 0) b = 0;

                    Image[i, j].R = (int)r;
                    Image[i, j].G = (int)g;
                    Image[i, j].B = (int)b;
                }
            }

        }

        public void HughCircleTransform()
        {
            Grayscale filterGrayscale = new Grayscale(0.2125, 0.7154, 0.0721);
            ImageBitmap8pp = filterGrayscale.Apply(ImageBitmap);

            Blur filterBlur = new Blur();
            filterBlur.ApplyInPlace(ImageBitmap8pp);

            //ContrastStretch filterContrast = new ContrastStretch();
            //filterContrast.ApplyInPlace(ImageBitmap8pp);

            ImageBitmap8pp.Save("test0.bmp");

            CannyEdgeDetector filter = new CannyEdgeDetector();
            filter.ApplyInPlace(ImageBitmap8pp);

            HoughCircleTransformation circleTransform = new HoughCircleTransformation(35);
            circleTransform.LocalPeakRadius = 300;
            circleTransform.ProcessImage(ImageBitmap8pp);
            Bitmap houghCirlceImage = circleTransform.ToBitmap();
            HoughCircle[] circles = circleTransform.GetCirclesByRelativeIntensity(0.6);

            foreach (HoughCircle circle in circles)
            {
                Pen redPen = new Pen(System.Drawing.Color.Red, 3);
                using (var graphics = Graphics.FromImage(ImageBitmap))
                {
                    graphics.DrawEllipse(redPen, circle.X- circle.Radius, circle.Y- circle.Radius, circle.Radius*2, circle.Radius*2);
                }
            }

            ImageBitmap8pp.Save("test1.bmp");
            houghCirlceImage.Save("test2.bmp");
            ImageBitmap.Save("test3.bmp");
        }
    }

    public struct Color
    {
        public int R, G, B;
    }
}
