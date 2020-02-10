using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RO
{
    public class ImageRecognition
    {
        public static int k;
        public List<BasicImage> TrainingImages { get; set; }
        public List<BasicImage> TestImages { get; set; }
        public float Result { get; set; }

        public ImageRecognition(int ka)
        {
            k = ka;
        }

        public ImageRecognition() { }

        public void CalculateTraits()
        {
            foreach (BasicImage image in TrainingImages)
            {
                image.CalculateTraits();
            }
            int i = 1;
            foreach (BasicImage image in TestImages)
            {
                image.CalculateTraits();

                System.Console.WriteLine("CalculateTraits " + i + "/" + TestImages.Count);
                i++;
            }
        }

        public void CalculateDistances(string metric)
        {
            int i = 1;
            foreach (BasicImage test in TestImages)
            {
                foreach (BasicImage train in TrainingImages)
                {
                    test.CalculateDistance(train, metric);
                }

                System.Console.WriteLine("CalculateDistances " + i + "/" + TestImages.Count);
                i++;
                //if (i == 100) break;
            }
        }

        public void Classify()
        {
            var sb = new StringBuilder();
            int classified = 0;
            int i = 1;

            foreach (BasicImage test in TestImages)
            {
                test.Classify();
                if (test.LabelFromClassification.ToString() == test.Label.ToString())
                    classified++;
                Console.WriteLine(i + " / " + TestImages.Count + " " + test.Label + "->" + test.LabelFromClassification);
                sb.Append(i + " / " + TestImages.Count + " " + test.Label + "->" + test.LabelFromClassification + "\r\n");

                var line = String.Format("{0},{1},{2}", i, test.Label, test.LabelFromClassification);

                i++;
                //if (i == 100) break;
            }

            Result = classified / (float)TestImages.Count;
            //Result = classified / 100.0f;
            Console.WriteLine("procent " + Result * 100);
            sb.Append("procent " + Result * 100 + "\r\n");
            System.IO.File.WriteAllText("Results.txt", sb.ToString());

        }

        public void Correlation()
        {
            int classified = 0;
            int i = 1;
            foreach (TextureImage testTexture in TestImages)
            {
                int rows = testTexture.Image.GetLength(0);
                int columns = testTexture.Image.GetLength(1);

                List<Tuple<double, TextureImage>> correlationCoeff = new List<Tuple<double, TextureImage>>();

                foreach (TextureImage trainingTexture in TrainingImages)
                {
                    double correlationImage = 0;
                    double correlationModulus = 0;

                    for (int x = 0; x < rows; x++)
                        for (int y = 0; y < columns; y++)
                        {
                            if (testTexture.ifTime) correlationImage += testTexture.Image[x, y] * trainingTexture.Image[x, y];
                            if (testTexture.ifDFT) correlationModulus += testTexture.Modulus[x, y] * trainingTexture.Modulus[x, y];
                        }

                    double correlationSum = 0;
                    if (testTexture.ifTime) correlationSum += correlationImage;
                    if (testTexture.ifDFT) correlationSum += correlationModulus;

                    correlationCoeff.Add(Tuple.Create(correlationSum, trainingTexture));
                }

                correlationCoeff.Sort((a, b) => b.Item1.CompareTo(a.Item1));

                testTexture.LabelFromClassification = correlationCoeff[0].Item2.Label;

                if (testTexture.LabelFromClassification.ToString() == testTexture.Label.ToString())
                    classified++;

                Console.WriteLine(i + " / " + TestImages.Count + " " + testTexture.Label + "->" + testTexture.LabelFromClassification);
                i++;
            }


            Result = classified / (float)TestImages.Count;
            Console.WriteLine("procent " + Result * 100);
        }

    }
}

