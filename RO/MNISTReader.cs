using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RO
{
    public static class MNISTReader
    {
        public static List<BasicImage> TrainingImages()
        {
            FileStream ifsLabels = new FileStream("train-labels.idx1-ubyte", FileMode.Open); // test labels
            FileStream ifsImages = new FileStream("train-images.idx3-ubyte", FileMode.Open); // test images

            BinaryReader brLabels = new BinaryReader(ifsLabels);
            BinaryReader brImages = new BinaryReader(ifsImages);

            int magic1 = ReadBigInt32(brImages); // discard
            int numImages = ReadBigInt32(brImages);
            int numRows = ReadBigInt32(brImages);
            int numCols = ReadBigInt32(brImages);

            int magic2 = ReadBigInt32(brLabels);
            int numLabels = ReadBigInt32(brLabels);

            byte[,] pixels = new byte[28,28];

            List<BasicImage> trainingData = new List<BasicImage>();

            // each test image
            for (int di = 0; di < 60000; ++di)
            {
                for (int i = 0; i < 28; ++i)
                {
                    for (int j = 0; j < 28; ++j)
                    {
                        byte b = brImages.ReadByte();
                        pixels[i,j] = b;
                    }
                }

                byte lbl = brLabels.ReadByte();

                trainingData.Add(new DigitImage(pixels, lbl));
            }

            ifsImages.Close();
            brImages.Close();
            ifsLabels.Close();
            brLabels.Close();

            return trainingData;
        }

        public static List<BasicImage> TestImages()
        {
            FileStream ifsLabels = new FileStream("t10k-labels.idx1-ubyte", FileMode.Open); // test labels
            FileStream ifsImages = new FileStream("t10k-images.idx3-ubyte", FileMode.Open); // test images

            BinaryReader brLabels = new BinaryReader(ifsLabels);
            BinaryReader brImages = new BinaryReader(ifsImages);

            int magic1 = ReadBigInt32(brImages); // discard
            int numImages = ReadBigInt32(brImages);
            int numRows = ReadBigInt32(brImages);
            int numCols = ReadBigInt32(brImages);

            int magic2 = ReadBigInt32(brLabels);
            int numLabels = ReadBigInt32(brLabels);

            byte[,] pixels = new byte[28,28];

            List<BasicImage> testData = new List<BasicImage>();

            // each test image
            for (int di = 0; di < 10000; ++di)
            {
                for (int i = 0; i < 28; ++i)
                {
                    for (int j = 0; j < 28; ++j)
                    {
                        byte b = brImages.ReadByte();
                        pixels[i,j] = b;
                    }
                }

                byte lbl = brLabels.ReadByte();

                testData.Add(new DigitImage(pixels, lbl));
            }

            ifsImages.Close();
            brImages.Close();
            ifsLabels.Close();
            brLabels.Close();

            return testData;
        }

        private static int ReadBigInt32(BinaryReader brImages)
        {
            var bytes = brImages.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}

//static void Main(string[] args)
//    {
//        try
//        {
//            Console.WriteLine("\nBegin\n");
//            FileStream ifsLabels =
//             new FileStream("train-labels.idx1-ubyte",
//             FileMode.Open); // test labels
//            FileStream ifsImages =
//             new FileStream("train-images.idx3-ubyte",
//             FileMode.Open); // test images

//            BinaryReader brLabels =
//             new BinaryReader(ifsLabels);
//            BinaryReader brImages =
//             new BinaryReader(ifsImages);

//            int magic1 = ReadBigInt32(brImages); // discard
//            int numImages = ReadBigInt32(brImages);
//            int numRows = ReadBigInt32(brImages);
//            int numCols = ReadBigInt32(brImages);

//            int magic2 = ReadBigInt32(brLabels);
//            int numLabels = ReadBigInt32(brLabels);

//            byte[][] pixels = new byte[28][];
//            for (int i = 0; i < pixels.Length; ++i)
//                pixels[i] = new byte[28];

//            // each test image
//            for (int di = 0; di < 10000; ++di)
//            {
//                for (int i = 0; i < 28; ++i)
//                {
//                    for (int j = 0; j < 28; ++j)
//                    {
//                        byte b = brImages.ReadByte();
//                        pixels[i][j] = b;
//                    }
//                }

//                byte lbl = brLabels.ReadByte();

//                DigitImage dImage =
//                  new DigitImage(pixels, lbl);
//                Console.WriteLine(dImage.ToString());
//                Console.ReadLine();
//            } // each image

//            ifsImages.Close();
//            brImages.Close();
//            ifsLabels.Close();
//            brLabels.Close();

//            Console.WriteLine("\nEnd\n");
//            Console.ReadLine();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//            Console.ReadLine();
//        }
//    } // Main

//private static int ReadBigInt32(BinaryReader brImages)
//{
//    var bytes = brImages.ReadBytes(sizeof(Int32));
//    if (BitConverter.IsLittleEndian)
//        Array.Reverse(bytes);
//    return BitConverter.ToInt32(bytes, 0);
//}
//} // Program
//} // ns

