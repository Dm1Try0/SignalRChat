﻿using Chat.API.Domain.Base;

namespace Chat.API.Web.Application.Messaging.EventItemMessages.ViewModels
{
    public class EventItemUpdateViewModel : ViewModelBase
    {
        public string Logger { get; set; } = null!;

        public string Level { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}