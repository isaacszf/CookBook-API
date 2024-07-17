using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Repositories;
using Domain.Repositories.User;
using Domain.Services.Cryptography;
using Exceptions;
using Exceptions.Base;
using FluentValidation.Results;

namespace Application.UseCases.User.Register;

public class RegisterUserUseCase: IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordEncrypter _passwordEncrypt;

    public RegisterUserUseCase(
        IUserWriteOnlyRepository writeOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPasswordEncrypter passwordEncrypt)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordEncrypt = passwordEncrypt;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson req)
    {
        await Validate(req);

        var user = _mapper.Map<Domain.Entities.User>(req);
        user.Password = _passwordEncrypt.Encrypt(req.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _writeOnlyRepository.Add(user);
        await _unitOfWork.Commit();
        
        return new ResponseRegisteredUserJson
        {
            Name = req.Name
        };
    }

    private async Task Validate(RequestRegisterUserJson req)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(req);

        var emailExists = await _readOnlyRepository.ExistsActiveUserWithEmail(req.Email);
        if (emailExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessageException.EMAIL_IN_USE));

        if (result.IsValid) return;
        var errors = result.Errors.Select(e => e.ErrorMessage);
        throw new ErrorOnValidationException(errors.ToList());
    }
}