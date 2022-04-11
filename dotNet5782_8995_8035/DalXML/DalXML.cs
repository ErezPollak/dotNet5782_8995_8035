using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DalFacade.Models;
using DO;

namespace Dal
{
    internal sealed class DalXML : IDal
    {
        /// <summary>
        /// properties that include the paths for teh XML files.
        /// </summary>
        private const string ConfigPath = @"..\xml\Config.xml";
        private const string BaseStationsPath = @"BaseStations.xml";
        private const string CustomersPath = @"Customers.xml";
        private const string DronesPath = @"Drones.xml";
        private const string ParcelsPath = @"Parcels.xml";
        private const string DroneChargesPath = @"..\xml\DroneCharges.xml";


        #region Singalton

        /// <summary>
        /// private constructor for the dal class, for the singleton.
        /// </summary>
        private DalXML()
        {
            //DataSourceXml.Initialize();
        }

        /// <summary>
        /// dal field intended to keep the instance of the bl that was created.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<DalXML> instance = new Lazy<DalXML>(() => new DalXML());

        /// <summary>
        /// property as an instance of for the get function.
        /// </summary>
        public static DalXML Instance => instance.Value;

        #endregion
        
        # region add options

        /// <summary>
        /// the function creates new base station according to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="baseStation">to add baseStation</param>
        public bool AddBaseStation(BaseStation baseStation)
        {
            var list = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            //checking that the number is not already in the list, in witch case exception will be thrown.
            if (list.Any(b => b.Id == baseStation.Id))
                throw new IdAlreadyExistsException(baseStation.Id, "baseStation");
            list.Add(baseStation);
            XmlTools.SaveListToXmlSerializer(list, BaseStationsPath);
            return true;
        }

        /// <summary>
        /// the function gets a new customer and adding it to the list, if the id already exists an exception will be thrown.
        /// </summary>
        /// <param name="customer"></param>
        public bool AddCustomer(Customer customer)
        {
            var list = XmlTools.LoadListFromXmlSerializer<Customer>(CustomersPath);
            //checking that the number is not already in the list, in witch case exception will be thrown.
            if (list.Any(c => c.Id == customer.Id))
                throw new IdAlreadyExistsException(customer.Id, "customer");
            list.Add(customer);
            XmlTools.SaveListToXmlSerializer(list, CustomersPath);
            return true;
        }

        /// <summary>
        /// the function creates new drone according to given specs, and adding it to the array. 
        /// while updating the config class.
        /// </summary>
        /// <param name="drone"></param>
        public bool AddDrone(Drone drone)
        {
            var list = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            //checking that the number is not already in the list, in witch case exception will be thrown.
            if (list.Exists(d => d.Id == drone.Id)) 
                throw new IdAlreadyExistsException(drone.Id, "drone");
            list.Add(drone);
            XmlTools.SaveListToXmlSerializer(list, DronesPath);
            return true;
        }

        /// <summary>
        ///  /// the function gets a new parcel and adding it to the list, if the id already exists an exception will be thrown.
        /// </summary>
        /// <param name="parcel"></param>
        public bool AddParcel(Parcel parcel)
        {
            var list = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            //checking that the number is not already in the list, in witch case exception will be thrown.
            if (list.Any(p => p.Id == parcel.Id))
                throw new IdAlreadyExistsException(parcel.Id, "parcel");
            list.Add(parcel);
            XmlTools.SaveListToXmlSerializer(list, ParcelsPath);
            return true;
        }

        #endregion

        #region update options

        /// <summary>
        /// updates the properties of the station
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="name"></param>
        /// <param name="slots"></param>
        /// <returns></returns>
        public bool UpdateBaseStation(int baseStationId, string name, int slots)
        {
            var baseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);

            var index = baseStations.FindIndex(b => b.Id == baseStationId);

            if (index == -1)
                throw new IdDontExistsException(baseStationId, "baseStation");

            BaseStation baseStation = baseStations[index];
            baseStation.Name = name;
            baseStation.ChargeSlots = slots;
            baseStations[index] = baseStation;

