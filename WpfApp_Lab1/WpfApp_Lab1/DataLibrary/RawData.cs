using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public delegate double FRaw(double x);
    public enum FRawEnum {Linear, Cubic, Random };
    public class RawData
    {  public double LeftEnd {get;set;}
       public double RightEnd { get; set; }
       public bool IsUniform { get; set; }
       public int nRawItems { get; set; }
       public double[] x { get; set; }
       public double[] y { get; set; }
       public List<RawDataItem> RawDataList { get; set; }
       public FRawEnum fRawEnum { get; set; }
       public static Random randf = new Random();
       public RawData(double LeftEnd, double RightEnd, int nRawItems, bool IsUniform, FRawEnum fRawEnum)
       {
            this.LeftEnd = LeftEnd;
            this.RightEnd = RightEnd;
            this.nRawItems = nRawItems; 
            this.IsUniform = IsUniform;
            this.fRawEnum = fRawEnum;
            x = new double[nRawItems];
            y = new double[nRawItems];
            RawDataList = new List<RawDataItem>();

            FRaw function;
            if (fRawEnum == FRawEnum.Linear) function = RawData.Linear;
            else if (fRawEnum == FRawEnum.Cubic) function = RawData.Cubic;
            else  function = RawData.Random; // (fRawEnum == FRawEnum.Random)

            if (IsUniform)
            {
                double h = (RightEnd - LeftEnd) / (nRawItems - 1);
                for (int j = 0; j < nRawItems; j++)
                {
                    x[j] = LeftEnd + j * h;
                    y[j] = function(x[j]);
                }
            }
            else
            {
                Random rand = new Random();
                rand.NextDouble();
                for (int j = 0; j < nRawItems; j++)
                {
                    x[j] = rand.NextDouble();
                }
                Array.Sort(x);
                x[0] = LeftEnd;
                for (int j = 1; j < nRawItems - 1; j++)
                {
                    x[j] = LeftEnd + x[j] * (RightEnd - LeftEnd);
                }
                x[nRawItems - 1] = RightEnd;
            }

            for (int j = 0; j < nRawItems; j++)
            {
                y[j] = function(x[j]);
                RawDataItem rdi = new RawDataItem(x[j], y[j]);
                RawDataList.Add(rdi);
            }
        }
        public static double Linear(double x) => x;
        public static double Cubic(double x) => x * x * x;
        public static double Random(double x) => randf.NextDouble();
        private RawData rawdata;
        public RawData rawData
        {
            get { return rawdata; }
            set
            {
                rawdata = value;
                OnPropertyChanged("rawData");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        public static void Save(string filename)
        {

        }
        public static void Load(string filename, ref RawData rawData)
        {

        }
    }
}
