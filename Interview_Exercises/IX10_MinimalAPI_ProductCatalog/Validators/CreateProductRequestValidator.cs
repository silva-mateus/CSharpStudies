using FluentValidation;
using IX10_MinimalAPI_ProductCatalog.Models;

namespace IX10_MinimalAPI_ProductCatalog.Validators;

// TODO: Implement validation rules using FluentValidation:
// - Name: Required, length 1-200
// - Description: Optional, max 1000 chars
// - Price: Must be > 0
// - Category: Required, non-empty

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Category)
            .NotEmpty();
    }
}

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Category)
            .NotEmpty();
    }
}
