﻿using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Chat.AuthService.Domain;
using Chat.AuthService.Domain.Base;
using Chat.AuthService.Web.Application.Messaging.EventItemMessages.ViewModels;
using MediatR;

namespace Chat.AuthService.Web.Application.Messaging.EventItemMessages.Queries
{
    /// <summary>
    /// EventItem delete
    /// </summary>
    public sealed class DeleteEventItem
    {
        public record Request(Guid Id) : IRequest<Operation<EventItemViewModel, string>>;

        public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
            : IRequestHandler<Request, Operation<EventItemViewModel, string>>
        {
            /// <summary>Handles a request</summary>
            /// <param name="request">The request</param>
            /// <param name="cancellationToken">Cancellation token</param>
            /// <returns>Response from the request</returns>
            public async Task<Operation<EventItemViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
            {
                var repository = unitOfWork.GetRepository<EventItem>();
                var entity = await repository.FindAsync(request.Id);
                if (entity == null)
                {
                    return Operation.Error("Entity not found");
                }

                repository.Delete(entity);
                await unitOfWork.SaveChangesAsync();
                if (!unitOfWork.LastSaveChangesResult.IsOk)
                {
                    return Operation.Error(unitOfWork.LastSaveChangesResult.Exception?.Message ?? AppData.Exceptions.SomethingWrong);
                }

                var mapped = mapper.Map<EventItemViewModel>(entity);
                if (mapped is not null)
                {
                    return Operation.Result(mapped);
                }

                return Operation.Error(AppData.Exceptions.MappingException);

            }
        }
    }
}