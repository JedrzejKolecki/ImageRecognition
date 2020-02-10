using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_DigitImages(object sender, RoutedEventArgs e)
        {
            ImageRecognition IR = new ImageRecognition(Convert.ToInt32(k_box.Text));
            var watch = new System.Diagnostics.Stopwatch();
            IR.TrainingImages = MNISTReader.TrainingImages();
            IR.TestImages = MNISTReader.TestImages();
            watch.Start();
            IR.CalculateTraits();
            IR.CalculateDistances(Metrice.SelectionBoxItem.ToString());
            IR.Classify();
            Result.Text = (IR.Result*100).ToString();
            watch.Stop();
            System.IO.File.WriteAllText("Resultstime.txt", $"Execution Time: {(float)watch.ElapsedMilliseconds/1000} s");

            Console.WriteLine($"Execution Time: {(float)watch.ElapsedMilliseconds / 1000} s");

        }

        private void Button_PhotoImages(object sender, RoutedEventArgs e)
        {
            ImageRecognition IR = new ImageRecognition(Convert.ToInt32(k_box.Text));
            var watch = new System.Diagnostics.Stopwatch();
            IR.TrainingImages = PhotoReader.TrainingImages();
            if ("Plain" == PhotoTest.SelectionBoxItem.ToString())
                IR.TestImages = PhotoReader.TestImagesPlain();
            if ("Light" == PhotoTest.SelectionBoxItem.ToString())
                IR.TestImages = PhotoReader.TestImagesLight();
            if ("Light_30deg" == PhotoTest.SelectionBoxItem.ToString())
                IR.TestImages = PhotoReader.TestImagesLight30();
            watch.Start();
            IR.CalculateTraits();
            IR.CalculateDistances(Metrice.SelectionBoxItem.ToString());
            IR.Classify();
            Result.Text = (IR.Result * 100).ToString();
            watch.Stop();
            System.IO.File.WriteAllText("Resultstime.txt", $"Execution Time: {(float)watch.ElapsedMilliseconds / 1000} s");

            Console.WriteLine($"Execution Time: {(float)watch.ElapsedMilliseconds / 1000} s");
        }

        private void Button_TextureImages(object sender, RoutedEventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            Law.CalculateMasks();
            ImageRecognition IR = new ImageRecognition(Convert.ToInt32(k_box.Text));
            //IR.TrainingImages = TextureReader.TrainingImages(true, true);
            //IR.TestImages = TextureReader.TestImages(true, true);

            IR.TrainingImages = TextureReader.TrainingImagesFromFile(true, true);
            IR.TestImages = TextureReader.TestImagesFromFile(true, true);

            watch.Start();
            IR.CalculateTraits();
            IR.CalculateDistances(Metrice.SelectionBoxItem.ToString());
            IR.Classify();
            Result.Text = (IR.Result * 100).ToString();
            watch.Stop();
            System.IO.File.WriteAllText("Resultstime.txt", $"Execution Time: {(float)watch.ElapsedMilliseconds / 1000} s");
            Console.WriteLine($"Execution Time: {(float)watch.ElapsedMilliseconds / 1000} s");
            
        }

        private void Button_TextureImagesCorrelation(object sender, RoutedEventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            ImageRecognition IR = new ImageRecognition();

            //IR.TrainingImages = TextureReader.TrainingImages(true, true);
            //IR.TestImages = TextureReader.TestImages(true, true);

            IR.TrainingImages = TextureReader.TrainingImagesFromFile(false, true);
            IR.TestImages = TextureReader.TestImagesFromFile(false, true);

            watch.Start();
            IR.Correlation();
            watch.Stop();
            Console.WriteLine($"Execution Time: {(float)watch.ElapsedMilliseconds / 1000} s");
        }

        private void Button_CountGrapes(object sender, RoutedEventArgs e)
        {
            List<GrapesImage> GrapesImages = GrapesImageReader.ReadGrapesImages();
        }
    }
}
