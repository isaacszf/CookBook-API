using Communication.Requests;
using Domain.Repositories;
using Domain.Repositories.User;
using Domain.Services.LoggedUser;
using Exceptions;
using Exceptions.Base;
using FluentValidation.Results;

namespace Application.UseCases.User.Profile;

public class UpdateProfileUseCase : IUpdateProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository updateOnlyRepository,
            IUserReadOnlyRepository readOnlyRepository,
            IUnitOfWork unitOfWork
        )
    {
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(RequestUpdateUserJson req)
    {
        var loggedUser = await _loggedUser.User();
        await Validate(req, loggedUser.Email);

        var user = await _updateOnlyRepository.GetById(loggedUser.Id);

        user.Email = req.Email;
        user.Name = req.Name;

        _updateOnlyRepository.Update(user);
        
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson req, string currentEmail)
    {
        var validator = new UpdateUserValidator();
        var result = validator.Validate(req);

        if (!currentEmail.Equals(req.Email))
        {
            var userExists = await _readOnlyRepository.ExistsActiveUserWithEmail(req.Email);
            if (userExists)
                result.Errors.Add(new ValidationFailure("email", ResourceMessageException.EMAIL_IN_USE));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors
                .Select(error => error.ErrorMessage)
                .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}