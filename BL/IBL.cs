﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        double DroneChrgingAlredy(DateTime dateTime, DateTime newdateTime = default);
        public void DroneToCharge(uint dronenumber);
        public double FreeDroneFromCharging(uint droneNumber, uint timeInCharge);
        public IEnumerable<DroneInCharge> FreeBaseFromDrone(uint baseNumber, int number);
        public BaseStation CllosetBase(Location location);
        public Location BaseLocation(uint base_number);
        public void AddBase(BaseStation baseStation);
        public void AddDrone(Drone drone, uint base_);
        public void AddClient(Client client);
        public uint AddPackege(Package package);
        public void UpdateBase(uint base_,string newName = "", int newNumber = -1); 
        public void UpdateDronelocation(uint drone, Location location);
        public void UpdateDroneName(uint droneId, string newName);
          public void UpdateClient(ref Client client);
        public Drone SpecificDrone(uint siralNuber);
        public Location ClientLocation(uint id);
        public void ConnctionPackegeToDrone(uint droneNumber);

        public double Distans(Location location1, Location location2);

        public IEnumerable<PackageToList> PackageWithNoDroneToLists();
        public IEnumerable<PackageToList> PackageToLists();
        public void PackegArrive(uint droneNumber);
        public void CollectPackegForDelivery(uint droneNumber);
        public Package ShowPackage(uint number);
        public void UpdatePackegInDal(Package package);
        public IEnumerable<DroneToList> DroneToLists();

        public IEnumerable<ClientToList> ClientToLists();
        public Client GetingClient(uint id);
        public IEnumerable<BaseStationToList> BaseStationWhitChargeStationsToLists();
        public IEnumerable<BaseStationToList> BaseStationToLists();
        public BaseStation BaseByNumber(uint baseNume);






    }
}
