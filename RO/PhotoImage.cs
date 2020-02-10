using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace RO
{
    class PhotoImage : BasicImage
    {
        public double[,] Image { get; set; }

        public PhotoImage(double[,] image, string label) :base(image, label)
        {
            Image = image;
            Label = label;

            Traits = new List<double>();
            Neighbours = new List<Neighbour>();
        }

        public override void CalculateTraits()
        {
            #region momenty
            double u00 = 0;

            for (int i = 0; i < Image.GetLength(0); ++i)
            {
                for (int j = 0; j < Image.GetLength(1); ++j)
                {
                    u00 += Image[i, j];
                }
            }

            double u10 = 0;
            double u01 = 0;

            for (int i = 0; i < Image.GetLength(0); ++i)
            {
                for (int j = 0; j < Image.GetLength(1); ++j)
                {
                    u10 += i * Image[i, j];
                    u01 += j * Image[i, j];
                }
            }

            u10 = u10 / u00;
            u01 = u01 / u00;

            double u20 = 0;
            double u02 = 0;
            double u11 = 0;

            for (int i = 0; i < Image.GetLength(0); ++i)
            {
                for (int j = 0; j < Image.GetLength(1); ++j)
                {
                    u20 += (i - u10) * (i - u10) * Image[i, j];
                    u02 += (j - u01) * (j - u01) * Image[i, j];
                    u11 += (i - u10) * (j - u01) * Image[i, j];
                }
            }

            u20 = u20 / u00;
            u02 = u02 / u00;
            u11 = u11 / u00;
            #endregion

            Traits.Add(u00);
            Traits.Add(u10);
            Traits.Add(u01);
            Traits.Add(u20);
            Traits.Add(u02);
            Traits.Add(u11);


            #region znajdz obszary
            //Contrast();
            //PartImage();
            //Image = RobertsCross();

            //List<int> Regions = new List<int>();
            //int RegionSize = 0;
            //IfAlreadyChecked = new bool[Image.GetLength(0), Image.GetLength(1)];

            //for (int i = 0; i < Image.GetLength(0); ++i)
            //    for (int j = 0; j < Image.GetLength(1); ++j)
            //        IfAlreadyChecked[i, j] = false;

            //for (int i = 0; i < Image.GetLength(0); ++i)
            //    for (int j = 0; j < Image.GetLength(1); ++j)
            //    {
            //        if (IfAlreadyChecked[i, j] == false)
            //        {
            //            stack.Push(new XY(i, j));
            //            while (stack.Count != 0)
            //            {
            //                XY pixel = stack.Pop();
            //                IfAlreadyChecked[pixel.X, pixel.Y] = true;
            //                FirstInGroup = Image[i, j];
            //                RegionSize++;
            //                RegionCheckNeighbours(pixel.X, pixel.Y);
            //            }

            //            Regions.Add(RegionSize);
            //            RegionSize = 0;
            //        }
            //    }

            //stack.Clear();

            //int BigRegions = 0;
            //for(int i = 0; i < Regions.Count; i++)
            //{
            //    if (Regions[i] > 50) BigRegions++;
            //}
            //Traits.Add(BigRegions);

            //int[] parts = {0, 0, 0, 0, 0,
            //                0, 0, 0, 0, 0};
            //for (int i = 0; i < Image.GetLength(0); ++i)
            //    for (int j = 0; j < Image.GetLength(1); ++j)
            //    {
            //        if (Image[i, j] == 0) parts[0]++;
            //        if (Image[i, j] == 0.1) parts[1]++;
            //        if (Image[i, j] == 0.2) parts[2]++;
            //        if (Image[i, j] == 0.3) parts[3]++;
            //        if (Image[i, j] == 0.4) parts[4]++;
            //        if (Image[i, j] == 0.5) parts[5]++;
            //        if (Image[i, j] == 0.6) parts[6]++;
            //        if (Image[i, j] == 0.7) parts[7]++;
            //        if (Image[i, j] == 0.8) parts[8]++;
            //        if (Image[i, j] == 0.9) parts[9]++;
            //    }
            //foreach (int part in parts)
            //    Traits.Add(part);
            #endregion
            Contrast();
            int[] histogram = new int[256];
            for (int i = 0; i < Image.GetLength(0); ++i)
                for (int j = 0; j < Image.GetLength(1); ++j)
                {
                    histogram[(int)Math.Round(Image[i, j] * 255)]++;
                }

            //ModifyHistogram(histogram);
            //histogram = new int[256];
            //for (int i = 0; i < Image.GetLength(0); ++i)
            //    for (int j = 0; j < Image.GetLength(1); ++j)
            //    {
            //        histogram[(int)Math.Round(Image[i, j] * 255)]++;
            //    }

            foreach (int h in histogram)
                Traits.Add(h);
        }

        public void RegionCheckNeighbours(int i, int j)
        {
            if (j - 1 >= 0)
            {
                if (IfAlreadyChecked[i, j - 1] == false)
                {
                    if (Math.Abs(FirstInGroup - Image[i, j - 1]) < 0.1)
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
                    if (Math.Abs(FirstInGroup - Image[i - 1, j]) < 0.1)
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
                    if (Math.Abs(FirstInGroup - Image[i + 1, j]) < 0.1)
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
                    if (Math.Abs(FirstInGroup - Image[i, j + 1]) < 0.1)
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

        public bool[,] IfAlreadyChecked;
        protected Stack<XY> stack = new Stack<XY>();
        private double FirstInGroup;

        private void Contrast()
        {
            for (int i = 0; i < Image.GetLength(0); ++i)
                for (int j = 0; j < Image.GetLength(1); ++j)
                {
                    double color = Image[i, j] * 255;
                    color = (color - 128) * 3 + 128;

                    if (color > 255) color = 255; if (color < 0) color = 0;

                    Image[i, j] = color / 255.0;
                }
        }

        private void PartImage()
        {
            for (int i = 0; i < Image.GetLength(0); ++i)
                for (int j = 0; j < Image.GetLength(1); ++j)
                {
                    Image[i, j] = Math.Round(Image[i, j], 1);
                }
        }

        private double[,] RobertsCross()
        {
            double[,] img = new double[Image.GetLength(0), Image.GetLength(1)];
            for (int i = 0; i < Image.GetLength(0)-1; ++i)
                for (int j = 0; j < Image.GetLength(1)-1; ++j)
                {
                    img[i, j] = Math.Sqrt(Math.Pow(Image[i, j] - Image[i + 1, j + 1], 2) + Math.Pow(Image[i, j + 1] - Image[i + 1, j], 2));
                }
            return img;
        }

        private void ModifyHistogram(int[] histogram)
        {
            double N = Image.GetLength(0) * Image.GetLength(1);
            for (int i = 0; i < Image.GetLength(0) - 1; ++i)
                for (int j = 0; j < Image.GetLength(1) - 1; ++j)
                {
                    int f = (int)Math.Round(Image[i, j]*255);
                    int sumHIS = 0;
                    for (int x = 0; x < f; x++)
                    {
                        sumHIS += histogram[x];
                    }

                    Image[i, j] = (255 * (sumHIS / N))/255.0;
                }
        }
    }
}
