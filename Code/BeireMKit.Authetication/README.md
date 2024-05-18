
# BeireMKit Authentication
The BeireMKit Authentication library was developed to simplify the implementation of authentication using JWT, with future plans to support authentication via Google and Facebook.

## Features

1. Jwt Auth.
2. Google Auth (In the Future) 
3. Facebook Auth (In the Future) 

## Requirements
Make sure you have installed the .NET Core 6 SDK on your machine before you start.

## How to use
* Model appsetthings.json
    ```
	"JwtSettings": {
		"Issuer": "test",
		"Audience": "test",
		"SecretKey": "your secret key"
	},
	```
* Add the Authentication service to Startup
    ```
	using BeireMKit.Authetication.Extensions;
	using BeireMKit.Authetication.Models;

    public void ConfigureServices(IServiceCollection services)
    {
		var jwtSetthings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        builder.Services.ConfigureJwtAuthentication(jwtSetthings);
		builder.Services.ConfigureJwtServices();
    }
    ```
 ## Example of use with JWT
 * Authentication example
	```
	using BeireMKit.Authetication.Interfaces.Jwt;
	using System.Security.Claims;
	
	public class AuthService : IAuthService
    {
        private readonly IBaseRepository<User> _repository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IBaseRepository<User> repository, IPasswordHasherService passwordHasher, IJwtTokenService jwtTokenService)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ServiceResult<User>> AuthenticateAsync(string email, string password)
        {
            var user = await _repository.FindAsync(x => x.Email == email)?.FirstOrDefault();

            if (user != null && _passwordHasher.VerifyPassword(user.Password, password))
            {
                user.JwtToken = _jwtTokenService.GenerateJwtToken(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                });

                var result = await _repository.UpdateAsync(user);
                return ServiceResult<User>.CreateValidResult(result);
            }
            return ServiceResult<User>.CreateInvalidResult("It was not possible to authenticate.", HttpStatusCode.Unauthorized);
        }
    }
	```
 * registration example
	```
	using BeireMKit.Authetication.Interfaces.Jwt;
	
	private readonly IBaseRepository<User> _repository;
    private readonly IPasswordHasherService _passwordHasher;

    public UserService(IBaseRepository<User> repository, IPasswordHasherService passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    } 
	
	public async Task<ServiceResult<User>> CreateAsync(User user)
    {
        user.Password = _passwordHasher.HashPassword(user.Password);
        var entity = await _repository.CreateAsync(user);

        return ServiceResult<User>.CreateValidResult(entity);
    }
	```