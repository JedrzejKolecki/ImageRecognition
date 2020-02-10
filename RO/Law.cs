using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RO
{
    public static class Law
    {
        private static double[] L5 = new double[] { 1, 4, 6, 4, 1};     // level
        private static double[] E5 = new double[] { -1, -2, 0, 2, 1 };  // edge
        private static double[] S5 = new double[] { -1, 0, 2, 0, -1 };  // spot
        private static double[] R5 = new double[] { 1, -4, 6, -4, 1 };  // ripple

        public static double[,] L5E5;
        public static double[,] E5L5;
        public static double[,] L5S5;
        public static double[,] S5L5;
        public static double[,] L5R5;
        public static double[,] R5L5;
        public static double[,] E5S5;
        public static double[,] S5E5;
        public static double[,] E5R5;
        public static double[,] R5E5;
        public static double[,] R5S5;
        public static double[,] S5R5;
        public static double[,] S5S5;
        public static double[,] E5E5;
        public static double[,] R5R5;

        public static void CalculateMasks()
        {
            L5E5 = OuterProduct(L5, E5);
            E5L5 = OuterProduct(E5, L5);
            L5S5 = OuterProduct(L5, S5);
            S5L5 = OuterProduct(S5, L5);
            L5R5 = OuterProduct(L5, R5);
            R5L5 = OuterProduct(R5, L5);
            E5S5 = OuterProduct(E5, S5);
            S5E5 = OuterProduct(S5, E5);
            E5R5 = OuterProduct(E5, R5);
            R5E5 = OuterProduct(R5, E5);
            R5S5 = OuterProduct(R5, S5);
            S5R5 = OuterProduct(S5, R5);
            S5S5 = OuterProduct(S5, S5);
            E5E5 = OuterProduct(E5, E5);
            R5R5 = OuterProduct(R5, R5);
        }

        private static double[,] OuterProduct(double[] u, double[] v)
        {
            double[,] A = new double[5, 5];

            for(int r = 0; r < 5; r++)
                for(int c = 0; c < 5; c++)
                {
                    A[r, c] = u[r] * v[c];
                }

            return A;
        }
    }
}
