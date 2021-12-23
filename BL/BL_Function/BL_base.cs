﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;
namespace BlApi
{
    partial class BL : IBL
    {
        /// <summary>
        /// calculation the most collset base station to a particular location
        /// </summary>
        /// <param name="location"> particular location</param>
        /// <returns> the most collset base station </returns>
        public BaseStation ClosestBase(Location location)
        {
            BaseStation baseStation = new BaseStation();
            baseStation.location = new Location();
            try
            {

                Location base_location = new Location();

                double? distans = null, distans2;
                foreach (DO.Base_Station base_ in dalObj.BaseStationList(x => true))
                {
                    base_location.Latitude = base_.latitude;
                    base_location.Longitude = base_.longitude;
                    distans2 = Distans(location, base_location);
                    if (distans > distans2 || distans == null)
                    {

                        distans = distans2;
                        baseStation = new BaseStation
                        {
                            location = base_location,
                            Name = base_.NameBase,
                            SerialNum = base_.baseNumber,
                            FreeState = base_.NumberOfChargingStations,
                            dronesInCharge = null
                        };

                    }
                }
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw (new ItemNotFoundException(ex));
            }
            return baseStation;
        }

        /// <summary>
        /// geting location for specific base station
        /// </summary>
        /// <param name="base_number"> serial number of base station</param>
        /// <returns> Location of the base station</returns>
        public Location BaseLocation(uint base_number)
        {
            DO.Base_Station base_Station = new DO.Base_Station();
            try
            {
                base_Station = dalObj.BaseStationByNumber(base_number);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw (new ItemNotFoundException(ex));
            }
            Location base_location = new Location();

            base_location.Latitude = base_Station.latitude;
            base_location.Longitude = base_Station.longitude;
            return base_location;
        }

        /// <summary>
        /// add base station
        /// </summary>
        /// <param name="baseStation"> serial number of the base station</param>
        public void AddBase(BaseStation baseStation)
        {
            try
            {
                dalObj.AddStation(new DO.Base_Station
                {
                    baseNumber = baseStation.SerialNum,
                    NameBase = baseStation.Name,
                    NumberOfChargingStations = baseStation.FreeState,
                    latitude = baseStation.location.Latitude,
                    longitude = baseStation.location.Longitude
                });
            }
            catch (DO.ItemFoundException ex)
            {
                throw (new ItemFoundExeption(ex));
            }

        }

        /// <summary>
        /// update base station
        /// </summary>
        /// <param name="base_">serial number of the base station</param>
        /// <param name="newName"> new name</param>
        /// <param name="newNumber"> charging states</param>
        public void UpdateBase(uint base_, string newName, string newNumber)
        {
            var baseUpdat = new DO.Base_Station();
            try
            {
                baseUpdat = dalObj.BaseStationByNumber(base_);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
            if (newName != "")
                baseUpdat.NameBase = newName;
            if (newNumber != "")
            {
                uint number;
                bool flag;
                flag = uint.TryParse(newNumber, out number);
                if (!flag)
                    throw new InputErrorException();
                int droneInCharge = dalObj.ChargingDroneList(x => x.idBaseStation == base_).Count();
                baseUpdat.NumberOfChargingStations = (droneInCharge <= number) ?
                    number : throw new UpdateChargingPositionsException(droneInCharge, base_);

            }
            dalObj.UpdateBase(baseUpdat);
        }

        /// <summary>
        /// search a specific station
        /// </summary>
        /// <param name="baseNume"> serial number</param>
        /// <returns> base station </returns>
        public BaseStation BaseByNumber(uint baseNume)
        {
            try
            {
                var baseStation = dalObj.BaseStationByNumber(baseNume);
                var baseReturn = new BaseStation { SerialNum = baseNume, location = new Location { Latitude = baseStation.latitude, Longitude = baseStation.longitude }, Name = baseStation.NameBase, FreeState = baseStation.NumberOfChargingStations };
                 
                baseReturn.dronesInCharge = new List<DroneInCharge>();

                baseReturn.dronesInCharge = (List<DroneInCharge>)(from drone in dalObj.ChargingDroneList(x => x.idBaseStation == baseNume)
                                                                  let butrry = (SpecificDrone(drone.IdDrone).ButrryStatus + droneChrgingAlredy(DateTime.Now - drone.EntringDrone))
                                                                  let newButrry = (butrry > 100) ? 100 : butrry
                                                                  select new DroneInCharge { butrryStatus = newButrry, SerialNum = drone.IdDrone });
                return baseReturn;
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
        }

        /// <summary>
        /// delete base station
        /// </summary>
        /// <param name="base_">serial number</param>
        public void DeleteBase(uint base_)
        {
            try
            {
                var baseStation = dalObj.BaseStationByNumber(base_);
                //relese the drone from chrge
                FreeBaseFromDrone(base_);

                dalObj.DeleteBase(base_);
                //find the clooset base
                var clooseBase = ClosestBase(new Location { Latitude = baseStation.latitude, Longitude = baseStation.longitude }).location;
                //send the drone to the closset base
                for (int i = 0; i < dronesListInBl.Count; i++)
                {
                    var drone = dronesListInBl[i];
                    if (drone.Location.Latitude == baseStation.latitude && drone.Location.Longitude == baseStation.longitude)
                        drone.Location = clooseBase;
                }


            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
        }

    }
}
