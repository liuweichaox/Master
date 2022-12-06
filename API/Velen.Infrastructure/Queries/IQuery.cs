using MediatR;

namespace Velen.Infrastructure.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}