using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    class DroneForList : INotifyPropertyChanged
    {
        BO.DroneForList bODroneForList;
        public DroneForList(BO.DroneForList  droneForList = null)
        {
            this.bODroneForList = droneForList;
        }
    
        public int Id 
        {
            get
            {
                return bODroneForList.Id;
            }
            set
            {
                bODroneForList.Id = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }

        public string Model { get; set; }
        public BO.Enums.WeightCategories Weight { get; set; }
        public double Battery { get; set; }
        public BO.Enums.DroneStatuses Status { get; set; }
        public BO.Location Location { get; set; }
        public int ParcelId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
         
    }
}
