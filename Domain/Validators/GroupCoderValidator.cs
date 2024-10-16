using FluentValidation;
using RiwiTalent.Application.DTOs;

namespace RiwiTalent.Domain.Validators
{
    public class GroupCoderValidator : AbstractValidator<GroupDto>
    {
        public GroupCoderValidator()
        {
            Include(new GroupNameRule());
            Include(new GroupDescriptionRule());
        }


        //validations
        public class GroupNameRule : AbstractValidator<GroupDto>
        {
            public GroupNameRule()
            {
                RuleFor(group => group.Name).NotEmpty()
                                            .WithMessage("The field Name is required");
            }
        }

        public class GroupDescriptionRule : AbstractValidator<GroupDto>
        {
            public GroupDescriptionRule()
            {
                RuleFor(group => group.Description).NotEmpty()
                                                    .WithMessage("The field Description is required")
                                                    .MaximumLength(100)
                                                    .WithMessage("Description must be maximum 100 characters");
            }
        }
    }
}