using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Entities.DTOs.Users;
using Entities.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;
        private readonly IUserDal _userDal;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IMapper mapper, IUserDal userDal)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
            _userDal = userDal;
        }

        public async Task<IDataResult<AccessToken>> CreateAccessTokenAsync(User user)
        {
            var claims = await _userService.GetClaimsAsync(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken);
        }

        public async Task<IDataResult<User>> LoginAsync(UserForLoginDto loginDto)
        {
            var userResult = await _userService.GetByEmailAsync(loginDto.Email);
            if (userResult.Data == null)
                return new ErrorDataResult<User>("Kullanıcı bulunamadı");

            var user = userResult.Data;

            bool isPasswordValid = HashingHelper.VerifyPasswordHash(
                loginDto.Password,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!isPasswordValid)
                return new ErrorDataResult<User>("Şifre hatalı");

            return new SuccessDataResult<User>(user, "Giriş başarılı");
        }

        public async Task<IDataResult<User>> RegisterAsync(UserForRegisterDto registerDto, string password)
        {
           byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.Status = true;

            // await _userService.AddAsync(_mapper.Map<UserCreateDto>(user)); // İstersen doğrudan user da yollayabilirsin
            await _userDal.AddAsync(user); // DTO'ya dönüştürmeden direkt entity
            return new SuccessDataResult<User>(user, "Kayıt başarılı");
        }

        public async Task<IResult> UserExistsAsync(string email)
        {
            var userResult = await _userService.GetByEmailAsync(email);
            if (userResult.Data != null)
                return new ErrorResult("Kullanıcı zaten var");
            return new SuccessResult();
        }
    }
}
