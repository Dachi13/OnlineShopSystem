namespace Catalog.API.Products.DeleteProduct;

public class DeleteProductCommandValidation : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
    }
}