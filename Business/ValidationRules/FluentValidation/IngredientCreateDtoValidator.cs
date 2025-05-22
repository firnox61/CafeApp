using Entities.Concrete;
using Entities.DTOs.Ingredients;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class IngredientCreateDtoValidator : AbstractValidator<IngredientCreateDto>
    {
        public IngredientCreateDtoValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty()
                .WithMessage("Malzeme ismi boş olamaz!");

            RuleFor(i => i.Unit)
                .NotEmpty()
                .WithMessage("Birim bilgisi boş olamaz!");

            RuleFor(i => i.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stok değeri negatif olamaz!");
        }
    }

}
