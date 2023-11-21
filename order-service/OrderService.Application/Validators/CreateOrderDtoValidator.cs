using FluentValidation;
using OrderService.Application.Dtos.Requests;

namespace OrderService.Application.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(createOrderDto => createOrderDto.FactoryId).GreaterThan(0);
        RuleFor(createOrderDto => createOrderDto.OrderType).IsInEnum();
    }
}
