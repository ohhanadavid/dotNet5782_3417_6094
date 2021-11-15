﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    partial class BL : IBL
    {
        /// <summary>
        /// add drone to list
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="base_"></param>
        public void AddDrone(Drone drone, uint base_)
        {
            try
            {

                drone.location = BaseLocation(base_);

            }
            catch (IDAL.DO.ItemNotFoundException ex)
            {
                throw (new ItemNotFoundException(ex));
            }
            drone.droneStatus = Drone_status.Maintenance;

            Random random = new Random();
            try
            {
                dalObj.AddDrone(drone.SerialNum, drone.Model, (uint)drone.weightCategory);
            }
            catch (IDAL.DO.ItemFoundException ex)
            {
                throw (new ItemFoundExeption(ex));
            }
            drone.butrryStatus = random.Next(20, 41);
            DroneToCharge(drone.SerialNum, base_);
            drones.Add(drone);

        }
        /// <summary>
        /// update new location for drone
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="location"></param>
        public void UpdateDronelocation(  uint drone, Location location)
        {
            
            int i = drones.FindIndex(x => x.SerialNum == drone);
            if (i == -1)
                throw (new ItemNotFoundException("Drone", drone));
            drones[i].location = location;
            
        }
        /// <summary>
        /// update new model for drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="newName"></param>
        public void UpdateDroneName( uint droneId, string newName)
        {
            IDAL.DO.Drone droneInData;
            try
            {

                droneInData = dalObj.DroneByNumber(droneId);
            }
            catch (IDAL.DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
            int i = drones.FindIndex(x => x.SerialNum == droneId);
            if (i == -1)
                throw new ItemNotFoundException("Drone", droneId);
            drones[i].Model = newName;
            droneInData.Model = newName;
            dalObj.UpdateDrone(droneInData);
        }

        public Drone SpsiciSpecificDrone(uint siralNuber)
        {
            var drone = drones.Find(x => x.SerialNum == siralNuber);
            if (drone == null)
                throw new ItemNotFoundException("drone", siralNuber);
            return drone;

        }

    }
}