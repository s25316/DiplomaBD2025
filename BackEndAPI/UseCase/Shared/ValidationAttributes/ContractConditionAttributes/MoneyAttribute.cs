﻿using System.ComponentModel.DataAnnotations;
using UseCase.Shared.ValidationAttributes.BaseAttributes;

namespace UseCase.Shared.ValidationAttributes.ContractConditionAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MoneyAttribute : BaseAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var moneyDecimal = (decimal?)value;
            if (moneyDecimal == null)
            {
                return ValidationResult.Success;
            }

            if (moneyDecimal.HasValue && moneyDecimal.Value < 0)
            {
                SetDecimalValue(validationContext, 0);
            }

            return ValidationResult.Success;
        }
    }
}
