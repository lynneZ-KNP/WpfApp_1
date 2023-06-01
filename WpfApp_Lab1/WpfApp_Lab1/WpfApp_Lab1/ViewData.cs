using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using DataLibrary;
using System.Windows;

namespace WpfApp_Lab1
{
    internal class ViewData : IDataErrorInfo
    {
        public RawData RawData { get; set; }
        public SplineData SplineData { get; set; }
        public double LeftEnd { get; set; }
        public double RightEnd { get; set; }
        public bool IsUniform { get; set; }
        public int nRawItems { get; set; }
        public FRawEnum fRawEnum { get; set; }

        public ViewData()
        {
            LeftEnd = 1;
            RightEnd = 2;
            IsUniform = false;
            nRawItems = 5;
            fRawEnum = FRawEnum.Linear;
        }
        public string this[string columnName]
        {
            get
            {   string err = null;
                if (columnName == "nRawItems" && nRawItems < 2) err = "nRawItems < 2";
                if ((columnName == "LeftEnd" || columnName == "RightEnd") && LeftEnd >= RightEnd) err = "LeftEnd >= RightEnd";
                return err;
            }
        }
        public string Error => "error";

        public void Execute()
        {
            try 
            {
                RawData = new RawData(LeftEnd,RightEnd,nRawItems,IsUniform,fRawEnum);
               
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message);
            }
        }
    }
}
