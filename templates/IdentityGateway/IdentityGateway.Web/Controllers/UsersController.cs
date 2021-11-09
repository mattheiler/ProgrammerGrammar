using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityGateway.Core.Commands.Users.CreateUser;
using IdentityGateway.Web.Models.Users;

namespace IdentityGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        public UsersController(ISender sender, IMapper mapper)
        {
            Sender = sender;
            Mapper = mapper;
        }

        public ISender Sender { get; }

        public IMapper Mapper { get; }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task Register(CreateUserRequest request)
        {
            await Sender.Send(new CreateUserCommand
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                Username = request.Username
            });
        }
    }
}