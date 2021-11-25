﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{

    public enum WeightCategories { Easy, Medium, Heavy }
    public enum Priority { Immediate, quick, Regular }
    public enum PackageStatus { Create, Assign, Collected, Arrived }
    public enum DroneStatus { Free, Maintenance, Work,Delete }
    public enum SpeedDrone { Free = 25, Easy = 20, Medium = 15, Heavy = 12 }
    public enum ButrryPer { Minute = 60 }
}
