﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            string print = "";
            print += $"Location: Latitude:{Latitude} Longitude:{Longitude}\n";
            return print;
        }
        
           

    }
}