            XmlTools.SaveListToXmlSerializer(baseStations, BaseStationsPath);

            return true;
        }

        /// <summary>
        /// update the properties of the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool UpdateCustomer(int customerId, string name, string phone)
        {
            var customers = XmlTools.LoadListFromXmlSerializer<Customer>(CustomersPath);

            var index = customers.FindIndex(d => d.Id == customerId);

            if (index == -1)
                throw new IdDontExistsException(customerId , "customer");

            var customer = customers[index];
            customer.Name = name;
            customer.Phone = phone;
            customers[index] = customer;

            XmlTools.SaveListToXmlSerializer(customers, CustomersPath);

            return true;
        }

        /// <summary>
        /// update the name of the drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateModelForADrone(int droneId, string model)
        {
            var drones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);

            var index = drones.FindIndex(d => d.Id == droneId);

            if (index == -1)
                throw new IdDontExistsException(droneId, "drone");

            var drone = drones[index];
            drone.Model = model;
            drones[index] = drone;

            XmlTools.SaveListToXmlSerializer(drones, DronesPath);

            return true;
        }

        /// <summary>
        /// creating a droneCharge and charging the drone.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <param name="droneId"></param>
        public bool ChargeDrone(int baseStationId, int droneId)
        {

            var baseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            var drones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);

            //keeps the index in witch the idNumber was found in order to update it without iterating over the list again.
            var baseStationIndex = baseStations.FindIndex(b => b.Id == baseStationId);
            if (baseStationIndex == -1)
                throw new IdDontExistsException(baseStationId, "base station");


            if (drones.All(d => d.Id == droneId))
                throw new IdDontExistsException(droneId, "drone");


            //update the number of the free charge slots at the base station to be one less.
            var newBaseStation = baseStations[baseStationIndex];
            --newBaseStation.ChargeSlots;
            baseStations[baseStationIndex] = newBaseStation;

            XmlTools.SaveListToXmlSerializer(baseStations, BaseStationsPath);

            //creating the charge drone ans adding it to the list of charges.
            var droneCharge = new DroneCharge() { DroneId = droneId, StationId = baseStationId, EntryIntoCharge = DateTime.Now.AddHours(-1) };
            var droneChargesRoot = XElement.Load(DroneChargesPath);
            droneChargesRoot.Add(new XElement("DroneCharge",
                new XElement("DroneId", droneCharge.DroneId),
                new XElement("StationId", droneCharge.StationId),
                new XElement("EntryIntoCharge", droneCharge.EntryIntoCharge)));
            droneChargesRoot.Save(DroneChargesPath);

            return true;
        }

        /// <summary>
        ///deleting the droneCharge from the list. 
        /// </summary>
        /// <param name="droneId"></param>
        public double UnChargeDrone(int droneId)
        {
            //deserializing the required Lists from the XML.
            var baseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            var drones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);

            //pooling the root of chargeDrones
            var droneChargesRoot = XElement.Load(DroneChargesPath);



            if (!drones.Exists(d => d.Id == droneId))
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //save the number of the base Station for later.

            var droneChargeToRemove = (from droneC in droneChargesRoot.Elements()
                let droneIdStr = droneC.Element("DroneId")?.Value
                where droneIdStr != null && int.Parse(droneIdStr) == droneId
                                            select droneC).FirstOrDefault();

            if(droneChargeToRemove == null)
            {
                throw new IdDontExistsException(droneId, "chargeDrone");
            }

            var stationIdStr = droneChargeToRemove.Element("StationId")?.Value;
            if (stationIdStr == null) return 0;
            var baseStationId = int.Parse(stationIdStr);
            var baseStationsIndex = baseStations.FindIndex(b => b.Id == baseStationId);


            //updates the number of free charging slots int he base station.
            //finds the index of the station and update when finds, no need for exception search, because the station exists for sure.

            //calculate the time that the drone was in charge.

            var entryToChargeStr = droneChargeToRemove.Element("EntryIntoCharge")?.Value;
            if (entryToChargeStr == null) return 0;
            var ts =
                DateTime.Now.Subtract(DateTime.Parse(entryToChargeStr));

            var minutesInCharge = ts.Hours * 60 + ts.Minutes + (double) ts.Seconds / 60;

            var newBaseStation = baseStations[baseStationsIndex];
            newBaseStation.ChargeSlots++;
            baseStations[baseStationsIndex] = newBaseStation;

            //deleting the droneCharge from the  database of lists.
            droneChargeToRemove.Remove();
            droneChargesRoot.Save(DroneChargesPath);

            XmlTools.SaveListToXmlSerializer(baseStations, BaseStationsPath);

            return minutesInCharge;

        }

        /// <summary>
        ///the function is giving the parcel the number of the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public bool AssignDroneToParcel(int parcelId, int droneId)
        {
            var parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            var drones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);


            //keeps the index in witch the idNumber was found in order to update it without iterating over the list again.
            //checking if the numbers of parcel and drone that was provided exist in the database or not. if not an exception will be thrown.

            var parcelIndex = parcels.FindIndex(p => parcelId == p.Id);
            if (parcelIndex == -1) //not found parcel in the list
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            if (!drones.Exists(d => droneId == d.Id)) //check if the drone is exists in data source.
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the number of the drone in the DroneId field of the parcel to have the updated droneId number.
            //according to the number that was found while looking for an exception.  
            var updatedParcel = parcels[parcelIndex];
            updatedParcel.AssignedTime = DateTime.Now;
            updatedParcel.DroneId = droneId;
            parcels[parcelIndex] = updatedParcel;

            XmlTools.SaveListToXmlSerializer(parcels, ParcelsPath);

            return true;
        }

        ///  <summary>
        /// updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        ///  </summary>
        ///  <param name="parcelId"></param>
        ///  <param name="droneId"></param>
        public bool PickingUpParcel(int parcelId, int droneId)
        {

            var parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            var drones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);

            //keeps the index in witch the idNumber was found in order to update it without iterating over the list again.

            //checking if the number of parcel that was provided exist in the database or not. if not an exception will be thrown.
            var parcelIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            if (!drones.Exists(d => d.Id == droneId)) //check if the drone in the list
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the time of pickedUp field to be now.
            //according to the number that was found while looking for an exception.  
            var newParcel = parcels[parcelIndex];
            newParcel.PickedUpTime = DateTime.Now;
            newParcel.DroneId = droneId;
            parcels[parcelIndex] = newParcel;

            XmlTools.SaveListToXmlSerializer(parcels, ParcelsPath);

            return true;
        }

        /// <summary>
        ///updating the time of delivering in the parcel, and changing the status of the drone to free.
        /// </summary>
        /// <param name="parcelId"></param>
        public bool DeliveringParcel(int parcelId)
        {
            var parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);

            //checking if the number of parcel that was provided exist in the database or not. if not an exception will be thrown.
            var parcelIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            //updating the time of delivered field to be now.
            //according to the number that was found while looking for an exception.  
            var newParcel = parcels[parcelIndex];
            newParcel.DroneId = 0;
            newParcel.DeliveryTime = DateTime.Now;
            parcels[parcelIndex] = newParcel;

            XmlTools.SaveListToXmlSerializer(parcels, ParcelsPath);

            return true;
        }


        #endregion

        #region get lists option

        /// <summary>
        /// returns the array of the base stations.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> f)
        {
            var baseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            return baseStations.Where(c => f(c));
        }

        public IEnumerable<DroneCharge> GetChargeDrones(Predicate<DroneCharge> f)
        {
            var droneChargesRoot = XElement.Load(DroneChargesPath);
            return (from ds in droneChargesRoot.Elements()
                let droneIdStr = ds.Element("DroneId")?.Value
                where droneIdStr != null
                let stationIdStr = ds.Element("StationId")?.Value
                where stationIdStr != null
                let entryToChargeStr = ds.Element("EntryIntoCharge")?.Value
                where entryToChargeStr != null
                select new DroneCharge()
                    {
                        DroneId = int.Parse(droneIdStr),
                        StationId = int.Parse(stationIdStr),
                        EntryIntoCharge = DateTime.Parse(entryToChargeStr)
                    }).Where(c => f(c));
        }

        /// <summary>
        /// returns the array of the base customers.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> f)
        {
            var customers = XmlTools.LoadListFromXmlSerializer<Customer>(CustomersPath);
            return customers.Where(c => f(c));
        }

        /// <summary>
        /// returns the array of the base drones.
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Drone> GetDrones(Predicate<Drone> f)
        {
            var drones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            return drones.Where(c => f(c));
        }

        /// <summary>
        /// returns the array of the purchases
        /// </summary>
        /// <param name="f"></param>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> f)
        {
            var parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            return parcels.Where(c => f(c));
        }

        #endregion

        #region show oobject options

        /// <summary>
        /// the function receives an ID number of a customer and returns the relevant customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetCustomer(int customerId)
        {
            var customers = XmlTools.LoadListFromXmlSerializer<Customer>(CustomersPath);
            var customerIndex = customers.FindIndex(c => c.Id == customerId);
            if(customerIndex == -1)
            {
                throw new IdDontExistsException(customerId , "customer");
            }
            return customers[customerIndex];
        }

        /// <summary>
        /// the function receives an ID number of a drone and returns the relevant drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            var drones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            var customerIndex = drones.FindIndex(c => c.Id == droneId);
            if (customerIndex == -1)
            {
                throw new IdDontExistsException(droneId, "drone");
            }
            return drones[customerIndex];
        }

        /// <summary>
        /// the function receives an ID number of a parcel and returns the relevant parcel.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            var parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            var customerIndex = parcels.FindIndex(c => c.Id == parcelId);
            if (customerIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "Parcel");
            }
            return parcels[customerIndex];
        }

        /// <summary>
        /// the function receives an ID number of  a base station and returns the relevant station.
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        public BaseStation GetBaseStation(int baseStationId)
        {
            var baseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            var customerIndex = baseStations.FindIndex(c => c.Id == baseStationId);
            if (customerIndex == -1)
            {
                throw new IdDontExistsException(baseStationId, "BaseStation");
            }
            return baseStations[customerIndex];
        }

        #endregion;

        #region operational functions

        /// <summary>
        /// the function returns the new serial number of the parcel.
        /// </summary>
        /// <returns></returns>
        public int GetSerialNumber()
        {
            var list = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);

            var serialNumber = list.Max(p => p.Id);

            return serialNumber + 20;
        }

        /// <summary>
        ///  the function gets an id of drone or a customer and returns the station that is closest to this drone or customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int GetClosestStation(int customerId)
        {
            var baseStations = GetBaseStations(_ => true);
            var customer = GetCustomer(customerId);

            //double distance
            var baseStationLst = baseStations.ToList();

            var clothesBaseStation = baseStationLst.OrderByDescending(b => Distance(customer.Location, b.Location)).FirstOrDefault();

            return clothesBaseStation.Id;
        }

        /// <summary>
        /// returns an array with the information of charging drones.
        /// </summary>
        /// <returns></returns>
        public double[] ElectricityUse()
        {
            var configElements = XElement.Load(ConfigPath);

            double[] configValues = {double.Parse(configElements.Element("Free").Value) ,
                double.Parse(configElements.Element("Light").Value),
                double.Parse(configElements.Element("Middle").Value),
                double.Parse(configElements.Element("Heavy").Value),
                double.Parse(configElements.Element("ChargingSpeed").Value)};

            return configValues;
        }

        private double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        private double Distance(Location l1, Location l2)
        {
            var r = 6371;

            var f1 = ConvertToRadians(l1.Latitude);
            var f2 = ConvertToRadians(l2.Latitude);

            var df = ConvertToRadians(l1.Latitude - l2.Latitude);
            var dl = ConvertToRadians(l1.Longitude - l2.Longitude);

            var a = Math.Sin(df / 2) * Math.Sin(df / 2) +
                    Math.Cos(f1) * Math.Cos(f2) *
                    Math.Sin(dl / 2) * Math.Sin(dl / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance.
            var d = r * c;

            return d;

        }

        #endregion
    }
}
