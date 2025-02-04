using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Commands.UploadImage;

public record UploadPersonImageCommand(long PersonId, IFormFile Image) : IRequest<Result<string>>;

public class UploadPersonImageCommandHandler(IUnitOfWork uot, IWebHostEnvironment environment)
    : IRequestHandler<UploadPersonImageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadPersonImageCommand request, CancellationToken cancellationToken)
    {
        var person =
            await uot.PersonsRepository.GetByIdAsync(request.PersonId, false, cancellationToken: cancellationToken);
        if (person == null)
            Result.Failure<string>(Error.NotFound);

        var uploadPath = Path.Combine(environment.ContentRootPath, "uploads");
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        if (!string.IsNullOrEmpty(person.PicturePath))
        {
            var oldImagePath = Path.Combine(environment.ContentRootPath, person.PicturePath.TrimStart('/'));
            if (File.Exists(oldImagePath))
                File.Delete(oldImagePath);
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
        var filePath = Path.Combine(uploadPath, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.Image.CopyToAsync(stream, cancellationToken);
        }

        person.PicturePath = $"/uploads/{fileName}";
        await uot.PersonsRepository.UpdateAsync(person, cancellationToken: cancellationToken);
        await uot.CompleteWorkAsync(cancellationToken);

        return Result.Success(person.PicturePath);
    }
}