using System.Text;
using System.Text.Json;
using FluentValidation;
using MediatR;
using Master.Application.Exceptions;

namespace Master.Application.Behaviors
{
    public class ValidatorBehavior<TRequest,TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Console.WriteLine(@"ValidatorBehavior Handle command type: " + request.GetType().Name+@"result json: "+JsonSerializer.Serialize(request));
            var errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (errors.Any())
            {
                var errorBuilder = new StringBuilder();

                errorBuilder.AppendLine("Invalid command, reason: ");

                foreach (var error in errors)
                {
                    errorBuilder.AppendLine(error.ErrorMessage);
                }

                throw new InvalidCommandException(errorBuilder.ToString(), null);
            }

            var response= next();
            Console.WriteLine(@"ValidatorBehavior Handle End command type: " + request.GetType().Name);
            return response;
        }
    }
}