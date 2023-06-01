using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class RawDataItem
    {
        public double x { get; set; }
        public double y { get; set; }

        public RawDataItem (double x, double y)
        {
            this.x = x; 
            this.y = y;    
        }
        public override string ToString()
        {
            return $"x = {x.ToString("F4")} y = {y.ToString("F4")}";
        }
    }
}
