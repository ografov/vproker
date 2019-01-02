using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vproker.Models
{
    [Table(nameof(Order))]
    public class Order 
    {
        public Order()
        {
            ID = Guid.NewGuid().ToString();
            StartDate = DateTime.UtcNow;
        }

        public String ID { get; set; }

        [Required(ErrorMessage = "Инструмент - самое главное!")]
        [Display(Name = "Инструмент")]
        public String ToolID { get; set; }

        private Tool tool;
        [Display(Name = "Инструмент")]
        public Tool Tool
        {
            get
            {
                return this.tool;
            }
            set
            {
                this.tool = value;
                this.PaidPledge = this.tool.Pledge;
                this.Price = this.tool.Price;
            }
        }

        [Required(ErrorMessage = "Клиент всегда прав")]
        [Display(Name = "Клиент")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Неправильно набран номер")]
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7}$", ErrorMessage = "Неправильно набран номер")]
        public string ClientPhoneNumber { get; set; }

        //[Required(ErrorMessage = "Требуется ввести серию и номер паспорта")]
        [Display(Name = "Паспорт клиента")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Серия и номер паспорта указаны не корректно")]
        public string ClientPassport { get; set; }

        [Required(ErrorMessage = "Номер договора лучше ввести")]
        [Display(Name = "Номер договора")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Уверен?")]
        public string ContractNumber { get; set; }

        [Display(Name = "Примечание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Залог бы взять")]
        [Display(Name = "Залог")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public Decimal PaidPledge { get; set; } = 0;

        [Display(Name = "Цена")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public Decimal Price { get; set; } = 0;

        // not user fields
        [Required]
        [Display(Name = "Начало аренды")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Оператор")]
        public string CreatedBy { get; set; }

        [Display(Name = "Конец aренды")]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Сумма заказа")]
        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public Decimal? Payment { get; set; }
        
        //[HiddenInput(DisplayValue = false)]
        [NotMapped]
        public bool IsClosed
        {
            get
            {
                return this.EndDate != null;
            }
        }
    }
}
