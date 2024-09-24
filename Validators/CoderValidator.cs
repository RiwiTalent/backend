using FluentValidation;
using RiwiTalent.Models;

namespace RiwiTalent.Validators
{
    public class CoderValidator : AbstractValidator<Coder>
    {
        public CoderValidator()
        {
            Include(new CoderFirstNameRule());
            Include(new CoderSecondNameRule());
            Include(new CoderFirstLastNameRule());
            Include(new CoderSecondLastNameRule());
            Include(new CoderEmailRule());
            // Include(new CoderPhotoRule());
            Include(new CoderAgeRule());
            // Include(new CoderCvRule());
        }

        //validations
        public class CoderFirstNameRule : AbstractValidator<Coder>
        {
            public CoderFirstNameRule()
            {
                RuleFor(coder => coder.FirstName).NotEmpty()
                                                 .WithMessage("The FirstName is required");

            }
        }
        
        public class CoderSecondNameRule : AbstractValidator<Coder>
        {
            public CoderSecondNameRule()
            {
                RuleFor(coder => coder.SecondName).NotEmpty()
                                                 .WithMessage("The SecondName is required");
            }
        }

        public class CoderFirstLastNameRule : AbstractValidator<Coder>
        {
            public CoderFirstLastNameRule()
            {
                RuleFor(coder => coder.FirstLastName).NotEmpty()
                                                 .WithMessage("The FirstLastName is required");
            }
        }

        public class CoderSecondLastNameRule : AbstractValidator<Coder>
        {
            public CoderSecondLastNameRule()
            {
                RuleFor(coder => coder.SecondLastName).NotEmpty()
                                                 .WithMessage("The SecondLastName is required");
            }
        }

        public class CoderEmailRule : AbstractValidator<Coder>
        {
            public CoderEmailRule()
            {
                RuleFor(coder => coder.Email).NotEmpty()
                                                 .WithMessage("The Email is required")
                                                 .EmailAddress()
                                                 .WithMessage("The Email isn't with correct format");
            }
        }

        // public class CoderPhotoRule : AbstractValidator<Coder>
        // {
        //     public CoderPhotoRule()
        //     {
        //         RuleFor(coder => coder.Photo).NotEmpty()
        //                                      .WithMessage("The field Photo can't be empty")
        //                                      .Must(ValidImageFormat)
        //                                      .WithMessage("The image must be a valid format (png, jpg, jpeg)");
        //     }

        //     public bool ValidImageFormat(string Photo)
        //     {
        //         var AllowFormat = new[] { "png", "jpg", "jpeg" };
        //         var Extension = Path.GetExtension(Photo)?.ToLower();
        //         return AllowFormat.Contains(Extension);
        //     }
        // }

        public class CoderAgeRule : AbstractValidator<Coder>
        {
            public CoderAgeRule()
            {
                RuleFor(coder => coder.Age).NotEmpty()
                                           .WithMessage("The field Age is required");
            }
        }

        // public class CoderCvRule : AbstractValidator<Coder>
        // {
        //     public CoderCvRule()
        //     {
        //         RuleFor(coder => coder.Cv).NotEmpty()
        //                                    .WithMessage("The field Cv is required")
        //                                    .Must(ValidCvFormat)
        //                                    .WithMessage("The Cv format valid is (pdf)")
        //                                    .Must(SizeCv)
        //                                    .WithMessage("The PDF file size must be less than 300KB.");
        //     }

        //     public bool ValidCvFormat(string CurriculumVitae)
        //     {
        //         var AllowCv = new[] { "pdf" };
        //         var Extension = Path.GetExtension(CurriculumVitae)?.ToLower();
        //         return AllowCv.Contains(Extension);
        //     }

        //     public bool SizeCv(string Size)
        //     {
        //         //we realize calc to convert KB at Bytes
        //         const int MaxFileSize = 300 * 1024;
        //         var fileInfo = new FileInfo(Size);
        //         return fileInfo.Length <= MaxFileSize;
        //     }
        // }
    }
}