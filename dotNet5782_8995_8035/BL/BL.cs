using System;
using System.Collections.Generic;
using DalObject;
using IDAL.DO;
using IBAL.BO;
using System.Linq;

namespace IBL
{
    public class BL : IBL
    {
        private IDAL.IDal dalObject;

        private double free; // for the electricity use of a free drone
        private double light; // for the electricity use of a drone that carrys a light wight.
        private double middel; // for the electricity use of a drone that carrys a middle wight.
        private double heavy; // for the electricity use of a drone that carrys a heavy wight.
        private double chargingSpeed;//for the speed of the charge. precentage for hour.

        private List<IBAL.BO.DroneForList> drones;


        private static Random r = new Random();     // a static value for 

        //static DateTime nulldate = new DateTime();//needed for comperation


        public BL()
        {

            dalObject = new DalObject.DalObject();

            drones = new List<DroneForList>();

            double[] electricityUse = dalObject.ElectricityUse();

            this.free = electricityUse[0];
            this.light = electricityUse[1];
            this.middel = electricityUse[2];
            this.heavy = electricityUse[3];
            this.chargingSpeed = electricityUse[4];

            //translats all the drones from the data lavel to 
            foreach (IDAL.DO.Drone dalDrone in dalObject.GetDrones(d => true))
            {
                this.drones.Add(new DroneForList() { Id = dalDrone.Id, Model = dalDrone.Model, Weight = (IBAL.BO.Enums.WeightCategories)dalDrone.MaxWeight, Status = (IBAL.BO.Enums.DroneStatuses)(r.Next() % 2 * 2) });
            }

            foreach (IDAL.DO.Parcel parcel in dalObject.GetParcels(parcel => parcel.DroneId != -1 && parcel.AcceptedTime != null))
            {

                double deliveryDistance = 0;
                //caculating the distance between the sender of the parcel, and the reciver. 
                deliveryDistance += distance(locationTranslate(dalObject.GetCustomer(parcel.SenderId).Location), locationTranslate(dalObject.GetCustomer(parcel.TargetId).Location));
                //caculating the distance between the reciver of the parcel, and the clothest station to the reciver. 
                deliveryDistance += distance(locationTranslate(dalObject.GetCustomer(parcel.TargetId).Location), locationTranslate(dalObject.GetBaseStation(dalObject.GetClothestStation(parcel.TargetId)).Location));


                this.GetDrone(parcel.DroneId).Status = IBAL.BO.Enums.DroneStatuses.DELIVERY;
                this.GetDrone(parcel.DroneId).ParcelId = parcel.Id;


                if (parcel.PickedUp == null)
                {

                    this.GetDrone(parcel.DroneId).Location = locationTranslate(dalObject.GetBaseStation(dalObject.GetClothestStation(parcel.SenderId)).Location);

                    deliveryDistance = distance(locationTranslate(dalObject.GetBaseStation(dalObject.GetClothestStation(parcel.SenderId)).Location), locationTranslate(dalObject.GetCustomer(parcel.SenderId).Location));

                    int minimumeValue = (int)(dalObject.ElectricityUse()[(int)this.GetDrone(parcel.DroneId).Weight] * deliveryDistance);

                    if (minimumeValue > 100) throw new IBAL.BO.NotEnoughRangeException($"the drone needs {minimumeValue} battary in order to complete to delivery. ");

                    this.GetDrone(parcel.DroneId).Battary = r.Next() % (100 - minimumeValue) + minimumeValue;

                }
                else
                {
                    this.GetDrone(parcel.DroneId).Location = locationTranslate(dalObject.GetCustomer(parcel.SenderId).Location);

                    int minimumeValue = (int)(dalObject.ElectricityUse()[(int)this.GetDrone(parcel.DroneId).Weight] * deliveryDistance);

                    if (minimumeValue > 100) throw new IBAL.BO.NotEnoughRangeException($"the drone needs {minimumeValue} battary in order to complete to delivery. ");

                    this.GetDrone(parcel.DroneId).Battary = r.Next() % (100 - minimumeValue) + minimumeValue;

                }
            }


            foreach (IBAL.BO.DroneForList drone in this.drones)
            {
                if (drone.Status == Enums.DroneStatuses.FREE)
                {
                    List<IDAL.DO.Parcel> parcels = dalObject.GetParcels(p => true).ToList();

                    int index = r.Next() % (parcels.Count);

                    drone.Location = locationTranslate(dalObject.GetCustomer(parcels[index].TargetId).Location);

                    double deliveryDistance = distance(locationTranslate(dalObject.GetBaseStation(dalObject.GetClothestStation(parcels[index].TargetId)).Location), locationTranslate(dalObject.GetCustomer(parcels[index].TargetId).Location));

                    double battayConcamption = deliveryDistance * dalObject.ElectricityUse()[(int)drone.Weight + 1];

                    if (battayConcamption > 100) throw new IBAL.BO.NotEnoughRangeException($"the drone needs {battayConcamption} battary in order to complete to delivery. ");

                    drone.Battary = (int)(r.NextDouble() * (100 - battayConcamption) + battayConcamption);

                }

                if (drone.Status == Enums.DroneStatuses.MAINTENANCE)
                {
                    int stationNumber = r.Next() % dalObject.GetBaseStationsNumber();
                    drone.Location = locationTranslate(dalObject.GetBaseStation(dalObject.GetBaseStationId(stationNumber)).Location);

                    drone.Battary = r.Next() % 20;

                }

            }



        }//end BL ctor



