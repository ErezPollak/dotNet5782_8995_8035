
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    internal sealed class DalXML : DalApi.IDal
    {


        #region Singalton

        /// <summary>
        /// private constructor for the dal class, for the singalton.
        /// </summary>
        private DalXML()
        {
            //DataSourceXML.Initialize();
        }

        /// <summary>
        /// dal field intended to keep the insstance of the bl that was created.
        /// </summary>
        private static readonly Lazy<DalXML> instance = new Lazy<DalXML>(() => new DalXML());

        public static DalXML Instance {
            get
            {
                return instance.Value;
            }
        }

        #endregion


        private string ConfigPath = @"..\xml\Config.xml";
        private string BaseStationsPath = @"BaseStations.xml";
        private string CustomersPath = @"Customers.xml";
        private string DronesPath = @"Drones.xml";
        private string ParcelsPath = @"Parcels.xml";
        private string DroneChargesPath = @"..\xml\DroneCharges.xml";



        public bool AddBaseStation(BaseStation baseStation)
        {
            List<BaseStation> list = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationsPath);

            if (list.Any(b => b.Id == baseStation.Id))
                throw new IdAlreadyExistsException(baseStation.Id, "baseSattion");

            list.Add(baseStation);
            XMLTools.SaveListToXMLSerializer<BaseStation>(list, BaseStationsPath);
            return true;
        }

        public bool AddCustumer(Customer customer)
        {
            List<Customer> list = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            list.Add(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(list, CustomersPath);
            return true;
        }

        public bool AddDrone(Drone drone)
        {
            List<Drone> list = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            if (list.Exists(d => d.Id == drone.Id)) 
                throw new IdAlreadyExistsException(drone.Id, "drone");

            list.Add(drone);
            XMLTools.SaveListToXMLSerializer<Drone>(list, DronesPath);
            return true;
        }

        public bool AddParcel(Parcel parcel)
        {
            List<Parcel> list = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            list.Add(parcel);
            XMLTools.SaveListToXMLSerializer<Parcel>(list, ParcelsPath);
            return true;
        }

        public bool UpdateBaseStation(int baseStationID, string name, int slots)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationsPath);

            int index = baseStations.FindIndex(b => b.Id == baseStationID);

            if (index == -1) throw new IdDontExistsException(baseStationID, "baseStation");

            BaseStation baseStation = baseStations[index];
            baseStation.Name = name;
            baseStation.ChargeSlots = slots;
            baseStations[index] = baseStation;

            XMLTools.SaveListToXMLSerializer<BaseStation>(baseStations, BaseStationsPath);

            return true;
        }

        public bool UpdateCustomer(int customerID, string name, string phone)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            int index = customers.FindIndex(d => d.Id == customerID);

            if (index == -1) throw new IdDontExistsException();

            Customer customer = customers[index];
            customer.Name = name;
            customer.Phone = phone;
            customers[index] = customer;

            XMLTools.SaveListToXMLSerializer<Customer>(customers, CustomersPath);

            return true;
        }

        public bool UpdateNameForADrone(int droneId, string model)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            int index = drones.FindIndex(d => d.Id == droneId);

            if (index == -1) throw new IdDontExistsException(droneId, "drone");

            Drone drone = drones[index];
            drone.Model = model;
            drones[index] = drone;

            XMLTools.SaveListToXMLSerializer<Drone>(drones, DronesPath);

            return true;
        }

        public bool ChargeDrone(int baseStationId, int droneId)
        {

            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationsPath);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.
            int baseStationIndex = baseStations.FindIndex(b => b.Id == baseStationId);
            if (baseStationIndex == -1)
                throw new IdDontExistsException(baseStationId, "base station");


            if (drones.All(d => d.Id != droneId))
                throw new IdDontExistsException(droneId, "drone");


            //update the number of the free charge slots at the base station to be one less.
            BaseStation newBaseStation = baseStations[baseStationIndex];
            --newBaseStation.ChargeSlots;
            baseStations[baseStationIndex] = newBaseStation;

            //creating the charge drone ans adding it to the list of charges.
            DroneCharge droneCharge = new DroneCharge() { DroneId = droneId, StationId = baseStationId, EntryIntoCharge = DateTime.Now.AddHours(-1) };
            XElement droneChargesRoot = XElement.Load(DroneChargesPath);
            droneChargesRoot.Add(new XElement("DroneCharge",
                new XElement("DroneId", droneCharge.DroneId),
                new XElement("StationId", droneCharge.StationId),
                new XElement("EntryIntoCharge", droneCharge.EntryIntoCharge)));
            droneChargesRoot.Save(DroneChargesPath);

            XMLTools.SaveListToXMLSerializer<BaseStation>(baseStations, BaseStationsPath);

            return true;
        }

        public double UnChargeDrone(int droneId)
        {
            //deserilazing the reqaierde Lists fron the XML.
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationsPath);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            //poolint the root of chargeDrones
            XElement droneChargesRoot = XElement.Load(DroneChargesPath);

            if (!drones.Exists(d => d.Id == droneId))
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //save the number of the base Station for later.
            int baseStationId = 0;

            XElement DroneChargeToRemove = (from droneC in droneChargesRoot.Elements()
                                            where int.Parse(droneC.Element("DroneId").Value) == droneId
                                            select droneC).FirstOrDefault();

            if(DroneChargeToRemove == null)
            {
                throw new IdDontExistsException(droneId, "chargeDrone");
            }
            baseStationId = int.Parse(DroneChargeToRemove.Element("StationId").Value);
            int baseStationsIndex = baseStations.FindIndex(b => b.Id == baseStationId);


            //updates the number of free charging slots int he base station.
            //finds the index of the station and update when finds, no need for excption search, because the station exists for sure.

            //caculate the time that the drone was in charge.

            TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(DroneChargeToRemove.Element("EntryIntoCharge").Value));
            //TimeSpan ts = DateTime.Now.Subtract(XmlConvert.ToDateTime(DroneChargeToRemove.Element("").Value));

            double mintesInCharge = ts.Hours * 60 + ts.Minutes + (double)ts.Seconds / 60;

            BaseStation newBaseStation = baseStations[baseStationsIndex];
            newBaseStation.ChargeSlots++;
            baseStations[baseStationsIndex] = newBaseStation;

            //deleting the drineCharge from the  database of lists.
            DroneChargeToRemove.Remove();
            droneChargesRoot.Save(DroneChargesPath);

            XMLTools.SaveListToXMLSerializer<BaseStation>(baseStations, BaseStationsPath);

            return mintesInCharge;
        }

        public bool AssignDroneToParcel(int parcelId, int droneId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);


            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.
            //checking if the numbers of parcel and drone that was provided exist in the database or not. if not an excption will be thrown.

            int parcelIndex = parcels.FindIndex(p => parcelId == p.Id);
            if (parcelIndex == -1) //not found parcel in the list
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            if (!drones.Exists(d => droneId == d.Id)) //check if the drone is exists in data sorse.
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the number of the drone in the DroneId field of the parcel to have the updated droneId number.
            //according to the number that was found while looking for an exception.  
            Parcel updatedParcel = parcels[parcelIndex];
            updatedParcel.AssigndedTime = DateTime.Now;
            updatedParcel.DroneId = droneId;
            parcels[parcelIndex] = updatedParcel;

            XMLTools.SaveListToXMLSerializer<Parcel>(parcels, ParcelsPath);

            return true;
        }

        public bool PickingUpParcel(int parcelId, int droneId)
        {

            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            //keeps the index in witch the idNumber was found in order to update it without iterting over the list again.

            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.
            int parcelIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            if (!drones.Exists(d => d.Id == droneId)) //check if the dron in the list
            {
                throw new IdDontExistsException(droneId, "drone");
            }

            //updating the time of pickedUp field to be now.
            //according to the number that was found while looking for an exception.  
            Parcel newParcel = parcels[parcelIndex];
            newParcel.PickedUpTime = DateTime.Now;
            newParcel.DroneId = droneId;
            parcels[parcelIndex] = newParcel;

            XMLTools.SaveListToXMLSerializer<Parcel>(parcels, ParcelsPath);

            return true;
        }

        public bool DeliveringParcel(int parcelId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);

            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.
            int parcelIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "parcel");
            }

            //updating the time of delivered field to be now.
            //according to the number that was found while looking for an exception.  
            Parcel newParcel = parcels[parcelIndex];
            newParcel.DroneId = 0;
            newParcel.DeliveryTime = DateTime.Now;
            parcels[parcelIndex] = newParcel;

            XMLTools.SaveListToXMLSerializer<Parcel>(parcels, ParcelsPath);

            return true;
        }


        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> f)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationsPath);
            return baseStations.Where(c => f(c));
        }

        public IEnumerable<DroneCharge> GetChargeDrones(Predicate<DroneCharge> f)
        {
            XElement droneChargesRoot = XElement.Load(DroneChargesPath);
            return (from ds in droneChargesRoot.Elements()
                    select new DroneCharge()
                    {
                        DroneId = int.Parse(ds.Element("DroneId").Value),
                        StationId = int.Parse(ds.Element("StationId").Value),
                        EntryIntoCharge = DateTime.Parse(ds.Element("EntryIntoCharge").Value)
                    }).Where(c => f(c));
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> f)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            return customers.Where(c => f(c));
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> f)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            return drones.Where(c => f(c));
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> f)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            return parcels.Where(c => f(c));
        }

        public Customer GetCustomer(int customerId)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            int custumerIndex = customers.FindIndex(c => c.Id == customerId);
            if(custumerIndex == -1)
            {
                throw new IdDontExistsException(customerId , "customer");
            }
            return customers[custumerIndex];
        }

        public Drone GetDrone(int droneId)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            int custumerIndex = drones.FindIndex(c => c.Id == droneId);
            if (custumerIndex == -1)
            {
                throw new IdDontExistsException(droneId, "drone");
            }
            return drones[custumerIndex];
        }

        public Parcel GetParcel(int parcelId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            int custumerIndex = parcels.FindIndex(c => c.Id == parcelId);
            if (custumerIndex == -1)
            {
                throw new IdDontExistsException(parcelId, "Parcel");
            }
            return parcels[custumerIndex];
        }

        public BaseStation GetBaseStation(int baseStationId)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationsPath);
            int custumerIndex = baseStations.FindIndex(c => c.Id == baseStationId);
            if (custumerIndex == -1)
            {
                throw new IdDontExistsException(baseStationId, "BaseStation");
            }
            return baseStations[custumerIndex];
        }



        public int GetSerialNumber()
        {
            throw new NotImplementedException();
        }

        public int GetClosestStation(int customerId)
        {
            IEnumerable<BaseStation> baseStations = GetBaseStations(_ => true);
            Customer customer = GetCustomer(customerId);

            //double distanc
            double minDistance = Distance(customer.Location, baseStations.ElementAt(0).Location);

            BaseStation clothestBaseStation = baseStations.OrderByDescending(b => Distance(customer.Location, b.Location)).FirstOrDefault();

            return clothestBaseStation.Id;
        }

        public double[] ElectricityUse()
        {
            XElement configElements = XElement.Load(ConfigPath);

            double[] configValues = {double.Parse(configElements.Element("Free").Value) ,
                                     double.Parse(configElements.Element("Light").Value),
                                     double.Parse(configElements.Element("Middel").Value),
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
            int R = 6371;

            double f1 = ConvertToRadians(l1.Latitude);
            double f2 = ConvertToRadians(l2.Latitude);

            double df = ConvertToRadians(l1.Latitude - l2.Latitude);
            double dl = ConvertToRadians(l1.Longitude - l2.Longitude);

            double a = Math.Sin(df / 2) * Math.Sin(df / 2) +
            Math.Cos(f1) * Math.Cos(f2) *
            Math.Sin(dl / 2) * Math.Sin(dl / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance.
            double d = R * c;

            return d;



            //var baseRad = Math.PI * l1.Latitude / 180;
            //var targetRad = Math.PI * l2.Latitude / 180;
            //var theta = l1.Longitude - l2.Longitude;
            //var thetaRad = Math.PI * theta / 180;

            //double dist =
            //    Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
            //    Math.Cos(targetRad) * Math.Cos(thetaRad);
            //dist = Math.Acos(dist);

            //dist = dist * 180 / Math.PI;
            //dist = dist * 9 * 1.1515; // the size in not the original size of earth.

            //return dist;
        }

    }
}
