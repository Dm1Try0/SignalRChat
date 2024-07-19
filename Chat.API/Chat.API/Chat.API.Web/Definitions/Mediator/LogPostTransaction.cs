using Calabonga.Results;
using Calabonga.UnitOfWork;
using Chat.API.Web.Application.Messaging.EventItemMessages.ViewModels;
using Chat.API.Web.Definitions.Mediator.Base;
using MediatR;

namespace Chat.API.Web.Definitions.Mediator
{
    public class LogPostTransactionBehavior : TransactionBehavior<IRequest<Operation<EventItemViewModel>>, Operation<EventItemViewModel>>
    {
        public LogPostTransactionBehavior(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}