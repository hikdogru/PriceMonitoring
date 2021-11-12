using FluentValidation;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.ValidationRules.FluentValidation
{
    public class ProductPriceValidator : AbstractValidator<ProductPrice>
    {
        public ProductPriceValidator()
        {
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ProductId).NotNull();
            RuleFor(x => x.ProductId).NotEmpty();

            RuleFor(x => x.SavedDate).NotNull();
            RuleFor(x => x.SavedDate).NotEmpty();

            RuleFor(x => x.ProductId).NotNull();
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}
