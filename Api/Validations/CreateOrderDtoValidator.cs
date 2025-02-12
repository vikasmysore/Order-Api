using Api.Dtos;
using FluentValidation;

namespace Api.Validations
{
    public class CreateOrderDtoValidator : AbstractValidator<OrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(orderDto => orderDto.ItemId).NotEmpty();

            RuleFor(orderDto => orderDto.Email).NotEmpty()
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(orderDto => orderDto.ItemCount).NotEmpty();

            RuleFor(orderDto => orderDto.ItemCount).GreaterThan(0);
        }
    }
}
