
namespace Basket.API.Basket.StoreBasket;

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(basket => basket.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(basket => basket.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}