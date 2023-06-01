using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DataLibrary
{
    public class SplineData
    {
        public string test_str { get; set; }
        public void Test ()
        {
            try
            {
                int n = 3;
                double[] x = new double[] { 0, 1, 2 };
                double[] y = new double[] { 0, 1, 2 };
                double derLeft = 1;
                double derRight = 1;
                int nN = 5;
                double[] xN = new double[] { 0, 0.5, 1, 1.5, 2 };
                double[] res = new double[3 * nN];
                int return_value = -123;
                MKLSplines(n, x, y, derLeft, derRight, nN, xN, res, ref return_value);
               
                test_str = $"return_value = {return_value}";
                for (int j = 0; j < nN; j++)
                { test_str += $"\nxN[{j}] = { xN[j]}  res[{j}] = {res[3 * j]} dres[{j}] = {res[3 * j + 1]}";}
               
            }
            catch(Exception ex)
            {
                throw new Exception("From SplineData.Test", ex);
            }
        }

        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll_CPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        void MKLSplines(int n, double[] x, double[] y, double derLeft, double derRight,
                        int nN, double[] xN, double[] res, ref int return_value);
    }
}
