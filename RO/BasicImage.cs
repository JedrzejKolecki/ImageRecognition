using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RO
{
    [Serializable()]
    public class BasicImage
    {
        [NonSerialized()] protected List<double> Traits;
        [NonSerialized()] public object LabelFromClassification;
        public object Label;
        [NonSerialized()] protected List<Neighbour> Neighbours;

        public BasicImage() { }

        public BasicImage(byte[,] pixels, byte label) { }

        public BasicImage(double[,] image, string label) { }

        public virtual void CalculateTraits() { }

        public void CalculateDistance(BasicImage TrainImage, string metric)
        {
            double distance = 0;

            if (metric == "Euclidean")
            {// metryka euklidesa
                for (int i = 0; i < Traits.Count; i++)
                {
                    distance += Math.Pow(Traits[i] - TrainImage.Traits[i], 2);
                }
                distance = Math.Sqrt(distance);
            }

            if (metric == "Manhattan")
            {// metryka uliczna
                for (int i = 0; i < Traits.Count; i++)
                {
                    distance += Math.Abs(Traits[i] - TrainImage.Traits[i]);
                }
            }

            if (metric == "Chebyshev")
            {// metryka Czebyszewa
                double maxDistance = 0;
                for (int i = 0; i < Traits.Count; i++)
                {
                    distance = Math.Abs(Traits[i] - TrainImage.Traits[i]);
                    if (maxDistance < distance)
                        maxDistance = distance;
                }
                distance = maxDistance;
            }

            if (Neighbours.Count < ImageRecognition.k)
            {
                Neighbours.Add(new Neighbour(distance, TrainImage));
            }
            else
            {
                Neighbours = Neighbours.OrderBy(x => x.Distance).ToList();
                if (distance < Neighbours[ImageRecognition.k - 1].Distance)
                {
                    Neighbours[ImageRecognition.k - 1] = new Neighbour(distance, TrainImage);
                }
            }
        }

        public void Classify()
        {
            Dictionary<object, int> Occurrences = new Dictionary<object, int>();

            foreach (Neighbour neighbour in Neighbours)
            {
                if (Occurrences.ContainsKey(neighbour.Image.Label))
                    Occurrences[neighbour.Image.Label]++;
                else
                    Occurrences.Add(neighbour.Image.Label, 0);
            }

            Occurrences = Occurrences.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            LabelFromClassification = Occurrences.Keys.First();
        }

        #region pomoce
        public struct Neighbour
        {
            public double Distance { get; set; }
            public BasicImage Image { get; set; }
            public Neighbour(double distance, BasicImage image)
            {
                Distance = distance; Image = image;
            }
        }
        #endregion
    }
}

