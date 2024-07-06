using MediatR;

namespace Master.Infrastructure.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
}