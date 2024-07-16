using Application.Services.Cryptography;
using Communication.Requests;
using Communication.Responses;
using Domain.Repositories.User;
using Domain.Security.Tokens;
using Exceptions.Base;

namespace Application.UseCases.User.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly PasswordEncrypt _passwordEncrypt;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    
    public DoLoginUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        PasswordEncrypt passwordEncrypt,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _readOnlyRepository = readOnlyRepository;
        _passwordEncrypt = passwordEncrypt;
        _accessTokenGenerator = accessTokenGenerator;
    }
    
    public async Task<ResponseLoginUserJson> Execute(RequestLoginUserJson req)
    {
        var encryptedPassword = _passwordEncrypt.Encrypt(req.Password);
        var user = await _readOnlyRepository.GetByEmailAndPassword(req.Email, encryptedPassword);

        if (user is null) throw new InvalidLoginException();
        
        return new ResponseLoginUserJson()
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson()
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }
}