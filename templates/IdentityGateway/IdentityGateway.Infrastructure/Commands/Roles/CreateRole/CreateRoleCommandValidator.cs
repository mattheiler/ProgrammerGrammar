using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using IdentityGateway.Core.Commands.Roles.CreateRole;

namespace IdentityGateway.Infrastructure.Commands.Roles.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            RoleManager = roleManager;
            Mapper = mapper;
        }

        public RoleManager<IdentityRole> RoleManager { get; }

        public IMapper Mapper { get; }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<CreateRoleCommand> context, CancellationToken cancellation = default)
        {
            var results = await base.ValidateAsync(context, cancellation);
            var role = Mapper.Map(context.InstanceToValidate, new IdentityRole());

            foreach (var validator in RoleManager.RoleValidators)
            {
                var result = await validator.ValidateAsync(RoleManager, role);
                if (result.Succeeded)
                    continue;

                foreach (var error in result.Errors) results.Errors.Add(new ValidationFailure(string.Empty, error.Description));
            }

            return results;
        }
    }
}