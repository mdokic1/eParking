using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EParkingOOAD.ViewModel
{
    
    public class Tacka
    {
        public Tacka(double x, double y)
        {
            this.xValue = x;
            this.yValue = y;
        }

        public double xValue { get; set; }
        public double yValue { get; set; }
    }
}