        ////**** adding option ****////


        public void AddBaseStation(IBAL.BO.BaseStation newBaseStation)
        {
            dalObject.AddBaseStation(newBaseStation.Id, newBaseStation.Name, locationTranslate(newBaseStation.Location), newBaseStation.ChargeSlots);
        }

        public void AddDrone(IBAL.BO.DroneForList newDrone)
        {
            foreach (IBAL.BO.DroneForList drone in this.drones)
            {
                if (drone.Id == newDrone.Id)
                {
                    throw new IBAL.BO.IdAlreadyExistsException(drone.Id, "drone");
                }
            }
            this.drones.Add(newDrone);
            dalObject.AddDrone(newDrone.Id, newDrone.Model, (IDAL.DO.WeightCategories)newDrone.Weight);
        }

        public void AddCustumer(IBAL.BO.Customer newCustomer)
        {
            dalObject.AddCustumer(newCustomer.Id, newCustomer.Name, newCustomer.Phone, locationTranslate(newCustomer.Location));
        }

        public void AddParcel(IBAL.BO.Parcel parcel)
        {
            dalObject.AddParcel(parcel.Id, parcel.Reciver.Id, parcel.Sender.Id, (IDAL.DO.WeightCategories)parcel.Weight, (IDAL.DO.Priorities)parcel.Priority, parcel.Drone.Id, parcel.CreationTime, parcel.DeliveringTime);
        }


        ////**** update options ****////


        public void UpdateDroneForAParcel(int parcelId, int droneId)
        {
            //checking if the numbers of parcel and drone that was provided exist in the database or not. if not an excption will be thrown.

            if (!dalObject.GetParcels(p => true).Any(p => p.Id == parcelId)) throw new IBAL.BO.IdDontExistsException(parcelId, "parcel");

            if (!dalObject.GetDrones(d => true).Any(d => d.Id == droneId)) throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            //if the id numbers were found in the lists we can call the function from Idal.
            dalObject.UpdateDroneForAParcel(parcelId, droneId);
        }

        public void PickingUpParcel(int parcelId)
        {
            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.

            if (!dalObject.GetParcels(p => true).Any(p => p.Id == parcelId)) throw new IBAL.BO.IdDontExistsException(parcelId, "parcel");

            //if the id numbers were found in the lists we can call the function from Idal.
            dalObject.PickingUpParcel(parcelId);
        }

