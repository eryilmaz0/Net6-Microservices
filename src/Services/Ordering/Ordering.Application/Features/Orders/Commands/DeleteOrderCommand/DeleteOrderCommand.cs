using MediatR;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrderCommand;

public class DeleteOrderCommand : IRequest
{
    public int Id { get; set; }
}