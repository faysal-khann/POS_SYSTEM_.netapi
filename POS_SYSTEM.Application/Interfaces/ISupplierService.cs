using POS_SYSTEM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<List<SupplierOutDto>> GetAllAsync();
        Task<SupplierOutDto?> GetByIdAsync(int id);
        Task<string> GetNextSupplierCodeAsync();
        Task<SupplierOutDto> CreateAsync(SupplierCreateDto dto);
        Task<SupplierOutDto?> UpdateAsync(int id, SupplierUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
