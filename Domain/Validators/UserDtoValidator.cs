using FluentValidation;
using RiwiTalent.Application.DTOs;

namespace RiwiTalent.Domain.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            //First we establish the functions that we gonna call
            Include(new UserEmailRule());
            Include(new UserPasswordRule());
        }

        //Validations
        public class UserEmailRule : AbstractValidator<UserDto>
        {
            public UserEmailRule()
            {
                RuleFor(user => user.Email).NotEmpty()
                                            .WithMessage("The field Email is required")
                                            .EmailAddress()
                                            .WithMessage("The Email format isn't correct");
            }
        }

        public class UserPasswordRule : AbstractValidator<UserDto>
        {
            public UserPasswordRule()
            {
                RuleFor(user => user.Password).NotEmpty()
                                                .WithMessage("The password is required")
                                                .MinimumLength(3)
                                                .WithMessage("The password must minimun 8 characters");
                                                /* .Matches("[A-Z]")
                                                .WithMessage("The password must contain at least one uppercase letter")
                                                .Matches("a-z")
                                                .WithMessage("The password must contain at least one lowercase letter")
                                                .Matches("[^a-zA-Z0-9]")
                                                .WithMessage("The password must conatine at least one special character"); */
            }
        }
    }
}
