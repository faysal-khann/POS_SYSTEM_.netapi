using Microsoft.Extensions.Configuration;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System;
using System.Collections.Generic;



namespace POS_SYSTEM.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _repo.GetByUsernameOrEmailAsync(dto.UsernameOrEmail);

            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAppException("Invalid username/email or password");

            if (user.Status != "Active")
                throw new ForbiddenAppException("This account is inactive. Contact your administrator.");

            var branch = await _repo.GetBranchByIdAsync(user.PrimaryBranchId);
            if (branch is null || branch.CompanyId != dto.CompanyId)
                throw new UnauthorizedAppException("User does not belong to the selected company");

            var company = await _repo.GetCompanyByIdAsync(dto.CompanyId);
            if (company is null)
                throw new NotFoundAppException("Company not found");

            user.LastLoginAt = DateTime.UtcNow;
            await _repo.UpdateLastLoginAsync(user);

            var permissionKeys = await _repo.GetPermissionKeysForUserAsync(user.UserId);
            var roleName = user.Role?.RoleName ?? "—";

            var token = CreateAccessToken(user.UserId, roleName);

            return new LoginResponseDto(
                token, "bearer", user.UserId, user.FullName, user.Username, user.Email,
                user.RoleId, roleName, company.CompanyId, company.CompanyName,
                branch.BranchId, branch.BranchName, permissionKeys
            );
        }

        public async Task<CompanyLookupDto> LookupCompanyAsync(string identifier)
        {
            var user = await _repo.GetByUsernameOrEmailAsync(identifier)
                ?? throw new NotFoundAppException("User not found");

            var branch = await _repo.GetBranchByIdAsync(user.PrimaryBranchId)
                ?? throw new NotFoundAppException("Branch not found for this user");

            var company = await _repo.GetCompanyByIdAsync(branch.CompanyId)
                ?? throw new NotFoundAppException("Company not found");

            return new CompanyLookupDto(company.CompanyId, company.CompanyName);
        }

        public async Task<CompanyLookupDto> VerifyCredentialsAsync(VerifyCredentialsRequestDto dto)
        {
            var user = await _repo.GetByUsernameOrEmailAsync(dto.UsernameOrEmail);

            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAppException("Invalid credentials");

            if (user.Status != "Active")
                throw new ForbiddenAppException("This account is inactive");

            var branch = await _repo.GetBranchByIdAsync(user.PrimaryBranchId)
                ?? throw new NotFoundAppException("Branch not found");

            var company = await _repo.GetCompanyByIdAsync(branch.CompanyId)
                ?? throw new NotFoundAppException("Company not found");

            return new CompanyLookupDto(company.CompanyId, company.CompanyName);
        }

        private string CreateAccessToken(int userId, string roleName)
        {
            var secret = _config["Jwt:SecretKey"] ?? "dev-only-fallback-change-me";
            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "1440");

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("role", roleName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    }
