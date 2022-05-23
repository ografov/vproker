using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Models
{
    public class Settings
    {
        // single object always
        public String ID { get; set; } = "Settings";

        [Display(Name = "Начать нумерацию договоров")]
        public int StartContractNumber { get; set; }

        public List<Partners> PartnersList { get; set; }

        public DateTime StartContractNumberSince { get; set; }

        internal void BeforeStore()
        {
	        this.StartContractNumberSince = DateTime.UtcNow;
        }
        [NotMapped]
        public string PartnerName { get; set; }

        [NotMapped]
        public float PartnerDiscount { get; set; }
        public Settings()
        {
	        PartnersList = new List<Partners>
	        {
		        new Partners("Test", 2), new Partners("Test", 2),
		        new Partners("Test", 2)
            };
        }
    }
}
