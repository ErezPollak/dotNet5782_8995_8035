
//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program is a public class for the namespace "DalObjects", that contains all the basic functions that can be done with the data structures.


using DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {

        //an data structure to contain all the charging of the drones.
        public IDAL.DO.DroneCharge[] charges;

        //ctor
        public DalObject()
        {
            DataSource.Initialize();

            charges = new IDAL.DO.DroneCharge[10];

            //initilaze the drone chrges to contain non realistic drone ids to represent that they are empty.
            for (int i = 0; i < 10; i++)
            {
                charges[i].droneId = -1;
            }

        }

        //////***add options***/////
        
        // the function creates new base station acording to given specs, and adding it to the array. while updating the config class.
        public void addBaseStation(string name, double longtude, double lattitude, int chargeslots)
        {
            DataSource.baseStations[DataSource.Config.freeBaseStation] = new IDAL.DO.BaseStation() { id = DataSource.Config.freeBaseStation, name = name, longitude = longtude, lattitude = lattitude, chargeSlots = chargeslots };
            ++DataSource.Config.freeBaseStation;
        }

        // the function creates new drone acording to given specs, and adding it to the array. while updating the config class.
        public void addDrone(string model, IDAL.DO.WeightCategories weight, IDAL.DO.DroneStatuses status, double battery)
        {
            DataSource.drones[DataSource.Config.freeDrone] = new IDAL.DO.Drone() { id = DataSource.Config.freeDrone, model = model, MaxWeight = weight, Status = status, battery = battery };
            ++DataSource.Config.freeDrone;
        }

        // the function creates new customer acording to given specs, and adding it to the array. while updating the config class.
        public void addCustumer(string name, string phone, double longtitude, double lattitude)
        {
            DataSource.customers[DataSource.Config.freeCustumer] = new IDAL.DO.Customer() { id = DataSource.Config.freeCustumer, name = name, phone = phone, longitude = longtitude, lattitude = lattitude };
            ++DataSource.Config.freeCustumer;
        }

        // the function creates new parcel acording to given specs, and adding it to the array. while updating the config class.
        public void addParcel(int senderId, int targetId, IDAL.DO.WeightCategories weight, IDAL.DO.Priorities praiority, int droneId, DateTime reqested, DateTime scheduled)
        {
            DataSource.parcheses[DataSource.Config.freePerches] = new IDAL.DO.Parcel() { id = DataSource.Config.freePerches, senderId = senderId, targetId = targetId, Weight = weight, priority = praiority, droneId = droneId, requested = reqested, scheduled = scheduled, delivered = new DateTime(), pickedUp = new DateTime() };
            ++DataSource.Config.freePerches;
        }

        ////***update options***/////
        
        //the function is givig the parcel the number of the drone.
        public void updateDroneForAParcel(int parcelId, int droneId)
        {
            DataSource.parcheses[parcelId].droneId = droneId;
        }

        //updating the time of pickup in the parcel, and changing the status of the drone to delivery.
        public void pickingUpParcel(int parcelId)
        {
            DataSource.parcheses[parcelId].pickedUp = DateTime.Now;
            DataSource.drones[DataSource.parcheses[parcelId].droneId].Status = IDAL.DO.DroneStatuses.DELIVERY;
        }

        //updating the time of delivering in the parcel, and changing the status of the drone to free.
        public void deliveringParcel(int parcelId)
        {
            DataSource.parcheses[parcelId].delivered = DateTime.Now;
            DataSource.drones[DataSource.parcheses[parcelId].droneId].Status = IDAL.DO.DroneStatuses.FREE;
        }


        // creating a droneCharge and chrging the drone.
        public void chargeDrone(int baseStationId, int droneId)
        {
            DataSource.drones[droneId].Status = IDAL.DO.DroneStatuses.FIXING;
            --DataSource.baseStations[baseStationId].chargeSlots;
            IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge() { droneId = droneId, stationId = baseStationId };

            for (int i = 0; i < 10; i++)
            {
                if(charges[i].droneId == -1)
                {
                    charges[i] = droneCharge;
                }
            }
        }


        //deleting the droneCharge from the array.
        public void unChargeDrone(int droneId)
        {
            DataSource.drones[droneId].battery = 100;
            DataSource.drones[droneId].Status = IDAL.DO.DroneStatuses.FREE;

            for (int i = 0; i < 10; i++)
            {
                if(charges[i].droneId == droneId)
                {
                    charges[i].droneId = -1;
                    ++DataSource.baseStations[charges[i].stationId].chargeSlots;
                }
            }

        }

        ////***show options***/////

        // writing the props of the base station according to given id.
        public void showBaseStation(int baseStationId)
        {
            Console.WriteLine(DataSource.baseStations[baseStationId].toString());
        }

        // writing the props of the drone according to given id.
        public void showDrone(int droneId)
        {
            Console.WriteLine(DataSource.drones[droneId].toString());
        }

        // writing the props of the customer according to given id.
        public void showCustomer(int customerId)
        {
            Console.WriteLine(DataSource.customers[customerId].toString());
        }

        // writing the props of the parcel according to given id.
        public void showParcel(int parchesId)
        {
            Console.WriteLine(DataSource.parcheses[parchesId].toString());
        }

        ////***showLists***////

        // printing all the props of all the base tsations.
        public void showBaseStationsList()
        {
            for (int i = 0; i < DataSource.Config.freeBaseStation; i++)
            {
                Console.WriteLine(DataSource.baseStations[i].toString());
            }
        }

        // printing all the props of all the drone.
        public void showDronesList()
        {
            for (int i = 0; i < DataSource.Config.freeDrone; i++)
            {
                Console.WriteLine(DataSource.drones[i].toString());
            }
        }

        // printing all the props of all the customers.
        public void showCustomersList()
        {
            for (int i = 0; i < DataSource.Config.freeCustumer; i++)
            {
                Console.WriteLine(DataSource.customers[i].toString());
            }
        }

        // printing all the props of all the parcels.
        public void showParchesesList()
        {
            for (int i = 0; i < DataSource.Config.freePerches; i++)
            {
                Console.WriteLine(DataSource.parcheses[i].toString());
            }
        }

        // show all the parcheses that have invalid number of a drone.
        public void showParchesesThatDontHaveADrone()
        {
            for (int i = 0; i < DataSource.Config.freePerches; i++)
            {
                if (DataSource.parcheses[i].droneId == -1)
                {
                    Console.WriteLine(DataSource.parcheses[i].toString());
                }
            }
        }

        //shoing all the base statioins that have more then zero avalible charging slots.
        public void showAvalibleBaseStations()
        {
            for (int i = 0; i < DataSource.Config.freeBaseStation; i++)
            {
                if (DataSource.baseStations[i].chargeSlots > 0)
                {
                    Console.WriteLine(DataSource.baseStations[i].toString());
                }
            }
        }

        //shoing all the ids of base statioins that have more then zero avalible charging slots.
        public void showAvalibleBaseStationsID()
        {
            for (int i = 0; i < DataSource.Config.freeBaseStation; i++)
            {
                if (DataSource.baseStations[i].chargeSlots > 0)
                {
                    Console.Write(DataSource.baseStations[i].id + "  ");
                }
            }
        }

        //shoing all the free drones.
        public void showListOfDronesForPercel(IDAL.DO.WeightCategories weight)
        {
            for (int i = 0; i < DataSource.Config.freeDrone; i++)
            {
                if(DataSource.drones[i].MaxWeight >= weight && DataSource.drones[i].Status == IDAL.DO.DroneStatuses.FREE)
                {
                    Console.Write(i + " ");
                }
            }
        }

    }
}
