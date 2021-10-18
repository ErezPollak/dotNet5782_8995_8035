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
                Console.WriteLine(DataSource.parcheses[i].toString());
            }
        }


        //add options

        public void addBaseStation(int id , int name , double longtude , double lattitude , int chargeslots)
        {
            DataSource.baseStations[DataSource.Config.freeBaseStation] = new IDAL.DO.BaseStation() {id = id , name = name , longitude = longtude , lattitude = lattitude , chargeSlots = chargeslots};
            ++DataSource.Config.freeBaseStation;
        }
        
        public  void addDrone(int id , string model , IDAL.DO.WeightCategories weight , IDAL.DO.DroneStatuses status , double battery)
        {
            DataSource.drones[DataSource.Config.freeDrone] = new IDAL.DO.Drone() {id = id , model = model , MaxWeight = weight , Status = status , battery = battery};
            ++DataSource.Config.freeDrone;
        }
        public void addCustumer(int id , string name , string phone , double longtitude , double lattitude)
        {
            DataSource.customers[DataSource.Config.freeCustumer] = new IDAL.DO.Customer() {id = id,name = name , phone = phone , longitude = longtitude , lattitude = lattitude };
            ++DataSource.Config.freeCustumer;
        }

        public void addParcel(int id , int senderId , int targetId , IDAL.DO.WeightCategories weight , IDAL.DO.Priorities praiority , DateTime reqested , DateTime )




    }
}
