using Communication.Requests;
using Domain.Repositories;
using Domain.Repositories.User;
using Domain.Services.Cryptography;
using Domain.Services.LoggedUser;
using Exceptions;
using Exceptions.Base;
using FluentValidation.Results;

namespace Application.UseCases.User.ChangePassword;

public class ChangeUserPasswordUseCase : IChangeUserPasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserPasswordUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository updateOnlyRepository,
            IPasswordEncrypter passwordEncrypter,
            IUnitOfWork unitOfWork
        )
    {
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _passwordEncrypter = passwordEncrypter;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(RequestChangeUserPasswordJson req)
    {
        var loggedUser = await _loggedUser.User();
        
        Validate(req, loggedUser);
        
        var user = await _updateOnlyRepository.GetById(loggedUser.Id);
        var encryptedNewPassword = _passwordEncrypter.Encrypt(req.NewPassword);
        user.Password = encryptedNewPassword;
        
        _updateOnlyRepository.Update(user);
        
        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangeUserPasswordJson req, Domain.Entities.User user)
    {
        var validator = new ChangeUserPasswordValidator();
        var result = validator.Validate(req);

        var encryptedPassword = _passwordEncrypter.Encrypt(req.Password);
        if (!user.Password.Equals(encryptedPassword))
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessageException.PASSWORD_DOES_NOT_MATCH));
        
        if (req.Password.Equals(req.NewPassword))
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessageException.PASSWORD_EQUAL));

        if (result.IsValid) return;
        
        var errorMessages = result.Errors
            .Select(error => error.ErrorMessage)
            .ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}