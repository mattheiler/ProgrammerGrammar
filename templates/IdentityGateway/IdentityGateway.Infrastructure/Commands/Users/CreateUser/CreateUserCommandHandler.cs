using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using IdentityGateway.Core.Commands.Users.CreateUser;

namespace IdentityGateway.Infrastructure.Commands.Users.CreateUser
{
    public class CreateUserCommandHandler : AsyncRequestHandler<CreateUserCommand>
    {
        public CreateUserCommandHandler(UserManager<IdentityUser> userManager, IMapper mapper)
        {
            UserManager = userManager;
            Mapper = mapper;
        }

        public UserManager<IdentityUser> UserManager { get; }

        public IMapper Mapper { get; }

        protected override async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await UserManager.CreateAsync(Mapper.Map(request, new IdentityUser()), request.Password);
        }
    }
}