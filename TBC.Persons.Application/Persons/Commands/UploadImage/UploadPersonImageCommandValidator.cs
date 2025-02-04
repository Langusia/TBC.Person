using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace TBC.Persons.Application.Persons.Commands.UploadImage;

public class UploadPersonImageCommandValidator : AbstractValidator<UploadPersonImageCommand>
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    public UploadPersonImageCommandValidator(IStringLocalizer<UploadPersonImageCommand> localizer)
    {
        RuleFor(x => x.Image)
            .NotNull()
            .WithMessage(localizer["image is required"].Value)
            .Must(IsValidFileExtension)
            .WithMessage(localizer["invalid file format"].Value)
            .Must(IsValidFileSize)
            .WithMessage(localizer["file too large maxSize is : {0} bytes", MaxFileSize].Value);
    }

    private bool IsValidFileExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }

    private bool IsValidFileSize(IFormFile file)
    {
        return file.Length <= MaxFileSize;
    }
}