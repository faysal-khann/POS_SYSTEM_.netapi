using POS_SYSTEM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Interfaces
{
    public interface IBranchService
    {
        Task<List<BranchListItemDto>> GetBranchesAsync(string? search);
        Task<BranchDetailDto?> GetDetailAsync(int id);
        Task<BranchDetailDto> CreateAsync(BranchCreateDto dto);
        Task<BranchDetailDto?> UpdateAsync(int id, BranchUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
