using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using Chat.API.Domain;
using Chat.API.Domain.Base;
using Chat.API.Web.Application.Messaging.EventItemMessages.ViewModels;
using MediatR;

namespace Chat.API.Web.Application.Messaging.EventItemMessages.Queries
{
    /// <summary>
    /// Request for EventItem by Identifier
    /// </summary>
    public sealed class GetEventItemById
    {
        public record Request(Guid Id) : IRequest<Operation<EventItemViewModel, string>>;

        public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
            : IRequestHandler<Request, Operation<EventItemViewModel, string>>
        {
            /// <summary>Handles a request getting log by identifier</summary>
            /// <param name="request">The request</param>
            /// <param name="cancellationToken">Cancellation token</param>
            /// <returns>Response from the request</returns>
            public async Task<Operation<EventItemViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
            {
                var id = request.Id;
                var repository = unitOfWork.GetRepository<EventItem>();
                var entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (entityWithoutIncludes == null)
                {
                    return Operation.Error($"Entity with identifier {id} not found");
                }

                var mapped = mapper.Map<EventItemViewModel>(entityWithoutIncludes);

                return mapped is not null
                    ? Operation.Result(mapped)
                    : Operation.Error(AppData.Exceptions.MappingException);
            }
        }
    }
}