﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public enum Weight_categories { Easy,Medium, Heavy }
        public enum AddItems { New_base_station, New_drone, New_client, New_package }
        public enum Update { Package_association, Package_collection, Package_delivery, Sending_a_drone_for_charging, Releasing_a_drone_from_a_charger }
        public enum Display { Base_stations, Drone,Client,Package}
        public enum View_the_lists { Base_stations, Drones, Clients, Packages, Packages_not_yet_associated, Base_stations_with_available_charging_stations }
        public enum Menu { Exit,AddItems, Update, Display, View_the_lists }
        public enum Drone_status { Free, Maintenance, charge}
        public enum Priority { Immediate , quick , Regular }
        public enum Distans_2_point { base_station=1,clien}
    }

}
