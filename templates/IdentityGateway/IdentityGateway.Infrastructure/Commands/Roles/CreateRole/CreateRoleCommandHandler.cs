using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using IdentityGateway.Core.Commands.Roles.CreateRole;

namespace IdentityGateway.Infrastructure.Commands.Roles.CreateRole
{
    public class CreateRoleCommandHandler : AsyncRequestHandler<CreateRoleCommand>
    {
        public CreateRoleCommandHandler(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            RoleManager = roleManager;
            Mapper = mapper;
        }

        public RoleManager<IdentityRole> RoleManager { get; }

        public IMapper Mapper { get; }

        protected override async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            await RoleManager.CreateAsync(Mapper.Map(request, new IdentityRole()));
        }
    }
}