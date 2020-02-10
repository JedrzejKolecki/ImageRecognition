using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace RO
{
    class TextureReader
    {
        public static List<BasicImage> TrainingImages(bool ifTime, bool ifDFT)
        {
            List<BasicImage> Photos = new List<BasicImage>();

            string[] photosLinen = Directory.GetFiles("texture-train/linen");
            for(int k = 0; k < photosLinen.Length; k++)
            {
                Bitmap bmp = new Bitmap(photosLinen[k]);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R / 255.0;
                    }

                Photos.Add(new TextureImage(photo, "linen", ifTime, ifDFT));
                Console.WriteLine("Dodano treningowy obrazek: linen " + k + "/" + photosLinen.Length);
            }

            string[] photosSalt = Directory.GetFiles("texture-train/salt");
            for (int k = 0; k < photosSalt.Length; k++)
            {
                Bitmap bmp = new Bitmap(photosSalt[k]);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R / 255.0;
                    }

                Photos.Add(new TextureImage(photo, "salt", ifTime, ifDFT));
                Console.WriteLine("Dodano treningowy obrazek: salt " + k + "/" + photosSalt.Length);
            }

            string[] photosStraw = Directory.GetFiles("texture-train/straw");
            for (int k = 0; k < photosStraw.Length; k++)
            {
                Bitmap bmp = new Bitmap(photosStraw[k]);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R / 255.0;
                    }

                Photos.Add(new TextureImage(photo, "straw", ifTime, ifDFT));
                Console.WriteLine("Dodano treningowy obrazek: straw " + k + "/" + photosStraw.Length);
            }

            string[] photosWood = Directory.GetFiles("texture-train/wood");
            for (int k = 0; k < photosWood.Length; k++)
            {
                Bitmap bmp = new Bitmap(photosWood[k]);
                double[,] photo = new double[bmp.Height, bmp.Width];

                for (int i = 0; i < bmp.Height; i++)
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        photo[i, j] = bmp.GetPixel(i, j).R / 255.0;
                    }

                Photos.Add(new TextureImage(photo, "wood", ifTime, ifDFT));
                Console.WriteLine("Dodano treningowy obrazek: wood " + k + "/" + photosWood.Length);
            }

            Stream stream = File.Open("TrainingTextures.xml", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, Photos);
            stream.Close();

            return Photos;
        }

        public static List<BasicImage> TestImages(bool ifTime, bool ifDFT)
        {
            List<BasicImage> Photos = new List<BasicImage>(); //ostateczna lista

            TextureImage label1 = new TextureImage();
            TextureImage test1 = new TextureImage();
            TextureImage label2 = new TextureImage();
            TextureImage test2 = new TextureImage();
            TextureImage label3 = new TextureImage();
            TextureImage test3 = new TextureImage();

            string[] testPhotos = Directory.GetFiles("texture-test/");

            foreach (string name in testPhotos)
            {
                switch (name)
                {
                    case "texture-test/label1.bmp":
                        label1 = GetImage(name, ifTime, ifDFT);
                        break;

                    case "texture-test/test1.bmp":
                        test1 = GetImage(name, ifTime, ifDFT);
                        break;

                    case "texture-test/label2.bmp":
                        label2 = GetImage(name, ifTime, ifDFT);
                        break;

                    case "texture-test/test2.bmp":
                        test2 = GetImage(name, ifTime, ifDFT);
                        break;

                    case "texture-test/label3.bmp":
                        label3 = GetImage(name, ifTime, ifDFT);
                        break;

                    case "texture-test/test3.bmp":
                        test3 = GetImage(name, ifTime, ifDFT);
                        break;
                }

            }

            Tuple<TextureImage, TextureImage> tuple1 = new Tuple<TextureImage, TextureImage>(label1, test1);
            Tuple<TextureImage, TextureImage> tuple2 = new Tuple<TextureImage, TextureImage>(label2, test2);
            Tuple<TextureImage, TextureImage> tuple3 = new Tuple<TextureImage, TextureImage>(label3, test3);

            Photos.AddRange(LabelImages(tuple1, ifTime, ifDFT));
            Console.WriteLine("Pierwszy obraz testowy wczytany");
            Photos.AddRange(LabelImages(tuple2, ifTime, ifDFT));
            Console.WriteLine("Drugi obraz testowy wczytany");
            Photos.AddRange(LabelImages(tuple3, ifTime, ifDFT));
            Console.WriteLine("Trzeci obraz testowy wczytany");

            Stream stream = File.Open("TestTextures.xml", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, Photos);
            stream.Close();

            return Photos;
        }

        public static List<BasicImage> TrainingImagesFromFile(bool ifTime, bool ifDFT)
        {
            List<BasicImage> Photos = new List<BasicImage>();

            Stream stream = File.Open("TrainingTextures.xml", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            Photos = (List<BasicImage>)formatter.Deserialize(stream);
            stream.Close();

            foreach(TextureImage im in Photos)
            {
                im.ifTime = ifTime;
                im.ifDFT = ifDFT;
            }

            return Photos;
        }

        public static List<BasicImage> TestImagesFromFile(bool ifTime, bool ifDFT)
        {
            List<BasicImage> Photos = new List<BasicImage>();

            Stream stream = File.Open("TestTextures.xml", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            Photos = (List<BasicImage>)formatter.Deserialize(stream);
            stream.Close();

            foreach (TextureImage im in Photos)
            {
                im.ifTime = ifTime;
                im.ifDFT = ifDFT;
            }

            return Photos;
        }

        //przypisanie labeli dla danych par obraz testowy - obraz z odpowiednimi labelami
        private static List<BasicImage> LabelImages(Tuple<TextureImage, TextureImage> tuple, bool ifTime, bool ifDFT)
        {
            TextureImage label = tuple.Item1;
            TextureImage test = tuple.Item2;

            List<BasicImage> list = new List<BasicImage>();
            int division = 8; //8 kolumn i 8 rzedow - 64 kwadraty
            int squareHeight = label.Image.GetLength(0) / division;
            int squareWidth = label.Image.GetLength(1) / division;

            int squareWidthNumber = 0;
            int squareHeightNumber = 0;

            string labelName = "notLabeled";
            double topColor = 0;

            int k = 0;

            while (squareWidthNumber < division && squareHeightNumber < division)
            {
                //szukanie najczesciej wystepujacego koloru w kwadracie
                List<double> colors = new List<double>();
                for (int x = squareHeight * squareHeightNumber; x < squareHeight * (squareHeightNumber + 1); x++)
                {
                    for (int y = squareWidth * squareWidthNumber; y < squareWidth * (squareWidthNumber + 1); y++)
                    {
                        colors.Add(label.Image[x, y]);
                    }
                }

                //wybranie najczestszego
                topColor = colors.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

                if (topColor == 224)
                    labelName = "linen";
                if (topColor == 160)
                    labelName = "salt";
                if (topColor == 96)
                    labelName = "straw";
                if (topColor == 32)
                    labelName = "wood";

                //Debug.WriteLine("Dodano label: " + labelName);

                //kwadrat 64 z testowego zbioru z labelem ktory ostatecznie trafia do listy
                double[,] photo = new double[squareHeight, squareWidth];

                for (int i = 0; i < squareHeight; i++)
                {
                    for (int j = 0; j < squareWidth; j++)
                    {
                        photo[i, j] = test.Image[i + squareHeight * squareHeightNumber, j + squareWidth * squareWidthNumber] / 255.0; //wczesniej nie bylo konwersji
                    }
                }

                TextureImage texture = new TextureImage(photo, labelName, ifTime, ifDFT);
                list.Add(texture);
                //texture.SaveToFile();

                Console.WriteLine("Dodano testowy obrazek: " + k + "/" + division*division);
                k++;

                squareWidthNumber++;
                if (squareWidthNumber == division)
                {
                    squareWidthNumber = 0;
                    squareHeightNumber++;
                }
            }
            return list;
        }

        //zrobienie obrazow z plikow
        private static TextureImage GetImage(string name, bool ifTime, bool ifDFT)
        {
            Bitmap labelbmp = new Bitmap(name);
            double[,] labelPhoto = new double[labelbmp.Height, labelbmp.Width];

            for (int i = 0; i < labelbmp.Height; i++)
                for (int j = 0; j < labelbmp.Width; j++)
                {
                    labelPhoto[i, j] = labelbmp.GetPixel(i, j).R; //tu nie dziele jeszcze
                }

            TextureImage img = new TextureImage();
            img.Image = labelPhoto;
            img.Label = name;
            return img;
        }
    }
}
