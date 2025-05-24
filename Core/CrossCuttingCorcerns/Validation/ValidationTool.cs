using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingCorcerns.Validation
{
    public static class ValidationTool
    {
        public static async Task ValidateAsync(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = await validator.ValidateAsync(context);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }

}
