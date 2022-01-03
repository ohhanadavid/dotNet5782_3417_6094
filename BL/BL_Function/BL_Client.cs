﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Collections;
using DalApi;
namespace BlApi
{
    partial class BL : IBL
    {
        /// <summary>
        /// return the client location
        /// </summary>
        /// <param name="id"> id client</param>
        /// <returns> client location</returns>
        public Location ClientLocation(uint id)
        {
            DO.Client client = new DO.Client();
            try
            {
                client = dalObj.CilentByNumber(id);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw (new ItemNotFoundException(ex));
            }
            Location location_client = new Location();
            location_client.Latitude = client.Latitude;
            location_client.Longitude = client.Longitude;
            return location_client;
        }

        /// <summary>
        /// add client
        /// </summary>
        /// <param name="client"> client to add</param>
        public void AddClient(Client client)
        {
            //checking id
            if (client.Id < 100000000)
            { throw new NumberNotEnoughException(9); }
            if (client.Id > 999999999)
            { throw new NumberMoreException(); }
            var distas = Distans(client.Location, ClosestBase(client.Location).Location);
            if (distas > 60)
                throw new ClientOutOfRangeException();
            //chcing phon number
            chekingFon(client.Phone);

            try
            {
                dalObj.AddClient(new DO.Client
                {
                    Id = client.Id,
                    Latitude = client.Location.Latitude,
                    Longitude = client.Location.Longitude,
                    Name = client.Name,
                    PhoneNumber = client.Phone
                });
            }
            catch (DO.ItemFoundException ex)
            {
                throw (new ItemFoundExeption(ex));
            }
        }

        /// <summary>
        /// help mathod to chack phone number
        /// </summary>
        /// <param name="fon"> phone number</param>
        void chekingFon(string fon)
        {
            if (fon.Count() < 10)
            { throw new NumberNotEnoughException(10); }
            if (fon.Count() > 10)
                
            { throw new NumberMoreException(); }
            if (fon[0] != '0' || fon[1] != '5')
            { throw new StartingException("0,5"); }
            if (fon.Any(c => c < '0' || c > '9'))
            { throw new IllegalDigitsException(); }
            
           

        }

        /// <summary>
        /// Update fields at a client
        /// </summary>
        /// <param name="client"> client </param>
        public void UpdateClient( Client client)
        {
            //checking id
            if (client.Id < 100000000)
            { throw new NumberNotEnoughException(9); }
            if (client.Id > 999999999)
            { throw new NumberMoreException(); }
            //checking phone number
            chekingFon(client.Phone);
            try
            {
                var clientFromDal = dalObj.CilentByNumber(client.Id);
                if (client.Name != "")
                    clientFromDal.Name = client.Name;
                if (client.Phone != "")
                    clientFromDal.PhoneNumber = client.Phone;
                if (client.Location.Latitude != 0)
                    clientFromDal.Latitude = client.Location.Latitude;
                if (client.Location.Longitude != 0)
                    clientFromDal.Latitude = client.Location.Longitude;

                dalObj.UpdateClient(clientFromDal);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }

        }

        /// <summary>
        /// Receiving a client by ID
        /// </summary>
        /// <param name="id"> client ID</param>
        /// <returns> client</returns>
        public Client GetingClient(uint id)
        {
            try
            {
                var client = dalObj.CilentByNumber(id);
                var loc = new Location();
                loc.Latitude = client.Latitude;
                loc.Longitude = client.Longitude;
                var returnClient = new Client { Id = client.Id, Name = client.Name, Phone = client.PhoneNumber, Location=loc};
                returnClient.ToClient = new List<PackageAtClient>();
                var packege = dalObj.PackegeList(x => x.GetingClient == id);
                

                if (packege.Count() != 0)
                    foreach (var packegeInList in packege)
                    {
                        returnClient.ToClient.Add(packegeInList.convretPackegeDalToPackegeAtClient(packegeInList.GetingClient,dalObj));
                    }
                returnClient.FromClient = new List<PackageAtClient>();
                packege = dalObj.PackegeList(x => x.SendClient == id);
                if (packege.Count() != 0)
                    foreach (var packegeInList in packege)
                    {
                        returnClient.FromClient.Add(packegeInList.convretPackegeDalToPackegeAtClient(packegeInList.SendClient,dalObj));
                    }
                return returnClient;
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }


        }

        /// <summary>
        /// delete client
        /// </summary>
        /// <param name="id"> client id</param>
        public void DeleteClient (uint id)
        {
            List<uint>packegesDelete = new List<uint>();
            try
            {
                dalObj.DeleteClient(id);
               
            }
            catch(DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }

        }

        
    
    }
}
