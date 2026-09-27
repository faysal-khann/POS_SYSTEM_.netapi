using POS_SYSTEM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerOutDto>> GetAllAsync();
        Task<CustomerOutDto?> GetByIdAsync(int id);
        Task<string> GetNextCustomerCodeAsync();
        Task<CustomerOutDto> CreateAsync(CustomerCreateDto dto);
        Task<CustomerOutDto?> UpdateAsync(int id, CustomerUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
