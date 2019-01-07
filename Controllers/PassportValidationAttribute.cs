using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vproker.Services;

namespace vproker.Controllers
{
    public class PassportValidationAttribute : ValidationAttribute
    {
        public PassportValidationAttribute()
        {
        }
        public override bool IsValid(object value)
        {
            return PassportCheck.Validate(value as string);
        }
    }
}
