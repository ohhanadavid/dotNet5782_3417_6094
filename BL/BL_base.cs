﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IBL.BO;

namespace IBL
{
    partial class BL : IBL
    {
        /// <summary>
        /// clculet the most collset base to the cilent
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public BaseStation CllosetBase(Location location)
        {
            BaseStation baseStation = new BaseStation();
            baseStation.location = new Location();
            try
            {

                Location base_location = new Location();

                double distans = -5, distans2;
                foreach (IDAL.DO.Base_Station base_ in dalObj.BaseStationList())
                {
                    base_location.Latitude = base_.latitude;
                    base_location.Longitude = base_.longitude;
                    distans2 = Distans(location, base_location);
                    if (distans > distans2 || distans == -5)
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
            catch (IDAL.DO.ItemNotFoundException ex)
            {
                throw (new ItemNotFoundException(ex));
            }
            return baseStation;
        }
        /// <summary>
        /// geting location for spsific base
        /// </summary>
        /// <param name="base_number"></param>
        /// <returns></returns>
        public Location BaseLocation(uint base_number)
        {
            IDAL.DO.Base_Station base_Station = new IDAL.DO.Base_Station();
            try
            {
                base_Station = dalObj.BaseStationByNumber(base_number);
            }
            catch (IDAL.DO.ItemNotFoundException ex)
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
        /// <param name="baseStation"></param>
        public void AddBase(BaseStation baseStation)
        {
            try
            {
                dalObj.AddStation(baseStation.SerialNum, baseStation.Name, baseStation.FreeState, baseStation.location.Latitude, baseStation.location.Longitude);
            }
            catch (IDAL.DO.ItemFoundException ex)
            {
                throw (new ItemFoundExeption(ex));
            }
            baseStation.dronesInCharge.Clear();
        }

        public void UpdateBase(string newName = "", int newNumber = -1)
        {
            var baseUpdat = new IDAL.DO.Base_Station();
            try
            {
                baseUpdat = dalObj.BaseStationByNumber((uint)newNumber);
            }
            catch (IDAL.DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
            if (newName != "")
                baseUpdat.NameBase = newName;
            if (newNumber != -1)
            {
                int droneInCharge = dalObj.ChargingDroneList().Count(x => x.idBaseStation == (uint)newNumber);
                baseUpdat.NumberOfChargingStations = (droneInCharge <= newNumber) ?
                    (uint)newNumber : throw new UpdateChargingPositionsException(droneInCharge);

            }
        }


    }
}