        public void DeliveringParcel(int parcelId)
        {

            //checking if the number of parcel that was provided exist in the database or not. if not an excption will be thrown.

            if (!dalObject.GetParcels(p => true).Any(p => p.Id == parcelId)) throw new IBAL.BO.IdDontExistsException(parcelId, "parcel");

            //if the id numbers were found in the lists we can call the function from Idal.
            dalObject.DeliveringParcel(parcelId);
        }

        public void ChargeDrone(int droneId)
        {
            int droneIndex = this.drones.FindIndex(d => d.Id == droneId);

            if (droneIndex == -1) throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            //geting the maximum distance the drone can make according to the level of battary multiplied by the number of kilometers the drone can do for one precent do battary. 
            double maxDistance = this.drones[droneIndex].Battary * dalObject.ElectricityUse()[(int)this.drones[droneIndex].Weight + 1];

            //returns all the stations that the drone has enough fuel to get to.
            List<IDAL.DO.BaseStation> baseStations = dalObject.GetBaseStations(delegate (IDAL.DO.BaseStation b) { return distance(locationTranslate(b.Location), GetDrone(droneId).Location) <= maxDistance; }).ToList();

            //sorting the stations according to the distance from the drone.
            baseStations = baseStations.OrderBy(b => distance(locationTranslate(b.Location), GetDrone(droneId).Location)).ToList();

            //gives the first station in the list that has free slots for charging.
            IDAL.DO.BaseStation? baseStation = baseStations.First(b => b.chargeSlots > 0);

            //if there is no suitable station an exception will be thrown.
            if (baseStation == null) throw new NotAbleToSendDroneToChargeException();

            //if the tation do exist activating the dal function.
            dalObject.ChargeDrone(baseStation.Value.id, droneId);

        }


        public void UnChargeDrone(int droneId)
        {

            if (!dalObject.GetDrones(d => true).Any(d => d.Id == droneId)) throw new IBAL.BO.IdDontExistsException(droneId, "drone");

            dalObject.UnChargeDrone(droneId);
        }


        ////**** getting options ****////

        /////// !!!!!!!Exceptions!!!!!!!!!!!!!    ///////////////////

        public IBAL.BO.BaseStation GetBaseStation(int baseStationId)
        {
            try
            {
                IDAL.DO.BaseStation dalBaseStation = dalObject.GetBaseStation(baseStationId);
                return new IBAL.BO.BaseStation()
                {
                    Id = dalBaseStation.id,
                    Name = dalBaseStation.name,
                    ChargeSlots = dalBaseStation.chargeSlots,
                    Location = locationTranslate(dalBaseStation.Location)
                };
            }
            catch (IDAL.DO.IdDontExistsException e)
            {
                throw new IBAL.BO.IdDontExistsException(baseStationId, "base station", e);
            }
        }

        public IBAL.BO.DroneForList GetDrone(int droneId)
        {
            int index = this.drones.FindIndex(d => d.Id == droneId);

            if (index == -1) throw new IBAL.BO.IdDontExistsException(droneId, "drone", new IDAL.DO.IdDontExistsException(droneId, "drone"));

            return this.drones[index];
        }

        public IBAL.BO.Customer GetCustomer(int customerId)
        {
            try
            {
                IDAL.DO.Customer dalCustomer = dalObject.GetCustomer(customerId);
                return new IBAL.BO.Customer()
                {
                    Id = dalCustomer.Id,
                    Name = dalCustomer.Name,
                    Phone = dalCustomer.Phone,
                    Location = locationTranslate(dalCustomer.Location)
                };
            }
            catch (IDAL.DO.IdDontExistsException e)
            {
                throw new IBAL.BO.IdDontExistsException(customerId, "customer", e);
            }
        }

