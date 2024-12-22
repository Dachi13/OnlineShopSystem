namespace Basket.API.Basket.DeleteBasket;

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(basket => basket.UserName).NotEmpty().WithMessage("UserName is required");
    }
}