using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Chat.AuthService.Web.Application.Messaging.EventItemMessages.ViewModels;
using Chat.AuthService.Web.Definitions.Mediator.Base;
using MediatR;

namespace Chat.AuthService.Web.Definitions.Mediator
{
    public class EventItemPostTransactionBehavior(IUnitOfWork unitOfWork)
        : TransactionBehavior<IRequest<Operation<EventItemViewModel>>, Operation<EventItemViewModel>>(unitOfWork);
}