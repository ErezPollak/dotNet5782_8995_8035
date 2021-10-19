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
        public DalObject()
        {
            DataSource.Initialize();
        }

        public void nnnnn()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(DataSource.drones[i].toString());
            }
        }


        //add options

        public void addBaseStation(int name, double longtude, double lattitude, int chargeslots)
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

        public void addParcel(int senderId, int targetId, IDAL.DO.WeightCategories weight, IDAL.DO.Priorities praiority, int droneId, DateTime reqested, DateTime scheduled, DateTime delivered, DateTime pickedUp)
        {
            DataSource.parcheses[DataSource.Config.freePerches] = new IDAL.DO.Parcel() { id = DataSource.Config.freePerches, senderId = senderId, targetId = targetId, Weight = weight, priority = praiority, droneId = droneId, requested = reqested, scheduled = scheduled, delivered = delivered, pickedUp = pickedUp };
            ++DataSource.Config.freePerches;
        }

        //update options

        public void mergeParcelToDrone(int parcelId, int droneId)
        {
            DataSource.parcheses[parcelId].droneId = droneId;
        }

        public void pickingUpParcel(int parcelId)
        {
            DataSource.parcheses[parcelId].pickedUp = DateTime.Now;
        }

        public void deliveringParcel(int parcelId)
        {
            DataSource.parcheses[parcelId].delivered = DateTime.Now;
        }

        public void chargeDrone(int baseStationId, int droneId)
        {
            DataSource.drones[droneId].Status = IDAL.DO.DroneStatuses.FIXING;
            DataSource.baseStations[baseStationId].chargeSlots--;
            IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge() { droneId = droneId, stationId = baseStationId };
        }



    }
}
