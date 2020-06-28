using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vproker.Models
{
    [Table(nameof(Order))]
    public partial class Order 
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
        [Required]
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

        [Required(ErrorMessage = "Клиент необходим!")]
        [Display(Name = "Клиент")]
        public String ClientID { get; set; }

        private Client client;
        [Display(Name = "Инструмент")]
        [Required]
        public Client Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.client = value;
            }
        }

        [Required(ErrorMessage = "Номер договора лучше ввести")]
        [Display(Name = "Номер договора")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Уверен?")]
        public string ContractNumber { get; set; }

        [Display(Name = "Заметка в начале")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Заметка в конце")]
        [DataType(DataType.MultilineText)]
        public string CloseDescription { get; set; }

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
