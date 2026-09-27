using System;
using System.Collections.Generic;
using System.Text;
using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<CompanyLookupDto> LookupCompanyAsync(string identifier);
        Task<CompanyLookupDto> VerifyCredentialsAsync(VerifyCredentialsRequestDto dto);
    }
}
