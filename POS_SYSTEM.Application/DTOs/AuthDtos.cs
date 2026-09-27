using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace POS_SYSTEM.Application.DTOs
{
    public record LoginRequestDto(int CompanyId, string UsernameOrEmail, string Password);

    public record LoginResponseDto(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        int UserId,
        string FullName,
        string Username,
        string Email,
        int RoleId,
        string RoleName,
        int CompanyId,
        string CompanyName,
        int PrimaryBranchId,
        string BranchName,
        List<string> PermissionKeys
    );

    public record VerifyCredentialsRequestDto(string UsernameOrEmail, string Password);

    public record CompanyLookupDto(int CompanyId, string CompanyName);
}
