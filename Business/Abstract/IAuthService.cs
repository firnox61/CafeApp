using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs;
using Entities.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<User>> RegisterAsync(UserForRegisterDto registerDto, string password);
        Task<IDataResult<User>> LoginAsync(UserForLoginDto loginDto);
        Task<IResult> UserExistsAsync(string email);
        Task<IDataResult<AccessToken>> CreateAccessTokenAsync(User user);
    }
}