        public IBAL.BO.Parcel GetParcel(int parcelId)
        {
            try
            {
                IDAL.DO.Parcel dalParcel = dalObject.GetParcel(parcelId);
                IBAL.BO.CoustomerForParcel sender = new CoustomerForParcel() { Id = dalParcel.SenderId, CustomerName = dalObject.GetCustomer(dalParcel.SenderId).Name };
                IBAL.BO.CoustomerForParcel reciver = new CoustomerForParcel() { Id = dalParcel.TargetId, CustomerName = dalObject.GetCustomer(dalParcel.TargetId).Name };
                IBAL.BO.DroneForParcel drone = new DroneForParcel() { Id = dalParcel.DroneId };
                return new IBAL.BO.Parcel()
                {
                    Id = dalParcel.Id,
                    Sender = sender,
                    Reciver = reciver,
                    Drone = drone,
                    Priority = (IBAL.BO.Enums.Priorities)dalParcel.Priority,
                    AssigningTime = DateTime.Now,
                    CreationTime = dalParcel.Requested,
                    DeliveringTime = dalParcel.DeliveryTime,
                    PickupTime = dalParcel.PickedUp
                };
            }
            catch (IDAL.DO.IdDontExistsException e)
            {
                throw new IBAL.BO.IdDontExistsException(parcelId, "parcel", e);
            }
        }



        public IEnumerable<IBAL.BO.BaseStationForList> GetBaseStations(Predicate<IDAL.DO.BaseStation> f)
        {

            List<IBAL.BO.BaseStationForList> baseStations = new List<IBAL.BO.BaseStationForList>();
            foreach (IDAL.DO.BaseStation baseStation in dalObject.GetBaseStations(f))
            {
                baseStations.Add(new IBAL.BO.BaseStationForList()
                {
                    Id = baseStation.id,
                    Name = baseStation.name,
                    FreeChargingSlots = baseStation.chargeSlots - dalObject.GetChargeDrones(cd => cd.StationId == baseStation.id).Count(),
                    TakenCharingSlots = dalObject.GetChargeDrones(cd => cd.StationId == baseStation.id).Count()
                });
            }
            return baseStations;
        }

        public IEnumerable<IBAL.BO.DroneForList> GetDrones(Predicate<IBAL.BO.DroneForList> f)
        {
            return this.drones;
        }

        public IEnumerable<IBAL.BO.CustomerForList> GetCustomers(Predicate<IBAL.BO.CustomerForList> f)
        {
            List<IBAL.BO.Customer> customers = new List<IBAL.BO.Customer>();
            foreach (IDAL.DO.Customer customer in dalObject.GetCustomers(c => true))
            {
                customers.Add(new IBAL.BO.Customer()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Location = locationTranslate(customer.Location)
                });
            }
            return customers;
        }

        public IEnumerable<IBAL.BO.ParcelForList> GetPacelss(Predicate<IBAL.BO.ParcelForList> f)
        {
            List<IBAL.BO.Parcel> parcels = new List<IBAL.BO.Parcel>();
            foreach (IDAL.DO.Parcel dalParcel in dalObject.GetParcels(p => true))
            {
                IBAL.BO.CoustomerForParcel sender = new CoustomerForParcel() { Id = dalParcel.SenderId, CustomerName = dalObject.GetCustomer(dalParcel.SenderId).Name };
                IBAL.BO.CoustomerForParcel reciver = new CoustomerForParcel() { Id = dalParcel.TargetId, CustomerName = dalObject.GetCustomer(dalParcel.TargetId).Name };
                IBAL.BO.DroneForParcel drone = new DroneForParcel() { Id = dalParcel.DroneId };

                parcels.Add(new IBAL.BO.Parcel()
                {
                    Id = dalParcel.Id,
                    Sender = sender,
                    Reciver = reciver,
                    Drone = drone,
                    Priority = (IBAL.BO.Enums.Priorities)dalParcel.Priority,
                    AssigningTime = DateTime.Now,
                    CreationTime = dalParcel.Requested,
                    DeliveringTime = dalParcel.DeliveryTime,
                    PickupTime = dalParcel.PickedUp
                });
            }
            return parcels;
        }





