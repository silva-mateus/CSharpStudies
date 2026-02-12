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
        // TODO: your validation rules go here
    }
}

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        // TODO: your validation rules go here
    }
}
