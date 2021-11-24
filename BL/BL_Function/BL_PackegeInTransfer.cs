﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL : IBL
    {
        PackageInTransfer convertPackegeDalToPackegeInTrnansfer(IDAL.DO.Package package)
        {
            var returnPackege = new PackageInTransfer
            {
                SerialNum = package.SerialNumber,
                WeightCatgory = (WeightCategories)package.WeightCatgory,
                Priority = (Priority)package.Priority,
                Source = ClientLocation(package.SendClient),
                Destination = ClientLocation(package.GetingClient),
                SendClient = clientInPackageFromDal(package.SendClient),
                RecivedClient = clientInPackageFromDal(package.GetingClient)
            };
            returnPackege.Distance = Distans(returnPackege.Source, returnPackege.Destination);
            returnPackege.InTheWay = (package.PackageArrived != new DateTime()) ? true : false;
            return returnPackege;
        }

        PackageInTransfer convertPackegeBlToPackegeInTrnansfer(Package package)
        {
            var returnPackege = new PackageInTransfer { Priority = package.priority, SendClient = package.SendClient, RecivedClient = package.RecivedClient, SerialNum = package.SerialNumber, WeightCatgory = package.weightCatgory, Source = ClientLocation(package.SendClient.Id), Destination = ClientLocation(package.RecivedClient.Id) };
            returnPackege.Distance = Distans(returnPackege.Source, returnPackege.Destination);
            returnPackege.InTheWay = (package.package_arrived != new DateTime()) ? true : false;
            return returnPackege;
        }
        //drone start delivery
        public void CollectPackegForDelivery(uint droneNumber)
        {
            var drone = dronesListInBl.Find(x => x.SerialNumber == droneNumber);
            if (drone == null)
                throw new ItemNotFoundException("Drone", droneNumber);
            var pacege =convertPackegeDalToPackegeInTrnansfer( dalObj.packegeByNumber( drone.numPackage));
            if (pacege.InTheWay != true)
            { new FunctionErrorException("ShowPackage||AddPackege"); }

            Location location = ClientLocation(pacege.SendClient.Id);
            drone.butrryStatus -= buttryDownWithNoPackege(drone.location, location);

            if (drone.butrryStatus < 0)
            { new FunctionErrorException("BatteryCalculationForFullShipping"); }

            drone.location = location;

            dalObj.PackageCollected(pacege.SerialNum);
            dronesListInBl[dronesListInBl.FindIndex(x => x.SerialNumber == droneNumber)] = drone;


        }

        public void PackegArrive(uint droneNumber)
        {
            var drone = dronesListInBl.Find(x => x.SerialNumber == droneNumber);
            if (drone == null)
                throw new ItemNotFoundException("Drone", droneNumber);
            var packege = convertPackegeDalToPackegeInTrnansfer(dalObj.packegeByNumber( drone.numPackage));
            if (packege.InTheWay == false)
                throw new FunctionErrorException("AddPackege||ShowPackage||updatePackegInDal||PackegArrive");
            drone.butrryStatus -= buttryDownPackegeDelivery(packege);
            if (drone.butrryStatus < 0)
                throw new FunctionErrorException("batteryCalculationForFullShipping");
            drone.location = ClientLocation(packege.RecivedClient.Id);
            drone.droneStatus = DroneStatus.Free;
            drone.numPackage = 0;
            packege.InTheWay = false;
            dalObj.PackageArrived(packege.SerialNum);
            dronesListInBl[dronesListInBl.FindIndex(x => x.SerialNumber == droneNumber)] = drone;

        }

        public void ConnectPackegeToDrone(uint droneNumber)
        {
            var drone = dronesListInBl.Find(x => x.SerialNumber == droneNumber);
            if (drone is null)
            { throw new ItemNotFoundException("Drone", droneNumber); }
            if (drone.droneStatus != DroneStatus.Free)
            { throw new DroneCantMakeDliveryException(); }

            IEnumerable<IDAL.DO.Package> packege, temp = new List<IDAL.DO.Package>();
            //locking for drone in first priorty 
            packege = dalObj.PackegeListWithNoDrone().Where(x => (WeightCategories)x.WeightCatgory <= drone.weightCategory);
            for (Priority i = Priority.Immediate; i <= Priority.Regular; i++)
            {

                for (WeightCategories j = drone.weightCategory; j >= WeightCategories.Easy; j++)
                {
                    temp = (packege.Where(x => (Priority)x.Priority == i && (WeightCategories)x.WeightCatgory == j));
                    if (temp != null)
                        break;
                }
                if (temp != null)
                {
                    var finalpackeg = cloosetPackege(drone.location, temp);
                    var buttry = batteryCalculationForFullShipping(drone.location, finalpackeg);
                    if (drone.butrryStatus - buttry > 0)
                    {
                        dalObj.ConnectPackageToDrone(finalpackeg.SerialNumber, drone.SerialNumber);
                        drone.numPackage =finalpackeg.SerialNumber;
                        dronesListInBl[dronesListInBl.FindIndex(x => x.SerialNumber == drone.SerialNumber)] = drone;
                        return;
                    }

                }


            }
            throw new DroneCantMakeDliveryException();


        }

    }
}