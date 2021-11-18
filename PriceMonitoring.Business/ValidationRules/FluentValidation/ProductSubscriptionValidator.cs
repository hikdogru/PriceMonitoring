using FluentValidation;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.ValidationRules.FluentValidation
{
    public class ProductSubscriptionValidator : AbstractValidator<ProductSubscription>
    {
        public ProductSubscriptionValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductId).NotNull();
            RuleFor(x => x.ProductId).GreaterThan(0);

            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.UserId).GreaterThan(0);


            RuleFor(x => x.ProductPriceId).NotEmpty();
            RuleFor(x => x.ProductPriceId).NotNull();
            RuleFor(x => x.ProductPriceId).GreaterThan(0);
        }
    }
}
