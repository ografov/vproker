﻿using Microsoft.AspNet.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vproker.Models
{
    public class Order 
    {
        public Order()
        {
            ID = Guid.NewGuid().ToString();
            StartDate = DateTime.UtcNow;
        }

        public String ID { get; set; }

        [Required]
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

        [Required]
        [Display(Name = "Клиент")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "Телефон клиента")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7}$", ErrorMessage = "Неправильно набран номер")]
        public string ClientPhoneNumber { get; set; }

        [Display(Name = "Примечание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Залог")]
        [DataType(DataType.Currency)]
        public Decimal PaidPledge { get; set; }

        [Display(Name = "Цена")]
        [DataType(DataType.Currency)]
        public Decimal Price { get; set; } = 0;

        // not user fields
        [Required]
        [Display(Name = "Начало аренды")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Кем создан")]
        public string CreatedBy { get; set; }

        [Display(Name = "Конец aренды")]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Сумма заказа")]
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
