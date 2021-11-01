using System;
using System.Collections.Generic;
using DalObject;

namespace IBL
{
    namespace BO
    {
        public class BL : IBL
        {
            private IDAL.IDal dalObject;
            
            private double free; // for the electricity use of a free drone
            private double light; // for the electricity use of a drone that carrys a light wight.
            private double middel; // for the electricity use of a drone that carrys a middle wight.
            private double heavy; // for the electricity use of a drone that carrys a heavy wight.
            private double chargingSpeed;//for the speed of the charge. precentage for hour.

            private List<IDAL.DO.Drone> drones;

            public BL()
            {
                dalObject = new DalObject.DalObject();

                double[] electricityUse = dalObject.ElectricityUse();

                this.free = electricityUse[0];
                this.light = electricityUse[1];
                this.middel = electricityUse[2];
                this.heavy = electricityUse[3];
                this.chargingSpeed = electricityUse[4];

                this.drones = (List<IDAL.DO.Drone>)dalObject.GetDrones();

                IEnumerable<IDAL.DO.Parcel> parcelsWithADrone = dalObject.GetParcelsWithDrones();

                foreach (IDAL.DO.Parcel parcel in parcelsWithADrone)
                {
                    for (int i = 0; i < this.drones.Count; i++)
                    {
                        if(parcel.droneId == drones[i].id)
                        {

                        }
                    }
                }

            }//end BL ctor

        }//END BL class
    }//end BO
}//end IBAL
