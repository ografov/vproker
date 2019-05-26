using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using vproker.Controllers;

namespace vproker.Models
{
    public class Client
    {
        public Client()
        {
            ID = Guid.NewGuid().ToString();
            //KnowSourceID = KnowledgeSource.AVITO_ID;
        }

        public String ID { get; set; }

        [Required(ErrorMessage = "Клиент всегда прав")]
        [Display(Name = "Клиент")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Неправильно набран номер")]
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7}$", ErrorMessage = "Неправильно набран номер")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Паспорт клиента")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Серия и номер паспорта указаны не корректно")]
        [PassportValidation(ErrorMessage ="Паспорт не действителен")]
        public string Passport { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Примечание")]
        public string Description { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        //#region phones
        //[Display(Name = "Телефон 1")]
        //public string Phone1 { get; set; }

        //[Display(Name = "Телефон 2")]
        //public string Phone2 { get; set; }

        //[Display(Name = "Телефон 3")]
        //public string Phone3 { get; set; }
        //#endregion

        //#region addresses

        //[Display(Name = "Адрес регистрации", GroupName = "Адрес")]
        //public string RegistrationAddress { get; set; }

        //[Display(Name = "Адрес проживания", GroupName = "Адрес")]
        //public string LivingAddress { get; set; }

        //[Display(Name = "Адрес проведения работ", GroupName = "Адрес")]
        //public string WorkingAddress { get; set; }

        //#endregion

        //[Display(Name = "Скидка (%)")]
        //public Int32 DiscountPercent { get; set; }

        //[Required]
        //[Display(Name = "Как узнал?")]
        //public string KnowSourceID { get; set; }
        //public KnowledgeSource KnowSource { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