        private double distance(IBAL.BO.Location l1, IBAL.BO.Location l2)
        {
            return Math.Sqrt(Math.Pow(l1.Longitude - l2.Longitude, 2) + Math.Pow(l1.Lattitude - l2.Lattitude, 2));
        }

        private IBAL.BO.Location locationTranslate(IDAL.DO.Location location)
        {
            return new IBAL.BO.Location() { Longitude = location.Longitude, Lattitude = location.Longitude };
        }

        private IDAL.DO.Location locationTranslate(IBAL.BO.Location location)
        {
            return new IDAL.DO.Location() { Longitude = location.Longitude, Lattitude = location.Longitude };
        }

        public int GetNextSerialNumberForParcel()
        {
            return dalObject.GetSerialNumber();
        }

        public IBAL.BO.CoustomerForParcel GetCustomerForParcel(int customerId)
        {
            IBAL.BO.CoustomerForParcel customer = new IBAL.BO.CoustomerForParcel()
            {
                Id = this.GetCustomer(customerId).Id,
                CustomerName = this.GetCustomer(customerId).Name
            };
            return customer;
        }

        //public IEnumerable<IBAL.BO.Parcel> GetParcelToDrone()
        //{
        //    List<IBAL.BO.Parcel> parcels = new List<IBAL.BO.Parcel>();
        //    foreach (IDAL.DO.Parcel parcel in dalObject.GetParcelToDrone())
        //    {
        //        parcels.Add(new IBAL.BO.Parcel()
        //        {
        //            Id = parcel.Id,
        //            SenderId = parcel.SenderId,
        //            TargetId = parcel.TargetId,
        //            DroneId = parcel.DroneId,
        //            Requested = parcel.Requested,
        //            Scheduled = parcel.Scheduled,
        //            priority = (IBAL.BO.Enums.Priorities)parcel.Priority,
        //            Weight = (IBAL.BO.Enums.WeightCategories)parcel.Weight,
        //            PickedUp = parcel.PickedUp,
        //            Delivered = parcel.Delivered
        //        });
        //    }
        //    return parcels;
        //}


        ////////functions for main


        public void SetNameForADrone(int droneId, string model)
        {

            this.GetDrone(droneId).Model = model;

            //dalObject.SetNameForADrone(droneId, model);

            int index = dalObject.GetDrones().ToList().FindIndex(d => (d.Id == droneId));

            IDAL.DO.Drone drone = dalObject.GetDrones().ToList()[index];

            drone.Model = model;

            dalObject.GetDrones().ToList()[index] = drone;

        }

        public void UpdateBaseStation(int basStationID, string name, int slots)
        {

            int index = dalObject.GetBaseStations(delegate (IDAL.DO.BaseStation b) { return true; }).ToList().FindIndex(d => (d.id == basStationID));

            if (index == -1) throw new IDAL.DO.IdDontExistsException();

            IDAL.DO.BaseStation baseStation = dalObject.GetBaseStations(delegate (IDAL.DO.BaseStation b) { return true; }).ToList()[index];

            baseStation.name = name;

            baseStation.chargeSlots = slots;

            dalObject.GetBaseStations(delegate (IDAL.DO.BaseStation b) { return true; }).ToList()[index] = baseStation;


            //dalObject.UpdateBaseStation(basStationID, name, slots);
        }

        public void UpdateCustomer(int customerID, string name, string phone)
        {
            int index = dalObject.GetCustomers().ToList().FindIndex(d => d.Id == customerID);

            if (index == -1) throw new IBAL.BO.IdDontExistsException();

            IDAL.DO.Customer customer = dalObject.GetCustomers().ToList()[index];

            customer.Name = name;

            customer.Phone = phone;

            dalObject.GetCustomers().ToList()[index] = customer;
        }
    }//END BL class
}//end IBAL
