using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using IdentityGateway.Core.Commands.Users.CreateUser;

namespace IdentityGateway.Infrastructure.Commands.Users.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(UserManager<IdentityUser> userManager, IMapper mapper)
        {
            UserManager = userManager;
            Mapper = mapper;
        }

        public UserManager<IdentityUser> UserManager { get; }

        public IMapper Mapper { get; }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<CreateUserCommand> context, CancellationToken cancellation = default)
        {
            var results = await base.ValidateAsync(context, cancellation);
            var user = Mapper.Map(context.InstanceToValidate, new IdentityUser());

            foreach (var validator in UserManager.UserValidators)
            {
                var result = await validator.ValidateAsync(UserManager, user);
                if (result.Succeeded)
                    continue;

                foreach (var error in result.Errors) results.Errors.Add(new ValidationFailure(string.Empty, error.Description));
            }

            return results;
        }
    }
}