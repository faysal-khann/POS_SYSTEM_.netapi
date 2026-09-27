using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repo;

        public BranchService(IBranchRepository repo) => _repo = repo;

        public async Task<List<BranchListItemDto>> GetBranchesAsync(string? search)
        {
            var branches = await _repo.SearchAsync(search);
            return branches.Select(b => new BranchListItemDto(
                b.BranchId, b.BranchCode, b.BranchName, b.ManagerName, b.Phone, b.Address,
                (b.IsActive ?? false) ? "Active" : "Inactive"
            )).ToList();
        }

        public async Task<BranchDetailDto?> GetDetailAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b is null) return null;

            return new BranchDetailDto(
                b.BranchId, b.BranchCode, b.BranchName, b.ManagerName, b.Phone, b.Email, b.Address,
                (b.IsActive ?? false) ? "Active" : "Inactive"
            );
        }

        public async Task<BranchDetailDto> CreateAsync(BranchCreateDto dto)
        {
            var code = await GenerateNextBranchCodeAsync();

            var branch = new Branch
            {
                CompanyId = dto.CompanyId,
                BranchCode = code,
                BranchName = dto.BranchName,
                ManagerName = dto.ManagerName,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                IsActive = (dto.Status ?? "Active") == "Active"
            };

            await _repo.AddAsync(branch);
            await _repo.SaveChangesAsync();

            return new BranchDetailDto(
                branch.BranchId, branch.BranchCode, branch.BranchName, branch.ManagerName,
                branch.Phone, branch.Email, branch.Address, (branch.IsActive ?? false) ? "Active" : "Inactive"
            );
        }

        public async Task<BranchDetailDto?> UpdateAsync(int id, BranchUpdateDto dto)
        {
            var branch = await _repo.GetByIdAsync(id);
            if (branch is null) return null;

            branch.BranchName = dto.BranchName;
            branch.ManagerName = dto.ManagerName;
            branch.Phone = dto.Phone;
            branch.Email = dto.Email;
            branch.Address = dto.Address;
            branch.IsActive = (dto.Status ?? "Active") == "Active";

            await _repo.SaveChangesAsync();

            return new BranchDetailDto(
                branch.BranchId, branch.BranchCode, branch.BranchName, branch.ManagerName,
                branch.Phone, branch.Email, branch.Address, (branch.IsActive ?? false) ? "Active" : "Inactive"
            );
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await _repo.GetByIdAsync(id);
            if (branch is null) return false;

            _repo.Delete(branch);
            return await _repo.SaveChangesAsync();
        }

        private async Task<string> GenerateNextBranchCodeAsync()
        {
            var last = await _repo.GetLastBranchWithCodePrefixAsync();
            if (last?.BranchCode is null) return "BR-001";

            var parts = last.BranchCode.Split('-');
            var lastNumber = parts.Length > 1 && int.TryParse(parts[1], out var n) ? n : 0;
            return $"BR-{(lastNumber + 1):D3}";
        }
    }
}
