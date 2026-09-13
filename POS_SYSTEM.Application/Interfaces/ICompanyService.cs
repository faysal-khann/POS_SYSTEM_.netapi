using POS_SYSTEM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<List<CompanyListItemDto>> GetCompaniesAsync(string? search);
        Task<CompanyDetailDto?> GetDetailAsync(int id);
        Task<CompanyOutDto> CreateAsync(CompanyCreateDto dto);
        Task<CompanyOutDto?> UpdateAsync(int id, CompanyCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
