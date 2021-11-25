﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL.DO;

namespace DalObject
{
     partial class DalObject : IDAL.IDal
    {


        /// <summary>
        /// Adding a new drone
        /// </summary>
        /// <param name="drone">drone to add</param>
        public void AddDrone( Drone drone)
        {
              if (DataSource.drones.Any(x=>x.SerialNumber==drone.SerialNumber&&x.Active))
             throw (new ItemFoundException("drone", drone.SerialNumber));
            int i = DataSource.drones.FindIndex(x => x.SerialNumber == drone.SerialNumber);
            if (i != -1)
                DataSource.drones.Add(new Drone()
                {
                    SerialNumber = drone.SerialNumber,
                    Model = drone.Model,
                    WeightCategory = (WeightCategories)drone.WeightCategory
                });
            else
                DataSource.drones[i] = drone;

        }

        /// <summary>
        /// Display drone data  
        /// </summary>
        /// <param name="droneNum"> drone number</param>
        /// <returns> String of data</returns>
        public Drone DroneByNumber(uint droneNum)
        {
            if (!DataSource.drones.Any(x=>x.SerialNumber==droneNum&&x.Active))
                throw (new ItemNotFoundException("drone", droneNum));

            foreach (Drone item in DataSource.drones)
            {
                if (item.SerialNumber == droneNum)
                {
                    return item;

                }
            }
            return DataSource.drones[0];
        }

        /// <summary>
        /// return list of the drones
        /// </summary>
        /// <returns> return list of the drones</returns>
        public IEnumerable<Drone> DroneList()
        {
            return DataSource.drones.ToList<Drone>();
        }

        /// <summary>
        /// send drone to charge
        /// </summary>
        /// <param name="drone"> drone number</param>
        /// <param name="base_"> Base station number that the drone will be sent to it </param>
        public void DroneToCharge(uint drone,uint base_)
        {
            if(!DataSource.drones.Any(x=>x.SerialNumber==drone&&x.Active))
            {
                throw (new ItemNotFoundException("drone", drone));
            }
            if (DataSource.base_Stations.All(x => x.baseNumber !=base_))
            {
                throw (new ItemNotFoundException("base station", base_));
            }
            if (DataSource.droneInCharge.Any(x => x.IdDrone == drone))
                throw new ItemFoundException("drone", drone);
           

            DataSource.droneInCharge.Add(new BatteryLoad { IdDrone = drone, idBaseStation = base_, EntringDrone = DateTime.Now });
            for (int i = 0; i < DataSource.base_Stations.Count; i++)
            {
                if(DataSource.base_Stations[i].baseNumber==base_)
                {
                    var baseNew = DataSource.base_Stations[i];
                    baseNew.NumberOfChargingStations--;
                    DataSource.base_Stations[i] = baseNew;
                }
            }
            
        }

        /// <summary>
        /// delete a spsific drone
        /// </summary>
        /// <param name="sirial"> drone number</param>
        public void DeleteDrone(uint sirial)
        {
            if (!DataSource.drones.Any(x=>x.SerialNumber==sirial))
                throw (new ItemNotFoundException("drone", sirial));
            for (int i = 0; i < DataSource.drones.Count(); i++)
            {
                if (DataSource.drones[i].SerialNumber == sirial)
                {
                   var drone= DataSource.drones[i];
                    drone.Active = false;
                    DataSource.drones[i] = drone;
                    return;
                }
            }
        }

        /// <summary>
        /// Returns how much electricity the drone needs:
        /// 0. Available, 1. Light weight 2. Medium weight 3. Heavy 4. Charging per minute
        /// </summary>
        public IEnumerable<double> Elctrtricity()
        {
            double[] elctricity = new double[5];
            elctricity[(int)ButturyLoad.Free] = DataSource.Config.free;
            elctricity[(int)ButturyLoad.Easy] = DataSource.Config.easyWeight;
            elctricity[(int)ButturyLoad.Medium] = DataSource.Config.mediomWeight;
            elctricity[(int)ButturyLoad.Heavy] = DataSource.Config.heavyWeight;
            elctricity[(int)ButturyLoad.Charging] = DataSource.Config.Charging_speed;
            return elctricity;
        }

        /// <summary>
        /// update fileds at a given drone
        /// </summary>
        /// <param name="drone"> a given drone</param>
        public void UpdateDrone(Drone drone)
        {
            int index = DataSource.drones.FindIndex(x => x.SerialNumber == drone.SerialNumber&&x.Active);
            if (index != -1)
                DataSource.drones[index] = drone;
            else
                throw (new IDAL.DO.ItemNotFoundException("drone", drone.SerialNumber));
        }
        
        /// <summary>
        /// return how much the drone is in charge
        /// </summary>
        /// <param name="drone"> a given drone</param>
        public TimeSpan TimeInCharge(uint drone)
        {
            int i = DataSource.droneInCharge.FindIndex(x => x.IdDrone == drone);
            if (i==-1)
            {
                throw (new ItemNotFoundException("drone", drone));
            }
            return (DateTime.Now - DataSource.droneInCharge[i].EntringDrone);

        }

        /// <summary>
        /// Release a drone from charging
        /// </summary>
        /// <param name="drone"> drone number</param>
        public void FreeDroneFromCharge(uint drone)
        {
            int i = DataSource.droneInCharge.FindIndex(x => x.IdDrone == drone);
                if (i == -1)
                throw (new ItemNotFoundException("drone", drone));
            DataSource.droneInCharge.RemoveAt(i);
           
        }


    }
}
