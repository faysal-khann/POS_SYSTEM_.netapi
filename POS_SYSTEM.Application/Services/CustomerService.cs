using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo) => _repo = repo;

        public async Task<List<CustomerOutDto>> GetAllAsync()
        {
            var customers = await _repo.GetAllAsync();
            return customers.Select(MapToDto).ToList();
        }

        public async Task<CustomerOutDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            return c is null ? null : MapToDto(c);
        }

        public async Task<string> GetNextCustomerCodeAsync()
        {
            var maxId = await _repo.GetMaxCustomerIdAsync();
            return $"CUS-{maxId + 1:D4}";
        }

        public async Task<CustomerOutDto> CreateAsync(CustomerCreateDto dto)
        {
            var code = await GetNextCustomerCodeAsync();

            var customer = new Customer
            {
                CustomerCode = code,
                CustomerName = dto.CustomerName,
                Phone = dto.Phone,
                Email = dto.Email,
                CustomerGroup = dto.CustomerGroup,
                DateOfBirth = dto.DateOfBirth,
                NationalIdTaxId = dto.NationalIdTaxId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                StateDivision = dto.StateDivision,
                PostalCode = dto.PostalCode,
                Country = dto.Country ?? "Bangladesh",
                OpeningBalance = dto.OpeningBalance,
                CreditLimit = dto.CreditLimit,
                DueAmount = dto.DueAmount,
                Notes = dto.Notes,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(customer);
            await _repo.SaveChangesAsync();
            return MapToDto(customer);
        }

        public async Task<CustomerOutDto?> UpdateAsync(int id, CustomerUpdateDto dto)
        {
            var customer = await _repo.GetByIdAsync(id);
            if (customer is null) return null;

            customer.CustomerName = dto.CustomerName;
            customer.Phone = dto.Phone;
            customer.Email = dto.Email;
            customer.CustomerGroup = dto.CustomerGroup;
            customer.DateOfBirth = dto.DateOfBirth;
            customer.NationalIdTaxId = dto.NationalIdTaxId;
            customer.AddressLine1 = dto.AddressLine1;
            customer.AddressLine2 = dto.AddressLine2;
            customer.City = dto.City;
            customer.StateDivision = dto.StateDivision;
            customer.PostalCode = dto.PostalCode;
            customer.Country = dto.Country;
            customer.OpeningBalance = dto.OpeningBalance;
            customer.CreditLimit = dto.CreditLimit;
            customer.DueAmount = dto.DueAmount;
            customer.Notes = dto.Notes;
            customer.Status = dto.Status;

            _repo.Update(customer);
            await _repo.SaveChangesAsync();
            return MapToDto(customer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _repo.GetByIdAsync(id);
            if (customer is null) return false;

            _repo.Delete(customer);
            return await _repo.SaveChangesAsync();
        }

        private static CustomerOutDto MapToDto(Customer c) => new(
            c.CustomerId, c.CustomerCode, c.CustomerName, c.Phone, c.Email, c.CustomerGroup,
            c.DateOfBirth, c.NationalIdTaxId, c.AddressLine1, c.AddressLine2, c.City,
            c.StateDivision, c.PostalCode, c.Country, c.OpeningBalance, c.CreditLimit,
            c.DueAmount, c.Notes, c.Status, c.CreatedAt
        );
    }
}
