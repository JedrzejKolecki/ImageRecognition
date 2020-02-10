using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RO
{
    public class DigitImage : BasicImage
    {
        // 1. ilosc bialych pikseli -niet
        // 2. ilosc czarnych piskeli -niet
        // 3. ilosc okienek (H)
        // 4. ilosc czarnych obiektow (C) -niet
        // 5. liczba eulera (C-H)
        // 6. stosunek czerni do bieli 1./2. -niet
        // 7. liczba czarnych piskeli w cwiartce 1
        // 8. liczba czarnych piskeli w cwiartce 2
        // 9. liczba czarnych piskeli w cwiartce 3
        // 10. liczba czarnych piskeli w cwiartce 4
        protected int[,] Pixels { get; set; }
        protected int[,] PixelsBW { get; set; }

        public DigitImage(byte[,] pixels, byte label) :base(pixels, label)
        {
            this.Pixels = new int[pixels.GetLength(0), pixels.GetLength(1)];
            this.PixelsBW = new int[pixels.GetLength(0), pixels.GetLength(1)];

            for (int i = 0; i < pixels.GetLength(0); ++i)
                for (int j = 0; j < pixels.GetLength(1); ++j)
                {
                    this.Pixels[i, j] = pixels[i, j];
                    if (pixels[i, j] > 0) this.PixelsBW[i, j] = 1; // zmienna do eksperymentow, podreca kontrast, 0 to biel, 1 to czern
                    else this.PixelsBW[i, j] = 0;
                }

            Label = label;
            Traits = new List<double>();
            Neighbours = new List<Neighbour>();
        }

        public override void CalculateTraits()
        {
            #region policz biale i czarne piksele
            int WhitePixels = 0;
            int BlackPixels = 0;
            int cw1 = 0;
            int cw2 = 0;
            int cw3 = 0;
            int cw4 = 0;

            double u00 = 0;

            for (int i = 0; i < 28; ++i)
            {
                for (int j = 0; j < 28; ++j)
                {
                    if ((i < 14) && (j < 14) && (PixelsBW[i, j] == 1))
                        cw1++;
                    if ((i >= 14) && (j < 14) && (PixelsBW[i, j] == 1))
                        cw2++;
                    if ((i < 14) && (j >= 14) && (PixelsBW[i, j] == 1))
                        cw3++;
                    if ((i >= 14) && (j >= 14) && (PixelsBW[i, j] == 1))
                        cw4++;
                    if (PixelsBW[i, j] == 0) WhitePixels++;
                    else BlackPixels++;

                    u00 += PixelsBW[i, j];
                }
            }

            double u10 = 0;
            double u01 = 0;

            for (int i = 0; i < 28; ++i)
            {
                for (int j = 0; j < 28; ++j)
                {
                    u10 += i * PixelsBW[i, j];
                    u01 += j * PixelsBW[i, j];
                }
            }

            u10 = u10 / u00;
            u01 = u01 / u00;

            double u20 = 0;
            double u02 = 0;
            double u11 = 0;

            for (int i = 0; i < 28; ++i)
            {
                for (int j = 0; j < 28; ++j)
                {
                    u20 += (i - u10)*(i - u10)* PixelsBW[i, j];
                    u02 += (j - u01) * (j - u01) * PixelsBW[i, j];
                    u11 += (i - u10) * (j - u01) * PixelsBW[i, j];
                }
            }

            u20 = u20 / u00;
            u02 = u02 / u00;
            u11 = u11 / u00;

            #endregion

            #region znajdz obszary
            int WhiteGroupsCount = 0;
            int BlackGroupsCount = 0;


            for (int i = 0; i < 28; ++i)
                for (int j = 0; j < 28; ++j)
                    IfAlreadyChecked[i, j] = false;

            for (int i = 0; i < 28; ++i)
                for (int j = 0; j < 28; ++j)
                {
                    if (IfAlreadyChecked[i, j] == false)
                    {
                        FirstInGroup = PixelsBW[i, j];
                        stack.Push(new XY(i, j));
                        while (stack.Count != 0)
                        {
                            XY pixel = stack.Pop();
                            IfAlreadyChecked[pixel.X, pixel.Y] = true;
                            RegionCheckNeighbours(pixel.X, pixel.Y);
                        }
                        if (FirstInGroup == 0) WhiteGroupsCount++;
                        else BlackGroupsCount++;
                    }
                }

            stack.Clear();
            #endregion

            // 1. ilosc bialych pikseli
            // Traits.Add(WhitePixels);
            // 2. ilosc czarnych piskeli
            //  Traits.Add(BlackPixels);
            // 3. ilosc okienek (H)
            Traits.Add((WhiteGroupsCount - 1)*10);
            // 4. ilosc czarnych obiektow (C)
            Traits.Add((BlackGroupsCount)*10);
            // 5. liczba eulera (C-H)
            Traits.Add((BlackGroupsCount - (WhiteGroupsCount - 1))*10);
            // 6. stosunek czerni do bieli 1./2.
            //   Traits.Add(WhitePixels / BlackPixels);
            Traits.Add(cw1);
            Traits.Add(cw2);
            Traits.Add(cw3);
            Traits.Add(cw4);
            Traits.Add(u00);
            Traits.Add(u10);
            Traits.Add(u01);
            Traits.Add(u20);
            Traits.Add(u02);
            Traits.Add(u11);
        }

        public void RegionCheckNeighbours(int i, int j)
        {
            if (j - 1 >= 0)
            {
                if (IfAlreadyChecked[i, j - 1] == false)
                {
                    if (FirstInGroup - PixelsBW[i, j - 1] == 0)
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
                    if (FirstInGroup - PixelsBW[i - 1, j] == 0)
                    {
                        IfAlreadyChecked[i - 1, j] = true;
                        stack.Push(new XY(i - 1, j));
                    }
                }
            }
            if (i + 1 < 28)
            {
                if (IfAlreadyChecked[i + 1, j] == false)
                {
                    if (FirstInGroup - PixelsBW[i + 1, j] == 0)
                    {
                        IfAlreadyChecked[i + 1, j] = true;
                        stack.Push(new XY(i + 1, j));
                    }
                }
            }
            if (j + 1 < 28)
            {
                if (IfAlreadyChecked[i, j + 1] == false)
                {
                    if (FirstInGroup - PixelsBW[i, j + 1] == 0)
                    {
                        IfAlreadyChecked[i, j + 1] = true;
                        stack.Push(new XY(i, j + 1));
                    }
                }
            }
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 28; ++i)
            {
                for (int j = 0; j < 28; ++j)
                {
                    if (this.Pixels[i, j] == 0)
                        s += " "; // white
                    else if (this.Pixels[i, j] == 255)
                        s += "O"; // black
                    else
                        s += "."; // gray
                }
                s += "\n";
            }
            s += this.Label.ToString();
            return s;
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

        public bool[,] IfAlreadyChecked = new bool[28, 28];
        protected Stack<XY> stack = new Stack<XY>();
        public int FirstInGroup;

    } 
}
