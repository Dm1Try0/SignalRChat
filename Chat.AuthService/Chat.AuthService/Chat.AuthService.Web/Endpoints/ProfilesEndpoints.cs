﻿using Calabonga.AspNetCore.AppDefinitions;
using Chat.AuthService.Domain.Base;
using Chat.AuthService.Web.Application.Messaging.ProfileMessages.Queries;
using Chat.AuthService.Web.Application.Messaging.ProfileMessages.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.AuthService.Web.Endpoints
{
    public sealed class ProfilesEndpointDefinition : AppDefinition
    {
        public override void ConfigureApplication(WebApplication app)
            => app.MapProfilesEndpoints();
    }

    internal static class ProfilesEndpointDefinitionExtensions
    {
        public static void MapProfilesEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/profiles").WithTags("Profiles");

            group.MapGet("roles", async ([FromServices] IMediator mediator, HttpContext context)
                    => await mediator.Send(new GetProfile.Request(), context.RequestAborted))
                .RequireAuthorization(AppData.PolicyDefaultName)
                .RequireAuthorization(x =>
                {
                    x.RequireClaim("Profiles:Roles:Get");
                })
                .Produces(200)
                .ProducesProblem(401)
                .WithOpenApi();

            group.MapPost("register", async ([FromServices] IMediator mediator, RegisterViewModel model, HttpContext context)
                    => await mediator.Send(new RegisterAccount.Request(model), context.RequestAborted))
                .Produces(200)
                .WithOpenApi()
                .AllowAnonymous();
        }
    }
}