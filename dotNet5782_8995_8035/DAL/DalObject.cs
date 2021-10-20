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

        public IDAL.DO.DroneCharge[] charges;


        public DalObject()
        {
            DataSource.Initialize();

            charges = new IDAL.DO.DroneCharge[10];

            for (int i = 0; i < 10; i++)
            {
                charges[i].droneId = -1;
            }

        }

        //add options

        public void addBaseStation(string name, double longtude, double lattitude, int chargeslots)
        {
            DataSource.baseStations[DataSource.Config.freeBaseStation] = new IDAL.DO.BaseStation() { id = DataSource.Config.freeBaseStation, name = name, longitude = longtude, lattitude = lattitude, chargeSlots = chargeslots };
            ++DataSource.Config.freeBaseStation;
        }

        public void addDrone(string model, IDAL.DO.WeightCategories weight, IDAL.DO.DroneStatuses status, double battery)
        {
            DataSource.drones[DataSource.Config.freeDrone] = new IDAL.DO.Drone() { id = DataSource.Config.freeDrone, model = model, MaxWeight = weight, Status = status, battery = battery };
            ++DataSource.Config.freeDrone;
        }
        public void addCustumer(string name, string phone, double longtitude, double lattitude)
        {
            DataSource.customers[DataSource.Config.freeCustumer] = new IDAL.DO.Customer() { id = DataSource.Config.freeCustumer, name = name, phone = phone, longitude = longtitude, lattitude = lattitude };
            ++DataSource.Config.freeCustumer;
        }

        public void addParcel(int senderId, int targetId, IDAL.DO.WeightCategories weight, IDAL.DO.Priorities praiority, int droneId, DateTime reqested, DateTime scheduled)
        {
            DataSource.parcheses[DataSource.Config.freePerches] = new IDAL.DO.Parcel() { id = DataSource.Config.freePerches, senderId = senderId, targetId = targetId, Weight = weight, priority = praiority, droneId = droneId, requested = reqested, scheduled = scheduled, delivered = new DateTime(), pickedUp = new DateTime() };
            ++DataSource.Config.freePerches;
        }

        //update options

        public void updateDroneForAParcel(int parcelId, int droneId)
        {
            DataSource.parcheses[parcelId].droneId = droneId;
        }

        public void pickingUpParcel(int parcelId)
        {
            DataSource.parcheses[parcelId].pickedUp = DateTime.Now;
            DataSource.drones[DataSource.parcheses[parcelId].droneId].Status = IDAL.DO.DroneStatuses.DELIVERY;
        }

        public void deliveringParcel(int parcelId)
        {
            DataSource.parcheses[parcelId].delivered = DateTime.Now;
            DataSource.drones[DataSource.parcheses[parcelId].droneId].Status = IDAL.DO.DroneStatuses.FREE;

        }

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

        //show options

        public void showBaseStation(int baseStationId)
        {
            Console.WriteLine(DataSource.baseStations[baseStationId].toString());
        }

        public void showDrone(int droneId)
        {
            Console.WriteLine(DataSource.drones[droneId].toString());
        }

        public void showCustomer(int customerId)
        {
            Console.WriteLine(DataSource.customers[customerId].toString());
        }

        public void showParcel(int parchesId)
        {
            Console.WriteLine(DataSource.parcheses[parchesId].toString());
        }

        //showLists

        public void showBaseStationsList()
        {
            for (int i = 0; i < DataSource.Config.freeBaseStation; i++)
            {
                Console.WriteLine(DataSource.baseStations[i].toString());
            }
        }

        public void showDronesList()
        {
            for (int i = 0; i < DataSource.Config.freeDrone; i++)
            {
                Console.WriteLine(DataSource.drones[i].toString());
            }
        }

        public void showCustomersList()
        {
            for (int i = 0; i < DataSource.Config.freeCustumer; i++)
            {
                Console.WriteLine(DataSource.customers[i].toString());
            }
        }

        public void showParchesesList()
        {
            for (int i = 0; i < DataSource.Config.freePerches; i++)
            {
                Console.WriteLine(DataSource.parcheses[i].toString());
            }
        }

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
