﻿using AutoMapper;
using Calabonga.Microservices.Core;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using Chat.API.Domain;
using Chat.API.Domain.Base;
using Chat.API.Web.Application.Messaging.EventItemMessages.ViewModels;
using MediatR;

namespace Chat.API.Web.Application.Messaging.EventItemMessages.Queries
{
    /// <summary>
    /// Request: EventItem creation
    /// </summary>
    public sealed class PostEventItem
    {
        public record Request(EventItemCreateViewModel Model) : IRequest<Operation<EventItemViewModel, string>>;

        public class Handler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Handler> logger)
            : IRequestHandler<Request, Operation<EventItemViewModel, string>>
        {
            public async Task<Operation<EventItemViewModel, string>> Handle(Request eventItemRequest, CancellationToken cancellationToken)
            {
                logger.LogDebug("Creating new EventItem");

                var entity = mapper.Map<EventItemCreateViewModel, EventItem>(eventItemRequest.Model);
                if (entity == null)
                {
                    var exceptionMapper = new MicroserviceUnauthorizedException(AppContracts.Exceptions.MappingException);
                    logger.LogError(exceptionMapper, "Mapper not configured correctly or something went wrong");

                    return Operation.Error(exceptionMapper.Message);
                }

                await unitOfWork.GetRepository<EventItem>().InsertAsync(entity, cancellationToken);
                await unitOfWork.SaveChangesAsync();

                var lastResult = unitOfWork.LastSaveChangesResult;
                if (lastResult.IsOk)
                {
                    var mapped = mapper.Map<EventItem, EventItemViewModel>(entity);
                    logger.LogInformation("New entity {EventItem} successfully created", entity);
                    return mapped is not null
                        ? Operation.Result(mapped)
                        : Operation.Error(AppData.Exceptions.MappingException);
                }

                var errorMessage = lastResult.Exception?.Message ?? AppData.Exceptions.SomethingWrong;
                logger.LogError(errorMessage, "Error data saving to Database or something went wrong");

                return Operation.Error(errorMessage);
            }
        }
    }
}