using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace RO
{
    [Serializable]
    class TextureImage : BasicImage
    {
        public double[,] Image { get; set; }
        private int[] Histogram;
        private int[] ModulusHistogram;
        public bool ifTime;
        public bool ifDFT;
        [NonSerialized()] private Complex[,] DFT;
        [NonSerialized()] private double[,] IDFT;
        public double[,] Modulus;
        [NonSerialized()] private LawFeatures LawFeaturesFromImage;

        //potrzebny do ładowania 
        public TextureImage()
        {

        }

        public TextureImage(double[,] image, string label, bool ifTime, bool ifDFT) : base(image, label)
        {
            this.Image = image;
            this.Label = label;
            this.ifDFT = ifDFT;
            this.ifTime = ifTime;
            //LawFeaturesFromImage = new LawFeatures();


            ModifyHistogram();

            #region LAW
            /*
            CalculateLawFeatures();

            Bitmap bmp0 = new Bitmap(64, 64, PixelFormat.Format32bppArgb);
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.L5E5xE5L5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_L5E5xE5L5.png");
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.L5R5xR5L5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_L5R5xR5L5.png");
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.E5S5xS5E5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_E5S5xS5E5.png");
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.L5S5xS5L5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_L5S5xS5L5.png");
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.E5R5xR5E5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_E5R5xR5E5.png");
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.R5S5xS5R5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_R5S5xS5R5.png");
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.S5S5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_S5S5.png");
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.R5R5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_R5R5.png");
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    int color = (int)(LawFeaturesFromImage.E5E5[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            bmp0.Save(Label + this.GetHashCode().ToString() + "_E5E5.png");
            */
            #endregion

            CalculateDFT();
            Swap();

            CalculateModulus();
            ModifyHistogramModulus();
            //SaveToFileModulus();

            //Swap();
            //CalculateIDFT();
        }

        public override void CalculateTraits()
        {
            Traits = new List<double>();
            Neighbours = new List<Neighbour>();

            //int division = 8; //8x8pikseli 64kwadraty
            int division = 16; //4x4piksele 256kwadratow

            int squareWidth = Modulus.GetLength(0) / division;
            int squareHeight = Modulus.GetLength(1) / division;

            int squareWidthNumber = 0;
            int squareHeightNumber = 0;
            #region W czasie

            if (ifTime)
            {
                while (squareWidthNumber < division && squareHeightNumber < division)
                {
                    Traits.Add(AverageBrightnessTime(squareWidth * squareWidthNumber, squareWidth * (squareWidthNumber + 1), squareHeight * squareHeightNumber, squareHeight * (squareHeightNumber + 1)));
                    squareHeightNumber++;

                    if (squareHeightNumber == division)
                    {
                        squareHeightNumber = 0;
                        squareWidthNumber++;
                    }
                }
            }

            #endregion

            squareWidthNumber = 0;
            squareHeightNumber = 0;

            #region W czestotliwosci

            if (ifDFT)
            {
                while (squareWidthNumber < division && squareHeightNumber < division)
                {
                    //AverageBrightness(0, halfWidth / 2, 0, halfHeight / 2)
                    //x = squareHeight * squareHeightNumber; x < squareHeight * (squareHeightNumber + 1); x++
                    //Traits.AddRange(CountWhite(squareWidth * squareWidthNumber, squareWidth * (squareWidthNumber + 1), squareHeight * squareHeightNumber, squareHeight * (squareHeightNumber + 1)));//(squareWidth * squareWidthNumber) + division, )
                    Traits.Add(AverageBrightness(squareWidth * squareWidthNumber, squareWidth * (squareWidthNumber + 1), squareHeight * squareHeightNumber, squareHeight * (squareHeightNumber + 1)));
                    squareHeightNumber++;

                    if (squareHeightNumber == division)
                    {
                        squareHeightNumber = 0;
                        squareWidthNumber++;
                    }
                }

                System.Console.WriteLine("DODANO CECH W LICZBIE: " + Traits.Count());
            }

            #endregion

            //int rows = Image.GetLength(0);
            //int columns = Image.GetLength(1);

            //for (int i = 0; i < rows; i++)
            //    for (int j = 0; j < columns; j++)
            //        Traits.Add(Image[i, j]);
        }



        private struct Complex
        {
            public double Re { get; set; }
            public double Im { get; set; }
            public Complex(double re, double im)
            {
                Re = re; Im = im;
            }
        }

        private void CalculateDFT()
        {
            int rows = Image.GetLength(0);
            int columns = Image.GetLength(1);

            Complex[,] DFT_2D = new Complex[rows, columns];
            DFT = new Complex[rows, columns];

            //dft dla rzedow
            for (int i = 0; i < rows; i++) //dla kazdego rzedu
            {
                // policz jednowymiarowe dft
                for (int k = 0; k < columns; k++) // kazdy wynik DFT
                {
                    double cos = 0.0;
                    double sin = 0.0;

                    for (int n = 0; n < columns; n++)
                    {
                        cos += Image[i, n] * Math.Cos(2 * Math.PI * n * k / columns);
                        sin += Image[i, n] * Math.Sin(2 * Math.PI * n * k / columns) * (-1.0);
                    }

                    DFT_2D[i, k] = new Complex(cos, sin);
                }
            }

            //dft dla kolumn
            for (int i = 0; i < columns; i++)
            {
                // policz jednowymiarowe dft
                for (int k = 0; k < rows; k++) // kazdy wynik DFT
                {
                    double cos = 0.0;
                    double sin = 0.0;

                    for (int n = 0; n < rows; n++)
                    {
                        cos += DFT_2D[n, i].Re * Math.Cos(2 * Math.PI * n * k / rows) + DFT_2D[n, i].Im * Math.Sin(2 * Math.PI * n * k / rows);
                        sin += DFT_2D[n, i].Im * Math.Cos(2 * Math.PI * n * k / rows) - DFT_2D[n, i].Re * Math.Sin(2 * Math.PI * n * k / rows);
                    }

                    DFT[k, i] = new Complex(cos, sin);
                }
            }
        }

        private void CalculateIDFT()
        {
            int rows = Image.GetLength(0);
            int columns = Image.GetLength(1);

            Complex[,] IDFT_2D = new Complex[rows, columns];
            IDFT = new double[rows, columns];

            //dft dla kolumn
            for (int i = 0; i < columns; i++)
            {
                // policz jednowymiarowe dft
                for (int k = 0; k < rows; k++) // kazdy wynik DFT
                {
                    double cos = 0.0;
                    double sin = 0.0;

                    for (int n = 0; n < rows; n++)
                    {
                        cos += DFT[n, i].Re * Math.Cos(2 * Math.PI * n * k / rows) - DFT[n, i].Im * Math.Sin(2 * Math.PI * n * k / rows);
                        sin += DFT[n, i].Im * Math.Cos(2 * Math.PI * n * k / rows) + DFT[n, i].Re * Math.Sin(2 * Math.PI * n * k / rows);
                    }

                    IDFT_2D[k, i] = new Complex(cos / rows, sin / rows);
                }
            }

            //dft dla rzedow
            for (int i = 0; i < rows; i++) //dla kazdego rzedu
            {
                // policz jednowymiarowe dft
                for (int k = 0; k < columns; k++) // kazdy wynik DFT
                {
                    double cos = 0.0;
                    double sin = 0.0;

                    for (int n = 0; n < columns; n++)
                    {
                        //wynik += _obrazPoRazie[i, n].Re * Math.Cos(2 * Math.PI * n * k / columns);
                        //wynik += _obrazPoRazie[i, n].Im * Math.Sin(2 * Math.PI * n * k / columns);
                        cos += IDFT_2D[i, n].Re * Math.Cos(2 * Math.PI * n * k / rows) - IDFT_2D[i, n].Im * Math.Sin(2 * Math.PI * n * k / rows);
                        sin += IDFT_2D[i, n].Im * Math.Cos(2 * Math.PI * n * k / rows) + IDFT_2D[i, n].Re * Math.Sin(2 * Math.PI * n * k / rows);
                    }

                    IDFT[i, k] = Math.Sqrt(cos * cos + sin * sin) / columns;
                }
            }
        }

        private void ModifyHistogram()
        {
            Histogram = new int[256];
            for (int i = 0; i < Image.GetLength(0); ++i)
                for (int j = 0; j < Image.GetLength(1); ++j)
                {
                    Histogram[(int)Math.Round(Image[i, j] * 255)]++;
                }

            double N = Image.GetLength(0) * Image.GetLength(1);

            double[] D = new double[256];
            for (int i = 0; i < 256; ++i)
            {
                int hSum = 0;
                for (int x = 0; x < i; x++)
                {
                    hSum += Histogram[x];
                }
                D[i] = hSum / N;
            }

            double D0 = 0;
            for (int i = 0; i < 256; ++i)
            {
                if (D[i] > 0)
                {
                    D0 = D[i];
                    break;
                }
            }

            for (int i = 0; i < Image.GetLength(0); i++)
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    Image[i, j] = (((D[(int)Math.Round(Image[i, j] * 255)] - D0) / (1 - D0)) * 255) / 255.0;
                    if (Image[i, j] < 0) Image[i, j] = 0;
                    if (Image[i, j] > 1) Image[i, j] = 1;
                }
        }

        private void ModifyHistogramModulus()
        {
            ModulusHistogram = new int[256];
            for (int i = 0; i < Modulus.GetLength(0); ++i)
                for (int j = 0; j < Modulus.GetLength(1); ++j)
                {
                    if (Modulus[i, j] < 0) Modulus[i, j] = 0;
                    if (Modulus[i, j] > 255) Modulus[i, j] = 255;
                    ModulusHistogram[(int)Math.Round(Modulus[i, j])]++;
                }

            double N = Modulus.GetLength(0) * Modulus.GetLength(1);

            double[] D = new double[256];
            for (int i = 0; i < 256; ++i)
            {
                int hSum = 0;
                for (int x = 0; x < i; x++)
                {
                    hSum += ModulusHistogram[x];
                }
                D[i] = hSum / N;
            }

            double D0 = 0;
            for (int i = 0; i < 256; ++i)
            {
                if (D[i] > 0)
                {
                    D0 = D[i];
                    break;
                }
            }

            for (int i = 0; i < Modulus.GetLength(0); i++)
                for (int j = 0; j < Modulus.GetLength(1); j++)
                {
                    Modulus[i, j] = (((D[(int)Math.Round(Image[i, j] * 255)] - D0) / (1 - D0)) * 255);

                    if (Modulus[i, j] < 0) Modulus[i, j] = 0;
                    if (Modulus[i, j] > 255) Modulus[i, j] = 255;
                }
        }

        private void CalculateModulus()
        {
            int rows = Image.GetLength(0);
            int columns = Image.GetLength(1);

            Modulus = new double[rows, columns];

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    double modul = Math.Sqrt(Math.Pow(DFT[x, y].Re, 2) + Math.Pow(DFT[x, y].Im, 2));
                    //double ln = Math.Log(modul + 1);
                    //if (ln < 0) ln = 0;
                    //if (ln > 255) ln = 255;
                    //ln += 50;
                    //ln = (ln - 128) * 1.1 + 128;
                    //if (ln < 0) ln = 0;
                    //if (ln > 255) ln = 255;
                    Modulus[x, y] = modul;
                }
            }

            Modulus[0, 0] = 0;
        }

        #region Cechy

        //srednia jasnosc w kwadracie w Time
        private double AverageBrightnessTime(int startX, int endX, int startY, int endY)
        {
            double brightness = 0;
            //System.Console.WriteLine("X1: " + startX + " X2: " + endX + " Y1: " + startY + " Y2: " + endY);
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    brightness += Image[x, y];
                }
            }

            brightness /= ((endX - startX) * (endY - startY));
            //System.Console.WriteLine("Jasnosc:" + brightness);
            return brightness;
        }

        //srednia jasnosc w kwadracie w Modulus
        private double AverageBrightness(int startX, int endX, int startY, int endY)
        {
            double brightness = 0;
            //System.Console.WriteLine("X1: " + startX + " X2: " + endX + " Y1: " + startY + " Y2: " + endY);
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    brightness += Modulus[x, y];
                }
            }
            brightness /= ((endX - startX) * (endY - startY));
            //System.Console.WriteLine("Jasnosc:" + brightness);
            return brightness;
        }

        //zliczanie po ilosci bialych pikseli w Modulus
        private List<double> CountWhite(int startX, int endX, int startY, int endY)
        {
            double brightness = 0;
            List<double> list = new List<double>();

            for (int x = startX; x < endX; x++)
            {
                int countWhiteRow = 0;
                for (int y = startY; y < endY; y++)
                {
                    if (Modulus[x, y] >= 240)
                        countWhiteRow++;
                }
                list.Add(countWhiteRow);
            }


            for (int y = startY; y < endY; y++)
            {
                int countWhiteColumn = 0;
                for (int x = startX; x < endX; x++)
                {
                    if (Modulus[x, y] >= 240)
                        countWhiteColumn++;
                }
                list.Add(countWhiteColumn);
            }

            //System.Console.WriteLine("Jasnosc:" + brightness);
            return list;
        }
        #endregion

        #region LAW
        private void CalculateLawFeatures()
        {
            int rows = Image.GetLength(0);
            int columns = Image.GetLength(1);

            double[,] LE = new double[rows, columns];
            LE = Filter(Law.L5E5);
            double[,] EL = new double[rows, columns];
            EL = Filter(Law.E5L5);
            LawFeaturesFromImage.L5E5xE5L5 = AverageMap(LE, EL);

            double[,] LR = new double[rows, columns];
            LR = Filter(Law.L5R5);
            double[,] RL = new double[rows, columns];
            RL = Filter(Law.R5L5);
            LawFeaturesFromImage.L5R5xR5L5 = AverageMap(LR, RL);

            double[,] ES = new double[rows, columns];
            ES = Filter(Law.E5S5);
            double[,] SE = new double[rows, columns];
            SE = Filter(Law.S5E5);
            LawFeaturesFromImage.E5S5xS5E5 = AverageMap(ES, SE);

            double[,] LS = new double[rows, columns];
            LS = Filter(Law.L5S5);
            double[,] SL = new double[rows, columns];
            SL = Filter(Law.S5L5);
            LawFeaturesFromImage.L5S5xS5L5 = AverageMap(LS, SL);

            double[,] ER = new double[rows, columns];
            ER = Filter(Law.E5R5);
            double[,] RE = new double[rows, columns];
            RE = Filter(Law.R5E5);
            LawFeaturesFromImage.E5R5xR5E5 = AverageMap(RE, ER);

            double[,] SR = new double[rows, columns];
            SR = Filter(Law.S5R5);
            double[,] RS = new double[rows, columns];
            RS = Filter(Law.R5E5);
            LawFeaturesFromImage.R5S5xS5R5 = AverageMap(RS, SR);

            LawFeaturesFromImage.S5S5 = Filter(Law.S5S5);

            LawFeaturesFromImage.R5R5 = Filter(Law.R5R5);

            LawFeaturesFromImage.E5E5 = Filter(Law.E5E5);
        }

        private double[,] AverageMap(double[,] a, double[,] b)
        {
            int rows = Image.GetLength(0);
            int columns = Image.GetLength(1);
            double[,] avr = new double[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    avr[i, j] = (a[i, j] + b[i, j]) / 2;
                }

            return avr;
        }

        private double[,] Filter(double[,] mask)
        {
            int rows = Image.GetLength(0);
            int columns = Image.GetLength(1);
            double[,] filtered = new double[rows, columns];

            int maskSize = 25;
            XY[] martix = new XY[maskSize];
            int center = 2;
            int q = 0;

            for (int i = -center; i <= center; i++)
                for (int k = -center; k <= center; k++)
                {
                    martix[q].X = k;
                    martix[q].Y = i;
                    q++;
                }

            double[] mask1D = new double[maskSize];
            for (int i = 0; i < mask.GetLength(0); i++)
                for (int j = 0; j < mask.GetLength(1); j++)
                {
                    mask1D[mask.GetLength(1) * i + j] = mask[i, j];
                }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    double sum = 0.0;

                    for (int k = 0; k < maskSize; k++)
                    {
                        int x = i + martix[k].X;
                        int y = j + martix[k].Y;
                        if (x >= 0 && x < rows && y >= 0 && y < columns)
                        {
                            sum += Image[x, y] * mask1D[k];
                        }
                    }

                    filtered[i, j] = Math.Abs(sum);
                }
            }

            return filtered;
        }
        #endregion

        #region Pomoce

        public struct LawFeatures
        {
            public double[,] L5E5xE5L5, L5S5xS5L5, L5R5xR5L5, E5S5xS5E5, E5R5xR5E5, R5S5xS5R5,
                S5S5, E5E5, R5R5;
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

        public void SaveToFile()
        {
            Bitmap bmp = new Bitmap(Image.GetLength(0), Image.GetLength(1), PixelFormat.Format32bppArgb);
            for (int i = 0; i < Image.GetLength(0); i++)
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    int color = (int)(Image[i, j] * 255);
                    bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(color, color, color));
                }

            bmp.Save(Label + this.GetHashCode().ToString() + ".png");
        }

        public void SaveToFileModulus()
        {
            Bitmap bmp0 = new Bitmap(Modulus.GetLength(0), Modulus.GetLength(1), PixelFormat.Format32bppArgb);
            for (int i = 0; i < Modulus.GetLength(0); i++)
                for (int j = 0; j < Modulus.GetLength(1); j++)
                {
                    int color = (int)(Modulus[i, j]);
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;
                    bmp0.SetPixel(i, j, System.Drawing.Color.FromArgb(color, color, color));
                }

            bmp0.Save(Label + this.GetHashCode().ToString() + "_Modulus.png");
        }

        private void Swap()
        {
            int rows = Image.GetLength(0);
            int columns = Image.GetLength(1);

            Complex[,] part1 = new Complex[rows / 2, columns / 2];
            Complex[,] part2 = new Complex[rows / 2, columns / 2];
            Complex[,] part3 = new Complex[rows / 2, columns / 2];
            Complex[,] part4 = new Complex[rows / 2, columns / 2];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                {
                    if (r < rows / 2 && c < columns / 2)
                        part1[r, c] = DFT[r, c];

                    if (r < rows / 2 && c >= columns / 2)
                        part2[r, c - (columns / 2)] = DFT[r, c];

                    if (r >= rows / 2 && c < columns / 2)
                        part3[r - (rows / 2), c] = DFT[r, c];

                    if (r >= rows / 2 && c >= columns / 2)
                        part4[r - (rows / 2), c - (columns / 2)] = DFT[r, c];
                }

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                {
                    if (r < rows / 2 && c < columns / 2)
                        DFT[r, c] = part4[r, c];
                    if (r < rows / 2 && c >= columns / 2)
                        DFT[r, c] = part3[r, c - (columns / 2)];
                    if (r >= rows / 2 && c < columns / 2)
                        DFT[r, c] = part2[r - (rows / 2), c];
                    if (r >= rows / 2 && c >= columns / 2)
                        DFT[r, c] = part1[r - (rows / 2), c - (columns / 2)];
                }
        }

        #endregion
    }
}
