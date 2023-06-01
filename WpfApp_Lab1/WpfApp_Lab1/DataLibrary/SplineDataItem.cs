using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DataLibrary
{
    public class SplineDataItem
    {
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll_CPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        void Wrap_Splines_Lab2_2022(int n, double[] x, double[] mData, double bc1_1, double bc1_2,
                                    int nN, double[] xUni, double[] res, ref int return_value);
    }
}
