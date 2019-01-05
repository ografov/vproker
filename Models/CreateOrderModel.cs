using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using vproker.Controllers;
using vproker.Services;

namespace vproker.Models
{
    public class CreateOrderModel : IValidatableObject
    {
        public CreateOrderModel()
        {
        }

        [Required(ErrorMessage = "Инструмент - самое главное!")]
        [Display(Name = "Инструмент")]
        public String ToolID { get; set; }

        [Display(Name = "Паспорт клиента")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Серия и номер паспорта указаны не корректно")]
        [PassportValidation(ErrorMessage = "Паспорт не действителен")]
        public string Passport { get; set; }

        [Required(ErrorMessage = "Неправильно набран номер")]
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7}$", ErrorMessage = "Неправильно набран номер")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ФИО")]
        public string ClientName { get; set; }

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!PassportCheck.Validate(this.Passport))
            {
                yield return new ValidationResult($"Паспорт более не действителен", new[] { nameof(Passport) });
            }
        }
        internal void Save(ClaimsPrincipal user, ApplicationDbContext appContext)
        {
            Client client = appContext.Clients.FirstOrDefault(o => String.Equals(o.PhoneNumber, this.PhoneNumber, StringComparison.InvariantCultureIgnoreCase));
            if (client == null)
            {
                client = new Client() { PhoneNumber = this.PhoneNumber, Name = this.ClientName, Passport = this.Passport };
                appContext.Clients.Add(client);
            }
            else
            {
                // need to revalidate passport
                if(!String.IsNullOrEmpty(client.Passport) && PassportCheck.Validate(client.Passport))
                {
                    throw new Exception("Паспорт более не действителен");
                }
            }

            var order = new Order()
            {
                ClientID = client.ID,
                Client = client,

                ToolID = ToolID,
                //Tool = tool,

                ContractNumber = ContractNumber,
                PaidPledge = PaidPledge,
                Description = Description,
                CreatedBy = user.Identity.Name,

                // have to fill as not nullable
                ClientName = client.Name,
                ClientPhoneNumber = client.PhoneNumber
            };
            appContext.Orders.Add(order);
        }
    }
}
