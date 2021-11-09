using AutoMapper;
using Microsoft.AspNetCore.Identity;
using IdentityGateway.Core.Commands.Users.CreateUser;

namespace IdentityGateway.Infrastructure.Commands.Users.CreateUser
{
    public class CreateUserCommandProfile : Profile
    {
        public CreateUserCommandProfile()
        {
            CreateMap<CreateUserCommand, IdentityUser>();
        }
    }
}