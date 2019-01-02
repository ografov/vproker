using System;
using System.ComponentModel.DataAnnotations;
using vproker.Services;

namespace vproker.Models
{
    public class CloseMaintainModel
    {
        public CloseMaintainModel()
        { 
        }
        public CloseMaintainModel(Maintain maintain)
        {
            this.Maintain = maintain;
            this.ID = maintain.ID;
        }

        public string ID { get; set; }
        public Maintain Maintain { get; set; }
        
    }
}
