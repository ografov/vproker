using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Models
{
    public class Client
    {
        public Client()
        {
            ID = Guid.NewGuid().ToString();
            KnowSourceID = KnowledgeSource.AVITO_ID;
        }

        public String ID { get; set; }

        #region name
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "ФИО")]
        public string FullName
        {
            get
            {
                return FirstName + " " + (String.IsNullOrEmpty(MiddleName) ? String.Empty : MiddleName+" ") + LastName;
            }
        }
        #endregion

        [Required]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        #region phones
        [Display(Name = "Телефон 1")]
        public string Phone1 { get; set; }

        [Display(Name = "Телефон 2")]
        public string Phone2 { get; set; }

        [Display(Name = "Телефон 3")]
        public string Phone3 { get; set; }
        #endregion

        #region addresses

        [Display(Name = "Адрес регистрации", GroupName = "Адрес")]
        public string RegistrationAddress { get; set; }

        [Display(Name = "Адрес проживания", GroupName = "Адрес")]
        public string LivingAddress { get; set; }

        [Display(Name = "Адрес проведения работ", GroupName = "Адрес")]
        public string WorkingAddress { get; set; }

        #endregion

        [Display(Name = "Скидка (%)")]
        public Int32 DiscountPercent { get; set; }

        [Display(Name = "Примечание")]
        public string Note { get; set; }

        #region document
        [Display(Name = "Серия", GroupName = "Документ")]
        [Required]
        public string DocumentSerial { get; set; }

        [Display(Name = "Номер", GroupName = "Документ")]
        [Required]
        public string DocumentNumber { get; set; }

        [Display(Name = "Дата выдачи", GroupName = "Документ")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DocumentGivenWhen { get; set; }

        [Display(Name = "Кем выдан", GroupName = "Документ")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string DocumentGivenBy { get; set; }

        [Display(Name = "Код подразделения", GroupName = "Документ")]
        public string DocumentUnitCode { get; set; }

        [Display(Name = "Изображение", GroupName = "Документ")]
        public byte[] DocumentData { get; set; }
        #endregion

        #region relationships

        [Required]
        [Display(Name = "Как узнал?")]
        public string KnowSourceID { get; set; }
        public KnowledgeSource KnowSource { get; set; }

        #endregion
    }
}
