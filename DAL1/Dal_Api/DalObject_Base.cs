﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using Ds;

namespace Dal
{
    sealed partial class DalObject : DalApi.IDal
    {

        
        /// <summary>
        ///Adding a new base station
        /// </summary>
        /// <param name="base_num">The base station number </param>
        /// <param name="name"> The name ot the station </param>
        /// <param name="numOfCharging">The amount of charging stations at the station </param>
        /// <param name="latitude">Latitude of the station</param>
        /// <param name="longitude">Longitude of the station</param>
        public void AddStation(uint base_num, string name, uint numOfCharging, double latitude, double longitude)
        {
            
            if (DataSource.base_Stations.Any(x => x.baseNumber == base_num))
                throw (new ItemFoundException("Base station", base_num));

            DataSource.base_Stations.Add(new Base_Station
            {
                baseNumber = base_num,
                NameBase = name,
                NumberOfChargingStations = numOfCharging,
                latitude = latitude,
                longitude = longitude

            });

        }

        /// <summary>
        /// Adding a new base station
        /// </summary>
        /// <param name="base_Station"> Base Station to add</param>
        public void AddStation(Base_Station base_Station)
        {
            if (DataSource.base_Stations.Any(x => x.baseNumber == base_Station.baseNumber))
                throw (new ItemFoundException("base station", base_Station.baseNumber));
            DataSource.base_Stations.Add(base_Station);
        }


        /// <summary>
        /// Display base station data desired   
        /// </summary>
        /// <param name="baseNum">Desired base station number</param>
        /// <returns> String of data </returns>
        public Base_Station BaseStationByNumber(uint baseNum)
        {

            if (!DataSource.base_Stations.Any(x => x.baseNumber == baseNum))
                throw (new ItemNotFoundException("Base station", baseNum));
            return DataSource.base_Stations[DataSource.base_Stations.FindIndex(x => x.baseNumber == baseNum)];

        }


        /// <summary>
        /// return a list of all the base stations
        /// </summary>
        public IEnumerable<Base_Station> BaseStationList(Predicate<Base_Station> predicate)
        {
            
            return DataSource.base_Stations.FindAll(predicate);
        }


       

        /// <summary>
        /// delete a spsific base for list
        /// </summary>
        /// <param name="sirial"> Base Station number</param>
        public void DeleteBase(uint sirial)
        {
            if (!DataSource.base_Stations.Any(x => x.baseNumber == sirial))
                throw (new ItemNotFoundException("Base station", sirial));
            DataSource.base_Stations.Remove(DataSource.base_Stations[DataSource.base_Stations.FindIndex(x => x.baseNumber == sirial)]);


        }

        /// <summary>
        /// update fileds at a given base station
        /// </summary>
        /// <param name="base_"> a given base station </param>
        public void UpdateBase(Base_Station base_)
        {
            int i = DataSource.base_Stations.FindIndex(x => x.baseNumber == base_.baseNumber);
            if (i == -1)
                throw (new DO.ItemNotFoundException("Base Station", base_.baseNumber));
            else
                DataSource.base_Stations[i] = base_;
        }
    }

}