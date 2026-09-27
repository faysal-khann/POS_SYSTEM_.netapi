using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repo;

        public CompanyService(ICompanyRepository repo) => _repo = repo;

        public async Task<List<CompanyListItemDto>> GetCompaniesAsync(string? search)
        {
            var companies = await _repo.SearchAsync(search);
            return companies.Select(c => new CompanyListItemDto(
                c.CompanyId, c.CompanyName, c.Phone, c.Email, c.Address, c.Currency,
                c.IsActive ?? false ? "Active" : "Inactive"
            )).ToList();
        }

        public async Task<CompanyDetailDto?> GetDetailAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c is null) return null;

            return new CompanyDetailDto(
                c.CompanyName, c.Phone, c.Email, c.Address, c.Country, c.Currency, c.TaxNo, c.IsActive
            );
        }

        public async Task<CompanyOutDto> CreateAsync(CompanyCreateDto dto)
        {
            var company = new Company
            {
                CompanyName = dto.CompanyName,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                Country = dto.Country,
                Currency = dto.Currency,
                TaxNo = dto.TaxNo,
                IsActive = dto.IsActive ?? true
            };

            await _repo.AddAsync(company);
            await _repo.SaveChangesAsync();

            return new CompanyOutDto(company.CompanyId, company.CompanyName);
        }

        public async Task<CompanyOutDto?> UpdateAsync(int id, CompanyCreateDto dto)
        {
            var company = await _repo.GetByIdAsync(id);
            if (company is null) return null;

            company.CompanyName = dto.CompanyName;
            company.Phone = dto.Phone;
            company.Email = dto.Email;
            company.Address = dto.Address;
            company.Country = dto.Country;
            company.Currency = dto.Currency;
            company.TaxNo = dto.TaxNo;
            company.IsActive = dto.IsActive ?? true;

            await _repo.SaveChangesAsync();

            return new CompanyOutDto(company.CompanyId, company.CompanyName);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _repo.GetByIdAsync(id);
            if (company is null) return false;

            _repo.Delete(company);
            return await _repo.SaveChangesAsync();
        }
    }
}
