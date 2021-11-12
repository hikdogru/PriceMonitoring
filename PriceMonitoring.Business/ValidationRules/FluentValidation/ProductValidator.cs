using FluentValidation;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).NotNull();

            RuleFor(x => x.Image).NotEmpty();
            RuleFor(x => x.Image).NotNull();

            RuleFor(x => x.WebsiteId).NotNull();
            RuleFor(x => x.WebsiteId).GreaterThan(0);
        }
    }
}
