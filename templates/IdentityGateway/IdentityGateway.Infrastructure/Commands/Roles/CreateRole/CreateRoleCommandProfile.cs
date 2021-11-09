using AutoMapper;
using Microsoft.AspNetCore.Identity;
using IdentityGateway.Core.Commands.Roles.CreateRole;

namespace IdentityGateway.Infrastructure.Commands.Roles.CreateRole
{
    public class CreateRoleCommandProfile : Profile
    {
        public CreateRoleCommandProfile()
        {
            CreateMap<CreateRoleCommand, IdentityRole>();
        }
    }
}